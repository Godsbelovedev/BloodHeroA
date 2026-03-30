using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task SignInAsync(UserDTO userDTO);
        Task SignOutAsync();
        Task<UserDTO?> GetCurrentUser();
    }
}
