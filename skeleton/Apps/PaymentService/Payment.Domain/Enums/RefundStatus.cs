using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Enums
{
    public enum RefundStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Completed = 4
    }
}
