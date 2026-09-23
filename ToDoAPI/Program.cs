using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToDoAPI.Data;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;
using ToDoAPI.Services.Categories;
using ToDoAPI.Services.Identity;
using ToDoAPI.Services.Tasks;
using ToDoAPI.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("ToDoApp");
builder.Services.AddDbContext<ToDoDbContext>(optBuilder => optBuilder.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<User>().AddEntityFrameworkStores<ToDoDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JwtBearer:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JwtBearer:Audience"],

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearer:SecurityKey"]!))
                    };
                });

builder.Services.AddScoped<IAuthentication, JwtAuthentication>();

builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<IUsersRepository, UsersIdentityRepository>();

builder.Services.AddScoped<ICategoriesManagement, CategoriesManagement>();
builder.Services.AddScoped<ICategoriesFetching, CategoriesFetching>();
builder.Services.AddScoped<ITasksManagement, TasksManagement>();
builder.Services.AddScoped<ITasksFetching, TasksFetching>();
builder.Services.AddScoped<IUsersManagement, UsersManagement>();
builder.Services.AddScoped<IUsersFetching, UsersFetching>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
