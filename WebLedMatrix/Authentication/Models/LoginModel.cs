using System.ComponentModel.DataAnnotations;

namespace WebLedMatrix.Authentication.Models
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}