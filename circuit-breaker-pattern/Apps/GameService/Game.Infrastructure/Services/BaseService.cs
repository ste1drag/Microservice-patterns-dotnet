using Game.Application.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Infrastructure.Services
{
    public class BaseService<T>: IAsyncRepository<T> where T : class
    {
        protected readonly GameDbContext _gameDbContext;

        public BaseService(GameDbContext gameDbContext)
        {
            _gameDbContext = gameDbContext;
        }

        public virtual Task<T> AddNewAsync(T item)
        {
            _gameDbContext.Set<T>().Add(item);
            _gameDbContext.SaveChanges();
            
            return Task.FromResult(item);
        }

        public virtual Task DeleteAsync(Guid id)
        {
            var item = _gameDbContext.Set<T>().Find(id);
            if (item != null)
            {
                _gameDbContext.Set<T>().Remove(item);
                _gameDbContext.SaveChanges();
            }
            return Task.CompletedTask;
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return Task.FromResult(_gameDbContext.Set<T>().ToList());
        }

        public virtual Task<T> GetAsync(Guid id)
        {
            var item = _gameDbContext.Set<T>().Find(id);
            return Task.FromResult(item!);
        }

        public virtual Task<T> UpdateAsync(T item)
        {
            _gameDbContext.Set<T>().Update(item);
            _gameDbContext.SaveChanges();
            return Task.FromResult(item);
        }
    }
}
