using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IDonorOrganizationService
    {
        Task<BaseResponse<IEnumerable<DonorOrganizationResponseDto>>> GetAllAsync();
        Task<BaseResponse<IEnumerable<DonorOrganizationResponseDto>>> GetAllForDonationAsync();
        Task<BaseResponse<DonorOrganizationResponseDto?>> GetByIdAsync(Guid id);
        Task<BaseResponse<DonorOrganizationResponseDto?>> GetByUserIdAsync(Guid userId);
        Task<BaseResponse<DonorOrganizationResponseDto>> AddAsync(DonorOrganizationRequestDto organization);
        Task<BaseResponse<DonorOrganizationResponseDto>> UpdateAsync(DonorOrganizationUpdateDto organization);
        Task<BaseResponse<bool>> DeleteAsync(Guid id);
    }
}
