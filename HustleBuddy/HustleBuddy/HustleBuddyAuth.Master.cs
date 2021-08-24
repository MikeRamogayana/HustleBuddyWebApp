using IOSG_Web_App.Controller;
using IOSG_Web_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HustleBuddy
{
    public partial class HustleBuddyAuth : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null)
            {
                System.Diagnostics.Debug.WriteLine(Request.RawUrl);
                if (Request.QueryString["auth_code"] != null)
                {
                    System.Diagnostics.Debug.WriteLine("Logged In");
                    if (Request.QueryString["auth_code"].Equals("logout"))
                    {
                        Session["LoggedIn"] = null;
                        System.Diagnostics.Debug.WriteLine("Logged out");
                        Response.Redirect("/Pages/Authentication/Authentication.aspx");
                    }
                }

                var vendorId = Session["LoggedIn"];
                if (vendorId != null)
                {
                    vendorId = int.Parse(Session["LoggedIn"].ToString());
                    if (!IsPostBack)
                    {
                        string url = "/vendor/get/vendorId/" + vendorId;
                        var result = APIController.GetRequest(url);
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            VendorModel vendor = APIController.Deserialize<VendorModel>(result.Content);
                            user.InnerText = vendor.firstName + " " + vendor.lastName;
                        }
                        return;
                    }
                }
            }

            if(Request.QueryString["auth_code"] != null)
            {
                if (Request.QueryString["auth_code"].Equals("register"))
                {
                    account.InnerHtml = "<a class='nav-link' style='color: black;' href='/Pages/Authentication/Authentication.aspx'>Login</a>";
                }
            }
            else
            {
                account.InnerHtml = "<a class='nav-link' style='color: black;' href='/Pages/Authentication/Authentication.aspx?auth_code=register'>Register</a>";
            }

        }
    }
}