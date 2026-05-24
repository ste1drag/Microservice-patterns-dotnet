using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Domain.Enums
{
    public enum PaymentCurrency
    {
        RSD = 1,
        EUR = 2,
        USD = 3
    }
    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3
    }
}
