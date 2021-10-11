using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Models
{
    public class Budget
    {
        public int budgetId { get; set; }
        public int vendorId { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public DateTime date { get; set; }

        public Budget(int budgetId, int vendorId, int year, int month, DateTime date)
        {
            this.budgetId = budgetId;
            this.vendorId = vendorId;
            this.year = year;
            this.month = month;
            this.date = date;

        }

        public Budget() { }
    }
}
