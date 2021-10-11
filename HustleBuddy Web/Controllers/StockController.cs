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
    public class StockController : Controller
    {
        private int vendorId = 1;

        [Route("stocks")]
        public IActionResult Index()
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

            List<StockViewModel> stockViews = new List<StockViewModel>();
            string url = "daily/get/date2/" + vendorId + "/" + DateTime.Now.Date.AddDays(-30).ToString("MM-dd-yyyy HH:mm:ss tt") + "/" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss tt");
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                List<DailyStock> dailyStocks = JsonSerializer.Deserialize<List<DailyStock>>(response.Content);
                dailyStocks = dailyStocks.OrderByDescending(d => d.date).ToList();
                foreach(var dailyStock in dailyStocks)
                {
                    DailyExpense dailyExpense = null;
                    url = "expense/get/stockId/" + dailyStock.stockId;
                    response = DatabaseContext.GetRequest(url);
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        dailyExpense = JsonSerializer.Deserialize<DailyExpense>(response.Content);
                    }

                    List<TradingStock> tradingStocks = null;
                    url = "trading/get/stockId/" + dailyStock.stockId;
                    response = DatabaseContext.GetRequest(url);
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        tradingStocks = JsonSerializer.Deserialize<List<TradingStock>>(response.Content);
                    }

                    stockViews.Add(new StockViewModel(dailyStock, dailyExpense, tradingStocks));
                }
            }

            url = "product/get/vendorId/" + vendorId;
            response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.products = JsonSerializer.Deserialize<List<Product>>(response.Content);
            }

            return View(stockViews.Take(7));
        }

        [HttpGet("stock/create")]
        public IActionResult Create()
        {
            DateTime date = DateTime.Now.ToLocalTime();
            DailyStock dailyStock = new DailyStock(0, vendorId, date, "", 0, 0);
            DailyExpense dailyExpense = new DailyExpense(0, 0, date, 0, 0, 0);
            string product_json = "";

            StockAddModel stockAdd = new StockAddModel(dailyStock, dailyExpense, product_json);

            List<Product> products = new List<Product>();
            products.Add(new Product("", 0, "Select Product", "", 0, 0, 0));
            string url = "product/get/vendorId/" + vendorId;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var product_list = JsonSerializer.Deserialize<List<Product>>(response.Content);
                product_list = product_list.OrderBy(p => p.productName).ToList();
                foreach(var product in product_list)
                {
                    products.Add(product);
                }
            }
            ViewBag.products = products;

            List<Location> locations = new List<Location>();
            locations.Add(new Location(0, 0, "Select Location", ""));
            url = "location/get/vendorId/" + vendorId;
            response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var location_list = JsonSerializer.Deserialize<List<Location>>(response.Content);
                location_list = location_list.OrderBy(l => l.location).ToList();
                foreach(var location in location_list)
                {
                    locations.Add(location);
                }
            }
            ViewBag.locations = locations;


            var sales = new List<Sale>();
            url = "sale/get/vendorId/" + vendorId;
            response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                sales = JsonSerializer.Deserialize<List<Sale>>(response.Content);
            }

            var topLocation = from sale in sales
                            group sale by sale.location into topLocations
                            select new
                            {
                                Location = topLocations.Key,
                                TotalSales = topLocations.Sum(s => s.quantity)
                            };
            topLocation = topLocation.OrderByDescending(o => o.TotalSales).Take(5);

            ViewBag.topLocation = topLocation.First().Location;

            var topProduct = from sale in sales
                             group sale by sale.productCode into topProducts
                             select new
                             {
                                 Code = topProducts.Key,
                                 TotalSales = topProducts.Sum(s => s.quantity)
                             };
            topProduct = topProduct.OrderByDescending(o => o.TotalSales).Take(5);

            ViewBag.topProduct = topProduct.First().Code;

            return PartialView(stockAdd);
        }

        public IActionResult Add(StockAddModel stockAdd)
        {
            string url = "daily/create";
            var response = DatabaseContext.PostRequest(url, stockAdd.dailyStock);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                url = "daily/get/date/" + vendorId + "/" + DateTime.Now.ToLocalTime().ToString("MM-dd-yyyy HH:mm:ss tt");
                response = DatabaseContext.GetRequest(url);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var dailyStock = JsonSerializer.Deserialize<DailyExpense>(response.Content);
                    DailyExpense dailyExpense = stockAdd.dailyExpense;
                    dailyExpense.stockId = dailyStock.stockId;

                    url = "expense/create";
                    response = DatabaseContext.PostRequest(url, dailyExpense);
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        var tradingStocksJson = stockAdd.product_json.Replace("[", "").Replace("]", "").Split(';');
                        foreach (var tradingStockJson in tradingStocksJson)
                        {
                            if (tradingStockJson.Equals(""))
                            {
                                continue;
                            }
                            TradingStock tradingStock = JsonSerializer.Deserialize<TradingStock>(tradingStockJson);
                            tradingStock.stockId = dailyStock.stockId;
                            url = "trading/create";
                            DatabaseContext.PostRequest(url, tradingStock);
                        }
                        return Redirect("~/stocks?success=create");
                    }
                }
            }

            return Redirect("~/stocks?error=create");
        }

        [HttpGet("stock/create/trading/{stockId}")]
        public IActionResult CreateTrading(int stockId)
        {
            List<Product> products = new List<Product>();
            products.Add(new Product("", vendorId, "Select Product", "", 0, 0, 0));
            string url = "product/get/vendorId/" + vendorId;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var product_list = JsonSerializer.Deserialize<List<Product>>(response.Content);
                product_list = product_list.OrderBy(p => p.productName).ToList();
                foreach(var product in product_list)
                {
                    products.Add(product);
                }
            }

            ViewBag.products = products;

            return PartialView(new TradingStock("", stockId, 0, 0));
        }

        public IActionResult AddTrading(TradingStock tradingStock)
        {
            string url = "trading/create";
            var response = DatabaseContext.PostRequest(url, tradingStock);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return Redirect("~/stocks?success=create");
            }
            return Redirect("~/stocks?error=create");
        }

        [HttpGet("stock/edit/trading/{stockId}/{productCode}")]
        public IActionResult EditTrading(int stockId, string productCode)
        {
            string url = "trading/get/productCode/" + stockId + "/" + productCode;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                TradingStock tradingStock = JsonSerializer.Deserialize<TradingStock>(response.Content);
                return PartialView(tradingStock);
            }

            return PartialView();
        }

        public IActionResult UpdateTrading(TradingStock tradingStock)
        {
            string url = "trading/update";
            var response = DatabaseContext.PutRequest(url, tradingStock);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return Redirect("~/stocks?success=update");
            }
            return Redirect("~/stocks?error=update");
        }

        [HttpGet("stock/delete/trading/{stockId}/{productCode}")]
        public IActionResult DeleteTrading(int stockId, string productCode)
        {
            TradingStock tradingStock = null;
            string productName = "PRODUCT";
            string url = "trading/get/productCode/" + stockId + "/" + productCode;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                tradingStock = JsonSerializer.Deserialize<TradingStock>(response.Content);
            }

            url = "product/get/productCode/" + productCode;
            response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                productName = JsonSerializer.Deserialize<Product>(response.Content).productName;
            }

            ViewBag.productName = productName;

            return PartialView(tradingStock);
        }

        public IActionResult RemoveTrading(TradingStock tradingStock)
        {
            string url = "trading/delete/" + tradingStock.stockId + "/" + tradingStock.productCode;
            var response = DatabaseContext.DeleteRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return Redirect("~/stocks?success=delete");
            }
            return Redirect("~/stocks?error=delete");
        }
    }
}
