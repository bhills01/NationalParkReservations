using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        IList<Reservation> Search(DateTime fromDate, DateTime toDate, int campgroundId);
        bool IsAvailable(DateTime fromDate, DateTime toDate, int campgroundId);
        bool MakeReservation(DateTime fromDate, DateTime toDate, string name, int campgroundId);
        //bool CheckAvailable(DateTime fromDate, DateTime toDate, int campgroundId);

    }
}
