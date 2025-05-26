using System.ComponentModel.DataAnnotations;

namespace WebApp_Task4.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "Password must contain at least 1 character")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
