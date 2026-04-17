using BloodHeroA.Models.Enums;

namespace BloodHeroA.Models.Entities
{
    public class BloodInventory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public BloodGroup BloodGroup { get; set; }
        public int StoredUnits { get; set; }
        public int ReleasedUnits { get; set; } = 0;
        public int ExpiredUnits { get; set; } = 0;
        public int UnitsAvailable => Math.Max(0, StoredUnits - ReleasedUnits - ExpiredUnits);
        public Guid? RecipientOrganizationId {get; set;}
        public Guid BankingOrganizationId { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}

