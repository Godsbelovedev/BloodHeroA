using BloodHeroA.Application.Services.Implementations;
using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BloodHeroA.Controllers
{
    [Authorize]
    public class DonationRequestController : Controller
    {
        private readonly IDonationRequestService _requestService;
        private readonly IAuthService _authService;
        private static Dictionary<Role, (string action, string controller)> _dashboards
            = new Dictionary<Role, (string action, string controller)>() 
             {
                { Role.RecipientOrganization, ("Dashboard", "RecipientOrganizationDashboard")},
                { Role.BankingOrganization, ("Dashboard", "BankingOrganizationDashboard")}
             };
        public DonationRequestController(IDonationRequestService requestService, IAuthService authService)
        {
            _requestService = requestService;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult SendRequestByRecipientOrganization() => View();

        [HttpPost]
        public async Task<IActionResult> SendRequestByRecipientOrganization(DonationRequestDto donationRequest)
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser == null)
            {
                TempData["failure"] = "user not authenticated";
                return RedirectToAction("Login", "Users");
            }
            if (!ModelState.IsValid)
            {
                return View(donationRequest);
            }
            var requestToSend = await _requestService.MakeRequestByRecipientOrganizationAsync(donationRequest);
            if (!requestToSend.Status || requestToSend.Data == null)
            {
                ViewBag.Error = requestToSend.Message;
                return View(donationRequest);
            }
            if (!_dashboards.TryGetValue(currentUser.Role, out var route))
            {
                ViewBag.Error = requestToSend.Message;
                return View(donationRequest);
            }
            //TempData["success"] = requestToSend.Message;
            return RedirectToAction(route.action, route.controller);
        }

        [HttpGet]
        public IActionResult SendRequestByBankingOrganization() => View();

        [HttpPost]
        public async Task<IActionResult> SendRequestByBankingOrganization(DonationRequestDto donationRequest)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                TempData["failure"] = "user not authenticated";
                return RedirectToAction("Login", "Users");
            }
            if (!ModelState.IsValid)
            {
                return View(donationRequest);
            }
            var requestToSend = await _requestService.MakeRequestByRecipientOrganizationAsync(donationRequest);
            if (!requestToSend.Status || requestToSend.Data == null)
            {
                ViewBag.Error = requestToSend.Message;
                return View(donationRequest);
            }
            if (!_dashboards.TryGetValue(currentUser.Role, out var route))
            {
                ViewBag.Error = requestToSend.Message;
                return View(donationRequest);
            }
            //TempData["success"] = requestToSend.Message;
            return RedirectToAction(route.action, route.controller);
        }
        [HttpGet]
        public async Task<IActionResult> GetByStatus(Status status)
        {
            var doationRequestsByStatus = await _requestService.GetByStatusAsync(status);
            if(!doationRequestsByStatus.Status)
            {
                ViewBag.Error = doationRequestsByStatus.Message;
                return View(new List<DonationRequestResponseDto>());
            }
            return View(doationRequestsByStatus.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStatusForSupply(Status status)
        {
            var doationRequestsByStatus = await _requestService.GetByStatusForSupplyAsync(status);
            if (!doationRequestsByStatus.Status)
            {
                ViewBag.Error = doationRequestsByStatus.Message;
                return View(new List<DonationRequestResponseDto>());
            }
            return View(doationRequestsByStatus.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetBankingOrganizationSentRequestsByStatus(Status status)
        {
            var doationRequestsByStatus = await _requestService.GetBankingOrganizationSentRequestsByStatusAsync(status);
            if (!doationRequestsByStatus.Status)
            {
                ViewBag.Error = doationRequestsByStatus.Message;
                return View(new List<DonationRequestResponseDto>());
            }
            return View(doationRequestsByStatus.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var doationRequest = await _requestService.GetByIdAsync(id);
            if (!doationRequest.Status)
            {
                ViewBag.Error = doationRequest.Message;
                return NotFound();
            }
            return View(doationRequest.Data);
        }

        //[HttpGet]
        //public IActionResult FilterByStatus() => View();

        //[HttpPost]
        //public IActionResult FilterByStatus(BloodGroup bloodGroup)
        //{
        //    if(bloodGroup == null)
        //    {
        //        return View(bloodGroup);
        //    }
        //    ViewBag.BloodType = bloodGroup;
        //}
    }
}
//Task<BaseResponse<DonationRequestResponseDto?>> GetByIdAsync(Guid id);
//Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>> GetAllAsync();
//Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>> GetByStatusAsync(Status status);
//Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>> CreateAsync
//    <form asp-action="Index" method="get">
//    < select name = "status" asp - items = "Html.GetEnumSelectList<RequestStatus>()" >
//        < option value = "" > --All-- </ option >
//    </ select >

//    < button type = "submit" > Filter </ button >
//</ form > (DonationRequestDto donationRequest);