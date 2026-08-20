using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Domain.Enums
{
    // Ordinal values MUST match Payment.Domain.Enums.PaymentCurrency
    // (DTOs are serialized as ints across service boundaries)
    public enum PaymentCurrency
    {
        RSD = 1,
        USD = 2,
        EUR = 3,
        GBP = 4,
        JPY = 5,
        AUD = 6,
        CAD = 7,
        CHF = 8,
        SEK = 9,
        NZD = 10
    }

    // Ordinal values MUST match Payment.Domain.Enums.PaymentStatus
    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Refunded = 4,
        Cancelled = 5
    }
}
