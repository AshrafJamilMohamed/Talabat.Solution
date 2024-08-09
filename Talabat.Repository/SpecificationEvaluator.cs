using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        // Build The Dynamic Query

        // context.Products.Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category)
      public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> DbSet, ISpecifications<TEntity> Spec)
        {
            var query = DbSet; // Dbcontex.Set<Product>()

            if (Spec.Criteria is not null)
                query = query.Where(Spec.Criteria); // context.Products.Where(P => P.Id == id)

            if(Spec.OrderBy is not null)
                query = query.OrderBy(Spec.OrderBy);

            else if (Spec.OrderByDesc is not null)
                query = query.OrderByDescending(Spec.OrderByDesc);

            // Check for Pagination if exit
            if(Spec.IsPaginationEnabled)
                query = query.Skip(Spec.Skip).Take(Spec.Take);

            // context.Products.Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category)
            query = Spec.Includes.Aggregate(query, (CurrentQuery, IncludesExp) => CurrentQuery.Include(IncludesExp));

            return query;
        }
    }
}
