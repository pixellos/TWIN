using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLedMatrix.Authentication.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebLedMatrix.Authentication.Infrastructure
{
    public class UserIdentityDbContext : IdentityDbContext<User>
    {
        public UserIdentityDbContext() : base("Slave/IdentityDb"){}

        static UserIdentityDbContext()
        {
            System.Data.Entity.Database.SetInitializer<UserIdentityDbContext>(new UserIdentityDbInitialize());
        }

        
        public static UserIdentityDbContext Create()
        {
            return new UserIdentityDbContext();
        }
    }
}
