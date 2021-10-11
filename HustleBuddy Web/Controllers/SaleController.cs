using HustleBuddy_Web.Data;
using HustleBuddy_Web.Models;
using HustleBuddy_Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Controllers
{
    public class SaleController : Controller
    {
        private int vendorId = 1;

        [Route("sales")]
        public IActionResult Index([FromQuery] string period, [FromQuery] string product, [FromQuery] string location, [FromQuery] int page, [FromQuery] int cashOrCredit)
        {
            if (HttpContext.Session != null)
            {
                if (HttpContext.Session.GetInt32("vendorId").HasValue)
                {
                    vendorId = HttpContext.Session.GetInt32("vendorId").Value;
                    if (vendorId == 0)
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

            if (period != null && product != null && location != null)
            {
                return SaleData(period, product, location, page, cashOrCredit);
            }
            else if(period != null && product != null && location == null)
            {
                return SaleData(period, product, "All Locations", page, cashOrCredit);
            }
            else if(period != null && product == null && location != null)
            {
                return SaleData(period, "ALL", location, page, cashOrCredit);
            }
            else if(period == null && product != null && location != null)
            {
                return SaleData("all", product, location, page, cashOrCredit);
            }
            else if(period != null && product == null && location == null)
            {
                return SaleData(period, "ALL", "All Locations", page, cashOrCredit);
            }
            else if(period == null && product == null && location != null)
            {
                return SaleData("all","ALL", location, page, cashOrCredit);
            }
            else if(period == null && product != null && location == null)
            {
                return SaleData("all", product, "All Locations", page, cashOrCredit);
            }
            else
            {
                return SaleData("all", "ALL", "All Locations", page, cashOrCredit);
            }
        }

        private IActionResult SaleData(string time, string productCode, string saleLocation, int page, int cashOrCredit)
        {
            string url = "sale/get/vendorId/" + vendorId;
            string to = DateTime.Now.ToLocalTime().ToString("MM-dd-yyyy HH:mm:ss tt");

            if (time.Equals("daily"))
            {
                url = "sale/get/date2/" + vendorId + "/" + DateTime.Now.ToLocalTime().Date.ToString("MM-dd-yyyy HH:mm:ss tt") + "/" + to;
            } else if (time.Equals("weekly"))
            {
                url = "sale/get/date2/" + vendorId + "/" + DateTime.Now.ToLocalTime().Date.AddDays(-7).ToString("MM-dd-yyyy HH:mm:ss tt") + "/" + to;
            }
            else if (time.Equals("monthly"))
            {
                url = "sale/get/date2/" + vendorId + "/" + DateTime.Now.ToLocalTime().Date.AddMonths(-1).ToString("MM-dd-yyyy HH:mm:ss tt") + "/" + to;
            }

            int pageCount = 1;
            SaleViewModel saleView = new SaleViewModel();
            List<Sale> sales = null;
            List<CreditSale> creditSales = null;

            var response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                sales = JsonSerializer.Deserialize<List<Sale>>(response.Content);
                if (!productCode.Equals("ALL"))
                {
                    sales = sales.Where(s => s.productCode.Equals(productCode)).ToList();
                }

                if(!saleLocation.Equals("All Locations"))
                {
                    sales = sales.Where(s => s.location.Equals(saleLocation)).ToList();
                }

                sales = sales.OrderByDescending(s => s.date).ToList();

                if (cashOrCredit == 0)
                {
                    sales = sales.Where(s => s.cashOrCredit.ToLower().Equals("cash")).ToList();
                    pageCount = GetPageNumber(sales.Count, 10);
                    sales = GetItems(sales, page, 10);
                }

                if (cashOrCredit == 1)
                {
                    sales = sales.Where(s => s.cashOrCredit.ToLower().Equals("credit")).ToList();
                    pageCount = GetPageNumber(sales.Count, 10);
                    sales = GetItems(sales, page, 10);
                    creditSales = new List<CreditSale>();
                    foreach(var sale in sales)
                    {
                        url = "credit/get/saleId/" + sale.saleId;
                        response = DatabaseContext.GetRequest(url);
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            creditSales.Add(JsonSerializer.Deserialize<CreditSale>(response.Content));
                        }
                    }
                }

                saleView.sales = sales;
                saleView.creditSales = creditSales;
            }

            ViewBag.pageCount = pageCount;

            List<Product> products = new List<Product>();
            products.Add(new Product("ALL", vendorId, "All Products", "", 0, 0, 0));
            url = "product/get/vendorId/" + vendorId;
            response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var product_list = JsonSerializer.Deserialize<List<Product>>(response.Content);
                product_list = product_list.OrderBy(p => p.productName).ToList();
                foreach (var product in product_list)
                {
                    products.Add(product);
                }
            }
            ViewBag.products = products;

            List<Location> locations = new List<Location>();
            locations.Add(new Location(0, vendorId, "All Locations", ""));
            url = "location/get/vendorId/" + vendorId;
            response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var location_list = JsonSerializer.Deserialize<List<Location>>(response.Content);
                location_list = location_list.OrderBy(l => l.location).ToList();
                foreach (var location in location_list)
                {
                    locations.Add(location);
                }
            }
            ViewBag.locations = locations;

            return View(saleView);
        }

        private int GetPageNumber(int value, int count)
        {
            int pages = value / count;
            if (value % count > 0)
            {
                pages++;
            }
            if (pages <= 0)
            {
                pages = 1;
            }
            return pages;
        }

        private List<Sale> GetItems(List<Sale> sales, int page, int count)
        {
            try
            {
                return sales.GetRange(page * count, count);
            }
            catch (Exception)
            {
                return sales.GetRange(page * count, (sales.Count % count));
            }
        }

        [HttpGet("sale/edit/credit/{saleId}")]
        public IActionResult Edit(int saleId)
        {
            CreditViewModel creditView = null;
            string url = "sale/get/saleId/" + saleId;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                Sale sale = JsonSerializer.Deserialize<Sale>(response.Content);
                url = "product/get/productCode/" + sale.productCode;
                response = DatabaseContext.GetRequest(url);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    Product product = JsonSerializer.Deserialize<Product>(response.Content);
                    url = "credit/get/saleId/" + saleId;
                    response = DatabaseContext.GetRequest(url);
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        CreditSale creditSale = JsonSerializer.Deserialize<CreditSale>(response.Content);
                        creditView = new CreditViewModel(product, sale, creditSale);
                    }
                }
            }

            return PartialView(creditView);
        }

        public IActionResult Update(CreditViewModel creditView)
        {
            string url = "credit/get/saleId/" + creditView.creditSale.saleId;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                CreditSale creditSale = JsonSerializer.Deserialize<CreditSale>(response.Content);
                creditSale.paidAmount += creditView.creditSale.paidAmount;
                creditSale.payDate = DateTime.Now.ToLocalTime();
                if(creditSale.paidAmount >= creditSale.creditAmount)
                {
                    creditSale.status = "paid";
                }

                url = "credit/update";
                response = DatabaseContext.PutRequest(url, creditSale);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    return Redirect("~/sales?success=update");
                }
            }
            return Redirect("~/sales?error=update");
        }
    }
}