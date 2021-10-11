using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Models
{
    public class DailyExpenseBudget
    {
        public int dailyExpenseBudgetId { get; set; }
        public int budgetId { get; set; }
        public double lunch { get; set; }
        public double transport { get; set; }
        public double other { get; set; }

        public DailyExpenseBudget(int dailyExpenseBudgetId, int budgteId, double lunch, double transport, double other)
        {
            this.dailyExpenseBudgetId = dailyExpenseBudgetId;
            this.budgetId = budgetId;
            this.lunch = lunch;
            this.transport = transport;
            this.other = other;
        }

        public DailyExpenseBudget() { }
    }
}
