using BloodHeroA.Application.Services.Implementations;
using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BloodHeroA.Controllers
{
    [Authorize(Roles = nameof(Role.RecipientOrganization))]
    public class RecipientOrganizationDashboardController : Controller
    {
        private readonly IAuthService _authService;
        private readonly INotificationService _notification;
        private readonly IDonationRequestService _donationRequestService;
        private readonly IBloodInventoryService _inventoryService;

        public RecipientOrganizationDashboardController(
            IAuthService authService,
            INotificationService notification,
            IDonationRequestService donationRequestService,
            IBloodInventoryService inventoryService)
        {
            _authService = authService;
            _notification = notification;
            _donationRequestService = donationRequestService;
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser == null)
            {
                ViewBag.Error = "User not authenticated";
                return RedirectToAction("Login", "Users");
            }
            var notificationCount = await _notification.
                GetUnreadNotificationsCountByUserIdAsync(currentUser.Id);
            ViewBag.UnreadCount = notificationCount;

            var pendingRequest = await _donationRequestService.
            NumberOfPendingRequestByRecipientOrganizationIdRequestAsync();
            ViewBag.PendingCount = pendingRequest.Data;

            var incompletedRequest = await _donationRequestService.
            NumberOfIncompletedRequestByRecipientOrganizationIdRequestAsync();
            ViewBag.incompletedCount = incompletedRequest.Data;

           var completedRequest = await _donationRequestService.
           NumberOfCompletedRequestByRecipientOrganizationIdRequestAsync();
           ViewBag.completedCount = completedRequest.Data;


            var bloodTypeO_Positive = await _inventoryService.
                GetAllInventoryForBloodGroupO_PositiveByRecipientOrganizationIdAsync();

            var oPositiveDetails = bloodTypeO_Positive.Data;
            if(oPositiveDetails == null)
            {
                return View();
            }
            ViewBag.OpositiveReceived = oPositiveDetails.ReleasedUnits;

            var bloodTypeO_Negative = await _inventoryService.
            GetAllInventoryForBloodGroupO_NegativeByRecipientOrganizationIdAsync();

            var oNegativeDetails = bloodTypeO_Negative.Data;
            if (oNegativeDetails == null)
            {
                return View();
            }
            ViewBag.ONegativeReceived = oNegativeDetails.ReleasedUnits;

           var bloodTypeA_Positive = await _inventoryService.
           GetAllInventoryForBloodGroupA_PositiveByRecipientOrganizationIdAsync();

            var aPositiveDetails = bloodTypeA_Positive.Data;
            if (aPositiveDetails == null)
            {
                return View();
            }
            ViewBag.APositiveReceived = aPositiveDetails.ReleasedUnits;

            var bloodTypeA_Negative = await _inventoryService.
            GetAllInventoryForBloodGroupA_NegativeByRecipientOrganizationIdAsync();

            var aNegativeDetails = bloodTypeA_Negative.Data;
            if (aNegativeDetails == null)
            {
                return View();
            }
            ViewBag.ANegativeReceived = aNegativeDetails.ReleasedUnits;

            var bloodTypeB_Positive = await _inventoryService.
            GetAllInventoryForBloodGroupB_PositiveByRecipientOrganizationIdAsync();

            var bPositiveDetails = bloodTypeB_Positive.Data;
            if (bPositiveDetails == null)
            {
                return View();
            }
            ViewBag.BPositiveReceived = bPositiveDetails.ReleasedUnits;

            var bloodTypeB_Negative = await _inventoryService.
          GetAllInventoryForBloodGroupB_NegativeByRecipientOrganizationIdAsync();

            var bNegativeDetails = bloodTypeB_Negative.Data;
            if (bNegativeDetails == null)
            {
                return View();
            }
            ViewBag.BNegativeReceived = bNegativeDetails.ReleasedUnits;

            var bloodTypeAB_Positive = await _inventoryService.
           GetAllInventoryForBloodGroupAB_PositiveByRecipientOrganizationIdAsync();

            var aBPositiveDetails = bloodTypeAB_Positive.Data;
            if (aBPositiveDetails == null)
            {
                return View();
            }
            ViewBag.ABPositiveReceived = aBPositiveDetails.ReleasedUnits;

            var bloodTypeAB_Negative = await _inventoryService.
            GetAllInventoryForBloodGroupAB_NegativeByRecipientOrganizationIdAsync();

            var aBNegativeDetails = bloodTypeAB_Negative.Data;
            if (aBNegativeDetails == null)
            {
                return View();
            }
            ViewBag.ABNegativeReceived = aBNegativeDetails.ReleasedUnits;

            return View();
        }
    }
}
