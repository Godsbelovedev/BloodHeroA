using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodHeroA.Controllers
{
    public class RecipientOrganizationController : Controller
    {
        private readonly IRecipientOrganizationService _organization;
        private readonly IAuthService _currentUser;
        public RecipientOrganizationController(IRecipientOrganizationService organization,
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
                return View(new List<RecipientResponseDto>());
            }
            ViewBag.Success = organizations.Message;
            return View(organizations.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var organization = await _organization.GetByIdAsync(id);
            if (!organization.Status || organization.Data == null)
            {
                return NotFound();
            }
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
            ViewBag.Success = response.Message;
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
            var updateDto = new RecipientUpdateDto
            {
                OrganizationName = detailsToUpdate.OrganizationName,
                Address = detailsToUpdate.Address,
                PhoneNumber = detailsToUpdate.PhoneNumber
            };
            return View(updateDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(RecipientUpdateDto updateDto)
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
            //TempData["success"] = update.Message;
            return RedirectToAction("Viewprofile");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CreateOrganization() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrganization(RecipientOrganizationDTO donorOrganizationDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(donorOrganizationDTO);
            }

            var createOrganization = await _organization.AddAsync(donorOrganizationDTO);

            if (!createOrganization.Status || createOrganization.Data == null)
            {
                ViewBag.Error = createOrganization.Message;
                return View(donorOrganizationDTO);
            }
            //TempData["success"] = createOrganization.Message;
            return RedirectToAction("Login", "Users");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var organizationToDelete = await _organization.DeleteAsync(id);
            if (!organizationToDelete.Status)
            {
                TempData["failure"] = organizationToDelete.Message;
            }
            if (organizationToDelete.Status)
            {
                TempData["success"] = organizationToDelete.Message;
            }
            return RedirectToAction("ViewAll");
        }
    }
}
