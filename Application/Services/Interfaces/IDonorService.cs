using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using System.Linq.Expressions;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IDonorService
    {
        Task<BaseResponse<DonorResponseDto?>> GetByIdAsync(Guid id);

        Task<BaseResponse<DonorResponseDto?>> GetByUserIdAsync(Guid userId);

        Task<BaseResponse<DonorResponseDto>> SelfRegisterDonorAsync(DonorRequestDto donorDto);

        Task<BaseResponse<DonorResponseDto>> RegisterDonorByOrganizationAsync(DonorRequestDto donorDto);

        Task<BaseResponse<DonorResponseDto?>> UpdateAsync(DonorUpdateDto donor);

        Task<BaseResponse<bool>> DeleteAsync(Guid id);

        Task<BaseResponse<IEnumerable<DonorResponseDto>>> GetAvailableDonorsAsync();

        Task<BaseResponse<IEnumerable<DonorResponseDto>>> 
            GetAvailableDonorsByDonorOrganizationIdAsync(Guid id);

        Task<BaseResponse<IEnumerable<DonorResponseDto>>> 
            GetDonorsByDonorOrganizationIdAsync();

        Task<BaseResponse<IEnumerable<DonorResponseDto>>> GetAllAsync();
    }
}
