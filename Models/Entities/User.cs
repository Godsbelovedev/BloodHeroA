using BloodHeroA.Models.Enums;
using System.Data;

namespace BloodHeroA.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = default!;
        public string HashPassWord { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public RecipientOrganization? RecipientOrganization { get; set; }
        public Guid? RecipientOrganizationId { get; set; }
        public Donor? Donor { get; set; }
        public Guid? DonorId { get; set; }
        public DonorOrganization? DonorOrganization { get; set; }
        public Guid? DonorOrganizationId { get; set; }
        public BankingOrganization? BankingOrganization { get; set; }
        public Guid? BankingOrganizationId { get; set; }
        public Role Role { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsAvailable { get; set; } = true;
        public ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
        public ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
