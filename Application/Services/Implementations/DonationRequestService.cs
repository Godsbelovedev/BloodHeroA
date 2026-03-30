using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using BloodHeroA.Repositories.IRepositories;

namespace BloodHeroA.Application.Services.Implementations
{
    public class DonationRequestService : IDonationRequestService
    {
        private readonly IDonationRequestRepository _donationRequest;
        private readonly IAuthService _authService;
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IRecipientOrganizationRepository _recipientOrganization;
        private readonly IBankingOrganizationRepository _bankingOrganization;
        private readonly INotificationService _notificationService;


        public DonationRequestService(IDonationRequestRepository donationRequest,
                      IAuthService authService, IUnitOfWorkRepository unitOfWork,
                      IRecipientOrganizationRepository recipientOrganization,
                      IBankingOrganizationRepository bankingOrganization,
                      INotificationService notificationService)
        {
            _donationRequest = donationRequest;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _recipientOrganization = recipientOrganization;
            _bankingOrganization = bankingOrganization;
            _notificationService = notificationService;
        }

        public async Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>> 
            MakeRequestByBankingOrganizationAsync(DonationRequestDto donationRequest)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return new BaseResponse<IEnumerable<DonationRequestResponseDto>>
                {
                    Data = null,
                    Message = "user not authenticated",
                    Status = false
                };
            }
            var bankingOrganizationToRecieve = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (bankingOrganizationToRecieve is null)
            {
                return new BaseResponse<IEnumerable<DonationRequestResponseDto>>
                {
                    Data = null,
                    Message = "organization not found",
                    Status = false
                };
            }

            var bankingOrganizations = await _bankingOrganization.GetAllAsync();
            if (!bankingOrganizations.Any())
            {
                return new BaseResponse<IEnumerable<DonationRequestResponseDto>>

                {
                    Data = null,
                    Message = "no organization found",
                    Status = false
                };
            }
            var requestId = Guid.NewGuid();
            var listOfRequest = new List<DonationRequest>();
            var createdRequestResponse = new List<DonationRequestResponseDto>();

            foreach (var organization in bankingOrganizations)
            {
                var checkDonationRequest = await _donationRequest.FindAsync
                (d => d.RequestStatus == Status.Pending &&
                d.RecipientOrganizationId == bankingOrganizationToRecieve.Id
                && d.BloodTypeNeeded == donationRequest.BloodTypeNeeded
                && d.UnitsRequested == donationRequest.UnitsRequested
                && d.BankingOrganizationId == organization.Id );

                if (checkDonationRequest.Count() > 0 || organization.Id == bankingOrganizationToRecieve.Id)
                    continue;

                var intendedRequest = new DonationRequest
                {
                    Id = requestId,
                    BloodTypeNeeded = donationRequest.BloodTypeNeeded,
                    BankingOrganizationId = organization.Id,
                    Note = donationRequest.Note,
                    RecipientOrganizationId = bankingOrganizationToRecieve.Id,
                    UnitsRequested = donationRequest.UnitsRequested,
                    UnitsRemained = donationRequest.UnitsRequested
                };
                listOfRequest.Add(intendedRequest);

                var notificationDto = new NotificationDTO
                {
                    Message = $"Dear {organization.OrganizationName},\n\n" +
                      $"A blood donation request has been sent by {bankingOrganizationToRecieve.OrganizationName}.\n\n" +
                      $"Blood Type Needed: {intendedRequest.BloodTypeNeeded}\n" +
                      $"Units Requested: {intendedRequest.UnitsRequested}\n" +
                      $"Note: {intendedRequest.Note}\n\n" +
                      $"Please review and respond promptly.\n\n" +
                      $"Regards,\nBlood Hero Admin",
                    Subject = "NEW BLOOD DONATION REQUEST",
                    ReceiverEmail = organization.Email
                };
                await _donationRequest.CreateAsync(intendedRequest);
                await _notificationService.SendNotificationAsync(notificationDto);

                createdRequestResponse.Add(new DonationRequestResponseDto
                {
                    Id = intendedRequest.Id,
                    BloodType = intendedRequest.BloodTypeNeeded,
                    Note = intendedRequest.Note,
                    RecipientOrganizationName = bankingOrganizationToRecieve.OrganizationName,
                    RequestDate = intendedRequest.CreatedAt,
                    UnitsRequested = intendedRequest.UnitsRequested,
                    BankigOrganizationName = organization.OrganizationName,
                });
            }
            await _unitOfWork.SaveChangesAsync();
            if (!listOfRequest.Any())
            {
                return BaseResponse<IEnumerable<DonationRequestResponseDto>>
               .Failure("Request already sent to all organizations");
            }

