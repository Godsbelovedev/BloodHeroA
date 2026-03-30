using BCrypt.Net;
using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;

namespace BloodHeroA.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWorkRepository _iUnitOfWork;
        public UserService(IUserRepository userRepository, IAuthService authService, IUnitOfWorkRepository iUnitOfWork)
        {
            _userRepository = userRepository;
            _authService = authService;
            _iUnitOfWork = iUnitOfWork;
        }

        public async Task<BaseResponse<UserDTO?>> ChangePassword(PasswordUpdateModel model)
        {
            var response = new BaseResponse<UserDTO?>();
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return new BaseResponse<UserDTO?>
                {
                    Data = null,
                    Message = "User not Authenticated",
                    Status = false
                };
            }
            var verifyPassword = BCrypt.Net.BCrypt.Verify(model.CurrentPassword, currentUser.Password);
            if (!verifyPassword)
            {
                return new BaseResponse<UserDTO?> { Status = false, Message = "Current password is incorrect" };
            }
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    return new BaseResponse<UserDTO?>
                    {
                        Data = null,
                        Message = "password mismatched",
                        Status = false
                    };
                }
                var password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                currentUser.Password = password;
            }
           
            await _iUnitOfWork.SaveChangesAsync();

            return new BaseResponse<UserDTO?>
            {
                Status = true,
                Message = "Password changed successfully",
                Data = new UserDTO
                {
                    Email = currentUser.Email,
                    Password = currentUser.Password,
                    DateCreated = currentUser.DateCreated,
                    FullName = currentUser.FullName,
                    Id = currentUser.Id,
                    IsDeletd = currentUser.IsDeletd,
                    Role = currentUser.Role
                }
            };
        }

        //public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        //{
        //    var userToDelete = await _userRepository.GetUserByIdAsync(id);
        //    if (userToDelete is null)
        //    {
        //        return BaseResponse<bool>.Failure("user do not exist");
        //    }
        //    userToDelete.IsDeleted = true;
        //    await _iUnitOfWork.SaveChangesAsync();
        //    return BaseResponse<bool>.Success(true);
        //}

        public async Task<BaseResponse<UserDTO?>> GetById(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user is null)
            {
                return new BaseResponse<UserDTO?>
                {
                    Data = null,
                    Message = "user not found",
                    Status = false
                };
            }
            return new BaseResponse<UserDTO?>
            {
                Data = new UserDTO
                {
                    DateCreated = user.CreatedAt,
                    Email = user.Email,
                    FullName = user.FullName,
                    Id = user.Id,
                    IsDeletd = user.IsDeleted,
                    Password = user.HashPassWord,
                    Role = user.Role
                },
                Message = "retrieved ",
                Status = false
            };
        }



        public async Task<BaseResponse<UserDTO?>> Login(UserLoginModel loginModel)
        {
            var loginUser = await _userRepository.GetUserAsync
               (u => u.Email == loginModel.Email && !u.IsDeleted);

            var isValidPassWord = BCrypt.Net.BCrypt.Verify(loginModel.PassWord, loginUser!.HashPassWord);
            if (loginUser is null && !isValidPassWord)
            {
                return new BaseResponse<UserDTO?>
                {
                    Data = null,
                    Message = "invalid Credentials",
                    Status = false
                };
            }
            var userDto = new UserDTO
            {
                DateCreated = loginUser!.CreatedAt,
                Email = loginUser.Email,
                FullName = loginUser.FullName,
                Id = loginUser.Id,
                IsDeletd = loginUser.IsDeleted,
                Password = loginUser.HashPassWord,
                Role = loginUser.Role
            };
            await _authService.SignInAsync(userDto);
            return new BaseResponse<UserDTO?>
            {
                Data = userDto,
                Message = "Login successful",
                Status = true
            };
        }
    }
}
