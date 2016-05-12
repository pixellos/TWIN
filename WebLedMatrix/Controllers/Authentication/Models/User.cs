using Microsoft.AspNet.Identity.EntityFramework;

namespace WebLedMatrix.Logic.Authentication.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}