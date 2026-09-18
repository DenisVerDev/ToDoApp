using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoAPI.Data;
using ToDoAPI.Data.Models;

namespace ToDoAPI.Tests.Fixtures
{
    public class BaseFixture : IDisposable
    {
        private readonly DbContextOptions<ToDoDbContext> _options;

        public readonly int _usersCount;

        public BaseFixture(int usersCount=10)
        {
            _usersCount = usersCount;

            _options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ToDoApp{Guid.NewGuid().ToString()};Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30").Options; // I will think about making it more clean later

            using (var dbContext = CreateDbContext())
            {
                dbContext.Database.EnsureCreated();
                FillDatabase(dbContext);
            }
        }

        protected virtual void FillDatabase(ToDoDbContext dbContext)
        {
            FillUsers(dbContext);
        }

        protected virtual void FillUsers(ToDoDbContext dbContext)
        {
            for (int i = 0; i < _usersCount; i++)
            {
                dbContext.Users.Add(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = $"test{i}@example.com",
                    NormalizedUserName = $"TEST{i}@EXAMPLE.COM",
                    Email = $"test{i}@example.com",
                    NormalizedEmail = $"TEST{i}@EXAMPLE.COM",
                    EmailConfirmed = true
                });
            }

            dbContext.SaveChanges();
        }

        public ToDoDbContext CreateDbContext()
            => new ToDoDbContext(_options);

        public void Dispose()
        {
            using(var dbContext = CreateDbContext())
                dbContext.Database.EnsureDeleted();
        }
    }
}
