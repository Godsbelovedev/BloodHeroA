using BloodHeroA.Models.Entities;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface INotificationRepository
    {
        Task<Notification?> GetByIdAsync(Guid id);
        Task AddAsync(Notification notification);
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<int> GetUnreadNotificationsCountByUserIdAsync(Guid userId);
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);
    }
}
