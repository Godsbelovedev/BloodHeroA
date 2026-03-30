using BloodHeroA.Application.Services.Interfaces;
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

            var oPositiveDetails = bloodTypeO_Positive.Data;
            if (oPositiveDetails == null)
            {
                return View();
            }
            ViewBag.OpositiveReleased = oPositiveDetails.ReleasedUnits;
            ViewBag.OpositiveStored = oPositiveDetails.StoredUnits;
            ViewBag.OpositiveExpired = oPositiveDetails.ExpiredUnits;
            ViewBag.OpositiveAvailable = oPositiveDetails.AvailableUnits;

            var bloodTypeO_Negative = await _inventoryService.
            GetAllInventoryForBloodGroupO_NegativeByBankingOrganizationIdAsync();

            var oNegativeDetails = bloodTypeO_Negative.Data;
            if (oNegativeDetails == null)
            {
                return View();
            }
            ViewBag.ONegativeReleased = oNegativeDetails.ReleasedUnits;
            ViewBag.ONegativeStored = oNegativeDetails.StoredUnits;
            ViewBag.ONegativeExpired = oNegativeDetails.ExpiredUnits;
            ViewBag.ONegativeAvailable = oNegativeDetails.AvailableUnits;


            var bloodTypeA_Positive = await _inventoryService.
            GetAllInventoryForBloodGroupA_PositiveByBankingOrganizationIdAsync();

            var aPositiveDetails = bloodTypeA_Positive.Data;
            if (aPositiveDetails == null)
            {
                return View();
            }
            ViewBag.APositiveReleased = aPositiveDetails.ReleasedUnits;
            ViewBag.APositiveStored = aPositiveDetails.StoredUnits;
            ViewBag.APositiveExpired = aPositiveDetails.ExpiredUnits;
            ViewBag.APositiveAvailable = aPositiveDetails.AvailableUnits;



            var bloodTypeA_Negative = await _inventoryService.
            GetAllInventoryForBloodGroupA_NegativeByBankingOrganizationIdAsync();

            var aNegativeDetails = bloodTypeA_Negative.Data;
            if (aNegativeDetails == null)
            {
                return View();
            }
            ViewBag.ANegativeReleased = aNegativeDetails.ReleasedUnits;
            ViewBag.ANegativeStored = aNegativeDetails.StoredUnits;
            ViewBag.ANegativeExpired = aNegativeDetails.ExpiredUnits;
            ViewBag.ANegativeAvailable = aNegativeDetails.AvailableUnits;

            var bloodTypeB_Positive = await _inventoryService.
            GetAllInventoryForBloodGroupB_PositiveByBankingOrganizationIdAsync();

            var bPositiveDetails = bloodTypeB_Positive.Data;
            if (bPositiveDetails == null)
            {
                return View();
            }
            ViewBag.BPositiveReleased = bPositiveDetails.ReleasedUnits;
            ViewBag.BPositiveStored = bPositiveDetails.StoredUnits;
            ViewBag.BPositiveExpired = bPositiveDetails.ExpiredUnits;
            ViewBag.BPositiveAvailable = bPositiveDetails.AvailableUnits;


            var bloodTypeB_Negative = await _inventoryService.
          GetAllInventoryForBloodGroupB_NegativeByBankingOrganizationIdAsync();

            var bNegativeDetails = bloodTypeB_Negative.Data;
            if (bNegativeDetails == null)
            {
                return View();
            }
            ViewBag.BNegativeReleased = bNegativeDetails.ReleasedUnits;
            ViewBag.BNegativeStored = bNegativeDetails.StoredUnits;
            ViewBag.BNegativeExpired = bNegativeDetails.ExpiredUnits;
            ViewBag.BNegativeAvailable = bNegativeDetails.AvailableUnits;

            var bloodTypeAB_Positive = await _inventoryService.
           GetAllInventoryForBloodGroupAB_PositiveByBankingOrganizationIdAsync();

            var aBPositiveDetails = bloodTypeAB_Positive.Data;
            if (aBPositiveDetails == null)
            {
                return View();
            }
            ViewBag.ABPositiveReleased = aBPositiveDetails.ReleasedUnits;
            ViewBag.ABPositiveStored = aBPositiveDetails.StoredUnits;
            ViewBag.ABPositiveExpired = aBPositiveDetails.ExpiredUnits;
            ViewBag.ABPositiveAvailable = aBPositiveDetails.AvailableUnits;

            var bloodTypeAB_Negative = await _inventoryService.
            GetAllInventoryForBloodGroupAB_NegativeByBankingOrganizationIdAsync();

            var aBNegativeDetails = bloodTypeAB_Negative.Data;
            if (aBNegativeDetails == null)
            {
                return View();
            }
            ViewBag.ABNegativeReleased = aBNegativeDetails.ReleasedUnits;
            ViewBag.ABNegativeStored = aBNegativeDetails.StoredUnits;
            ViewBag.ABNegativeExpired = aBNegativeDetails.ExpiredUnits;
            ViewBag.ABNegativeAvailable = aBNegativeDetails.AvailableUnits;

            return View();
        }
    }
}
