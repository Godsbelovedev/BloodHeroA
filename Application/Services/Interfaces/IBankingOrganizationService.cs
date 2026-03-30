using BloodHeroA.DTOs;
using static BloodHeroA.DTOs.RecipientOrganizationDTO;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IBankingOrganizationService
    {
        Task<BaseResponse<IEnumerable<BankingOrganizationResponseDto>>> GetAllAsync();
        Task<BaseResponse<BankingOrganizationResponseDto?>> GetByIdAsync(Guid id);
        Task<BaseResponse<BankingOrganizationResponseDto?>> GetByUserIdAsync(Guid userId);
        Task<BaseResponse<BankingOrganizationResponseDto>>  AddAsync(BankingOrganizationDTO bankingOrganizationDTO);
        Task<BaseResponse<BankingOrganizationResponseDto>> UpdateAsync(BankingOrganizationUpdateDto updateDto);
        Task<BaseResponse<bool>> DeleteAsync(Guid id);
    }
}
