using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Identity;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext dbcontext;
        private Hashtable Repositories;
        public UnitOfWork(StoreContext Dbcontext)
        {
            dbcontext = Dbcontext;
            Repositories = new Hashtable(); 
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
             var Key = typeof(TEntity).Name;
            if (!Repositories.ContainsKey(Key))
              Repositories.Add(Key, new GenericRepository<TEntity>(dbcontext));
            
            return Repositories[Key] as IGenericRepository<TEntity>;
        }
        public async Task<int> CompleteAsync()
         => await dbcontext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
         => await dbcontext.DisposeAsync();

    }
}
