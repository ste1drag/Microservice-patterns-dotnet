using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.Contracts.Repository
{
    public interface IStadiumRepository: IAsyncRepository<Domain.Entities.Stadium>
    {
        Task <List<Domain.Entities.StadiumSeat>> GetStadiumSeatsByStadiumId(string stadiumId);
        Task<bool> IsGameSeatAvailable(string stadiumSeatId, string gameId);
    }
}
