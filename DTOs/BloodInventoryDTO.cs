using BloodHeroA.Models.Enums;

namespace BloodHeroA.DTOs
{
   
    public class BloodInventoryResponseDTO
    {
        public BloodGroup BloodGroup { get; set; }
        public int StoredUnits { get; set; }
        public int ReleasedUnits { get; set; }
        public int ExpiredUnits { get; set; }
        public int AvailableUnits { get; set; }
    }
}