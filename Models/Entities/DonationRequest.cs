using BloodHeroA.Models.Enums;

namespace BloodHeroA.Models.Entities
{
    public class DonationRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public RecipientOrganization RecipientOrganization { get; set; } = default!;
        public Guid RecipientOrganizationId { get; set; }
        public BankingOrganization BankingOrganization { get; set; } = default!;
        public Guid BankingOrganizationId { get; set; }
        //public ReleasedBlood? ReleasedBlood { get; set; }
        //public Guid? ReleasedBloodId { get; set; }
        public BloodGroup BloodTypeNeeded { get; set; }
        public int UnitsRequested { get; set; }
        public int UnitsSupplied { get; set; }
        public int UnitsRemained { get; set; }
        public string? Note { get; set; }
        public Status RequestStatus { get; set; } = Status.Pending;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ReleasedBlood> ReleasedBloods { get; set; } = new List<ReleasedBlood>();
    }
}
