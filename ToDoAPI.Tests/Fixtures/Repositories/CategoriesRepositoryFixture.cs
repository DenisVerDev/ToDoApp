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
        public ICategoriesRepository CR { get; private set; }

        public CategoriesRepositoryFixture()
            : base()
        {
            CR = new CategoriesRepository(DbContext);
            FillCategories();
        }

        private void FillCategories()
        {
            for (int i = 0; i < _usersCount; i++)
            {
                DbContext.Categories.Add(new Category
                {
                    Name = $"category{i}",
                    Color = $"FFFFFF",
                    AuthorId = DbContext.Users.Find(i+1).Id
                });

                DbContext.Categories.Add(new Category
                {
                    Name = $"category{i}_{i}",
                    Color = $"FFFFFF",
                    AuthorId = DbContext.Users.Find(i + 1).Id
                });
            }
        }
    }
}
