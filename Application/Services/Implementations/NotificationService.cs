using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using BloodHeroA.Repositories.IRepositories;

namespace BloodHeroA.Application.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;
        public NotificationService(IUserRepository userRepository,
            INotificationRepository notificationRepository,
            IUnitOfWorkRepository unitOfWork,
            IEmailService emailService,
            IAuthService authService)
        {
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _authService = authService;
        }

        public async Task<BaseResponse<NotificationResponseDto>> 
                        SendNotificationAsync(NotificationDTO dTO)
        {
            var currentUser = await _authService.GetCurrentUser();

            Guid senderId;
            string adminIdInString = "dd38778b-eab3-4107-82f3-81e2c9d0f4d9";
            bool IsValidAdminId = Guid.TryParse(adminIdInString, out Guid adminId);

            string senderName;
            string senderEmail;

            if (currentUser == null || currentUser.Role == Role.Admin)
            {
                senderName = "Admin";
                senderEmail = "admin@bloodhero.com";
                senderId = adminId;
            }
            else
            {
                senderName = currentUser.FullName;
                senderEmail = currentUser.Email;
                senderId = currentUser.Id;
            }
            var receiver = await _userRepository.GetUserAsync(d => d.Email == dTO.ReceiverEmail);
            if (receiver is null)
            {
                return new BaseResponse<NotificationResponseDto>
                {
                    Data = null,
                    Message = "receiver not found",
                    Status = false
                };
            }

            var notification = new Notification
            {
                Message = dTO.Message,
                ReceiverId = receiver.Id,
                SenderId = senderId,
                Subject = dTO.Subject
            };
            await _emailService.SendAsync(receiver.Email, dTO.Subject, dTO.Message);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            var dto = new NotificationResponseDto
            {

                Id = notification.Id,
                Message = notification.Message,
                SentDate = notification.DateSent,
                SenderName = senderName,
                Subject = dTO.Subject,
                SenderEmail = senderEmail
            };
            return BaseResponse<NotificationResponseDto>.Success(dto);

        }

        //public async Task<BaseResponse<IEnumerable<NotificationResponseDto>>> GetAllAsync()
        //{
        //    var allNotifications = await _notificationRepository.GetAllAsync();
        //    if(!allNotifications.Any())
        //    {
        //        return new BaseResponse<IEnumerable<NotificationResponseDto>>
        //        {
        //            Data = null,
        //            Message = "no record found",
        //            Status = false
        //        };
        //    }
        //    var listOfMesaages = new List<NotificationResponseDto>();
        //    foreach (var notification in allNotifications)
        //    {
        //        listOfMesaages.Add(new NotificationResponseDto
        //        {
        //            Message = notification.Message,
        //            Id = notification.Id,
        //            Subject = notification.Subject,
        //            SenderName = notification.Sender!.FullName,
        //            SentDate = notification.DateSent
        //    });
        //    }
        //    return new BaseResponse<IEnumerable<NotificationResponseDto>>
        //    {
        //        Data = listOfMesaages,
        //        Message = "retrieved successfully",
        //        Status = true
        //    };
        //}

        public async Task<BaseResponse<NotificationResponseDto?>> GetByIdAsync(Guid id)
        {
            var checkNotification = await _notificationRepository.GetByIdAsync(id);
            if (checkNotification is null)
            {
                return new BaseResponse<NotificationResponseDto?>
                {
                    Data = null,
                    Message = "no record found",
                    Status = false
                };
            }
            if(checkNotification.IsRead == false)
            {
                checkNotification.IsRead = true;
                await _unitOfWork.SaveChangesAsync();
            }
            var sender = await _userRepository.GetUserByIdAsync(checkNotification.SenderId);
            if (sender is null)
            {
                return new BaseResponse<NotificationResponseDto?>
                {
                    Data = null,
                    Message = "sender not found",
                    Status = false
                };
            }
            var notification = new NotificationResponseDto
            {
                Message = checkNotification.Message,
                Id = checkNotification.Id,
                Subject = checkNotification.Subject,
                SenderName = checkNotification.Sender!.FullName,
                SentDate = checkNotification.DateSent,
                SenderEmail = sender.Email,
                IsRead = checkNotification.IsRead
            };
        
            return new BaseResponse<NotificationResponseDto?>
            {
                Data = notification,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<NotificationResponseDto>>> GetNotificationsByUserIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser is null)
            {
                return new BaseResponse<IEnumerable<NotificationResponseDto>>
                {
                    Data = null,
                    Message = "user not authenticated",
                    Status = false
                };
            }

            var allNotifications = await _notificationRepository.GetNotificationsByUserIdAsync(currentUser.Id);
            if (!allNotifications.Any())
            {
                return new BaseResponse<IEnumerable<NotificationResponseDto>>
                {
                    Data = null,
                    Message = "no record found",
                    Status = false
                };
            }
            var listOfMessages = new List<NotificationResponseDto>();
            foreach (var notification in allNotifications)
            {
                var sender = await _userRepository.GetUserByIdAsync(notification.SenderId);
               
                listOfMessages.Add(new NotificationResponseDto
                {
                    Message = notification.Message,
                    Id = notification.Id,
                    Subject = notification.Subject,
                    SenderName = notification.Sender!.FullName,
                    SentDate = notification.DateSent,
                    SenderEmail = sender!.Email,
                    IsRead = notification.IsRead,
                    Role = currentUser.Role
                });
            }
            return new BaseResponse<IEnumerable<NotificationResponseDto>>
            {
                Data = listOfMessages,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<int> GetUnreadNotificationsCountByUserIdAsync(Guid userId)
        {
                return await _notificationRepository.
                GetUnreadNotificationsCountByUserIdAsync(userId);
        }

        public async Task MarkMessageAsRead(Guid id)
        {
            var messageToMark = await _notificationRepository.GetByIdAsync(id);
            if(messageToMark != null)
                messageToMark.IsRead = true;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
