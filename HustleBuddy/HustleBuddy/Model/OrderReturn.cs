using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOSG_Web_App.Model
{
    public class OrderReturn
    {
        public int returnId { get; set; }
        public int orderId { get; set; }
        public double cashMade { get; set; }
        public DateTime date { get; set; }

        public OrderReturn(int returnId, int orderId, double cashMade, DateTime date)
        {
            this.returnId = returnId;
            this.orderId = orderId;
            this.cashMade = cashMade;
            this.date = date;
        }

        public OrderReturn()
        {

        }
    }
}