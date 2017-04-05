using Microsoft.AspNet.Identity;
using System.Collections;
using System;
using System.Web.Mvc;

namespace WebLedMatrix.Controllers
{
    public static class IdentityResultExtensions
    {
        public static void FoldMessages(this Controller controller, IdentityResult result, string notSucceededMessage)
        {
            if (!result.Succeeded)
            {
                controller.ModelState.AddModelError("", notSucceededMessage);
                foreach (string error in result.Errors)
                {
                    controller.ModelState.AddModelError("", error);
                }
            }
        }
    }
}