using BloodHeroA.Models.Enums;

namespace BloodHeroA.Models.Entities
{
    public class BankingOrganization
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public User? User { get; set; }
        public Guid UserId { get; set; } = default!;
        public string OrganizationName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TotalDonations { get; set; }
        public int TotalStorage { get; set; }
        public int TotalExpired { get; set; }
        public int TotalRelease { get; set; }
        public Role Role { get; set; }
        public bool IsDeleted { get; set; } = false;
        //public bool IsApproved { get; set; } = false;
        //public ICollection<Donor> Donors { get; set; } = new List<Donor>();
        public ICollection<BloodTestResult> BloodTestResults { get; set; } = new List<BloodTestResult>();
        //public ICollection<RecipientOrganization> RecipientOrganizations { get; set; } 
        //= new List<RecipientOrganization>();
       // public ICollection<DonorOrganization> DonorOrganizations { get; set; }
       //= new List<DonorOrganization>();
        public ICollection<BloodStorage> BloodStorages { get; set; }= new List<BloodStorage>();
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
        public ICollection<ReleasedBlood> ReleasedBloods { get; set; } = new List<ReleasedBlood>();
        public ICollection<DonationRequest> DonationRequests { get; set; } = new List<DonationRequest>();
    }

    //public string RegistrationNumber { get; set; } = default!;
}
