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
    public class FinanceController : Controller
    {
        private int vendorId = 1;

        [Route("finance")]
        public IActionResult Index([FromQuery] int month, [FromQuery] int year)
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

            if (month == 0 || year == 0)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            double expenses = 0.00;
            double revenue = 0.00;
            double profit = 0.00;
            DailyExpense dailyExpenses = new DailyExpense(0, 0, DateTime.Now, 0, 0, 0);
            List<Pair<string, double>> productExpenses = new List<Pair<string, double>>();
            MonthlyBudget monthlyBudget = new MonthlyBudget();

            string url = "budget/get/daily/" + vendorId + "/" + month + "/" + year;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                dailyExpenses = JsonSerializer.Deserialize<DailyExpense>(response.Content);
            }

            url = "budget/get/product/" + vendorId + "/" + month + "/" + year;
            response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                productExpenses = JsonSerializer.Deserialize<List<Pair<string, double>>>(response.Content);
            }

            Pair<string, double> rev = new Pair<string, double>("Amount", 0);
            url = "budget/get/revenue/" + vendorId + "/" + month + "/" + year;
            response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                rev = JsonSerializer.Deserialize<Pair<string, double>>(response.Content);
            }

            url = "budget/get/monthly/" + vendorId + "/" + year + "/" + month;
            response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                monthlyBudget = JsonSerializer.Deserialize<MonthlyBudget>(response.Content);
            }

            List<Product> products = new List<Product>();
            url = "product/get/vendorId/" + vendorId;
            response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                products = JsonSerializer.Deserialize<List<Product>>(response.Content);
            }
            ViewBag.products = products;

            List<Pair<int, string>> years = new List<Pair<int, string>>();
            for(int i = 0; i < 5; i++)
            {
                years.Add(new Pair<int, string>(DateTime.Now.Year - i, (DateTime.Now.Year - i).ToString()));
            }
            ViewBag.years = years;

            List<Pair<int, string>> months = new List<Pair<int, string>>() {
                new Pair<int, string>(1, "January"), 
                new Pair<int, string>(2, "February"),
                new Pair<int, string>(3, "March"),
                new Pair<int, string>(4, "April"),
                new Pair<int, string>(5, "May"),
                new Pair<int, string>(6, "June"),
                new Pair<int, string>(7, "July"),
                new Pair<int, string>(8, "August"),
                new Pair<int, string>(9, "September"),
                new Pair<int, string>(10, "October"),
                new Pair<int, string>(11, "November"),
                new Pair<int, string>(12, "December")
            };
            ViewBag.months = months;

            revenue = rev.value;
            expenses = dailyExpenses.transport + dailyExpenses.lunch + dailyExpenses.other + productExpenses.Sum(p => p.value);
            profit = revenue - expenses;

            return View(new FinanceViewModel(expenses, revenue, profit, dailyExpenses, productExpenses, monthlyBudget));
        }

        [HttpGet("budget/create")]
        public IActionResult Create()
        {
            Budget budget = new Budget(0, vendorId, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.ToLocalTime());
            DailyExpenseBudget dailyExpense = new DailyExpenseBudget(0, 0, 0, 0, 0);
            BudgetAddModel budgetAdd = new BudgetAddModel(dailyExpense, budget, "");

            List<Pair<int, string>> months = new List<Pair<int, string>>() {
                new Pair<int, string>(1, "January"),
                new Pair<int, string>(2, "February"),
                new Pair<int, string>(3, "March"),
                new Pair<int, string>(4, "April"),
                new Pair<int, string>(5, "May"),
                new Pair<int, string>(6, "June"),
                new Pair<int, string>(7, "July"),
                new Pair<int, string>(8, "August"),
                new Pair<int, string>(9, "September"),
                new Pair<int, string>(10, "October"),
                new Pair<int, string>(11, "November"),
                new Pair<int, string>(12, "December")
            };
            ViewBag.months = months;

            List<Product> products = new List<Product>()
            {
                new Product("", vendorId, "No Products", "", 0, 0, 0)
            };
            var url = "product/get/vendorId/" + vendorId;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                products = JsonSerializer.Deserialize<List<Product>>(response.Content);
            }
            ViewBag.products = products;

            return PartialView(budgetAdd);
        }

        public IActionResult Add(BudgetAddModel budgetAdd)
        {
            var productsJson = budgetAdd.productsJson.Split(";");
            List<ProductExpenseBudget> productExpenses = new List<ProductExpenseBudget>();
            foreach(var productJson in productsJson)
            {
                if (productJson.Equals(""))
                {
                    continue;
                }
                productExpenses.Add(JsonSerializer.Deserialize<ProductExpenseBudget>(productJson));
            }

            MonthlyBudget monthlyBudget = new MonthlyBudget(budgetAdd.dailyExpenseBudget, productExpenses, budgetAdd.budget);

            string url = "budget/get/monthly/" + vendorId + "/" + budgetAdd.budget.year + "/" + budgetAdd.budget.month;
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                MonthlyBudget currentMonthlyBudget = JsonSerializer.Deserialize<MonthlyBudget>(response.Content);
                currentMonthlyBudget.budget.date = monthlyBudget.budget.date;
                currentMonthlyBudget.dailyExpenseBudget.lunch = monthlyBudget.dailyExpenseBudget.lunch;
                currentMonthlyBudget.dailyExpenseBudget.transport = monthlyBudget.dailyExpenseBudget.transport;
                currentMonthlyBudget.dailyExpenseBudget.other = monthlyBudget.dailyExpenseBudget.other;

                foreach(var productExpense in productExpenses)
                {
                    try
                    {
                        currentMonthlyBudget.productExpenseBudget.Where(b => b.productCode.Equals(productExpense.productCode)).First().amount = productExpense.amount;
                    }
                    catch (Exception)
                    {
                        url = "budget/create/product";
                        productExpense.budgetId = currentMonthlyBudget.budget.budgetId;
                        DatabaseContext.PostRequest(url, productExpense);
                    }
                }

                url = "budget/update";
                response = DatabaseContext.PutRequest(url, currentMonthlyBudget);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Redirect("~/finance?success=update");
                }
                return Redirect("~/finance?error=update");
            }

            url = "budget/create";
            response = DatabaseContext.PostRequest(url, monthlyBudget);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return Redirect("~/finance?success=create");
            }
            return Redirect("~/finance?error=create");
        }
    }
}
