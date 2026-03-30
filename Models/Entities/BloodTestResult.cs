using BloodHeroA.Models.Enums;

namespace BloodHeroA.Models.Entities
{
    public class BloodTestResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public BloodGroup BloodGroup { get; set; }
        public HealthStatus HIV { get; set; }
        public HealthStatus HepatitisB { get; set; }
        public HealthStatus Syphilis { get; set; }
        public HealthStatus Cancer { get; set; }
        public HealthStatus HeartDisease { get; set; }
        public HealthStatus Hemophilic { get; set; }
        public HealthStatus IVDrugConsumer { get; set; }
        public HealthStatus ChronicDisease { get; set; }
        public HealthStatus SevereLungsDisease { get; set; }
        public HealthStatus Tattoo { get; set; }
        public Donation Donation { get; set; } = default!;
        public Guid DonationId { get; set; }
        public BankingOrganization BankingOrganization { get; set; } = default!;
        public Guid BankingOrganizationId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsHealthy { get; set; }
        public string TestRemark { get; set; } = default!;
    }

}

//public Donor Donor { get; set; } = default!;
//public Guid DonorId { get; set; }
// public DonorOrganization? DonorOrganization { get; set; } = default!;
//public Guid? DonorOrganizationId { get; set; }