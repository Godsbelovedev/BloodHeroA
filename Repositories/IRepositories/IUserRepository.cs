using BloodHeroA.Models.Entities;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<User?> GetUserByIdAsync(Guid Id);
        Task<User?> GetUserAsync(Expression<Func<User, bool>> exp);
        Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>> exp);
        //Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        void UpdateUserAsync(User user);
        Task<User?> LoginAsync(string email, string password);
    }
}
