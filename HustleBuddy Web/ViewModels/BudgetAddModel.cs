using HustleBuddy_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.ViewModels
{
    public class BudgetAddModel
    {
        public DailyExpenseBudget dailyExpenseBudget { get; set; }
        public Budget budget { get; set; }
        public string productsJson { get; set; }

        public BudgetAddModel(DailyExpenseBudget dailyExpenseBudget, Budget budget, string productsJson)
        {
            this.dailyExpenseBudget = dailyExpenseBudget;
            this.budget = budget;
            this.productsJson = productsJson;
        }

        public BudgetAddModel()
        {
        }
    }
}
