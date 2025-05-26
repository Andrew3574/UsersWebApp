using System.ComponentModel.DataAnnotations;

namespace WebApp_Task4.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "Password must contain at least 1 character")]
        public string Password { get; set; }
    }
}
