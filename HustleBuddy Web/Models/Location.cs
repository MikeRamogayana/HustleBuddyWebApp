using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Models
{
    public class Location
    {
        public int locationId { get; set; }
        public int vendorId { get; set; }
        public string location { get; set; }
        public string description { get; set; }

        public Location(int locationId, int vendorId, string location, string description)
        {
            this.locationId = locationId;
            this.vendorId = vendorId;
            this.location = location;
            this.description = description;
        }

        public Location()
        {
        }
    }
}
