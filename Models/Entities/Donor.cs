using BloodHeroA.Models.Enums;

namespace BloodHeroA.Models.Entities
{
    public class Donor
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public BloodGroup BloodGroup { get; set; }
        public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
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
        public DateTime LastDonationDate { get; set; }
        public DateTime NextDueDonationDate { get; set; } = DateTime.UtcNow;
        public int TotalDonations { get; set; }
        public Role Role { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAvailable => NextDueDonationDate <= DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DonorOrganization? DonorOrganization { get; set; }
        public Guid? DonorOrganizationId { get; set; }
        //public BankingOrganization BankingOrganization { get; set; } = default!;
        //public Guid BankingOrganizationId { get; set; }
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    }
}

// public BankingOrganization BankingOrganization { get; set; } = default!;
//public Guid BankingOrganizationId { get; set; }
//public BloodTestResult? BloodTestResult { get; set; } = default!;
//public Guid? BloodTestResultId { get; set; }
//public BloodStorage? BloodStorage { get; set; } = default!;
//public Guid? BloodStorageId { get; set; }
//public bool IsApproved { get; set; } = false;