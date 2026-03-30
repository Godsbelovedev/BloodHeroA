using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodHeroA.Controllers
{
    [Authorize]
    public class DonorsController : Controller
    {
        private readonly IDonorService _donorService;
        private readonly IAuthService _currentUser;
        public DonorsController(IAuthService currentUser, IDonorService donorService)
        {
            _currentUser = currentUser;
            _donorService = donorService;
        }
        [HttpGet]
        public async Task<IActionResult> ViewAll()
        {
            var donors = await _donorService.GetAllAsync();
            if (!donors.Status)
            {
                ViewBag.Error = donors.Message;
                return View(new List<DonorResponseDto>());
            }
            ViewBag.Source = "ViewAll";
            ViewBag.Success = donors.Message;
            return View(donors.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id, string source)
        {
            var donor = await _donorService.GetByIdAsync(id);
            if (!donor.Status || donor.Data == null)
            {
                return NotFound();
            }
            ViewBag.Source = source;
            ViewBag.Success = donor.Message;
            return View(donor.Data);
        }

        [HttpGet]
        public async Task<IActionResult> ViewProfile()
        {
            var currentUser = await _currentUser.GetCurrentUser();
            if (currentUser == null)
            {
                TempData["failure"] = "user not auntheticated";
                return RedirectToAction("Login", "Users");
            }
            var response = await _donorService.GetByUserIdAsync(currentUser.Id);
            if (!response.Status || response.Data == null)
            {
                return NotFound();
            }
            ViewBag.Error = response.Message;
            return View(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var currentUser = await _currentUser.GetCurrentUser();
            if (currentUser == null)
            {
                TempData["failure"] = "user not auntheticated";
                return RedirectToAction("Login", "Users");
            }
            var donorToUpdate = await _donorService.GetByUserIdAsync(currentUser.Id);
            if (!donorToUpdate.Status || donorToUpdate.Data == null)
            {
                return NotFound();
            }
            var detailsToUpdate = donorToUpdate.Data;
            var updateDto = new DonorUpdateDto
            {
               FirstName = detailsToUpdate.FirstName,
               MiddleName = detailsToUpdate.MiddleName,
               LastName = detailsToUpdate.LastName,
               DateOfBirth = detailsToUpdate.DateOfBirth,
               Gender = detailsToUpdate.Gender,
               MaritalStatus = detailsToUpdate.MaritalStatus,
               PhoneNumber = detailsToUpdate.PhoneNumber,
               StateOfOrigin = detailsToUpdate.StateOfOrigin
            };
            return View(updateDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(DonorUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateDto);
            }
            var currentUser = await _currentUser.GetCurrentUser();
            if (currentUser == null)
            {
                TempData["failure"] = "user not auntheticated";
                return RedirectToAction("Login", "Users");
            }
            var update = await _donorService.UpdateAsync(updateDto);
            if (!update.Status || update.Data == null)
            {
                ViewBag.Error = update.Message;
                return View(updateDto);
            }
            TempData["success"] = update.Message;
            return RedirectToAction("Viewprofile");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SelfRegisterDonor() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SelfRegisterDonor(DonorRequestDto donorRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return View(donorRequestDto);
            }

            var createDonor = await _donorService.SelfRegisterDonorAsync(donorRequestDto);

            if (!createDonor.Status || createDonor.Data == null)
            {
                ViewBag.Error = createDonor.Message;
                return View(donorRequestDto);
            }
            TempData["success"] = createDonor.Message;
            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterDonorByOrganization() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterDonorByOrganization(DonorRequestDto donorRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return View(donorRequestDto);
            }

            var createDonor = await _donorService.RegisterDonorByOrganizationAsync(donorRequestDto);

            if (!createDonor.Status || createDonor.Data == null)
            {
                ViewBag.Error = createDonor.Message;
                return View(donorRequestDto);
            }
            TempData["success"] = createDonor.Message;
            return RedirectToAction("Dashboard", "DonorOrganizationDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var donorToDelete = await _donorService.DeleteAsync(id);
            if (!donorToDelete.Status)
            {
                TempData["failure"] = "failed to delete  blood bank";
            }
            if (donorToDelete.Status)
            {
                TempData["success"] = "blood bank deleted  successfully";
            }
            return RedirectToAction("ViewAll");
        }
        [HttpGet]
        public async Task<IActionResult> GetAvailableDonors()
        {

            var donors = await _donorService.GetAvailableDonorsAsync();
            if (!donors.Status)
            {
                ViewBag.Error = donors.Message;
                return View(new List<DonorResponseDto>());
            }
            ViewBag.Source = "GetAvailableDonors";
            ViewBag.Success = donors.Message;
            return View(donors.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetAvailableDonorsByDonorOrganizationId(Guid donorOrganizationId)
        {

            var donors = await _donorService.GetAvailableDonorsByDonorOrganizationIdAsync(donorOrganizationId);
            if (!donors.Status)
            {
                ViewBag.Error = donors.Message;
                return View(new List<DonorResponseDto>());
            }
            ViewBag.Source = "GetAvailableDonorsByDonorOrganizationId";
            ViewBag.OrganizationId = donorOrganizationId;
            ViewBag.Success = donors.Message;
            return View(donors.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetDonorsByDonorOrganizationId()
        {

            var donors = await _donorService.GetDonorsByDonorOrganizationIdAsync();
            if (!donors.Status)
            {
                ViewBag.Error = donors.Message;
                return View(new List<DonorResponseDto>());
            }
            ViewBag.Source = "GetDonorsByDonorOrganizationId";
            ViewBag.Success = donors.Message;
            return View(donors.Data);
        }
    }
}
//Task<BaseResponse<DonorResponseDto?>> GetByIdAsync(Guid id);
//Task<BaseResponse<DonorResponseDto?>> GetByUserIdAsync(Guid userId);
//Task<BaseResponse<DonorResponseDto>> SelfRegisterDonorAsync(DonorRequestDto donorDto);
//Task<BaseResponse<DonorResponseDto>> RegisterDonorByOrganizationAsync(DonorRequestDto donorDto);
//Task<BaseResponse<DonorResponseDto?>> UpdateAsync(DonorUpdateDto donor);
//Task<BaseResponse<bool>> DeleteAsync(Guid id);
//Task<BaseResponse<IEnumerable<DonorResponseDto>>> GetAvailableDonorsAsync();
//Task<BaseResponse<IEnumerable<DonorResponseDto>>> GetAvailableDonorsByDonorOrganizationUserIdAsync();
//Task<BaseResponse<IEnumerable<DonorResponseDto>>> GetDonorsByDonorOrganizationUserIdAsync
//                                             ();
//Task<BaseResponse<IEnumerable<DonorResponseDto>>> GetAllAsync();