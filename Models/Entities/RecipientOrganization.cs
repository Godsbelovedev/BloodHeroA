using BloodHeroA.Models.Enums;

namespace BloodHeroA.Models.Entities
{
    public class RecipientOrganization
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string OrganizationName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Role Role { get; set; }
        public int TotalRecievedBlood{ get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<DonationRequest> DonationRequests { get; set; } =
        new List<DonationRequest>();
        //public ICollection<BankingOrganization> BankingOrganizations { get; set; } =
        //new List<BankingOrganization>();
        public ICollection<ReleasedBlood> ReleasedBloods { get; set; } =
        new List<ReleasedBlood>();

    }
}

// public ICollection<BankingOrganization> BankingOrganizations { get; set; } =
//new List<BankingOrganization>();