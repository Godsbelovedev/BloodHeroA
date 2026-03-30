using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodHeroA.Controllers
{
    [Authorize]
    public class DonorOrganizationController : Controller
    {
        private readonly IDonorOrganizationService _organization;
        private readonly IAuthService _currentUser;
        public DonorOrganizationController(IDonorOrganizationService organization,
                                              IAuthService currentUser)
        {
            _organization = organization;
            _currentUser = currentUser;
        }
        [HttpGet]
        public async Task<IActionResult> ViewAll()
        {
            var organizations = await _organization.GetAllAsync();
            if (!organizations.Status)
            {
                ViewBag.Error = organizations.Message;
                return View(new List<DonorOrganizationResponseDto>());
            }
            ViewBag.Source = "ViewAll";
            ViewBag.Success = organizations.Message;
            return View(organizations.Data);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllForDonations()
        {
            var organizations = await _organization.GetAllAsync();
            if (!organizations.Status)
            {
                ViewBag.Error = organizations.Message;
                return View(new List<DonorOrganizationResponseDto>());
            }
            ViewBag.Source = "ViewAllForDonations";
            ViewBag.Success = organizations.Message;
            return View(organizations.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id, string source)
        {
            var organization = await _organization.GetByIdAsync(id);
            if (!organization.Status || organization.Data == null)
            {
                return NotFound();
            }
            ViewBag.Source = source;
            ViewBag.Success = organization.Message;
            return View(organization.Data);
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
            var response = await _organization.GetByUserIdAsync(currentUser.Id);
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
                TempData["failure"] = "user not aunthenticated";
                return RedirectToAction("Login", "Users");
            }
            var organization = await _organization.GetByUserIdAsync(currentUser.Id);
            if (!organization.Status || organization.Data == null)
            {
                return NotFound();
            }
            var detailsToUpdate = organization.Data;
            var updateDto = new DonorOrganizationUpdateDto
            {
                OrganizationName = detailsToUpdate.OrganizationName,
                Address = detailsToUpdate.Address,
                PhoneNumber = detailsToUpdate.PhoneNumber
            };
            return View(updateDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(DonorOrganizationUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateDto);
            }
            var currentUser = await _currentUser.GetCurrentUser();
            if (currentUser == null)
            {
                TempData["failure"] = "user not aunthenticated";
                return RedirectToAction("Login", "Users");
            }
            var update = await _organization.UpdateAsync(updateDto);
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
        public IActionResult CreateOrganization() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrganization(DonorOrganizationRequestDto donorOrganizationDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(donorOrganizationDTO);
            }

            var createOrganization = await _organization.AddAsync(donorOrganizationDTO);

            if (!createOrganization.Status)
            {
                ViewBag.Error = createOrganization.Message;
                return View(donorOrganizationDTO);
            }
            TempData["success"] = createOrganization.Message;
            return RedirectToAction("Login", "Users");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var organizationToDelete = await _organization.DeleteAsync(id);
            if (!organizationToDelete.Status)
            {
                TempData["failure"] = "failed to delete  blood bank";
            }
            if (organizationToDelete.Status)
            {
                TempData["success"] = "blood bank deleted  successfully";
            }
            return RedirectToAction("ViewAll");
        }
    }
}