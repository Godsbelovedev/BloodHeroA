using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BloodHeroA.Controllers
{
    [Authorize(Roles = nameof(Role.Donor))]
    public class DonorDashboardController : Controller
    {
        private readonly IDonationService _donationService;

        public DonorDashboardController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var totalDonations = await _donationService.GetTotalDonationsByDonorIdAsync();
            ViewBag.TotalDonation = totalDonations.Data;

            var successfulDonations = await _donationService.GetSuccessfulDonationsByDonorIdAsync();
            ViewBag.successfulDonation = successfulDonations.Data;

            var healthyDonations = await _donationService.GetHealthyDonationsByDonorIdAsync();
            ViewBag.healthyDonation = healthyDonations.Data;
            return View();
        }
    }
}
