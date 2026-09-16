using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("ToDoApp");
builder.Services.AddDbContext<ToDoDbContext>(optBuilder => optBuilder.UseSqlServer(connectionString));

builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
