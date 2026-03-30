using BloodHeroA.DTOs;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IBloodTestResultService
    {
        Task<BaseResponse<BloodTestResultResponseDto>> CreateAsync(BloodTestResultDTO bloodTestResult);
        Task<BaseResponse<BloodTestResultResponseDto>> UpdateAsync(BloodTestResultUpdateDTO bloodTestResultUpdate);
        Task<BaseResponse<IEnumerable<BloodTestResultResponseDto>>> GetAllTestByBankingOrganizationAsync();
        Task<BaseResponse<BloodTestResultResponseDto?>> GetByDonationIdAsync(Guid donationId);
        Task<BaseResponse<bool>> DeleteAsync(Guid donationId);
    }
}
