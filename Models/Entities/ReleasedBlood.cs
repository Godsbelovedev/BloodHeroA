using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;

namespace BloodHeroA.Models.Entities
{
    public class ReleasedBlood
    {
        public Guid Id { get; set; } = Guid.NewGuid();

            public Guid? DonationRequestId { get; set; }
            public DonationRequest? DonationRequest { get; set; } 
            public Guid BloodStorageId { get; set; }
            public Guid BankingOrganizationId { get; set; }
            public BankingOrganization BankingOrganization { get; set; } = default!;

            public Guid RecipientOrganizationId { get; set; }
            public RecipientOrganization RecipientOrganization { get; set; } = default!;

            public BloodGroup BloodGroup { get; set; }

            public Status Status { get; set; } = Status.Pending;

            public int UnitsReleased { get; set; }

            public string ReasonForRelease { get; set; } = default!;

            public DateTime ReleasedAt { get; set; } = DateTime.UtcNow;

    }
}
//public class ReleasedBlood
//{
//    public Guid Id { get; set; } = Guid.NewGuid();

//    public Guid DonationRequestId { get; set; }
//    public DonationRequest DonationRequest { get; set; } = default!;

//    public Guid BloodStorageId { get; set; }
//    public BloodStorage BloodStorage { get; set; } = default!;

//    public Guid BankingOrganizationId { get; set; }
//    public BankingOrganization BankingOrganization { get; set; } = default!;

//    public Guid RecipientOrganizationId { get; set; }
//    public RecipientOrganization RecipientOrganization { get; set; } = default!;

//    public BloodGroup BloodGroup { get; set; }

//    public int UnitsReleased { get; set; }

//    public string ReasonForRelease { get; set; } = default!;

//    public DateTime ReleasedAt { get; set; } = DateTime.UtcNow;
//}