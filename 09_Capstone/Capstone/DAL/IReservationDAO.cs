using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        bool IsAvailable(DateTime fromDate, DateTime toDate, int siteId);
        bool MakeReservation(DateTime fromDate, DateTime toDate, string name, int campgroundId);
    }
}
