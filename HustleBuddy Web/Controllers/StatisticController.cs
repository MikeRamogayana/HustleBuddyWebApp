using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Controllers
{
    public class StatisticController : Controller
    {
        private int vendorId = 1;

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

            return View();
        }
    }
}
