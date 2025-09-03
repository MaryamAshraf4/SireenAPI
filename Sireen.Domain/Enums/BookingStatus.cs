using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Enums
{
    public enum BookingStatus
    {
        Pending = 1,
        Confirmed = 2, 
        CheckedIn = 3,  
        CheckedOut = 4, 
        Cancelled = 5,
        NoShow = 6
    }
}
