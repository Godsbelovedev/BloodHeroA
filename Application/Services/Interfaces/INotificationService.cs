using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<BaseResponse<NotificationResponseDto?>> GetByIdAsync(Guid id);
        Task<BaseResponse<NotificationResponseDto>>
                        SendNotificationAsync(NotificationDTO dTO);
        // Task<BaseResponse<IEnumerable<NotificationResponseDto>>> GetAllAsync();
        Task MarkMessageAsRead(Guid id);
        Task<BaseResponse<IEnumerable<NotificationResponseDto>>> GetNotificationsByUserIdAsync();
        Task<int> GetUnreadNotificationsCountByUserIdAsync(Guid userId);
        //Task<BaseResponse<bool>> MarkNotificationAsReadAsync(Guid id);
    }
}
