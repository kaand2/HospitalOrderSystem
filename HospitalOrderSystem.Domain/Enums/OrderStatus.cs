using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalOrderSystem.Domain.Enums
{
    public enum OrderStatus
    {
        Draft = 1,
        Active = 3,
        InProgress = 4,
        Completed = 5,
        Cancelled = 6,
        Paused = 7,
        Restarted = 8
    }
}
