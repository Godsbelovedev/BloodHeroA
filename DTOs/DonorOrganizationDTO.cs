using BloodHeroA.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BloodHeroA.DTOs
{
        public class DonorOrganizationRequestDto
        {
            [Required, Display(Name = "Organization Name")]
            public string OrganizationName { get; set; } = default!;

            [Required]
            public string Address { get; set; } = default!;

            [Required, EmailAddress]
            public string Email { get; set; } = default!;

            [Required, Phone, Display(Name = "Phone Number")]
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

            [Required(ErrorMessage = "Please select a parent Blood Bank")]
            public Guid BankingOrganizationId { get; set; }
        }

        public class DonorOrganizationResponseDto
        {
            public Guid Id { get; set; }
            public string OrganizationName { get; set; } = default!;
            public string Email { get; set; } = default!;
            public string PhoneNumber { get; set; } = default!;
            public string Address { get; set; } = default!;
            public int TotalRegisteredDonors { get; set; }
            public int TotalDonations { get; set; }
            public DateTime CreatedAt { get; set; }
            public bool IsDeleted { get; set; }
        }
    

        public class DonorOrganizationUpdateDto
        {
           // public Guid Id { get; set; }
            public string? OrganizationName { get; set; }
            public string? Address { get; set; }
            public string? PhoneNumber { get; set; }
        }
    
}

