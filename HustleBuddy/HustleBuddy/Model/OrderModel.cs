using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOSG_Web_App.Model
{
    public class OrderModel
    {
        public int orderId { get; set; }
        public int vendorId { get; set; }
        public string customerName { get; set; }
        public string contactNumber { get; set; }
        public string location { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public DateTime dateMade { get; set; }
        public DateTime dateExpected { get; set; }

        public OrderModel(int orderId, int vendorId, string customerName, string contactNumber,
            string location, string status, string description, DateTime dateMade, DateTime dateExpected)
        {
            this.orderId = orderId;
            this.vendorId = vendorId;
            this.customerName = customerName;
            this.contactNumber = contactNumber;
            this.location = location;
            this.status = status;
            this.description = description;
            this.dateMade = dateMade;
            this.dateExpected = dateExpected;
        }

        public OrderModel()
        {

        }
    }
}