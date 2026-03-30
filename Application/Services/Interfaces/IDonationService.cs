using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using System.Linq.Expressions;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IDonationService
    {
        Task<BaseResponse<DonationResponseDto?>> GetByIdAsync(Guid id);
        Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetAllAsync();
        Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetAllUntestedDonationsAsync();
        Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetDonationsForStorageAsync();

        Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetDonationsByDonorIdAsync(Guid donorId);
        Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetDonationsByDonorOrganizationIdAsync(Guid donorOrganizatioIdId);
        Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetDonationsByBankingOrganizationIdAsync(Guid BankingOrganizatioId);
        Task<BaseResponse<DonationResponseDto>> CreateAsync(DonationDTO donationDto );
        Task<BaseResponse<bool>> DeleteAsync(Guid id);
        Task<BaseResponse<int>> GetTotalDonationsAsync();
        Task<BaseResponse<int>> GetHealthyDonationsAsync();
        Task<BaseResponse<int>> GetSuccessfulDonationsAsync();
        Task<BaseResponse<int>> GetTotalDonationsByBankingOrganizationIdAsync();
        Task<BaseResponse<int>> GetHealthyDonationsByBankingOrganizationIdAsync();
        Task<BaseResponse<int>> GetSuccessfulDonationsByBankingOrganizationIdAsync();
        Task<BaseResponse<int>> GetTotalDonationsByDonorOrganizationIdAsync();
        Task<BaseResponse<int>> GetHealthyDonationsByDonorOrganizationIdAsync();
        Task<BaseResponse<int>> GetSuccessfulDonationsByDonorOrganizationIdAsync();
        Task<BaseResponse<int>> GetTotalDonationsByDonorIdAsync();
        Task<BaseResponse<int>> GetHealthyDonationsByDonorIdAsync();
        Task<BaseResponse<int>> GetSuccessfulDonationsByDonorIdAsync();
    }
}

