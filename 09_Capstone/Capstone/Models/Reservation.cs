using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public string Name { get; set; }
        public int ReservationId { get; set; }
        public int SiteId { get; set; }
        public int MaxOccupants { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
