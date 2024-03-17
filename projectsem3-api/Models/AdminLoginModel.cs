using System.ComponentModel.DataAnnotations;

namespace projectsem3_api.Models
{
    public class AdminLoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
