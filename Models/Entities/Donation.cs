using BloodHeroA.Models.Enums;

namespace BloodHeroA.Models.Entities
{
    public class Donation
    {

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Donor Donor { get; set; } = default!;
        public Guid DonorId { get; set; }
        public DonorOrganization? DonorOrganization { get; set; }
        public Guid? DonorOrganizationId { get; set; }
        public BloodTestResult? BloodTestResult { get; set; }
        public Guid? BloodTestResultId { get; set; }
        public BankingOrganization BankingOrganization { get; set; } = default!;
        public Guid BankingOrganizationId { get; set; }
        public BloodStorage? BloodStorage { get; set; } = default!;
        public Guid? BloodStorageId { get; set; }
        public int UnitsDonated { get; set; }
        public bool IsSuccessful { get; set; } = false;
        public string DonationRemark { get; set; } = default!;
        public BloodGroup DonatedBloodType { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsTested { get; set; } = false;
        public bool IsStored { get; set; } = false;
        public bool IsHealthy { get; set; } = false;

    }
}
