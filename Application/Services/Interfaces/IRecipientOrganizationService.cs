using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using static BloodHeroA.DTOs.RecipientOrganizationDTO;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IRecipientOrganizationService
    {
        Task<BaseResponse<IEnumerable<RecipientResponseDto>>> GetAllAsync();
        Task<BaseResponse<RecipientResponseDto?>> GetByIdAsync(Guid id);
        Task<BaseResponse<RecipientResponseDto?>> GetByUserIdAsync(Guid userId);
        Task<BaseResponse<RecipientResponseDto>> AddAsync(RecipientOrganizationDTO recipientOrganizationDTO);
        Task<BaseResponse<RecipientResponseDto>> UpdateAsync(RecipientUpdateDto updateDto);
        Task<BaseResponse<bool>> DeleteAsync(Guid id);
    }
}
