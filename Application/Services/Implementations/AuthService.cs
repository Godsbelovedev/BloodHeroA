using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Principal;

namespace BloodHeroA.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        public AuthService(IHttpContextAccessor httpContextAccessor, 
                           IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<UserDTO?> GetCurrentUser()
        {
            var idFromHttp = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isValidId = Guid.TryParse(idFromHttp, out Guid userId);

            if(!isValidId)
            {
                return null;
            }

            var currentUser = await _userRepository.GetUserByIdAsync(userId);

            if(currentUser is null)
            {
                return null;
            }
            return new UserDTO
            {
                DateCreated = currentUser.CreatedAt,
                Email = currentUser.Email,
                FullName = currentUser.FullName,
                Id = currentUser.Id,
                Password = currentUser.HashPassWord,
                Role = currentUser.Role
            };
        }

        public async Task SignInAsync(UserDTO userDTO)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDTO.Id.ToString()),
                new Claim(ClaimTypes.Email, userDTO.Email),
                new Claim(ClaimTypes.Role, userDTO.Role.ToString())
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principals = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principals);
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
