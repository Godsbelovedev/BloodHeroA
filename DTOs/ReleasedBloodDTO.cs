using BloodHeroA.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodHeroA.DTOs
{   
        public class ReleasedBloodRequestDto
        {
            public Guid DonationRequestId { get; set; }
            public Guid BloodStorageId { get; set; }
            //public BloodGroup BloodTypeReleased { get; set; }
            public int UnitToRelease { get; set; }
            //[Required]
            //public string EssenceOfRelease { get; set; } = default!;
        }
  
        public class ReleasedBloodResponseDto
        {
            public Guid Id { get; set; }
            public string BankingOrganizationName { get; set; } = default!;
            public string? RecipientOrganizationName { get; set; }
            public Guid? DonationRequestId { get; set; }
            public BloodGroup BloodType { get; set; } = default!;
            public int Quantity { get; set; }
           // public string Essence { get; set; } = default!;
            public DateTime ReleaseDate { get; set; }
            public Status Status { get; set; }
        }
    
}

