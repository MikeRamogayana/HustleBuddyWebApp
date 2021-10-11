using HustleBuddy_Web.Data;
using HustleBuddy_Web.Models;
using HustleBuddy_Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Controllers
{
    public class DashboardController : Controller
    {
        private int vendorId;

        public IActionResult Index([FromQuery] string period)
        {
            if (HttpContext.Session != null)
            {
                if (HttpContext.Session.GetInt32("vendorId").HasValue)
                {
                    vendorId = HttpContext.Session.GetInt32("vendorId").Value;
                    if(vendorId == 0)
                    {
                        return Redirect("/authentication?location=" + Request.Path);
                    }
                }
                else
                {
                    return Redirect("/authentication?location=" + Request.Path);
                }
            }
            else
            {
                return Redirect("/authentication?location=" + Request.Path);
            }

            if (period == null)
            {
                return DashboardData("all");
            }
            return DashboardData(period);
        }
        
        public IActionResult DashboardData(string time)
        {
            DateTime from = DateTime.Now.ToLocalTime().Date.AddYears(-1);
            DateTime to = DateTime.Now.ToLocalTime();
            if(time.Equals("daily"))
            {
                from = DateTime.Now.ToLocalTime().Date;
            } 
            else if (time.Equals("weekly"))
            {
                from = DateTime.Now.ToLocalTime().Date.AddDays(-7);
            }
            else if (time.Equals("monthly"))
            {
                from = DateTime.Now.ToLocalTime().Date.AddMonths(-1);
            }

            List<Sale> sales = GetSales(vendorId, from, to);
            int numberOfSales = sales.Where(s => s.cashOrCredit.Equals("cash")).Sum(s => s.quantity);
            int numberOfPendingOrders = GetNumberOfPendingOrders(vendorId);
            int numberOfCreditSales = sales.Where(s => s.cashOrCredit.Equals("credit")).Sum(s => s.quantity);

            List<LocationVsSale> locationVsSales = new List<LocationVsSale>();
            var locations = from sale in sales
                            group sale by sale.location into topLocations
                            select new
                            {
                                Location = topLocations.Key,
                                TotalSales = topLocations.Sum(s => s.quantity)
                            };
            locations = locations.OrderByDescending(o => o.TotalSales).Take(5);
            foreach (var location in locations)
            {
                locationVsSales.Add(new LocationVsSale(location.Location, location.TotalSales));
            }

            List<ProductVsSaleVsRevenue> productVsSales = new List<ProductVsSaleVsRevenue>();
            var products = from sale in sales
                            group sale by sale.productCode into topProducts
                            select new
                            {
                                Code = topProducts.Key,
                                TotalSales = topProducts.Sum(s => s.quantity)
                            };
            products = products.OrderByDescending(o => o.TotalSales).Take(5);
            foreach (var product in products)
            {
                string url = "product/get/productCode/" + product.Code;
                var response = DatabaseContext.GetRequest(url);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var prod = JsonSerializer.Deserialize<Product>(response.Content);
                    productVsSales.Add(new ProductVsSaleVsRevenue(prod.productName, product.TotalSales, product.TotalSales * prod.sellingPrice));
                }
                else
                {
                    productVsSales.Add(new ProductVsSaleVsRevenue(product.Code, product.TotalSales, product.TotalSales));
                }
            }

            DashboardViewModel dashboardView = new DashboardViewModel(productVsSales, locationVsSales, numberOfPendingOrders, numberOfSales, numberOfCreditSales);
            return View("Index", dashboardView);
        }

        private int GetNumberOfPendingOrders(int vendorId)
        {
            int number = 0;
            string url = "order/get/status/" + vendorId + "/pending";
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                number = JsonSerializer.Deserialize<List<Order>>(response.Content).Count;
            }
            return number;
        }

        private List<Sale> GetSales(int vendorId, DateTime from, DateTime to)
        {
            List<Sale> sales = null;
            string url = "sale/get/date2/" + vendorId + "/" + from.ToString("MM-dd-yyyy HH:mm:ss tt") + "/" + to.ToString("MM-dd-yyyy HH:mm:ss tt");
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                sales = JsonSerializer.Deserialize<List<Sale>>(response.Content);
            }
            return sales;
        }
    }
}
