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
        public ToDoDbContext DbContext { get; private set; }

        public readonly int _usersCount;

        public BaseFixture(int usersCount=10)
        {
            _usersCount = usersCount;

            DbContext = new ToDoDbContext(new DbContextOptionsBuilder<ToDoDbContext>()
                .UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ToDoApp{Guid.NewGuid().ToString()};Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30").Options); // I will think about making it more clean later
        
            FillUsers();
        }

        protected virtual void FillUsers()
        {
            for (int i = 0; i < _usersCount; i++)
            {
                DbContext.Users.Add(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = $"test{i}@example.com",
                    NormalizedUserName = $"TEST{i}@EXAMPLE.COM",
                    Email = $"test{i}@example.com",
                    NormalizedEmail = $"TEST{i}@EXAMPLE.COM",
                    EmailConfirmed = true
                });
            }

            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }
    }
}
