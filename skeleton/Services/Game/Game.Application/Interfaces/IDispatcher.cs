using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.Interfaces
{
    public interface IDispatcher
    {
        Task<TResult> Send<TResult>(ICommand<TResult> command);
        Task<TResult> Query<TResult>(IQuery<TResult> query);
    }
}
