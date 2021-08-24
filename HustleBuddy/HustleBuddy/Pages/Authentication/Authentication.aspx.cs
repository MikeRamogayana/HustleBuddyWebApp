using IOSG_REST_API.Models;
using IOSG_Web_App.Controller;
using IOSG_Web_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HustleBuddy.Pages.Authentication
{
    public partial class Authentication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["LoggedIn"] != null)
            {
                Response.Redirect("/Pages/Dashboard/Dashboard.aspx");
            }

            if(Request.QueryString["auth_code"] == null)
            {
                return;
            }

            var code = Request.QueryString["auth_code"];

            if(code.Equals("reset"))
            {
                login.Visible = false;
                register.Visible = false;
                reset.Visible = true;
            } else if (code.Equals("register"))
            {
                login.Visible = false;
                reset.Visible = false;
                register.Visible = true;
            } else
            {
                reset.Visible = false;
                register.Visible = false;
                login.Visible = true;
            }
        }

        // Login
        protected void UserLogin(object sender, EventArgs e)
        {
            string email = username.Value;
            string pass = password.Value;
            string url = String.Format("/vendor/login/" + email + "/" + pass);

            var result = APIController.GetRequest(url);

            if (result != null)
            {
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    VendorModel vendor = APIController.Deserialize<VendorModel>(result.Content);
                    Session["LoggedIn"] = vendor.vendorId;
                    if(Request.QueryString["url"] != null)
                    {
                        Response.Redirect(Request.QueryString["url"]);
                    }
                    Response.Redirect("/Pages/Dashboard/Dashboard.aspx");
                }
                else
                {
                    errorMessage.Visible = true;
                }
            }
        }

        // Reset Password
        protected void BtnResetPassword(object sender, EventArgs e)
        {
            if (code.Text.Equals(Session["ResetCode"].ToString()))
            {
                if (pass.Text.Length <= 8)
                {
                    status.Visible = true;
                    status.InnerHtml = "<p class='col-sm-12 col-form-label text-danger'>Password should at least be 8 characters!!!</p>";
                    return;
                }
                else if (!pass.Text.Equals(confirmpass.Text))
                {
                    status.Visible = true;
                    status.InnerHtml = "<p class='col-sm-12 col-form-label text-danger'>Password does not match!!!</p>";
                    return;
                }
                string url = "/vendor/reset/" + Session["ResetId"].ToString() + "/" + Session["ResetEmail"].ToString() + "/" + pass.Text;
                var result = APIController.PutRequest<VendorModel>(url, null);

                status.Visible = true;
                status.InnerHtml = "<p class='col-sm-12 col-form-label text-success'>" + result.Content + "</p>";
            }
            else
            {
                status.Visible = true;
                status.InnerHtml = "<p class='col-sm-12 col-form-label text-danger'>Invalid One-Time-Pin entered!!!</p>";
            }
        }

        // Send Reset Code
        protected void SendResetCode(object sender, EventArgs e)
        {
            string url = "/vendor/get/email/" + resetemail.Text;
            var result = APIController.GetRequest(url);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                VendorModel vendor = APIController.Deserialize<VendorModel>(result.Content);
                int rand = GenerateCode();
                Session["ResetCode"] = rand;
                Session["ResetEmail"] = vendor.email;
                Session["ResetId"] = vendor.vendorId;

                url = "/email/send";
                EmailMessage em = new EmailMessage(vendor.email, "Password Reset OTP", rand.ToString());
                result = APIController.PostRequest(url, em);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    code.Enabled = true;
                    pass.Enabled = true;
                    confirmpass.Enabled = true;
                    status.Visible = true;
                    status.InnerHtml = "<p class='col-sm-12 col-form-label text-success'>Reset Code sent to: " + vendor.email + " successfully.</p>";
                }
                else
                {
                    status.Visible = true;
                    status.InnerHtml = "<p class='col-sm-12 col-form-label text-danger'>Code could not be sent!</p>";
                }
            }
            else
            {
                status.Visible = true;
                status.InnerHtml = "<p class='col-sm-12 col-form-label text-danger'>Account not found!.</p>";
            }

        }

        // Register
        protected void UserRegistration(object sender, EventArgs e)
        {
            string firstName = firstname.Value;
            string lastName = lastname.Value;
            string Email = email.Value;
            string Password = password.Value;
            string contactNumber = contact.Value;
            string residentialAddress = address.Value;
            string companyName = company.Value;

            VendorModel vendor = new VendorModel(0, firstName, lastName, Email, Password,
                contactNumber, residentialAddress, companyName);
            string url = "/vendor/create";

            var result = APIController.PostRequest(url, vendor);

            if (result != null)
            {
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    Response.Redirect("/Pages/Authentication/Login");
                }
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    errorMessage.InnerText = result.Content;
                    errorMessage.Visible = true;
                }
            }
            else
            {
                errorMessage.InnerText = "Could not connect to remote server!";
                errorMessage.Visible = true;
            }
        }

        // Generate Code
        private int GenerateCode()
        {
            Random r = new Random();
            int code = r.Next(1000, 10000);

            return code;
        }

    }
}