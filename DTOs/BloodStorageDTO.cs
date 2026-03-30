using BloodHeroA.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodHeroA.DTOs
{
    public class BloodStorageDTO
    {
        public Guid DonationId { get; set; }
        public BloodGroup BloodGroup { get; set; }
        //public string BankingOrganizationName { get; set; } = default!;
        //public string DonorOrganizationName { get; set; } = default!;
        //public string DonorName { get; set; } = default!;
        public StorageLocation Location { get; set; }
    }

    public class BloodStorageResponseDto
    {
        public Guid Id { get; set; }
        public BloodGroup BloodGroup { get; set; } = default!;
        public int UnitStored { get; set; }
        public Guid DonationId { get; set; }
        public Guid DonorId { get; set; }
        public string DonorFullName { get; set; } = default!;
        public StorageLocation Location { get; set; }
        public string BankingOrganizationName { get; set; } = default!;
        public string DonorOrganizationName { get; set; } = default!;
        public DateTime DateStored { get; set; }
    }
}


