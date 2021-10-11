using System;

namespace HustleBuddy_Web.Models
{
    public class DailyExpense
    {
        public int expenseId { get; set; }
        public int stockId { get; set; }
        public DateTime date { get; set; }
        public double transport { get; set; }
        public double lunch { get; set; }
        public double other { get; set; }

        public DailyExpense(int expenseId, int stockId, DateTime date, double transport, double lunch, double other)
        {
            this.expenseId = expenseId;
            this.stockId = stockId;
            this.date = date;
            this.transport = transport;
            this.lunch = lunch;
            this.other = other;
        }

        public DailyExpense()
        {
        }
    }
}
