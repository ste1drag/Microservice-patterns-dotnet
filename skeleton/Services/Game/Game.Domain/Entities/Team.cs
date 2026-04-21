using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Domain.Entities
{
    class Team
    {
        #region Properties
        public Guid Id { get; set; }
        public Guid StadiumId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public Stadium HomeStadium { get; set; }

        #endregion
    }
}
