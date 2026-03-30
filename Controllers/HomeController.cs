using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BloodHero.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBloodStorageService _bloodStorageService;
        public HomeController(ILogger<HomeController> logger, IBloodStorageService bloodStorageService)
        {
            _logger = logger;
            _bloodStorageService = bloodStorageService;
        }

        public async Task<IActionResult> Index()
        {
            await _bloodStorageService.GetExpiredAsync();
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
