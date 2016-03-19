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

        private UserIdentityManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<UserIdentityManager>();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new[] { "User Not Found" });
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
            var manager = new UserManaging(UserManager,AuthManager);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return PartialView("Greetings", User.Identity.Name);
            }

            if (ModelState.IsValid && manager.TryLogin(loginModel))
            {
                return PartialView("AccessGranted");
            }
            else
            {
                ModelState.AddModelError(String.Empty,"Input incorrect data");
            }
            return PartialView(loginModel);
        }

        [Authorize]
        public ActionResult LogOut()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                                {
                                    FirstName = model.FirstName, 
                                    LastName = model.LastName, 
                                    PhoneNumber = model.TelephoneNumber, 
                                    UserName = model.Name, 
                                    Email = model.Email
                                };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            return View(model);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            User user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string password)
        {
            User user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass = await UserManager.PasswordValidator.ValidateAsync(password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if ((validEmail.Succeeded && validPass == null)
                    || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User Not Found");
            }

            return View(user);
        }
    }
}