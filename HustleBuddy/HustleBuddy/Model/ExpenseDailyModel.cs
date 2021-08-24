using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOSG_Web_App.Model
{
    public class ExpenseDailyModel
    {
        public int expenseId { get; set; }
        public int stockId { get; set; }
        public DateTime date { get; set; }
        public double transport { get; set; }
        public double lunch { get; set; }
        public double other { get; set; }

        public ExpenseDailyModel(int expenseId, int stockId, DateTime date, double transport, double lunch, double other)
        {
            this.expenseId = expenseId;
            this.stockId = stockId;
            this.date = date;
            this.transport = transport;
            this.lunch = lunch;
            this.other = other;
        }

        public ExpenseDailyModel()
        {

        }
    }
}