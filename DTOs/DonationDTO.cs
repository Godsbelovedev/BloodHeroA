using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodHeroA.DTOs
{
        public class DonationDTO
        {
            [Required]
            public Guid DonorId { get; set; }

            [Required]
            public string DonationRemark { get; set; } = default!;
            [Required]
            [Display(Name = "Was Donation Successful?")]
            public bool IsSuccessful { get; set; }
            public Guid? DonorOrganizationId { get; set; }
    }
   
    public class DonationResponseDto
    {
        public Guid Id { get; set; }
        public DateTime DonationDate { get; set; }
        public int UnitsDonated { get; set; }
        public string DonorName { get; set; } = default!;
        public string DonorOrganizationName { get; set; } = default!;
        public string BankingOrganizationName { get; set; } = default!;
        public BloodGroup BloodGroup { get; set; } = default!;
        public bool IsTested { get; set; }
    }
    public class DonationUpdateDTO
    {
        [Required]
        public Guid DonationId { get; set; }
        [Required]
        public int UnitsDonated { get; set; }
        [Required]
        public BloodGroup BloodGroup { get; set; } = default!;
    }
}

