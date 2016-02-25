using System.ComponentModel.DataAnnotations;
using WebLedMatrix.Logic.Authentication.Abstract;

namespace WebLedMatrix.Logic.Authentication.Models
{
    public class CreateModel : ITwinUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string TelephoneNumber { get; set; }
    }
}