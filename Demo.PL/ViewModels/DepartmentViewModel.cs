using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        [MaxLength(30, ErrorMessage = "Name must be less than 29 letters"), MinLength(2, ErrorMessage = "Name must be greater than 2 letters.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "*")]
        [MaxLength(30, ErrorMessage = "Name must be less than 29 letters"), MinLength(2, ErrorMessage = "Name must be greater than 2 letters.")]
        public string DeptManager { get; set; }
    }
}
