using System.ComponentModel.DataAnnotations;

namespace WebLedMatrix.Authentication.Abstract
{
    public interface TwinUser
    {
        [Required]
        string FirstName { get; set; }
        [Required]
        string LastName { get; set; }
        [Required]
        string TelephoneNumber { get; set; }
    }
}