using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using System.Linq.Expressions;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IDonationRequestService
    {
        Task<BaseResponse<DonationRequestResponseDto?>> GetByIdAsync(Guid id);
        Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>> 
            GetBankingOrganizationSentRequestsByStatusAsync(Status status);
        Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>> GetByStatusAsync(Status status);
        Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>> GetByStatusForSupplyAsync(Status status);
        Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>> MakeRequestByRecipientOrganizationAsync
                              (DonationRequestDto donationRequest);
        Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>> MakeRequestByBankingOrganizationAsync
                              (DonationRequestDto donationRequest);
        Task<BaseResponse<int>> NumberOfPendingRequestByRecipientOrganizationIdRequestAsync();
        Task<BaseResponse<int>> NumberOfIncompletedRequestByRecipientOrganizationIdRequestAsync();
        Task<BaseResponse<int>> NumberOfCompletedRequestByRecipientOrganizationIdRequestAsync();
        Task<BaseResponse<int>> NumberOfPendingRequestByBankingOrganizationIdRequestAsync();
        Task<BaseResponse<int>> NumberOfIncompletedRequestByBankingOrganizationIdRequestAsync();
        Task<BaseResponse<int>> NumberOfCompletedRequestByBankingOrganizationIdRequestAsync();

        //Task<DonationRequest?> GetDonationRequestAsync(Expression<Func<User, bool>> exp);
    }
}
