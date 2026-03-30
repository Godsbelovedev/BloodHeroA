using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodHeroA.DTOs
{
    public class DonorRequestDto
    {
        [Required] public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }
        [Required] public string LastName { get; set; } = default!;

        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = default!;

        [Required, Phone, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = default!;

        public BloodGroup BloodGroup { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public StateOfOrigin StateOfOrigin { get; set; }
        //public HealthStatus HIV { get; set; }
        //public HealthStatus HepatitisB { get; set; }
        //public HealthStatus Syphilis { get; set; }
        //public HealthStatus Cancer { get; set; }
        //public HealthStatus HeartDisease { get; set; }
        //public HealthStatus Hemophilic { get; set; }
        public HealthStatus IVDrugConsumer { get; set; }
        //public HealthStatus ChronicDisease { get; set; }
        //public HealthStatus SevereLungsDisease { get; set; }
        public HealthStatus Tattoo { get; set; }
        //public BankingOrganization BankingOrganization { get; set; } = default!;
        //public Guid BankingOrganizationId { get; set; }
    }
    public class DonorResponseDto
    {
        public Guid Id { get; set; }
        public Guid? donorOrganizationId { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public BloodGroup BloodGroup { get; set; } = default!;
        public Gender Gender { get; set; } = default!;
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastDonationDate { get; set; }
        public string? DonorOrganizationName { get; set; }
        public int TotalDonations { get; set; }
        public DateTime DateOfBirth { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public StateOfOrigin StateOfOrigin { get; set; }
        public HealthStatus? HIV { get; set; }
        public HealthStatus? HepatitisB { get; set; }
        public HealthStatus? Syphilis { get; set; }
        public HealthStatus? Cancer { get; set; }
        public HealthStatus? HeartDisease { get; set; }
        public HealthStatus? Hemophilic { get; set; }
        public HealthStatus IVDrugConsumer { get; set; }
        public HealthStatus? ChronicDisease { get; set; }
        public HealthStatus? SevereLungsDisease { get; set; }
        public HealthStatus Tattoo { get; set; }
        public DateTime RegisterdDate { get; set; }

    }


    public class DonorUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public StateOfOrigin? StateOfOrigin { get; set; } = default!;
    }

}