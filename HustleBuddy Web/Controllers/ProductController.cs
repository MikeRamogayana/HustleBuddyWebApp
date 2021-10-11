using HustleBuddy_Web.Data;
using HustleBuddy_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Controllers
{
    public class ProductController : Controller
    {
        private int vendorId = 1;

        [Route("products")]
        public IActionResult Index([FromQuery] int page)
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

            int pageCount = 1;
            List<Product> products = null;
            string url = "product/get/vendorId/" + vendorId;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                products = JsonSerializer.Deserialize<List<Product>>(response.Content);
                pageCount = GetPageNumber(products.Count, 10);
                products = GetItems(products, page, 10);
            }
            ViewBag.pageCount = pageCount;
            return View(products);
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

        private List<Product> GetItems(List<Product> items, int page, int count)
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

        [HttpGet("product/create")]
        public IActionResult Create()
        {
            DateTime dateTime = DateTime.Now;
            string code = "P" + dateTime.ToString("yy") + dateTime.ToString("MM") + dateTime.ToString("dd") + dateTime.ToString("HH") + dateTime.ToString("mm") + dateTime.ToString("ss") + dateTime.ToString("tt");
            Product product = new Product(code, vendorId, "Product 1", "New Product", 0, 0, 0);
            return PartialView(product);
        }

        public IActionResult Add(Product product)
        {
            string url = "product/create";
            var response = DatabaseContext.PostRequest(url, product);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return Redirect("~/products?success=create");
            }
            return Redirect("~/products?error=create");
        }

        [HttpGet("product/view/{productCode}")]
        public IActionResult View(string productCode)
        {
            string url = "product/get/productCode/" + productCode;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                Product product = JsonSerializer.Deserialize<Product>(response.Content);
                return PartialView(product);
            }
            return PartialView();
        }

        [HttpGet("product/edit/{productCode}")]
        public IActionResult Edit(string productCode)
        {
            string url = "product/get/productCode/" + productCode;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                Product product = JsonSerializer.Deserialize<Product>(response.Content);
                return PartialView(product);
            }
            return PartialView();
        }

        public IActionResult Update(Product product)
        {
            string url = "product/update";
            var response = DatabaseContext.PutRequest(url, product);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return Redirect("~/products?success=update");
            }
            return Redirect("~/products?error=update");
        }

        [HttpGet("product/delete/{productCode}")]
        public IActionResult Delete(string productCode)
        {
            string url = "product/get/productCode/" + productCode;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                Product product = JsonSerializer.Deserialize<Product>(response.Content);
                return PartialView(product);
            }
            return PartialView();
        }

        public IActionResult Remove(Product product)
        {
            string url = "product/delete/" + product.productCode;
            var response = DatabaseContext.DeleteRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return Redirect("~/products?success=delete");
            }
            return Redirect("~/products?error=delete");
        }
    }
}
