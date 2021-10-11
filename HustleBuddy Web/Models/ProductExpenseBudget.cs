using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Models
{
    public class ProductExpenseBudget
    {
        public int productExpeseBudgetId { get; set; }
        public int vendorId { get; set; }
        public string productCode { get; set; }
        public int budgetId { get; set; }
        public double amount { get; set; }

        public ProductExpenseBudget(int productExpenseBudgetId, int vendorId, string productCode, int budgetId, double amount)
        {
            this.productExpeseBudgetId = productExpeseBudgetId;
            this.vendorId = vendorId;
            this.productCode = productCode;
            this.budgetId = budgetId;
            this.amount = amount;
        }

        public ProductExpenseBudget() { }
    }
}