            return BaseResponse<IEnumerable<DonationRequestResponseDto>>
               .Success(createdRequestResponse, "Request sent to concerned organizations");
        }

        public async Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>>
                        MakeRequestByRecipientOrganizationAsync(DonationRequestDto donationRequest)
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser is null)
            {
                return new BaseResponse<IEnumerable<DonationRequestResponseDto>>
                {
                    Data = null,
                    Message = "user not authenticated",
                    Status = false
                };
            }
            var recipientOrganization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (recipientOrganization is null)
            {
                return new BaseResponse<IEnumerable<DonationRequestResponseDto>>
                {
                    Data = null,
                    Message = "organization not found",
                    Status = false
                };
            }
            
            var bankingOrganizations = await _bankingOrganization.GetAllAsync();
            if(!bankingOrganizations.Any())
            {
                return new BaseResponse<IEnumerable<DonationRequestResponseDto>>

                {
                    Data = null,
                    Message = "no organization found",
                    Status = false
                };
            }
            var requestId = Guid.NewGuid();
            var listOfRequest = new List<DonationRequest>();
            var createdRequestResponse = new List<DonationRequestResponseDto>();

            foreach (var organization in bankingOrganizations)
            {
                var checkDonationRequest = await _donationRequest.FindAsync
                (d => d.RequestStatus == Status.Pending &&
                d.RecipientOrganizationId == recipientOrganization.Id
                && d.BloodTypeNeeded == donationRequest.BloodTypeNeeded
                && d.UnitsRequested == donationRequest.UnitsRequested
                && d.BankingOrganizationId == organization.Id);

                if (checkDonationRequest.Count() > 0)
                    continue;
                
                var intendedRequest = new DonationRequest
                {
                    Id = requestId,
                    BloodTypeNeeded = donationRequest.BloodTypeNeeded,
                    BankingOrganizationId = organization.Id,
                    Note = donationRequest.Note,
                    RecipientOrganizationId = recipientOrganization.Id,
                    UnitsRequested = donationRequest.UnitsRequested,
                    UnitsRemained = donationRequest.UnitsRequested
                };
                listOfRequest.Add(intendedRequest);

                var notificationDto = new NotificationDTO
                {
                    Message = $"Dear {organization.OrganizationName},\r\n\r\n" +
                      $"A blood donation request has been sent by {recipientOrganization.OrganizationName}.\r\n\r\n" +
                      $"Blood Type Needed: {intendedRequest.BloodTypeNeeded}\r\n\r\n" +
                      $"Units Requested: {intendedRequest.UnitsRequested}\r\n\r\n" +
                      $"Note: {intendedRequest.Note}\r\n\r\n" +
                      $"Please review and respond promptly.\r\n\r\n" +
                      $"Regards,\r\n\r\nBlood Hero Admin",
                    Subject = "NEW BLOOD DONATION REQUEST",
                    ReceiverEmail = organization.Email
                };
                await _donationRequest.CreateAsync(intendedRequest);
                await _unitOfWork.SaveChangesAsync();
                await _notificationService.SendNotificationAsync(notificationDto);

                createdRequestResponse.Add(new DonationRequestResponseDto
                {
                    Id = intendedRequest.Id,
                    BloodType = intendedRequest.BloodTypeNeeded,
                    Note = intendedRequest.Note,
                    RecipientOrganizationName = recipientOrganization.OrganizationName,
                    RequestDate = intendedRequest.CreatedAt,
                    UnitsRequested = intendedRequest.UnitsRequested,
                    BankigOrganizationName = organization.OrganizationName,
                });
            }
            await _unitOfWork.SaveChangesAsync();
            if (!listOfRequest.Any())
            {
                return BaseResponse<IEnumerable<DonationRequestResponseDto>>
               .Failure("Request already sent to all organizations");
            }

            return BaseResponse<IEnumerable<DonationRequestResponseDto>>
               .Success(createdRequestResponse, "Request sent to concerned organizations");
        }

        public async Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>>
        GetBankingOrganizationSentRequestsByStatusAsync(Status status)
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser == null)
            {
                return BaseResponse<IEnumerable<DonationRequestResponseDto>>
                                .Failure("user not aunthenticated");
            }
            var organization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if(organization == null)
            {
                return BaseResponse<IEnumerable<DonationRequestResponseDto>>
                                .Failure("organization not found");
            }
            var donationRequests = await _donationRequest.FindAsync
              (r => r.RequestStatus == status && r.RecipientOrganizationId == organization.Id);

            if(!donationRequests.Any())
            {
                return BaseResponse<IEnumerable<DonationRequestResponseDto>>
                                 .Failure("no record found");
            }
            var listOfAllRequests = new List<DonationRequestResponseDto>();
            foreach(var donationRequest in donationRequests)
            {
                listOfAllRequests.Add(new DonationRequestResponseDto
                {
                    Id = donationRequest.Id,
                    BloodType = donationRequest.BloodTypeNeeded,
                    Note = donationRequest.Note,
                    RecipientOrganizationName = donationRequest.RecipientOrganization.OrganizationName,
                    RequestDate = donationRequest.CreatedAt,
                    UnitsRequested = donationRequest.UnitsRequested,
                    BankigOrganizationName = donationRequest.BankingOrganization.OrganizationName
                });
            }
            return new BaseResponse<IEnumerable<DonationRequestResponseDto>>
            {
                Data = listOfAllRequests,
                Message = "retrived successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<DonationRequestResponseDto?>> GetByIdAsync(Guid id)
        {
            var donationRequest = await _donationRequest.GetByIdAsync(id);
            if (donationRequest is null)
            {
                return BaseResponse<DonationRequestResponseDto?>.Failure("no record found");
            }
            var donationRequestResponse = new DonationRequestResponseDto
                {
                    Id = donationRequest.Id,
                    BloodType = donationRequest.BloodTypeNeeded,
                    Note = donationRequest.Note,
                    RecipientOrganizationName = donationRequest.RecipientOrganization.OrganizationName,
                    RequestDate = donationRequest.CreatedAt,
                    UnitsRequested = donationRequest.UnitsRequested,
                    BankigOrganizationName = donationRequest.BankingOrganization.OrganizationName,
                    Status = donationRequest.RequestStatus
            };

            return BaseResponse<DonationRequestResponseDto?>
            .Success(donationRequestResponse, "record retrieved successfully");
        }
        public async Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>> GetByStatusAsync(Status status)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return BaseResponse<IEnumerable<DonationRequestResponseDto>>
                                .Failure("user not authenticated");
            }

            var recipientOrganization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (recipientOrganization is null)
            {
                return BaseResponse<IEnumerable<DonationRequestResponseDto>>
                                .Failure("recipient organization not found");
            }


            var donationRequests = await _donationRequest.FindAsync(d =>
                    d.RequestStatus == status &&
                    !d.IsDeleted &&
                    d.RecipientOrganizationId == recipientOrganization.Id);

            if (!donationRequests.Any())
            {
                return BaseResponse<IEnumerable<DonationRequestResponseDto>>
                                 .Failure("no record found");
            }

            var listOfAllRequests = donationRequests.Select(donationRequest => new DonationRequestResponseDto
            {
                Id = donationRequest.Id,
                BloodType = donationRequest.BloodTypeNeeded,
                Note = donationRequest.Note,
                RecipientOrganizationName = donationRequest.RecipientOrganization?.OrganizationName ?? "",
                RequestDate = donationRequest.CreatedAt,
                UnitsRequested = donationRequest.UnitsRequested,
                BankigOrganizationName = donationRequest.BankingOrganization?.OrganizationName ?? "",
                Status = donationRequest.RequestStatus
            }).DistinctBy(r => r.Id).ToList();

            return new BaseResponse<IEnumerable<DonationRequestResponseDto>>
            {
                Data = listOfAllRequests,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<DonationRequestResponseDto>>>GetByStatusForSupplyAsync(Status status)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return BaseResponse<IEnumerable<DonationRequestResponseDto>>
                                .Failure("user not authenticated");
            }

            var bankingOrganization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (bankingOrganization is null)
            {
                return BaseResponse<IEnumerable<DonationRequestResponseDto>>
                                .Failure("recipient organization not found");
            }


            var donationRequests = await _donationRequest.FindAsync(d =>
                    d.RequestStatus == status &&
                    !d.IsDeleted &&
                    d.BankingOrganizationId == bankingOrganization.Id);

            if (!donationRequests.Any())
            {
                return BaseResponse<IEnumerable<DonationRequestResponseDto>>
                                 .Failure("no record found");
            }

            var listOfAllRequests = donationRequests.Select(donationRequest => new DonationRequestResponseDto
            {
                Id = donationRequest.Id,
                BloodType = donationRequest.BloodTypeNeeded,
                Note = donationRequest.Note,
                RecipientOrganizationName = donationRequest.RecipientOrganization?.OrganizationName ?? "",
                RequestDate = donationRequest.CreatedAt,
                UnitsRequested = donationRequest.UnitsRequested,
                BankigOrganizationName = donationRequest.BankingOrganization?.OrganizationName ?? "",
                Status = donationRequest.RequestStatus
            }).ToList();

            return new BaseResponse<IEnumerable<DonationRequestResponseDto>>
            {
                Data = listOfAllRequests,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<int>> NumberOfCompletedRequestByBankingOrganizationIdRequestAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser == null)
            {
                return BaseResponse<int>.Failure("user not authenticated");
            }
            var bankingOrganization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if(bankingOrganization == null)
            {
                return BaseResponse<int>.Failure("banking organization not found");
            }
            var numberOfRequest = await _donationRequest.FindCountAsync
                (r => r.RequestStatus == Status.Completed 
                && r.BankingOrganizationId == bankingOrganization.Id);
            if(numberOfRequest == 0)
            {
                return BaseResponse<int>.Failure("no request made");
            }
            return BaseResponse<int>.Success(numberOfRequest);
        }

        public async Task<BaseResponse<int>> NumberOfCompletedRequestByRecipientOrganizationIdRequestAsync()
        {

            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                return BaseResponse<int>.Failure("user not authenticated");
            }
            var recipientOrganization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (recipientOrganization == null)
            {
                return BaseResponse<int>.Failure("banking organization not found");
            }
            var numberOfRequest = await _donationRequest.FindCountAsync
                (r => r.RequestStatus == Status.Completed
                && r.RecipientOrganizationId == recipientOrganization.Id);
            if (numberOfRequest == 0)
            {
                return BaseResponse<int>.Failure("no request made");
            }
            return BaseResponse<int>.Success(numberOfRequest);
        }

        public async Task<BaseResponse<int>> NumberOfIncompletedRequestByBankingOrganizationIdRequestAsync()
        {

            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                return BaseResponse<int>.Failure("user not authenticated");
            }
            var bankingOrganization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (bankingOrganization == null)
            {
                return BaseResponse<int>.Failure("banking organization not found");
            }
            var numberOfRequest = await _donationRequest.FindCountAsync
                (r => r.RequestStatus == Status.InProgress
                && r.BankingOrganizationId == bankingOrganization.Id);
            if (numberOfRequest == 0)
            {
                return BaseResponse<int>.Failure("no request made");
            }
            return BaseResponse<int>.Success(numberOfRequest);
        }

        public async Task<BaseResponse<int>> NumberOfIncompletedRequestByRecipientOrganizationIdRequestAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                return BaseResponse<int>.Failure("user not authenticated");
            }
            var recipientOrganization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (recipientOrganization == null)
            {
                return BaseResponse<int>.Failure("banking organization not found");
            }
            var numberOfRequest = await _donationRequest.FindCountAsync
                (r => r.RequestStatus == Status.InProgress
                && r.RecipientOrganizationId == recipientOrganization.Id);
            if (numberOfRequest == 0)
            {
                return BaseResponse<int>.Failure("no request made");
            }
            return BaseResponse<int>.Success(numberOfRequest);
        }

        public async Task<BaseResponse<int>> NumberOfPendingRequestByBankingOrganizationIdRequestAsync()
        {

            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                return BaseResponse<int>.Failure("user not authenticated");
            }
            var bankingOrganization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (bankingOrganization == null)
            {
                return BaseResponse<int>.Failure("banking organization not found");
            }
            var numberOfRequest = await _donationRequest.FindCountAsync
                (r => r.RequestStatus == Status.Pending
                && r.BankingOrganizationId == bankingOrganization.Id);
            if (numberOfRequest == 0)
            {
                return BaseResponse<int>.Failure("no request made");
            }
            return BaseResponse<int>.Success(numberOfRequest);
        }

        public async Task<BaseResponse<int>> NumberOfPendingRequestByRecipientOrganizationIdRequestAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                return BaseResponse<int>.Failure("user not authenticated");
            }
            var recipientOrganization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (recipientOrganization == null)
            {
                return BaseResponse<int>.Failure("banking organization not found");
            }
            var numberOfRequest = await _donationRequest.FindCountAsync
                (r => r.RequestStatus == Status.Pending
                && r.RecipientOrganizationId == recipientOrganization.Id);
            if (numberOfRequest == 0)
            {
                return BaseResponse<int>.Failure("no request made");
            }
            return BaseResponse<int>.Success(numberOfRequest);
        }
        //public Task<DonationRequest?> GetDonationRequestAsync(Expression<Func<User, bool>> exp)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
