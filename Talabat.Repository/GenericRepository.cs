using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Identity;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext context;

        public GenericRepository(StoreContext _context)
        {
            context = _context;
        }

        #region WithOut Specifications

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            ///if (typeof(T) == typeof(Product))
            ///{
            ///    return (IEnumerable<T>)await context.Products.AsNoTracking()
            ///                  .Include(P => P.Brand)
            ///                  .Include(P => P.Category)
            ///                  .ToListAsync();
            ///}



            return await context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            ///if (typeof(T) == typeof(Product))
            ///{
            ///    return await context.Products.Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as T;
            ///}
            return await context.Set<T>().FindAsync(id);
        }

        #endregion

        #region  With Specifications

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> specifications)
           => await ApplyQuery(specifications).ToListAsync();

        public async Task<T?> GetWithSpecAsync(ISpecifications<T> specifications)
           => await ApplyQuery(specifications).FirstOrDefaultAsync();


        public async Task<int> GetCountAsync(ISpecifications<T> specifications)
         => await ApplyQuery(specifications).CountAsync();


        #endregion

        private IQueryable<T> ApplyQuery(ISpecifications<T> spec)
            => SpecificationEvaluator<T>.GetQuery(context.Set<T>(), spec);

        public void Delete(T Entity)
          => context.Remove(Entity);

        public void Update(T Entity)
          => context.Update(Entity);

        public void Add(T Entity)
          => context.Add(Entity);
    }
}
