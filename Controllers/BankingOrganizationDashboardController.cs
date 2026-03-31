using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BloodHeroA.Controllers
{
    [Authorize(Roles = nameof(Role.BankingOrganization))]
    public class BankingOrganizationDashboardController : Controller
    {
        private readonly IBloodInventoryService _inventoryService;

        public BankingOrganizationDashboardController(IBloodInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var bloodTypeO_Positive = await _inventoryService.
               GetAllInventoryForBloodGroupO_PositiveByBankingOrganizationIdAsync();

            var oPositiveDetails = bloodTypeO_Positive.Data ?? new BloodInventoryResponseDTO();

            ViewBag.OpositiveReleased = oPositiveDetails?.ReleasedUnits ?? 0;
            ViewBag.OpositiveStored = oPositiveDetails?.StoredUnits ?? 0;
            ViewBag.OpositiveExpired = oPositiveDetails?.ExpiredUnits ?? 0;
            ViewBag.OpositiveAvailable = oPositiveDetails?.AvailableUnits ?? 0;

            var bloodTypeO_Negative = await _inventoryService.
            GetAllInventoryForBloodGroupO_NegativeByBankingOrganizationIdAsync();

            var oNegativeDetails = bloodTypeO_Negative.Data ?? new BloodInventoryResponseDTO();
            ViewBag.ONegativeReleased = oNegativeDetails?.ReleasedUnits ?? 0;
            ViewBag.ONegativeStored = oNegativeDetails?.StoredUnits ?? 0;
            ViewBag.ONegativeExpired = oNegativeDetails?.ExpiredUnits ?? 0;
            ViewBag.ONegativeAvailable = oNegativeDetails?.AvailableUnits ?? 0;


            var bloodTypeA_Positive = await _inventoryService.
            GetAllInventoryForBloodGroupA_PositiveByBankingOrganizationIdAsync();

            var aPositiveDetails = bloodTypeA_Positive.Data ?? new BloodInventoryResponseDTO();
            
            ViewBag.APositiveReleased = aPositiveDetails?.ReleasedUnits ?? 0;
            ViewBag.APositiveStored = aPositiveDetails?.StoredUnits ?? 0;
            ViewBag.APositiveExpired = aPositiveDetails?.ExpiredUnits ?? 0;
            ViewBag.APositiveAvailable = aPositiveDetails?.AvailableUnits?? 0;



            var bloodTypeA_Negative = await _inventoryService.
            GetAllInventoryForBloodGroupA_NegativeByBankingOrganizationIdAsync();

            var aNegativeDetails = bloodTypeA_Negative.Data ?? new BloodInventoryResponseDTO();
           
            ViewBag.ANegativeReleased = aNegativeDetails?.ReleasedUnits ?? 0;
            ViewBag.ANegativeStored = aNegativeDetails?.StoredUnits ?? 0;
            ViewBag.ANegativeExpired = aNegativeDetails?.ExpiredUnits ?? 0;
            ViewBag.ANegativeAvailable = aNegativeDetails?.AvailableUnits ?? 0;

            var bloodTypeB_Positive = await _inventoryService.
            GetAllInventoryForBloodGroupB_PositiveByBankingOrganizationIdAsync();

            var bPositiveDetails = bloodTypeB_Positive.Data ?? new BloodInventoryResponseDTO();
         
            ViewBag.BPositiveReleased = bPositiveDetails?.ReleasedUnits ?? 0;
            ViewBag.BPositiveStored = bPositiveDetails?.StoredUnits ?? 0;
            ViewBag.BPositiveExpired = bPositiveDetails?.ExpiredUnits ?? 0;
            ViewBag.BPositiveAvailable = bPositiveDetails?.AvailableUnits ?? 0;


            var bloodTypeB_Negative = await _inventoryService.
          GetAllInventoryForBloodGroupB_NegativeByBankingOrganizationIdAsync();

            var bNegativeDetails = bloodTypeB_Negative.Data ?? new BloodInventoryResponseDTO();
           
            ViewBag.BNegativeReleased = bNegativeDetails?.ReleasedUnits ?? 0;
            ViewBag.BNegativeStored = bNegativeDetails?.StoredUnits ?? 0;
            ViewBag.BNegativeExpired = bNegativeDetails?.ExpiredUnits ?? 0;
            ViewBag.BNegativeAvailable = bNegativeDetails?.AvailableUnits ?? 0;

            var bloodTypeAB_Positive = await _inventoryService.
           GetAllInventoryForBloodGroupAB_PositiveByBankingOrganizationIdAsync();

            var aBPositiveDetails = bloodTypeAB_Positive.Data ?? new BloodInventoryResponseDTO();
          
            ViewBag.ABPositiveReleased = aBPositiveDetails?.ReleasedUnits ?? 0;
            ViewBag.ABPositiveStored = aBPositiveDetails?.StoredUnits ?? 0;
            ViewBag.ABPositiveExpired = aBPositiveDetails?.ExpiredUnits ?? 0;
            ViewBag.ABPositiveAvailable = aBPositiveDetails?.AvailableUnits ?? 0;

            var bloodTypeAB_Negative = await _inventoryService.
            GetAllInventoryForBloodGroupAB_NegativeByBankingOrganizationIdAsync();

            var aBNegativeDetails = bloodTypeAB_Negative.Data ?? new BloodInventoryResponseDTO();
            
            ViewBag.ABNegativeReleased = aBNegativeDetails?.ReleasedUnits ?? 0;
            ViewBag.ABNegativeStored = aBNegativeDetails?.StoredUnits ?? 0;
            ViewBag.ABNegativeExpired = aBNegativeDetails?.ExpiredUnits ?? 0;
            ViewBag.ABNegativeAvailable = aBNegativeDetails?.AvailableUnits ?? 0;

            return View();
        }
    }
}
