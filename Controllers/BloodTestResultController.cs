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
    public class BloodTestResultController : Controller
    {
        private readonly IBloodTestResultService _bloodTestResult;
        private readonly IDonationService _donationService;
        private readonly IAuthService _authService;
        private readonly IBankingOrganizationService _organizationService;
        public BloodTestResultController(IBloodTestResultService bloodTestResult,
                                         IDonationService donationService,
                                         IAuthService authService,
                                         IBankingOrganizationService organizationService)
        {
            _bloodTestResult = bloodTestResult;
            _donationService = donationService;
            _authService = authService;
            _organizationService = organizationService;
        }
        [HttpGet]
        public async Task<IActionResult> Create(Guid donationId )
        {
            var donation = await _donationService.GetByIdAsync(donationId);
            if(!donation.Status || donation.Data == null)
            {
                ViewBag.Error = donation.Message;
                return NotFound();
            }
            var details = donation.Data;
            var model = new BloodTestResultDTO
            {
                DonationId = donationId,
                DonorName = details.DonorName,
                DonorOrganizationName = details.DonorOrganizationName,
                BankingOrganizationName = details.BankingOrganizationName
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BloodTestResultDTO bloodTestResultDto)
        {
            if (!ModelState.IsValid)
            {
                return View(bloodTestResultDto);
            }

            var createTest = await _bloodTestResult.CreateAsync(bloodTestResultDto);

            if (!createTest.Status || createTest.Data == null)
            {
                ViewBag.Error = createTest.Message;
                return View(bloodTestResultDto);
            }
            //TempData["success"] = createTest.Message;
            return RedirectToAction("GetAllUntestedDonations", "Donations");
        }
        [HttpGet]
        public async Task<IActionResult> EditDonationTest(Guid donationId)
        {
            var testToEdit = await _bloodTestResult.GetByDonationIdAsync(donationId);
            if (!testToEdit.Status || testToEdit.Data == null)
            {
                return NotFound();
            }
            var detailsToUpdate = testToEdit.Data;
            var updateDto = new BloodTestResultUpdateDTO
            {
               BloodGroup = detailsToUpdate.BloodGroup,
               Cancer = detailsToUpdate.Cancer,
               ChronicDisease = detailsToUpdate.ChronicDisease,
               HeartDisease = detailsToUpdate.HeartDisease,
               Hemophilic = detailsToUpdate.Hemophilic,
               HepatitisB = detailsToUpdate.HepatitisB,
               HIV = detailsToUpdate.HIV,
               IsHealthy = detailsToUpdate.IsHealthy,
               IVDrugConsumer = detailsToUpdate.IVDrugConsumer,
               SevereLungsDisease = detailsToUpdate.SevereLungsDisease,
               Syphilis = detailsToUpdate.Syphilis,
               Tattoo = detailsToUpdate.Tattoo,
               TestRemark = detailsToUpdate.TestRemark
            };
            return View(updateDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDonationTest(BloodTestResultUpdateDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateDto);
            }
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                TempData["failure"] = "user not aunthenticated";
                return RedirectToAction("Login", "Users");
            }
            var update = await _bloodTestResult.UpdateAsync(updateDto);
            if (!update.Status || update.Data == null)
            {
                ViewBag.Error = update.Message;
                return View(updateDto);
            }
            //TempData["success"] = update.Message;
            return RedirectToAction("GetDonationsForStorage", "Donations");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTestByBankingOrganization()
        {
            var tests = await _bloodTestResult.GetAllTestByBankingOrganizationAsync();
            if (!tests.Status)
            {
                ViewBag.Error = tests.Message;
                return View(new BloodTestResultResponseDto());
            }
            return View(tests.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetByDonationId(Guid donationId)
        {
            var donations = await _bloodTestResult.GetByDonationIdAsync(donationId);
            if (!donations.Status)
            {
                ViewBag.Error = donations.Message;
                return NotFound();
            }
            return View(donations.Data);
        }
        //public async Task<IActionResult> Delete(Guid donationId)
        //{
        //    var donationToDelete = await _donationService.DeleteAsync(donationId);
        //    if (!donationToDelete.Status)
        //    {
        //        ViewBag.Failure = donationToDelete.Message;
        //    }
        //    ViewBag.Success = donationToDelete.Message;

        //    return RedirectToAction("ViewAll");
        //}
    }
}
//Task<BaseResponse<BloodTestResultResponseDto>> CreateAsync(BloodTestResultDTO bloodTestResult);
//Task<BaseResponse<BloodTestResultResponseDto>> UpdateAsync(BloodTestResultUpdateDTO bloodTestResultUpdate);
//Task<BaseResponse<IEnumerable<BloodTestResultResponseDto>>> GetAllTestAsync();
//Task<BaseResponse<BloodTestResultResponseDto?>> GetByIdAsync(Guid id);