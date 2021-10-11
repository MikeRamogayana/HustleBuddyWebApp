using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.ViewModels
{
    public class ProductVsSaleVsRevenue
    {
        public string product { get; set; }
        public int sales { get; set; }
        public double revenue { get; set; }

        public ProductVsSaleVsRevenue(string product, int sales, double revenue)
        {
            this.product = product;
            this.sales = sales;
            this.revenue = revenue;
        }

        public ProductVsSaleVsRevenue()
        {
        }
    }
}
