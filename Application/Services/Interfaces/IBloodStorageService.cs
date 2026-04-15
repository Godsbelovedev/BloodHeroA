using BloodHeroA.DTOs;
using BloodHeroA.Models.Enums;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IBloodStorageService
    {
        Task<BaseResponse<BloodStorageResponseDto>> 
        CreateStorageAsync(BloodStorageDTO bloodStorage);
       
        Task<BaseResponse<IEnumerable<BloodStorageResponseDto>>>
                GetForSupplyAsync(BloodGroup bloodGroup);
        Task<BaseResponse<IEnumerable<BloodStorageResponseDto>>>
        GetStoragesForMultiSupplyAsync(BloodGroup bloodGroup);
        Task<BaseResponse<BloodStorageResponseDto?>> GetByIdAsync(Guid id);
        Task<BaseResponse<bool?>> DeleteAsync(Guid id);
        Task<BaseResponse<IEnumerable<BloodStorageResponseDto>>> GetExpiredAsync();
        Task UpdateExpiredBloodCountAsync();
    }
}

