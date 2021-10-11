using HustleBuddy_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.ViewModels
{
    public class FinanceViewModel
    {
        public double totalExpenses { get; set; }
        public double totalRevenue { get; set; }
        public double totalProfit { get; set; }
        public DailyExpense dailyExpenses { get; set; }
        public List<Pair<string, double>> productExpenses { get; set; }
        public MonthlyBudget monthlyBudget { get; set; }

        public FinanceViewModel(double totalExpenses, double totalRevenue, double totalProfit, DailyExpense dailyExpenses, List<Pair<string, double>> productExpenses, MonthlyBudget monthlyBudget)
        {
            this.totalExpenses = totalExpenses;
            this.totalRevenue = totalRevenue;
            this.totalProfit = totalProfit;
            this.dailyExpenses = dailyExpenses;
            this.productExpenses = productExpenses;
            this.monthlyBudget = monthlyBudget;
        }

        public FinanceViewModel()
        {
        }
    }
}
