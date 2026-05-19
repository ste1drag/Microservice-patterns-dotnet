using Payment.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Services
{
    public class BaseService<T> : IAsyncRepository<T> where T : class
    {
        protected readonly PaymentDbContext _paymentDbcontext;

        public BaseService(PaymentDbContext paymentDbContext)
        {
            _paymentDbcontext = paymentDbContext;
        }

        public Task<T> AddNewAsync(T item)
        {
            _paymentDbcontext.Set<T>().Add(item);
            _paymentDbcontext.SaveChanges();

            return Task.FromResult(item);
        }

        public Task DeleteAsync(Guid id)
        {
            var item = _paymentDbcontext.Set<T>().Find(id);

            if (item != null)
            {
                _paymentDbcontext.Set<T>().Remove(item);
                _paymentDbcontext.SaveChanges();
            }

            return Task.CompletedTask;
        }

        public Task<List<T>> GetAllAsync()
        {
            return Task.FromResult(_paymentDbcontext.Set<T>().ToList());
        }

        public Task<T> GetAsync(Guid id)
        {
            var item = _paymentDbcontext.Set<T>().Find(id);

            if (item != null)
            {
                return Task.FromResult(item);
            }

            return Task.FromResult<T>(null);
        }

        public Task<T> UpdateAsync(T item)
        {
            _paymentDbcontext.Set<T>().Update(item);
            _paymentDbcontext.SaveChanges();
            return Task.FromResult(item);
        }
    }
}
