
using System.ComponentModel.DataAnnotations;

namespace BloodHeroA.DTOs
{
    public class RecipientOrganizationDTO
    {
        [Required(ErrorMessage = "Organization name is required")]
        public string OrganizationName { get; set; } = default!;

        [Required]
        public string Address { get; set; } = default!;

        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; } = default!;

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = default!;

        [Required, Phone]
        public string PhoneNumber { get; set; } = default!;
    }
   
        public class RecipientResponseDto
        {
            public Guid Id { get; set; }
            public string OrganizationName { get; set; } = default!;
            public string Address { get; set; } = default!;
            public string Email { get; set; } = default!;
            public string PhoneNumber { get; set; } = default!;
            public DateTime RegisteredDate { get; set; }
            public int TotalRecivedBlood { get; set; }
            public bool IsDeleted { get; set; }
        }
   
        public class RecipientUpdateDto
        {
            //public Guid Id { get; set; }
            public string OrganizationName { get; set; } = default!;
            public string Address { get; set; } = default!;
            public string PhoneNumber { get; set; } = default!;
        }
  
}

