using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HustleBuddy
{
    public partial class HustleBuddyMain : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] == null)
            {
                Response.Redirect("/Pages/Authentication/Authentication.aspx?url=" + Request.RawUrl);
            }
        }
    }
}