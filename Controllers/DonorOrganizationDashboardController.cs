using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BloodHeroA.Controllers
{
    public class DonorOrganizationDashboardController : Controller
    {
        private readonly IDonationService _donationService;
        private readonly IAuthService _authservice;
        private readonly IDonorOrganizationService _organization;
        private readonly IDonorService _donor;
        public DonorOrganizationDashboardController(IDonationService donationService,
                                                    IAuthService authservice,
                                                    IDonorOrganizationService organization,
                                                    IDonorService donor)
        {
            _donationService = donationService;
            _authservice = authservice;
            _organization = organization;
            _donor = donor;
        }

        [Authorize(Roles = nameof(Role.DonorOrganization))]
        public async Task<IActionResult> Dashboard()
        {
            var totalDonations = await _donationService.GetTotalDonationsByDonorOrganizationIdAsync();
            ViewBag.TotalDonations = totalDonations.Data;

            var successfulDonations = await _donationService.GetSuccessfulDonationsByDonorOrganizationIdAsync();
            ViewBag.SuccessfulDonations = successfulDonations.Data;

            var healthyDonations = await _donationService.GetHealthyDonationsByDonorOrganizationIdAsync();
            ViewBag.HealthyDonations = healthyDonations.Data;

            var currentUser = await _authservice.GetCurrentUser();
            if(currentUser == null)
            {
                return View();
            }
            var organization = await _organization.GetByUserIdAsync(currentUser.Id);
            if (organization.Data == null || !organization.Status)
            {
                return View();
            }
            ViewBag.TotalDonors = organization.Data.TotalRegisteredDonors;
            var availableDonors = await _donor.GetAvailableDonorsByDonorOrganizationIdAsync(organization.Data.Id);

            if(availableDonors.Data == null || !availableDonors.Status)
            {
                return View();
            }
            ViewBag.AvailableDonors = availableDonors.Data.Count();

            return View();
        }
    }
}
