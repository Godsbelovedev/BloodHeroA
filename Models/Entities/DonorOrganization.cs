using BloodHeroA.Models.Enums;

namespace BloodHeroA.Models.Entities
{
    public class DonorOrganization
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string OrganizationName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Role Role { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalRegisteredDonors { get; set; }
        public int TotalDonations { get; set; }
        //public bool IsApproved { get; set; } = false;
        //public BankingOrganization BankingOrganization { get; set; } = default!;
        //public Guid BankingOrganizationId { get; set; }
        public ICollection<Donor> Donors { get; set; } = new List<Donor>();
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    }
}
