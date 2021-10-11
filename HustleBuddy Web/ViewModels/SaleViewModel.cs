using HustleBuddy_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.ViewModels
{
    public class SaleViewModel
    {
        public List<Sale> sales { get; set; }
        public List<CreditSale> creditSales { get; set; }

        public SaleViewModel(List<Sale> sales, List<CreditSale> creditSales)
        {
            this.sales = sales;
            this.creditSales = creditSales;
        }

        public SaleViewModel(List<Sale> sales)
        {
            this.sales = sales;
            this.creditSales = null;
        }

        public SaleViewModel()
        {
        }
    }
}
