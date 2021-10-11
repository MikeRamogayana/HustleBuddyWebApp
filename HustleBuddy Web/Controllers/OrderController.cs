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
    public class OrderController : Controller
    {
        private int vendorId = 1;

        [Route("orders")]
        public IActionResult Index([FromQuery] string status, [FromQuery] int page, [FromQuery] string search)
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

            List<Order> orders = null;
            int pageCount = 1;
            string url = "order/get/vendorId/" + vendorId;
            if(search != null)
            {
                url = "order/get/string/" + vendorId + "/" + search;
            }
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                orders = JsonSerializer.Deserialize<List<Order>>(response.Content);
                if(status != null)
                {
                    if(status != "all")
                    {
                        orders = orders.Where(o => o.status.ToLower().Equals(status.ToLower())).ToList();
                    }
                }
                pageCount = GetPageNumber(orders.Count, 10);
                orders = orders.OrderByDescending(o => o.orderId).ToList();
                orders = GetItems(orders, page, 10);
                orders = orders.OrderByDescending(o => o.orderId).ToList();
            }
            ViewBag.pageCount = pageCount;
            return View(orders);
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

        private List<Order> GetItems(List<Order> items, int page, int count)
        {
            try
            {
                return items.GetRange(page * count, count);
            }
            catch (Exception)
            {
                return items.GetRange(page * count, (items.Count % count));
            }
        }

        [HttpGet("order/create")]
        public IActionResult Create()
        {
            Order order = new Order(0, vendorId, "", "", "", "pending", "", DateTime.Now.ToLocalTime(), DateTime.Now.Date.AddDays(2));
            List<Product> products = new List<Product>();
            products.Add(new Product("", 0, "Select Product", "", 0, 0, 0));
            string url = "product/get/vendorId/" + vendorId;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var product_list = JsonSerializer.Deserialize<List<Product>>(response.Content);
                product_list.OrderBy(p => p.productName);
                foreach(var product in product_list)
                {
                    products.Add(product);
                }
            }

            ViewBag.products = products;
            return PartialView(new OrderAddModel(order, ""));
        }

        public IActionResult Add(OrderAddModel orderAddModel)
        {
            int orderId = 0;
            string url = "order/create";
            var response = DatabaseContext.PostRequest(url, orderAddModel.order);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                url = "order/get/recent/" + vendorId + "/" + orderAddModel.order.customerName + "/" + orderAddModel.order.contactNumber;
                response = DatabaseContext.GetRequest(url);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    orderId = JsonSerializer.Deserialize<Order>(response.Content).orderId;
                }
            }
            if (orderId > 0)
            {
                url = "orderedproduct/create";
                var orderedProductsJson = orderAddModel.products_json.Replace("[", "").Replace("]", "").Split(";");
                foreach(var orderedProductJson in orderedProductsJson)
                {
                    if (orderedProductJson.Equals(""))
                    {
                        continue;
                    }
                    var orderedProduct = JsonSerializer.Deserialize<OrderedProduct>(orderedProductJson);
                    orderedProduct.orderId = orderId;
                    DatabaseContext.PostRequest(url, orderedProduct);
                }
                return Redirect("~/orders?success=create");
            }
            return Redirect("~/orders?error=create");
        }

        [HttpGet("order/view/{orderId}")]
        public IActionResult View(int orderId)
        {
            OrderViewModel orderView = null;
            string url = "order/get/orderId/" + orderId;
            var response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var order = JsonSerializer.Deserialize<Order>(response.Content);
                url = "orderedproduct/get/" + order.orderId;
                response = DatabaseContext.GetRequest(url);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var orderedProducts = JsonSerializer.Deserialize<List<OrderedProduct>>(response.Content);
                    List<Product> products = new List<Product>();
                    foreach (var orderedProduct in orderedProducts)
                    {
                        url = "product/get/productCode/" + orderedProduct.productCode;
                        response = DatabaseContext.GetRequest(url);
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            products.Add(JsonSerializer.Deserialize<Product>(response.Content));
                        }
                    }
                    orderView = new OrderViewModel(order, orderedProducts, products);
                }
            }
            return PartialView(orderView);
        }

        [HttpGet("order/edit/{orderId}")]
        public IActionResult Edit(int orderId)
        {
            OrderViewModel orderView = null;
            string url = "order/get/orderId/" + orderId;
            var response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var order = JsonSerializer.Deserialize<Order>(response.Content);
                url = "orderedproduct/get/" + order.orderId;
                response = DatabaseContext.GetRequest(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var orderedProducts = JsonSerializer.Deserialize<List<OrderedProduct>>(response.Content);
                    List<Product> products = new List<Product>();
                    foreach (var orderedProduct in orderedProducts)
                    {
                        url = "product/get/productCode/" + orderedProduct.productCode;
                        response = DatabaseContext.GetRequest(url);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            products.Add(JsonSerializer.Deserialize<Product>(response.Content));
                        }
                    }
                    orderView = new OrderViewModel(order, orderedProducts, products);
                }
            }

            List<Product> view_products = new List<Product>();
            view_products.Add(new Product("", 0, "Select Product", "", 0, 0, 0));
            url = "product/get/vendorId/" + vendorId;
            response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var product_list = JsonSerializer.Deserialize<List<Product>>(response.Content);
                product_list.OrderBy(p => p.productName);
                foreach (var product in product_list)
                {
                    view_products.Add(product);
                }
            }

            ViewBag.products = view_products;

            return PartialView(orderView);
        }

        public IActionResult Update(OrderViewModel orderView)
        {
            string url = "order/update";
            var response = DatabaseContext.PutRequest(url, orderView.order);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Redirect("~/orders?success=update");
            }
            return Redirect("~/orders?error=update");
        }

        [HttpGet("order/delete/{orderId}")]
        public IActionResult Delete(int orderId)
        {
            string url = "order/get/orderId/" + orderId;
            var response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Order order = JsonSerializer.Deserialize<Order>(response.Content);
                return PartialView(order);
            }
            return PartialView();
        }

        public IActionResult Remove(Order order)
        {
            string url = "orderedproduct/delete/orderId/" + order.orderId;
            var response = DatabaseContext.DeleteRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                url = "order/delete/" + order.orderId;
                response = DatabaseContext.DeleteRequest(url);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    return Redirect("~/orders?success=delete");
                }
            }
            return Redirect("~/orders?error=delete");
        }
    }
}
