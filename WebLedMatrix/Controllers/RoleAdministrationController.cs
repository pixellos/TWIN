using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.Owin;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models;
using WebLedMatrix.Logic.Authentication.Models.Roles;

namespace WebLedMatrix.Controllers
{
    public static class IdentityResultWrapper
    {
        public static void FoldMessages(this System.Web.Mvc.Controller controller, IdentityResult result,string notSucceededMessage)
        {
            if (!result.Succeeded)
            {
                controller.ModelState.AddModelError("",notSucceededMessage);    
            }

            foreach (string error in result.Errors)
            {
                controller.ModelState.AddModelError("",error);
            }
        }
    }

    public class RoleAdministrationController : Controller
    {
        RoleAdministrating RoleAdministrating => new RoleAdministrating(RoleManager, UserManager);

        private UserIdentityManager UserManager => HttpContext.GetOwinContext().GetUserManager<UserIdentityManager>();

        private AppRoleManager RoleManager => HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();

        public ActionResult Index()
        {
            return View(RoleManager.Roles.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await RoleAdministrating.DeleteRole(id);
                return RedirectToAction("Index");

            }
            catch (RoleResultException exception)
            {
                return View("Error", exception.ErrorStrings);
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            return View(await RoleAdministrating.CreateRoleEditModel(id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            if (!ModelState.IsValid)
                return View("Error", new[] {"Model is wrong. Please reload website and try again later."});

            try
            {
                await RoleAdministrating.AddMembers(model.RoleName, model.IdsToAdd);
                await RoleAdministrating.DeleteMembers(model.RoleName, model.IdsToDelete);
                return RedirectToAction("Index");
            }
            catch (RoleResultException exception)
            {
                return View("Error", exception.ErrorStrings);
            }
            catch (RoleNotFoundException)
            {
                return View("Error", new[]  { "Role Not Found" });
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await RoleAdministrating.AddRole(name);
                }
                catch (RoleResultException exception)
                {
                    foreach (string errorString in exception.ErrorStrings)
                    {
                        ModelState.AddModelError("", errorString);
                    }
                    return View(name);
                }
            }
            return View("Index");
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}