using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodHeroA.Controllers
{
    [Authorize]
    public class BankingOrganizationsController : Controller
    {
       
        public BankingOrganizationsController(IBankingOrganizationService organization, 
                                              IAuthService currentUser)
        {
            _organization = organization;
            _currentUser = currentUser;
        }

        private readonly IBankingOrganizationService _organization;
        private readonly IAuthService _currentUser;

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ViewAll()
        {
            var organizations = await _organization.GetAllAsync();
            if (!organizations.Status)
            {
                ViewBag.Error = organizations.Message;
                return View(new List<BankingOrganizationResponseDto>());
            }
           // ViewBag.Success = organizations.Message;
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
           // ViewBag.Success = organization.Message;
            return View(organization.Data);
        }

        [HttpGet]
        public async Task<IActionResult> ViewProfile()
        {
            var currentUser = await _currentUser.GetCurrentUser();
            if(currentUser == null)
            {
                TempData["failure"] = "user not auntheticated";
                return RedirectToAction("Login", "Users");
            }
            var response = await _organization.GetByUserIdAsync(currentUser.Id);
            if (!response.Status || response.Data == null)
            {
                return NotFound();
            }
           // ViewBag.Error = response.Message;
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
            var organization = await _organization.GetByUserIdAsync(currentUser.Id);
            if(!organization.Status || organization.Data == null)
            {
                return NotFound();
            }
            var detailsToUpdate = organization.Data;
            var updateDto = new BankingOrganizationUpdateDto
            {
                OrganizationName = detailsToUpdate.OrganizationName,
                Address = detailsToUpdate.Address,
                PhoneNumber = detailsToUpdate.PhoneNumber
            };
            return View(updateDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(BankingOrganizationUpdateDto updateDto)
        {
            if(!ModelState.IsValid)
            {
                return View(updateDto);
            }
            var currentUser = await _currentUser.GetCurrentUser();
            if (currentUser == null)
            {
                TempData["failure"] = "user not auntheticated";
                return RedirectToAction("Login", "Users");
            }
            var update = await _organization.UpdateAsync(updateDto);
            if(!update.Status || update.Data == null)
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
        public async Task<IActionResult> CreateOrganization(BankingOrganizationDTO bankingOrganizationDTO)
        {
            if(!ModelState.IsValid)
            {
                return View(bankingOrganizationDTO);
            }

            var createOrganization = await _organization.AddAsync(bankingOrganizationDTO);

            if(!createOrganization.Status || createOrganization.Data == null)
            {
                ViewBag.Error = createOrganization.Message;
                return View(bankingOrganizationDTO);
            }
            //TempData["success"] = createOrganization.Message;
            return RedirectToAction("Login", "Users");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var organizationToDelete = await _organization.DeleteAsync(id);
            if(!organizationToDelete.Status)
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













//Task<BaseResponse<IEnumerable<BankingOrganizationResponseDto>>> GetAllAsync();
//Task<BaseResponse<BankingOrganizationResponseDto?>> GetByIdAsync(Guid id);
//Task<BaseResponse<BankingOrganizationResponseDto?>> GetByUserIdAsync(Guid userId);
//Task<BaseResponse<BankingOrganizationResponseDto>> AddAsync(BankingOrganizationDTO bankingOrganizationDTO);
//Task<BaseResponse<BankingOrganizationResponseDto>> UpdateAsync(BankingOrganizationUpdateDto updateDto);
//Task<BaseResponse<bool>> DeleteAsync(Guid id);