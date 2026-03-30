using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodHeroA.DTOs
{
    public class BloodTestResultDTO
    {
        public Guid DonationId { get; set; }
        public string BankingOrganizationName { get; set; } = default!;
        public string DonorOrganizationName { get; set; } = default!;
        public string DonorName { get; set; } = default!;
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
        public string TestRemark { get; set; } = default!;
        public bool IsHealthy { get; set; }

    }

    public class BloodTestResultResponseDto
    {
        public Guid Id { get; set; }
        public DateTime TestDate { get; set; }

        public string DonorFullName { get; set; } = default!;
        public BloodGroup BloodGroup { get; set; } = default!;
        public Guid DonationId { get; set; }
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
        public string? TestRemark { get; set; }
        public bool? IsHealthy { get; set; }
    }
    public class BloodTestResultUpdateDTO
    {
        public Guid Id { get; set; }
        public BloodGroup? BloodGroup { get; set; }
        public HealthStatus? HIV { get; set; }
        public HealthStatus? HepatitisB { get; set; }
        public HealthStatus? Syphilis { get; set; }
        public HealthStatus? Cancer { get; set; }
        public HealthStatus? HeartDisease { get; set; }
        public HealthStatus? Hemophilic { get; set; }
        public HealthStatus? IVDrugConsumer { get; set; }
        public HealthStatus? ChronicDisease { get; set; }
        public HealthStatus? SevereLungsDisease { get; set; }
        public HealthStatus? Tattoo { get; set; }
        public string? TestRemark { get; set; }
        public bool? IsHealthy { get; set; }
    }
}

