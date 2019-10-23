using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        public int CampgroundId { get; set; }
        public int ParkId { get; set; }
        public string Name { get; set; }
        public DateTime OpenMonth { get; set; }
        public DateTime ClosedMonth { get; set; }
        public decimal DailyFee { get; set; }




    }
}
