# Backend

## What technologies I want to master and utilize
1. ASP.NET Core Identity
2. Global Exceptions Handling
3. Logging

## Design Structure for API
1. Data Access Layer (interfaces and their realizations working with database stuff)
2. Business Logic Layer (interfaces and their realizations working with business logic while conceiling database operations)
3. Controllers Layer

### Data Access Layer
1. ITasksRepository - AddTaskAsync (returns Task entity, cause Id will have a value), DeleteTaskAsync (returns nothing), FetchTaskAsync (by Id only, because there are no other unique values that can be used for identification, maybe using predicate), FetchTasksAsync (predicate search), AnyTaskAsync (predicate). You know what, how about making these methods as generic as possible, so that they could be used for any situation and I didn't have a need to add new functionality, BLL will be fine.
2. ICategoriesRepository - 
3. Technically, Users have Identity to work with so for now I let it be. Though it can be argued that Identity logic should be hidden too.

Problem: There is a problem with more complicated sql queries like grouping, sorting, skipping and other stuff. How about these methods would return simple IQueryable? And then we can build from it?

## Database
If I am gonna use Identity, then it is obvious that certain tables are gonna be created first. That means that I cannot plan from scratch.

In order to design the database we need to understand what we need from the assignment and what ASP.NET Core Identity provides.

Assignment wants:
1. Task entity - title, description, author, author's categories. Thats the minimum I can enfure from tech assignment. There must be an author, categories and some content to edit and view.
2. Category entity - name, color, author, tasks. Again, the minimum. There obviously must be an author, category can be asigned to tasks. Name is self-explanatory while color just serves visual distinction function.
3. User entity - email, password_hash, tasks, categories. Again the minimum. Password hash is my own interpretation.

ASP.NET Core Identity provides:
1. User Entity - name, email, claims, logins, tokens, roles, password hash and many more stuff. ASP.NET Core Identity takes all the burden with managing membership/session logic.
2. Bunch of other stuff that is not as important, because I am here for User Entity.

We can create our own User Entity derived from IdentityUser.

I am gonna use Code First approach.

# Development diary

## 15.09.2026

### What I want to achieve
|Task|Status|
|-|-|
|1. Design the database with ASP.NET Core Identity in mind.|DONE|
|2. Create the database.|DONE|
|3. Design only neccessary DAL.|IN PROGRESS|
|4. Decide the approach in building DAL.|DONE|
|5. Fully create DAL.|IN PROGRESS|
|6. Create xUnit tests for DAL.|Pending...|

## 16.09.2026

### What I want to achieve
|Task|Status|
|-|-|
|1. Design only neccessary DAL.|DONE|
|2. Fully create DAL.|DONE?|
|3. Create xUnit tests for DAL.|Pending...|

### What I need to do
1. ~~I need to add UpdateTaskAsync and UpdateCategoryAsync methods.~~
2. ~~I need to check how ON DELETE CASCADE and ON DELETE NO ACTION is working and make a fix to deletion operations, so that BLL would not concern itself with such stuff.~~
3. ~~I need to understand and design how attaching and detaching categories will work.~~
4. ~~I need to think about seperating from ToDoDbContext to some abstraction.~~
5. **Think about IUsersRepository and how it is supposed to be? Maybe not really? Like we have Identity that does all the work for us, though I am inexperienced in it, so I will wait untill I am actually working with it.**


## 17.09.2026

### What I want to achive
|Task|Status|
|-|-|
|1. Create xUnit tests for DAL.|IN PROGRESS|
|2. Design Business Logic Layer (Services)|Pending...|
|3. Implement Business Logic Layer (Services)|Pending...|

### What I need to do
1. Apparently, my Assert.AllAsync are not what I thought they are and I must remove them.
2. I need to track changes in the database, so that I can check if anything recently was changed. Or create a class which will do comparison of database states for me or simply override Equals in Category entity class