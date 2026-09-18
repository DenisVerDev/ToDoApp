using System;
using System.Collections.Generic;
using System.Text;
using ToDoAPI.Data;
using ToDoAPI.Data.Models;

namespace ToDoAPI.Tests.Fixtures.Repositories
{
    public class TasksRepositoryFixture : CategoriesRepositoryFixture
    {
        public TasksRepositoryFixture()
            : base()
        {}

        protected override void FillDatabase(ToDoDbContext dbContext)
        {
            base.FillDatabase(dbContext);
            FillTasks(dbContext);
        }

        private void FillTasks(ToDoDbContext dbContext)
        {
            var categories = dbContext.Categories.ToList();

            for(int i = 0; i< categories.Count; i++)
            {
                dbContext.Tasks.Add(new Data.Models.Task()
                {
                    Title = $"task{i}",
                    AuthorId = categories[i].AuthorId,
                    //Categories = new List<Category>() { categories[i] }
                });
            }

            dbContext.SaveChanges();
        }
    }
}
