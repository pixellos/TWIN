using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using WebLedMatrix.Logic.Authentication.Infrastructure;

namespace WebLedMatrix.Controllers
{
    public class HomeController : Controller
    {
     
        public ActionResult Index()
        {
            return View(GetData("Index"));
        }

        [Authorize(Roles = "Users")]
        public ActionResult OtherAction()
        {
            return View("Index", GetData("OtherAction"));
        }

        public ActionResult DropDataBase()
        {
            UserIdentityDbContext.Create().Database.Initialize(true);
            return View("Index");
        }

        private Dictionary<string, object> GetData(string actionName)
        {
            var dict = new Dictionary<string, object>
            {
                { "Action", actionName },
                { "User", HttpContext.User.Identity.Name },
                { "Authenticated", HttpContext.User.Identity.IsAuthenticated },
                { "Auth Type", HttpContext.User.Identity.AuthenticationType },
                { "In Users Role", HttpContext.User.IsInRole("Users") }
            };
            return dict;
        }
    }
}
