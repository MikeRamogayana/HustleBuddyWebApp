using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOSG_Web_App.Model
{
    public class LocationModel
    {
        public int locationId { get; set; }
        public int vendorId { get; set; }
        public string location { get; set; }
        public string description { get; set; }

        public LocationModel(int locationId, int vendorId, string location, string description)
        {
            this.locationId = locationId;
            this.vendorId = vendorId;
            this.location = location;
            this.description = description;
        }

        public LocationModel()
        {

        }
    }
}