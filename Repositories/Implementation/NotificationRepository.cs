using BloodHeroA.Data;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BloodHeroA.Repositories.Implementation
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications
            .Where(i => !i.IsDeleted).AsNoTracking().ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(Guid id)
        {
            return await _context.Notifications
           .SingleOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }
        public async Task<int> GetUnreadNotificationsCountByUserIdAsync(Guid userId)
        {
            return await _context.Notifications
            .CountAsync(i => i.ReceiverId == userId && 
            !i.IsDeleted && !i.IsRead);
        }
        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return await _context.Notifications.Include(r => r.Receiver)
            .Where(i => i.ReceiverId == userId && !i.IsDeleted).ToListAsync();
        }

    }
}
