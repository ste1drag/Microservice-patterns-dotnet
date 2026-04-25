using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Domain.Entities
{
    public class Stadium
    {
        #region Properties
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Capacity { get; set; }
        public List<Game> Games { get; set; }
        public Team HomeTeam { get; set; }
        public List<StadiumSeat> Seats { get; set; }
        #endregion
    }
}
