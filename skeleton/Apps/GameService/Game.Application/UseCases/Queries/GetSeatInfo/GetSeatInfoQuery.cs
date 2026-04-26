using Game.Application.Interfaces;
using Game.Application.UseCases.Queries.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Queries.GetSeatInfo
{
    public class GetSeatInfoQuery: IQuery<GameSeatViewModel>
    {
        public Guid GameId { get; set; }
        public Guid SeatId { get; set; }
    }
}
