using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NLog;
using WebLedMatrix.Logic.Authentication.Concrete;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models;

namespace WebLedMatrix.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        private UserIdentityManager UserManager => HttpContext.GetOwinContext().GetUserManager<UserIdentityManager>();

        private IAuthenticationManager AuthManager => HttpContext.GetOwinContext().Authentication;

        private DefaultUserValidator Validator => new DefaultUserValidator(UserManager,AuthManager);

        private UserManaging Manager => new UserManaging(Validator,UserManager,AuthManager);

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await Manager.TryDeleteUser(id);
                return RedirectToAction("Index");
            }
            catch (ResultException exception)
            {
                AddErrorsFromResult(exception.ResultErrors);
                return View("Error");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("Greetings", User.Identity.Name);
            }

            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
        public PartialViewResult Login(LoginModel loginModel)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return PartialView("Greetings", User.Identity.Name);

            if (ModelState.IsValid && Manager.TryLogin(loginModel))
                return PartialView("AccessGranted");

            ModelState.AddModelError(string.Empty,"Input incorrect data");

            return PartialView(loginModel);
        }

        [Authorize]
        public ActionResult LogOut()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreationModel model)
        {
            if (!ModelState.IsValid) AddErrorsFromResult(new []{"Something goes wrong - model is invalid, try reloading page"});
            try
            {
                await Manager.TryCreateUser(model);
                return View("Succeded",(object)"Create was successfull");
            }
            catch (ResultException resultException)
            {
                AddErrorsFromResult(resultException.ResultErrors);
            }

            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            User user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return RedirectToAction("Index","Home");

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string password)
        {
            try
            {
                await  Manager.TryUpdateUser(id, email, password);
            }
            catch (ResultException exception)
            {
                AddErrorsFromResult(exception.ResultErrors);
            }

            return View(new User() {Id = id, Email = email});
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private void AddErrorsFromResult(IEnumerable<String> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}