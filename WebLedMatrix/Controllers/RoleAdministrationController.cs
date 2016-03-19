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
    public class RoleAdministrationController : Controller
    {
        RoleAdministrating _roleAdministrating => new RoleAdministrating(RoleManager, UserManager);
        // GET: RoleAdministration
        public ActionResult Index()
        {
            return View(RoleManager.Roles.ToList());
        }


        private UserIdentityManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<UserIdentityManager>();
            }
        }
        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            AppRole role = await RoleManager.FindByIdAsync(id);
            if (role == null)
                return View("Error", new string[] { "Role Not Found" });

            IdentityResult result = await RoleManager.DeleteAsync(role);

            if (result.Succeeded)
                return RedirectToAction("Index");

            return View("Error", result.Errors);
        }

        public async Task<ActionResult> Edit(string id)
        {
            return View(await _roleAdministrating.CreateRoleEditModel(id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            if (ModelState.IsValid)
            try
            {
                await _roleAdministrating.AddMembers(model.RoleName, model.IdsToAdd);
                await _roleAdministrating.DeleteMembers(model.RoleName, model.IdsToDelete);
            }
            catch (RoleResultException exception)
            {
                return View("Error", exception.ErrorStrings);
            }
            catch (RoleNotFoundException)
            {
                return View("Error", new string[] { "Role Not Found" });
            }

            return View("Index");
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
                    await _roleAdministrating.AddRole(name);
                }
                catch (RoleResultException exception)
                {
                    foreach (string errorString in exception.ErrorStrings)
                    {
                        ModelState.AddModelError(String.Empty, errorString);
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