using Microsoft.AspNet.Identity;

namespace WebLedMatrix.Controllers
{
    public static class IdentityResultExtensions
    {
        public static void FoldMessages(this System.Web.Mvc.Controller controller, IdentityResult result,string notSucceededMessage)
        {
            if (!result.Succeeded)
            {
                controller.ModelState.AddModelError("",notSucceededMessage);

                foreach (string error in result.Errors)
                {
                    controller.ModelState.AddModelError("", error);
                }
            }
        }
    }
}