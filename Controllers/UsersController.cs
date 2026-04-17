using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodHeroA.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _currentUser;
        private readonly Dictionary<Role, (string action, string controller)> _dashboards
            = new Dictionary<Role, (string action, string controller)>()
              {
                { Role.Admin, ("Dashboard", "AdminDashboard")},
                { Role.BankingOrganization, ("Dashboard", "BankingOrganizationDashboard")},
                { Role.RecipientOrganization, ("Dashboard", "RecipientOrganizationDashboard")},
                { Role.DonorOrganization, ("Dashboard", "DonorOrganizationDashboard")},
                { Role.Donor, ("Dashboard", "DonorDashboard")}
              };
        public UsersController(IUserService userService, IAuthService currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }
            var login = await _userService.Login(loginModel);
            if (!login.Status)
            {
                ViewBag.Error = login.Message;
                return View(loginModel);
            }
            var user = login.Data;
            if (user is null)
            {
                ViewBag.Error = login.Message;
                return View(loginModel);
            }

            if (!_dashboards.TryGetValue(user.Role, out var route))
            {
                ViewBag.Error = login.Message;
                return View(loginModel);
            }

            ViewBag.Success = login.Message;
            return RedirectToAction(route.action, route.controller);
        }

        [HttpGet]
        [Authorize]
        public IActionResult UpdatePassword() => View();

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(PasswordUpdateModel passwordUpdateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(passwordUpdateModel);
            }
            var currentUser = await _currentUser.GetCurrentUser();
            if (currentUser == null)
            {
                TempData["failure"] = "User not authenticated";
                return RedirectToAction("Login", "Users");
            }
            var update = await _userService.ChangePassword(passwordUpdateModel);
            if (update.Data is null || !update.Status)
            {
                ViewBag.Error = update.Message;
                return View(passwordUpdateModel);
            }

            if (!_dashboards.TryGetValue(currentUser.Role, out var route))
            {
                TempData["failure"] = "User not authenticated";
                return RedirectToAction("Login", "Users");

            }
            //TempData["success"] = update.Message;
            return RedirectToAction(route.action, route.controller);
        }
        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> GetByID(Guid id)
        //{
        //    var user = await _userService.GetById(id);
        //    if (!user.Status || user.Data == null)
        //    {
        //        return NotFound();
        //    }
        //    TempData["success"] = user.Message;
        //    return View(user.Data);
        //}

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _currentUser.GetCurrentUser();
            if (user == null)
            {
                return NotFound();
            }
            //TempData["success"] = "Successful";
            return View(user);
        }
    }
}

//Task<BaseResponse<UserDTO?>> Login(UseB2F5EB62rLoginModel loginModel);
//Task<BaseResponse<UserDTO?>> GetById(Guid id);
//Task<BaseResponse<UserDTO?>> ChangePAssword(UserUpdateModel model);