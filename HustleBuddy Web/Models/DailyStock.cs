using System;

namespace HustleBuddy_Web.Models
{
    public class DailyStock
    {
        public int stockId { get; set; }
        public int vendorId { get; set; }
        public DateTime date { get; set; }
        public string location { get; set; }
        public double cost { get; set; }
        public double expectedReturn { get; set; }

        public DailyStock(int stockId, int vendorId, DateTime date, string location, double cost, double expectedReturn)
        {
            this.stockId = stockId;
            this.vendorId = vendorId;
            this.date = date;
            this.location = location;
            this.cost = cost;
            this.expectedReturn = expectedReturn;
        }

        public DailyStock()
        {
        }
    }
}
