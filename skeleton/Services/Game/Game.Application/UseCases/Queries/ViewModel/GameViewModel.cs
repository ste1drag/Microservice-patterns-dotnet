using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Queries.ViewModel
{
    public class GameViewModel
    {
        public Guid Id { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string StadiumName { get; set; }
        public DateTime Date { get; set; }
    }
}
