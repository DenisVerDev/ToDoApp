using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAPI.Tests.Fixtures.Repositories
{
    [CollectionDefinition("TasksRepositoryCollection")]
    public class TasksRepositoryFixtureDefinition : ICollectionFixture<TasksRepositoryFixture>
    {}
}
