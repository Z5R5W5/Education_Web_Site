using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{3}", ErrorMessage = "Enter valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        [MinLength(8, ErrorMessage = "Password must be greater than 7 letters"), MaxLength(32, ErrorMessage = "Password must be  than 7 letters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
