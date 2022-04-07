using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enums
{
    public enum OrderStatusEnum
    {
        Waiting = 1,
        Approved = 2,
        Supply = 3,
        OnTheWay = 4,
        Completed = 5,
        Cancelled = 6
    }
}
