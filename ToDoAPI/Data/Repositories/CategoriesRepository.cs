using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories.FetchBuilder;

namespace ToDoAPI.Data.Repositories
{
    public class CategoriesRepository (ToDoDbContext _dbContext) : ICategoriesRepository
    {
        #region Add

        public async System.Threading.Tasks.Task AddCategoryAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task AddCategoriesAsync(IEnumerable<Category> categories)
        {
            if(categories is null)
                throw new ArgumentNullException(nameof(categories));

            if (!categories.Any())
                throw new ArgumentException(nameof(categories));

            await _dbContext.Categories.AddRangeAsync(categories);
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Update

        public async System.Threading.Tasks.Task UpdateCategoryAsync(Category category)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateCategoriesAsync(IEnumerable<Category> categories)
        {
            _dbContext.Categories.UpdateRange(categories);
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Delete

        public async System.Threading.Tasks.Task DeleteCategoryAsync(Category category)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category));

            var original = await FetchCategoryAsync(c => c.Id == category.Id);

            if (original is null)
                throw new Exception($"Provided {nameof(category)}'s signature is absent in the origin.");

            if (!CompareSnapshots(category, original))
                throw new Exception($"Provided {nameof(category)}'s signature is different from it's origin.");

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteCategoriesAsync(ICollection<Category> categories)
        {
            if (categories is null)
                throw new ArgumentNullException(nameof(categories));

            if (!categories.Any())
                throw new ArgumentException(nameof(categories));

            var originals = await FetchCategoriesAsync(c => categories.Contains(c));

            if (originals.Count != categories.Count())
                throw new Exception($"Some of the elements from {nameof(categories)} do not exist in the origin.");

            if (!CompareSnapshots(categories, originals))
                throw new Exception($"Provided {nameof(categories)}'s signature is different from it's origin.");

            _dbContext.Categories.RemoveRange(categories);
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Fetch

        public async Task<Category?> FetchCategoryAsync(Expression<Func<Category, bool>> predicate)
            => await GetPredicateQuery(predicate).AsNoTracking().FirstOrDefaultAsync();

        public async Task<Category?> FetchCategoryAsync(Expression<Func<Category, bool>> predicate, IFetchBuilder<Category> fetchBuilder)
            => await fetchBuilder.Build(GetPredicateQuery(predicate)).AsNoTracking().FirstOrDefaultAsync();

        public async Task<List<Category>> FetchCategoriesAsync(Expression<Func<Category, bool>> predicate)
            => await GetPredicateQuery(predicate).AsNoTracking().ToListAsync();

        public async Task<List<Category>> FetchCategoriesAsync(Expression<Func<Category, bool>> predicate, IFetchBuilder<Category> fetchBuilder)
            => await fetchBuilder.Build(GetPredicateQuery(predicate)).AsNoTracking().ToListAsync();

        #endregion

        #region Other

        public async Task<bool> AnyCategoryAsync(Expression<Func<Category, bool>> predicate)
            => await _dbContext.Categories.AnyAsync(predicate);

        #endregion

        #region Hidden Inner Methods

        private IQueryable<Category> GetPredicateQuery(Expression<Func<Category, bool>> predicate)
            => _dbContext.Categories.Where(predicate).AsQueryable();

        #endregion

        #region ISnapshot<Category>

        public object TakeSnapshot(Category obj)
            => new { obj.Id, obj.Name, obj.Color, obj.AuthorId };

        public object TakeSnapshot(ICollection<Category> collection)
        {
            var snapshots = TakeSnapshots(collection);
            return new { CollectionSnapshot = String.Join(",", snapshots.Select(s => s.ToString())) };
        }

        public object[] TakeSnapshots(ICollection<Category> collection)
            => collection.Select(c => new { c.Id, c.Name, c.Color, c.AuthorId }).ToArray();

        public async Task<object> TakeSnapshotAsync()
        {
            var snapshots = await TakeSnapshotsAsync();
            return new { CollectionSnapshot = String.Join(",", snapshots.Select(s => s.ToString())) };
        }

        public async Task<object[]> TakeSnapshotsAsync()
            => await _dbContext.Categories.Select(c => new { c.Id, c.Name, c.Color, c.AuthorId }).ToArrayAsync();

        public bool CompareSnapshots(Category first, Category second)
        {
            var firstSnapshot = TakeSnapshot(first);
            var secondSnapshot = TakeSnapshot(second);

            return firstSnapshot.Equals(secondSnapshot);
        }

        public bool CompareSnapshots(ICollection<Category> first, ICollection<Category> second)
        {
            var firstSnapshot = TakeSnapshot(first);
            var secondSnapshot = TakeSnapshot(second);

            return firstSnapshot.Equals(secondSnapshot);
        }

        #endregion
    }
}
