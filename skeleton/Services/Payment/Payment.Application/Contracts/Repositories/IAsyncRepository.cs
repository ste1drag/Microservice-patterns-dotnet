using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Contracts.Repositories
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<T> AddNewAsync(T item);
        Task<T> UpdateAsync(T item);
    }
}
