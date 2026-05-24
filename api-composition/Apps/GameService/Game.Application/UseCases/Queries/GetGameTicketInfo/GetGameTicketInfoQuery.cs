using Game.Application.Interfaces;
using Game.Application.UseCases.Queries.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Queries.GetGameTicketInfo
{
    public class GetGameInfoQuery: IQuery<GameTicketViewModel>
    {
        public Guid TicketId { get; set; }
    }
}
