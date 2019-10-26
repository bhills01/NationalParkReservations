using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public int CampgroundID { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupants { get; set; }
        public bool Accesible { get; set; }
        public int MaxRvLength { get; set; }
        public bool Utilities { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
}
