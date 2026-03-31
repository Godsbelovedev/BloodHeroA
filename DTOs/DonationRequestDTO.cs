using BloodHeroA.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodHeroA.DTOs
{
        public class DonationRequestDto
        {
            [Required]
            public BloodGroup BloodTypeNeeded { get; set; }

            [Required]
            public int UnitsRequested { get; set; }
            public string? Note { get; set; }
        }
    
        public class DonationRequestResponseDto
        {
            public Guid Id { get; set; }
            public Guid GeneralId { get; set; }
            public string RecipientOrganizationName { get; set; } = default!;
            public string BankigOrganizationName { get; set; } = default!;
            public BloodGroup BloodType { get; set; }
            public int UnitsRequested { get; set; }
            public DateTime RequestDate { get; set; }
            public string? Note { get; set; }
            public Status Status { get; set; }
    }
}


