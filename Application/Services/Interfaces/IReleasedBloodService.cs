

using BloodHeroA.DTOs;
using BloodHeroA.Models.Enums;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IReleasedBloodService
    {
        Task<BaseResponse<ReleasedBloodResponseDto>>
        CreateAsync(ReleasedBloodRequestDto releasedBlood);

        Task<BaseResponse<IEnumerable<ReleasedBloodResponseDto>>>
               GetReleasedByTypeAsync(BloodGroup bloodGroup);

        Task<BaseResponse<ReleasedBloodResponseDto?>> GetByIdAsync(Guid id);
    }
}
//Task<BaseResponse<ReleasedBloodResponseDto>> CreateAsync(ReleasedBloodDTO dto);

//Task<BaseResponse<IEnumerable<ReleasedBloodResponseDto>>> GetAllAsync();

//Task<BaseResponse<IEnumerable<ReleasedBloodResponseDto>>> GetByRequestIdAsync(Guid requestId);