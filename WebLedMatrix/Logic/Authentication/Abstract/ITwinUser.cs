using System.ComponentModel.DataAnnotations;

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
    }
}