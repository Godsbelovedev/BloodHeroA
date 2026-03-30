using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;

namespace BloodHeroA.Models.Entities
{
    public class BloodStorage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public BloodGroup BloodGroup { get; set; }
        public Donation Donation { get; set; } = default!;
        public Guid DonationId { get; set; }
        public Guid? ReleasedId { get; set; }
        public Guid? DonationRequestId { get; set; }
        public StorageLocation StorageLocation { get; set; }
        public BankingOrganization BankingOrganization { get; set; } = default!;
        public Guid BankingOrganizationId { get; set; }
        public int UnitStored { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired { get; set; }
        public bool ExpiryProcess { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsReleased { get; set; } = false;

    }
}
//public class BloodStorage
//{
//    public Guid Id { get; set; } = Guid.NewGuid();

//    public Guid DonationId { get; set; }
//    public Donation Donation { get; set; } = default!;

//    public Guid BankingOrganizationId { get; set; }
//    public BankingOrganization BankingOrganization { get; set; } = default!;

//    public BloodGroup BloodGroup { get; set; }

//    public int UnitsStored { get; set; }

//    public int UnitsRemaining { get; set; }

//    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//    public bool IsDeleted { get; set; } = false;
//}



//public Donor Donor { get; set; } = default!;
//public Guid DonorId { get; set; }
//public DonorOrganization? DonorOrganization { get; set; }
//public Guid? DonorOrganizationId { get; set; }


//public Guid? BloodInventoryId { get; set; }
//