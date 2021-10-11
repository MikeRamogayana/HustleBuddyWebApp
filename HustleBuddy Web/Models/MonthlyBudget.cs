using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Models
{
    public class MonthlyBudget
    {
        public DailyExpenseBudget dailyExpenseBudget { get; set; }
        public List<ProductExpenseBudget> productExpenseBudget { get; set; }
        public Budget budget { get; set; }

        public MonthlyBudget(DailyExpenseBudget dailyExpenseBudget, List<ProductExpenseBudget> productExpenseBudget, Budget budget)
        {
            this.dailyExpenseBudget = dailyExpenseBudget;
            this.budget = budget;
            this.productExpenseBudget = productExpenseBudget;

        }

        public MonthlyBudget() { }
    }
}
