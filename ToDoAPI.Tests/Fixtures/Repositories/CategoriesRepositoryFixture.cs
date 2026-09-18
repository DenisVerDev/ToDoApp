using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoAPI.Data;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;

namespace ToDoAPI.Tests.Fixtures.Repositories
{
    public class CategoriesRepositoryFixture : BaseFixture
    {
        public CategoriesRepositoryFixture()
            : base()
        {}

        protected override void FillDatabase(ToDoDbContext dbContext)
        {
            base.FillDatabase(dbContext);
            FillCategories(dbContext);
        }

        private void FillCategories(ToDoDbContext dbContext)
        {
            for (int i = 0; i < _usersCount; i++)
            {
                dbContext.Categories.Add(new Category
                {
                    Name = $"category{i}",
                    Color = $"FFFFFF",
                    AuthorId = dbContext.Users.Skip(i).Take(1).First().Id
                });

                dbContext.Categories.Add(new Category
                {
                    Name = $"category{i}_{i}",
                    Color = $"FFFFFF",
                    AuthorId = dbContext.Users.Skip(i).Take(1).First().Id
                });
            }

            dbContext.SaveChanges();
        }
    }
}
