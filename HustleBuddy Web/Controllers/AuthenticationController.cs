using HustleBuddy_Web.Data;
using HustleBuddy_Web.Models;
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
    public class AuthenticationController : Controller
    {
        private int vendorId = 0;

        [Route("authentication")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("authentication/account")]
        public IActionResult Account()
        {
            if (HttpContext.Session != null)
            {
                if (HttpContext.Session.GetInt32("vendorId").HasValue)
                {
                    vendorId = HttpContext.Session.GetInt32("vendorId").Value;
                    string url = "vendor/get/vendorId/" + vendorId;
                    var response = DatabaseContext.GetRequest(url);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Vendor vendor = JsonSerializer.Deserialize<Vendor>(response.Content);
                        return View(vendor);
                    }
                }
            }
            return Redirect("/authentication?location=/");
        }

        [Route("authentication/login")]
        public IActionResult Login([FromQuery] string email, [FromQuery] string password, [FromQuery] string location)
        {
            string url = "vendor/login/" + email + "/" + password;
            var response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Vendor vendor = JsonSerializer.Deserialize<Vendor>(response.Content);
                HttpContext.Session.SetInt32("vendorId", vendor.vendorId);
                HttpContext.Session.SetString("vendorName", vendor.companyName);

                if(location == null)
                {
                    return Redirect("/");
                }
                return Redirect(location);
            }
            return Redirect("~/authentication?error=login&location=" + location);
        }

        public IActionResult Register(Vendor vendor)
        {
            string url = "vendor/register";
            var response = DatabaseContext.PostRequest(url, vendor);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Redirect("~/authentication?success=register");
            }
            return Redirect("~/authentication?error=register");
        }

        [Route("authentication/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/authentication");
        }

        public IActionResult Update(Vendor vendor)
        {
            if (HttpContext.Session != null)
            {
                if (HttpContext.Session.GetInt32("vendorId").HasValue)
                {
                    vendorId = HttpContext.Session.GetInt32("vendorId").Value;
                    if (vendorId == 0)
                    {
                        return Redirect("/authentication?location=/authentication/account");
                    }
                }
                else
                {
                    return Redirect("/authentication?location=/authentication/account");
                }
            }
            else
            {
                return Redirect("/authentication?location=/authentication/account");
            }

            string url = "vendor/update";
            var response = DatabaseContext.PutRequest(url, vendor);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return Redirect("~/authentication/account?success=update");
            }
            return Redirect("~/authentication/account?error=update");
        }

        [Route("authentication/forgot")]
        public IActionResult SendResetCode([FromQuery] string email)
        {
            string url = "vendor/get/email/";
            if (email == null || email == "" || email == "null") 
            {
                url += HttpContext.Session.GetString("resetemail");
            } else
            {
                url += email;
            }
            
            var response = DatabaseContext.GetRequest(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                Vendor vendor = JsonSerializer.Deserialize<Vendor>(response.Content);
                string resetcode = GenerateCode().ToString();
                EmailMessage message = new EmailMessage(0, "autoreply.hustlebuddy@gmail.com", vendor.email, "Password Reset OTP", resetcode);
                url = "/email/send";
                response = DatabaseContext.PostRequest(url, message);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContext.Session.SetString("resetcode", resetcode);
                    HttpContext.Session.SetInt32("resetid", vendor.vendorId);
                    HttpContext.Session.SetString("resetemail", vendor.email);
                    return Redirect("~/authentication?success=forgot");
                }
            }
            return Redirect("~/authentication?error=forgot");
        }

        [Route("authentication/reset")]
        public IActionResult ResetPassowrd([FromQuery] string resetcode, [FromQuery] string password)
        {
            if (resetcode.Equals(HttpContext.Session.GetString("resetcode")))
            {
                string url = "vendor/reset/" + HttpContext.Session.GetInt32("resetid") + "/" + HttpContext.Session.GetString("resetemail") + "/" + password;
                var response = DatabaseContext.PutRequest(url, new Vendor());
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContext.Session.Clear();
                    return Redirect("~/authentication?success=reset");
                }
            }
            return Redirect("~/authentication?error=reset");
        }

        [Route("authentication/change")]
        public IActionResult ChangePassowrd([FromQuery] string currentpassword, [FromQuery] string newpassword)
        {
            string url = "vendor/get/vendorId/" + HttpContext.Session.GetInt32("vendorId");
            var response = DatabaseContext.GetRequest(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Vendor vendor = JsonSerializer.Deserialize<Vendor>(response.Content);
                if (currentpassword.Equals(vendor.password))
                {
                    url = "vendor/reset/" + vendor.vendorId + "/" + vendor.email + "/" + newpassword;
                    response = DatabaseContext.PutRequest(url, new Vendor());
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return Redirect("~/authentication/account?success=change");
                    }
                }
            }
            return Redirect("~/authentication/account?error=change");
        }

        private int GenerateCode()
        {
            Random r = new Random();
            int code = r.Next(10000, 100000);

            return code;
        }
    }
}