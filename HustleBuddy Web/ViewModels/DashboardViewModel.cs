using HustleBuddy_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.ViewModels
{
    public class DashboardViewModel
    {
        public List<ProductVsSaleVsRevenue> productVsSales { get; set; }
        public List<LocationVsSale> locationVsSales { get; set; }
        public int orders { get; set; }
        public int sales { get; set; }
        public int credit { get; set; }

        public DashboardViewModel(List<ProductVsSaleVsRevenue> productVsSales, List<LocationVsSale> locationVsSales, int orders, int sales, int credit)
        {
            this.productVsSales = productVsSales;
            this.locationVsSales = locationVsSales;
            this.orders = orders;
            this.sales = sales;
            this.credit = credit;
        }

        public DashboardViewModel()
        {
        }
    }
}
