using BloodHeroA.DTOs;
using BloodHeroA.Models.Enums;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<UserDTO?>> Login(UserLoginModel loginModel);
        Task<BaseResponse<UserDTO?>> GetById(Guid id);
        //Task<BaseResponse<UserDTO?>> GetByEmail(string email);
        Task<BaseResponse<UserDTO?>> ChangePassword(PasswordUpdateModel model);
        //Task<BaseResponse<bool>> DeleteAsync(Guid id);
    }
}
