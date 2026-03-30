using BloodHeroA.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BloodHeroA.DTOs
{
    public class BankingOrganizationDTO
    {
        [Required, Display(Name = "Blood Bank Name")]
        public string OrganizationName { get; set; } = default!;

        [Required]
        public string Address { get; set; } = default!;

        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        [Required, Phone]
        public string PhoneNumber { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = default!;
    }

    public class BankingOrganizationResponseDto
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Address { get; set; } = default!;
        public int TotalDonations { get; set; }
        public int TotalStorage { get; set; }
        public int TotalRelease { get; set; }
        public int TotalExpired { get; set; }
        public int Available => TotalStorage - (TotalRelease + TotalExpired);
        public DateTime CreatedAt { get; set; }
    }

    public class BankingOrganizationUpdateDto
    {
        public string? OrganizationName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
