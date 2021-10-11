using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.ViewModels
{
    public class LocationVsSale
    {
        public string location { get; set; }
        public int sales { get; set; }

        public LocationVsSale(string location, int sales)
        {
            this.location = location;
            this.sales = sales;
        }

        public LocationVsSale()
        {

        }
    }
}
