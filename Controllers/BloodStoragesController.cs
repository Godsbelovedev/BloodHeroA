using BloodHeroA.Application.Services.Implementations;
using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BloodHeroA.Controllers
{
    [Authorize]
    public class BloodStoragesController : Controller
    {
        private readonly IBloodStorageService _bloodStorageService;
        private readonly IAuthService _authService;
        private readonly IDonationService _donationService;

        public BloodStoragesController(IBloodStorageService bloodStorageService,
                                       IAuthService authService,
                                       IDonationService donationService)
        {
            _bloodStorageService = bloodStorageService;
            _authService = authService;
            _donationService = donationService;
        }
        [HttpGet]
        public async Task<IActionResult> Create(Guid donationId, BloodGroup bloodGroup)
        {
            var donation = await _donationService.GetByIdAsync(donationId);
            if (!donation.Status || donation.Data == null)
            {
                ViewBag.Error = donation.Message;
                return NotFound();
            }
            var details = donation.Data;
            ViewBag.BloodGroup = bloodGroup;
            var model = new BloodStorageDTO
            {
                DonationId = donationId,
                BloodGroup = bloodGroup
                //DonorName = details.DonorName,
                //DonorOrganizationName = details.DonorOrganizationName,
                //BankingOrganizationName = details.BankingOrganizationName,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BloodStorageDTO bloodStorageDto, Guid donationRequestId)
        {
            if (!ModelState.IsValid)
            {
                return View(bloodStorageDto);
            }
            ViewBag.RequestId = donationRequestId;
            var createStorage = await _bloodStorageService.CreateStorageAsync(bloodStorageDto);

            if (!createStorage.Status || createStorage.Data == null)
            {
                ViewBag.Error = createStorage.Message;
                return View(bloodStorageDto);
            }
            TempData["success"] = createStorage.Message;
            return RedirectToAction("GetDonationsForStorage", "Donations");
        }

        [HttpGet]
        public async Task<IActionResult> GetStorageById(Guid id)
        {
            var storages = await _bloodStorageService.GetByIdAsync(id);
            if (!storages.Status || storages.Data == null)
            {
                ViewBag.Error = storages.Message;
                return NotFound();
            }
            return View(storages.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetStorageForSupply(BloodGroup bloodGroup, Guid? requestId)
        {
          
                var storages = await _bloodStorageService.GetForSupplyAsync(bloodGroup);
                if (!storages.Status)
                {
                    ViewBag.Error = storages.Message;
                    return View(new List<BloodStorageResponseDto>());
                }
                ViewBag.RequestId = requestId;
                return View(storages.Data);
        }
        public async Task<IActionResult> GetExpired()
        {

            var expiredStorages = await _bloodStorageService.GetExpiredAsync();
            if (!expiredStorages.Status)
            {
                ViewBag.Error = expiredStorages.Message;
                return View(new List<BloodStorageResponseDto>());
            }
            return View(expiredStorages.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var storageToDelete = await _bloodStorageService.DeleteAsync(id);
            if (!storageToDelete.Status)
            {
                TempData["failure"] = "failed to delete  blood bank";
            }
            if (storageToDelete.Status)
            {
                TempData["success"] = "blood bank deleted  successfully";
            }
            return RedirectToAction("GetExpired");
        }
    }
}

