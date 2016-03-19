using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models;
using WebLedMatrix.Logic.Authentication.Models.Roles;

namespace WebLedMatrix.Controllers
{
    public class RoleEditModel 
    {
        public AppRole Role { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<User> NonMembers { get; set; }
    }
}