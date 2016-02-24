using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebLedMatrix.Authentication.Infrastructure
{
    public static class IdentityHelper
    {
        public static MvcHtmlString GetUserName(this HtmlHelper html, string id)
        {
            UserIdentityManager manager = HttpContext.Current.GetOwinContext().GetUserManager<UserIdentityManager>();
            return new MvcHtmlString(manager.FindByIdAsync(id).Result.UserName);
        }
    }
}
