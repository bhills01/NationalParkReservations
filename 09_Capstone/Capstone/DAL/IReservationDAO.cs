using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        IList<Reservation> Search(int siteID);
        bool IsAvailable(DateTime fromDate, DateTime toDate, int siteID);
        bool MakeReservation(DateTime fromDate, DateTime toDate, string name, int siteID);

    }
}
