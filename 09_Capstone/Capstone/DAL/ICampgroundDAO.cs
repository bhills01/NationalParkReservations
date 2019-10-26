using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICampgroundDAO
    {
        //IList<Campground> GetAllCampground();
        IList<Campground> Search(int parkID);
    }
}
