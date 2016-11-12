using System.ComponentModel.DataAnnotations;
using WebLedMatrix.Logic.Authentication.Models.Roles;

namespace WebLedMatrix.Logic.Authentication.Abstract
{
    public interface ITwinUser
    {
        [Required]
        string FirstName { get; set; }

        [Required]
        string LastName { get; set; }

        [Required]
        string TelephoneNumber { get; set; }

        [Required]
        string About { get; set; }
    }
}