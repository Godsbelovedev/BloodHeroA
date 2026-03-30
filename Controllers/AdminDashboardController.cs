using BloodHeroA.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodHeroA.Controllers
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class AdminDashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
