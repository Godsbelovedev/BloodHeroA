using BloodHeroA.Data;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
            .Where(i => !i.IsDeleted).ToListAsync();
        }

        public async Task<User?> GetUserAsync(Expression<Func<User, bool>> exp)
        {
            return await _context.Users.FirstOrDefaultAsync(exp);
        }

        public async Task<User?> GetUserByIdAsync(Guid Id)
        {
            return await _context.Users
            .FirstOrDefaultAsync(r => r.Id == Id && !r.IsDeleted);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>> exp)
        {
            return await _context.Users.Where(exp).ToListAsync();
        }

        public async Task<User?> LoginAsync(string email, string Password)
        {
            return await _context.Users.FirstOrDefaultAsync
             (e => e.Email == email && e.HashPassWord == Password && !e.IsDeleted);
        }

        public void UpdateUserAsync(User user)
        {
             _context.Users.Update(user);
        }
    }
}
