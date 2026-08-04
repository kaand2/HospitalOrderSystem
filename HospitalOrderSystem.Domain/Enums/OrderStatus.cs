using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalOrderSystem.Domain.Enums
{
    public enum OrderStatus
    {
        Draft = 1,
        Pending = 3,
        Approved = 4,
        Completed = 5,
        Cancelled = 6
    }
}
