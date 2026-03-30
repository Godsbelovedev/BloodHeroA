using BloodHeroA.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodHeroA.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public Role Role { get; set; }
        public bool IsDeletd { get; set; } = false;
        public string Password { get; set; } = default!;
        public string FullName { get; set; } = default!;
    }
    public class PasswordUpdateModel
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = default!;

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "The new password must be at least {2} characters long.")]
        public string NewPassword { get; set; } = default!;

        [Required(ErrorMessage = "Please confirm your new password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation do not match.")]
        public string ConfirmPassword { get; set; } = default!;
    }
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string PassWord { get; set; } = default!;
    }
}
