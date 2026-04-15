using BloodHeroA.Application.Services.Implementations;
using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BloodHeroA.Controllers
{
    [Authorize]
    public class DonationsController : Controller
    {
        private readonly IDonationService _donationService;
        private readonly IDonorOrganizationService _donorOrganization;
        private readonly IBankingOrganizationService _bankingOrganization;
        private readonly IAuthService _currentUser;
        public DonationsController(IAuthService currentUser,
                                   IDonationService donationService,
                                   IBankingOrganizationService bankingOrganization,
                                   IDonorOrganizationService donorOrganization)
        {
            _currentUser = currentUser;
            _donationService = donationService;
            _bankingOrganization = bankingOrganization;
            _donorOrganization = donorOrganization;
        }
        [HttpGet]
        public IActionResult Create(Guid donorId, Guid donorOrganizationId, string source)
        {
            var model = new DonationDTO
            {
                DonorId = donorId,
                DonorOrganizationId = donorOrganizationId
            };
            ViewBag.Source = source;
            ViewBag.DonorOrganizationId = donorOrganizationId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonationDTO donationDto, string source)
        {
            if (!ModelState.IsValid)
            {
                return View(donationDto);
            }
            var createDonoation = await _donationService.CreateAsync(donationDto);

            if (!createDonoation.Status || createDonoation.Data == null)
            {
                ViewBag.Error = createDonoation.Message;
                return View(donationDto);
            }
            ViewBag.Success = createDonoation.Message;
            var donorOrganizationId = donationDto.DonorOrganizationId;
            if (donorOrganizationId.HasValue 
                && donorOrganizationId.Value != Guid.Empty)
            {
                return RedirectToAction("GetAvailableDonorsByDonorOrganizationId", "Donors", new { donorOrganizationId = donationDto.DonorOrganizationId.Value });
            }
            ViewBag.Source = "GetAvailableDonorsByDonorOrganizationId";
            ViewBag.DonorOrganizationId = donationDto.DonorOrganizationId;
            return RedirectToAction("GetAvailableDonors", "Donors");
        }

        [HttpGet]
        public  async Task<IActionResult> GetById(Guid id, string source)
        {
            var donation = await _donationService.GetByIdAsync(id);
            ViewBag.Source = source;
            if(!donation.Status || donation.Data == null)
            {
                ViewBag.Error = donation.Message;
                return NotFound();
            }
            return View(donation.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var donations = await _donationService.GetAllAsync();
            if (!donations.Status)
            {
                ViewBag.Error = donations.Message;
                return View(new List<DonationResponseDto>());
            }
            return View(donations.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUntestedDonations()
        {
            var donations = await _donationService.GetAllUntestedDonationsAsync();
            if (!donations.Status)
            {
                ViewBag.Error = donations.Message;
                return View(new List<DonationResponseDto>());
            }
            return View(donations.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetDonationsForStorage()
        {
            var donations = await _donationService.GetDonationsForStorageAsync();
            if (!donations.Status)
            {
                ViewBag.Error = donations.Message;
                return View(new List<DonationResponseDto>());
            }
            return View(donations.Data);
        }

        public async Task<IActionResult> Delete(Guid donationId)
        {
            var donation = await _donationService.GetByIdAsync(donationId);
            if (!donation.Status || donation.Data == null)
            {
                TempData["failure"] = donation.Message;
                return RedirectToAction("Dashboard", "BankingOrganizationDashboard");
            }
            var isTested = donation.Data.IsTested;
            var donationToDelete = await _donationService.DeleteAsync(donationId);
            if (!donationToDelete.Status || donationToDelete.Data == false)
            {
                TempData["failure"] = donationToDelete.Message;
                return RedirectToAction("Dashboard", "BankingOrganizationDashboard");
            }

            TempData["success"] = donationToDelete.Message;

            if(isTested)
            {
                return RedirectToAction("GetDonationsForStorage", "Donations");
            }
            return RedirectToAction("GetAllUntestedDonations", "Donations");
        }
    }
}
//Task<BaseResponse<DonationResponseDto?>> GetByIdAsync(Guid id);
//Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetAllAsync();
//Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetAllUntestedDonationsAsync();
//Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetDonationsForStorageAsync();

//Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetDonationsByDonorIdAsync();
//Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetDonationsByDonorOrganizationIdAsync();
//Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetDonationsByBankingOrganizationIdAsync();
//Task<BaseResponse<DonationResponseDto>> CreateAsync(DonationDTO donationDto);