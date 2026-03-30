using BloodHeroA.Application.Services.Implementations;
using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodHeroA.Controllers
{
    [Authorize]
    public class BloodReleasedController : Controller
    {
        private readonly IReleasedBloodService _releasedBlood;
        //private readonly IDonationRequestService _donationRequest;
        private readonly IBloodStorageService _storageService;
        public BloodReleasedController(IReleasedBloodService releasedBlood, 
                                       //IDonationRequestService donationRequest, 
                                       IBloodStorageService storageService)
        {
            _releasedBlood = releasedBlood;
            //_donationRequest = donationRequest;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid storageId, Guid requestId)
        {
            var storageToRelease = await _storageService.GetByIdAsync(storageId);
            if (!storageToRelease.Status || storageToRelease.Data == null)
            {
                ViewBag.Error = storageToRelease.Message;
                return NotFound();
            }
            var details = storageToRelease.Data;
            var model = new ReleasedBloodRequestDto
            {
                BloodStorageId = details.Id,
                BloodTypeReleased = details.BloodGroup,
                DonationRequestId = requestId,
                QuantityReleased = 1
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReleasedBloodRequestDto releasedBlood)
        {
            if (!ModelState.IsValid)
            {
                return View(releasedBlood);
            }

            var createRelease = await _releasedBlood.CreateAsync(releasedBlood);

            if (!createRelease.Status || createRelease.Data == null)
            {
                ViewBag.Error = createRelease.Message;
                return View(releasedBlood);
            }
            TempData["success"] = createRelease.Message;
            return RedirectToAction("GetStorageForSupply", "BloodStorages",
                                    new {bloodGroup = releasedBlood.BloodTypeReleased,
                                         requestId = releasedBlood.DonationRequestId});
        }
    }
}
//Task<BaseResponse<ReleasedBloodResponseDto>>
//        CreateAsync(ReleasedBloodRequestDto releasedBlood);

//Task<BaseResponse<IEnumerable<ReleasedBloodResponseDto>>>
//               GetReleasedByTypeAsync(BloodGroup bloodGroup);

//Task<BaseResponse<ReleasedBloodResponseDto?>> GetByIdAsync(Guid id);