using BloodHeroA.Application.Services.Implementations;
using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BloodHeroA.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IAuthService _authService;
        private readonly Dictionary<Role, (string action, string controller)> _dashboards
            = new Dictionary<Role, (string action, string controller)>()
              {
                { Role.Admin, ("Dashboard", "AdminDashboard")},
                { Role.BankingOrganization, ("Dashboard", "BankingOrganizationDashboard")},
                { Role.RecipientOrganization, ("Dashboard", "RecipientOrganizationDashboard")},
                { Role.DonorOrganization, ("Dashboard", "DonorOrganizationDashboard")},
                { Role.Donor, ("Dashboard", "DonorDashboard")}
              };
        public NotificationsController(INotificationService notificationService, IAuthService authService)
        {
            _notificationService = notificationService;
            _authService = authService;
        }
        [HttpGet]
       
        public IActionResult SendNotification() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendNotification(NotificationDTO dTO)
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser == null)
            {
                ViewBag.Error = "User not authenticated";
                return RedirectToAction("Login", "Users");
            }

            if (!ModelState.IsValid)
            {
                return View(dTO);
            }
           
            var notification = await _notificationService.SendNotificationAsync(dTO);
            
            if(!notification.Status)
            {
                ViewBag.Error = notification.Message;
                return View(dTO);
            }
            var details = notification.Data;
            if (!_dashboards.TryGetValue(currentUser.Role, out var route))
            {
                TempData["failure"] = "User not authenticated";
                return RedirectToAction("Login", "Users");
            }
            //TempData["success"] = notification.Message;
            return RedirectToAction(route.action, route.controller);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                ViewBag.Error = "User not authenticated";
                return RedirectToAction("Login", "Users");
            }
            var notifcations = await _notificationService.GetNotificationsByUserIdAsync();
            if(!notifcations.Status)
            {
                ViewBag.Error = notifcations.Message;
                return View(new List<NotificationResponseDto>());
            }
            return View(notifcations.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                ViewBag.Error = "User not authenticated";
                return RedirectToAction("Login", "Users");
            }
            var notifcation = await _notificationService.GetByIdAsync(id);
            if (!notifcation.Status)
            {
                ViewBag.Error = notifcation.Message;
                return NotFound();
            }
            await _notificationService.MarkMessageAsRead(id);
            return View(notifcation.Data);
        }
        [HttpGet]
        public async Task<IActionResult> ReplyMessage(Guid id)
        {
            var messageToReply = await _notificationService.GetByIdAsync(id);
            if(!messageToReply.Status || messageToReply.Data == null)
            {
                return NotFound();
            }

            var messageDTO = new NotificationDTO
            {
                ReceiverEmail = messageToReply.Data.SenderEmail
            };
            return View(messageDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyMessage(NotificationDTO dTO)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                ViewBag.Error = "User not authenticated";
                return RedirectToAction("Login", "Users");
            }

            if (!ModelState.IsValid)
            {
                return View(dTO);
            }

            var notification = await _notificationService.SendNotificationAsync(dTO);

            if (!notification.Status)
            {
                ViewBag.Error = notification.Message;
                return View(dTO);
            }
            var details = notification.Data;
            if (!_dashboards.TryGetValue(currentUser.Role, out var route))
            {
                TempData["failure"] = "User not authenticated";
                return RedirectToAction("Login", "Users");
            }
            //TempData["success"] = notification.Message;
            return RedirectToAction(route.action, route.controller);
        }
       
    }
}

//Task<BaseResponse<NotificationResponseDto?>> GetByIdAsync(Guid id);
//Task<BaseResponse<NotificationResponseDto>>
//                        SendNotificationAsync(NotificationDTO dTO);
//Task<BaseResponse<IEnumerable<NotificationResponseDto>>> GetAllAsync();

//Task<BaseResponse<IEnumerable<NotificationResponseDto>>> GetNotificationsByUserIdAsync();