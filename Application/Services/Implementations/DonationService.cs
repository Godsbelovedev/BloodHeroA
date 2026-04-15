using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.Implementation;
using BloodHeroA.Repositories.IRepositories;
using System.Drawing;

namespace BloodHeroA.Application.Services.Implementations
{
    public class DonationService : IDonationService
    {
        private readonly IDonationRepository _donationRepository;
        private readonly IBankingOrganizationRepository _bankingRepository;
        private readonly IDonorOrganizationRepository _donorOrganizationRepository;
        private readonly IDonorRepository _donorRepository;
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;


        public DonationService(IDonationRepository donationRepository,
            IUnitOfWorkRepository unitOfWork, IAuthService authService,
            IBankingOrganizationRepository bankingRepository,
            IDonorRepository donorRepository,
            IDonorOrganizationRepository donorOrganizationRepository, 
            INotificationService notificationService)
        {
            _donationRepository = donationRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _bankingRepository = bankingRepository;
            _donorRepository = donorRepository;
            _notificationService = notificationService;
            _donorOrganizationRepository = donorOrganizationRepository;
        }

        public async Task<BaseResponse<DonationResponseDto>> CreateAsync(DonationDTO donationDto)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return BaseResponse<DonationResponseDto>.Failure("User not authenticated");
            }
            var bankingOrganization = await _bankingRepository.GetByUserIdAsync(currentUser.Id);

            if (bankingOrganization is null)
            {
                return BaseResponse<DonationResponseDto>.Failure("banking organization not found");
            }
            var donor = await _donorRepository.GetByIdAsync(donationDto.DonorId);
            if (donor is null)
            {
                return BaseResponse<DonationResponseDto>.Failure("donor not found");
            }
            if (donor.NextDueDonationDate > DateTime.UtcNow)
            {
                return BaseResponse<DonationResponseDto>.Failure("not due for next donation");
            }

            if (!donationDto.IsSuccessful)
            {
                return BaseResponse<DonationResponseDto>.Failure("donation not successfull");
            }

            var donation = new Donation
            {
                BankingOrganizationId = bankingOrganization.Id,
                DonorId = donor.Id,
                DonatedBloodType = donor.BloodGroup,
                DonationRemark = donationDto.DonationRemark,
                UnitsDonated = 1,
                DonorOrganizationId = donor.DonorOrganizationId,
                IsSuccessful = donationDto.IsSuccessful
            };
            bankingOrganization.TotalDonations += 1;
            donor.LastDonationDate = DateTime.UtcNow;
            donor.TotalDonations += donation.UnitsDonated;
            
            if(donor.DonorOrganization is not null)
            {
               donor.DonorOrganization.TotalDonations += donation.UnitsDonated;
            }
            donor.NextDueDonationDate = donation.CreatedAt.AddMinutes(6);
            await _donationRepository.CreateAsync(donation);
            await _unitOfWork.SaveChangesAsync();

            var notification = new NotificationDTO
            {
                Subject = " Your Donation Can Save Lives!\r\n\r\n",

                Message = $"   Dear {donor.FirstName + " " + donor.LastName}," +
                          $"\r\n\r\nThank you for donating blood today. " +
                          $"Your generosity has the power to save lives and " +
                          $"bring hope to patients and families in need." +
                          $"\r\n\r\n We are grateful for your support and look" +
                          $" forward to seeing you again in future donation drives." +
                          $"\r\n\r\nBloodHero Team",
                ReceiverEmail = donor.Email,
                SendererEmail = currentUser.Email
            };
            await _notificationService.SendNotificationAsync(notification);

            return new BaseResponse<DonationResponseDto>
            {
                Data = new DonationResponseDto
                {
                   Id = donation.Id,
                   DonorName = $"{donor.FirstName} {donor.MiddleName} {donor.LastName}",
                   BankingOrganizationName = bankingOrganization.OrganizationName,
                   DonorOrganizationName = donor.DonorOrganization?.OrganizationName 
                                                                 ?? "Independent Donor",
                   BloodGroup = donor.BloodGroup,
                   UnitsDonated = donation.UnitsDonated,
                   DonationDate = donation.CreatedAt
                },
                Message = "create successful",
                Status = true
            };
        }


        public async Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetAllAsync()
        {
            var allDonations = await _donationRepository.GetAllAsync();

            if (!allDonations.Any())
            {
                return BaseResponse<IEnumerable<DonationResponseDto>>.
                                       Failure("no record found");
            }

            var donationsList = new List<DonationResponseDto>();
            foreach (var donation in allDonations)
            {
                donationsList.Add(new DonationResponseDto
                {
                    BankingOrganizationName = donation.DonationRemark,
                    BloodGroup = donation.DonatedBloodType,
                    DonationDate = donation.CreatedAt,
                    DonorName = donation.Donor.User.FullName,
                    DonorOrganizationName = donation.DonorOrganization?.OrganizationName
                                                                 ?? "independent donor",
                    Id = donation.Id,
                    UnitsDonated = donation.UnitsDonated
                });
            }

            return new BaseResponse<IEnumerable<DonationResponseDto>>
            {
                Data = donationsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetAllUntestedDonationsAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return BaseResponse<IEnumerable<DonationResponseDto>>
                                         .Failure("User not authenticated");
            }
            var bankingOrganization = await _bankingRepository.GetByUserIdAsync(currentUser.Id);

            if (bankingOrganization is null)
            {
                return BaseResponse<IEnumerable<DonationResponseDto>>
                                         .Failure("banking organization not found");
            }

            var untestedDonations = await _donationRepository
            .FindAsync(d => !d.IsDeleted && !d.IsTested && 
            d.IsSuccessful && d.BankingOrganizationId == bankingOrganization.Id);

            if (!untestedDonations.Any())
            {
                return BaseResponse<IEnumerable<DonationResponseDto>>.
                                       Failure("no record found");
            }

            var donationsList = new List<DonationResponseDto>();
            foreach (var untestedDonation in untestedDonations)
            {
                donationsList.Add(new DonationResponseDto
                {
                    BankingOrganizationName = untestedDonation.DonationRemark,
                    BloodGroup = untestedDonation.DonatedBloodType,
                    DonationDate = untestedDonation.CreatedAt,
                    DonorName = $"{untestedDonation.Donor?.FirstName} {untestedDonation.Donor?.LastName}".Trim(),
                    DonorOrganizationName = untestedDonation.DonorOrganization?.OrganizationName
                                                                 ?? "independent donor",
                    Id = untestedDonation.Id,
                    UnitsDonated = untestedDonation.UnitsDonated,
                });
            }

            return new BaseResponse<IEnumerable<DonationResponseDto>>
            {
                Data = donationsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<DonationResponseDto?>> GetByIdAsync(Guid id)
        {
            var donation = await _donationRepository.GetByIdAsync(id);

            if (donation is null)
            {
                return BaseResponse<DonationResponseDto?>. Failure("no record found");
            }

            var donationResponse = new DonationResponseDto
                {
                    BankingOrganizationName = donation.DonationRemark,
                    BloodGroup = donation.DonatedBloodType,
                    DonationDate = donation.CreatedAt,
                    DonorName = $"{donation.Donor?.FirstName} {donation.Donor?.LastName}".Trim(),
                    DonorOrganizationName = donation.DonorOrganization?.OrganizationName
                                                                 ?? "independent donor",
                    Id = donation.Id,
                    UnitsDonated = donation.UnitsDonated
                };
            

            return new BaseResponse<DonationResponseDto?>
            {
                Data = donationResponse,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<DonationResponseDto>>>
        GetDonationsByBankingOrganizationIdAsync(Guid BankingOrganizatioId)
        {
            var allDonations = await _donationRepository
            .GetDonationsByBankingOrganizationIdAsync(BankingOrganizatioId);

            if (!allDonations.Any())
            {
                return BaseResponse<IEnumerable<DonationResponseDto>>.
                                       Failure("no record found");
            }

            var donationsList = new List<DonationResponseDto>();
            foreach (var donation in allDonations)
            {
                donationsList.Add(new DonationResponseDto
                {
                    BankingOrganizationName = donation.DonationRemark,
                    BloodGroup = donation.DonatedBloodType,
                    DonationDate = donation.CreatedAt,
                    DonorName = $"{donation.Donor?.FirstName} {donation.Donor?.LastName}".Trim(),
                    DonorOrganizationName = donation.DonorOrganization?.OrganizationName
                                                                 ?? "independent donor",
                    Id = donation.Id,
                    UnitsDonated = donation.UnitsDonated
                });
            }

            return new BaseResponse<IEnumerable<DonationResponseDto>>
            {
                Data = donationsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetDonationsByDonorIdAsync(Guid donorId)
        {
            var allDonations = await _donationRepository.GetDonationsByDonorIdAsync(donorId);

            if (!allDonations.Any())
            {
                return BaseResponse<IEnumerable<DonationResponseDto>>.
                                       Failure("no record found");
            }

            var donationsList = new List<DonationResponseDto>();
            foreach (var donation in allDonations)
            {
                donationsList.Add(new DonationResponseDto
                {
                    BankingOrganizationName = donation.DonationRemark,
                    BloodGroup = donation.DonatedBloodType,
                    DonationDate = donation.CreatedAt,
                    DonorName = $"{donation.Donor?.FirstName} {donation.Donor?.LastName}".Trim(),
                    DonorOrganizationName = donation.DonorOrganization?.OrganizationName
                                                                 ?? "independent donor",
                    Id = donation.Id,
                    UnitsDonated = donation.UnitsDonated
                });
            }

            return new BaseResponse<IEnumerable<DonationResponseDto>>
            {
                Data = donationsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<DonationResponseDto>>> 
            GetDonationsByDonorOrganizationIdAsync(Guid donorOrganizatioId)
        {
            var allDonations = await _donationRepository
            .GetDonationsByDonorOrganizationIdAsync(donorOrganizatioId);

            if (!allDonations.Any())
            {
                return BaseResponse<IEnumerable<DonationResponseDto>>.
                                       Failure("no record found");
            }

            var donationsList = new List<DonationResponseDto>();
            foreach (var donation in allDonations)
            {
                donationsList.Add(new DonationResponseDto
                {
                    BankingOrganizationName = donation.DonationRemark,
                    BloodGroup = donation.DonatedBloodType,
                    DonationDate = donation.CreatedAt,
                    DonorName = $"{donation.Donor?.FirstName} {donation.Donor?.LastName}".Trim(),
                    DonorOrganizationName = donation.DonorOrganization!.OrganizationName,
                    Id = donation.Id,
                    UnitsDonated = donation.UnitsDonated
                });
            }

            return new BaseResponse<IEnumerable<DonationResponseDto>>
            {
                Data = donationsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<DonationResponseDto>>> GetDonationsForStorageAsync()
        {

            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return BaseResponse<IEnumerable<DonationResponseDto>>
                                         .Failure("User not authenticated");
            }
            var bankingOrganization = await _bankingRepository.GetByUserIdAsync(currentUser.Id);

            if (bankingOrganization is null)
            {
                return BaseResponse<IEnumerable<DonationResponseDto>>
                                         .Failure("banking organization not found");
            }
            var donationsForStorage = await _donationRepository
             .FindAsync(r => r.IsTested && !r.IsDeleted && r.IsSuccessful 
             && r.BankingOrganizationId == bankingOrganization.Id && !r.IsStored);

            if(!donationsForStorage.Any())
            {
                return BaseResponse<IEnumerable<DonationResponseDto>>
                                         .Failure("no record found");
            }

            var donationsList = new List<DonationResponseDto>();
            foreach (var donation in donationsForStorage)
            {
                donationsList.Add(new DonationResponseDto
                {
                    BankingOrganizationName = donation.DonationRemark,
                    BloodGroup = donation.DonatedBloodType,
                    DonationDate = donation.CreatedAt,
                    DonorName = $"{donation.Donor?.FirstName} {donation.Donor?.LastName}".Trim(),
                    DonorOrganizationName = donation.DonorOrganization?.OrganizationName ?? "Nill",
                    Id = donation.Id,
                    UnitsDonated = donation.UnitsDonated
                });
            }

            return new BaseResponse<IEnumerable<DonationResponseDto>>
            {
                Data = donationsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<int>> GetHealthyDonationsAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser == null || currentUser.Role != Models.Enums.Role.Admin)
            {
                return BaseResponse<int>.Failure("user not autheticated");
            }
            var healthyDonations = await _donationRepository.GetCountsAsync(r => r.IsHealthy == true);
            if(healthyDonations == 0)
            {
                return BaseResponse<int>.Failure("no healthy donation made");
            }
            return BaseResponse<int>.Success(healthyDonations);
        }

        public async Task<BaseResponse<int>> GetHealthyDonationsByBankingOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Models.Enums.Role.BankingOrganization)
            {
                return BaseResponse<int>.Failure("user not autheticated");
            }

            var bankingOrganization = await _bankingRepository.GetByUserIdAsync(currentUser.Id);
            if(bankingOrganization == null)
            {
                return BaseResponse<int>.Failure("organization not found");
            }

            var healthyDonations = await _donationRepository.GetCountsAsync(
            r => r.IsHealthy == true && r.BankingOrganizationId == bankingOrganization.Id);

            if (healthyDonations == 0)
            {
                return BaseResponse<int>.Failure("no healthy donation made");
            }

            return BaseResponse<int>.Success(healthyDonations);
        }

        public async Task<BaseResponse<int>> GetHealthyDonationsByDonorIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Models.Enums.Role.Donor)
            {
                return BaseResponse<int>.Failure("user not autheticated");
            }

            var donor = await _donorRepository.GetByUserIdAsync(currentUser.Id);
            if (donor == null)
            {
                return BaseResponse<int>.Failure("donor not found");
            }

            var healthyDonations = await _donationRepository.GetCountsAsync(
            r => r.IsHealthy == true && r.DonorId == donor.Id);

            if (healthyDonations == 0)
            {
                return BaseResponse<int>.Failure("no healthy donation made");
            }

            return BaseResponse<int>.Success(healthyDonations);
        }

        public async Task<BaseResponse<int>> GetHealthyDonationsByDonorOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Models.Enums.Role.DonorOrganization)
            {
                return BaseResponse<int>.Failure("user not autheticated");
            }

            var donorOrganization = await _donorOrganizationRepository.GetByUserIdAsync(currentUser.Id);
            if (donorOrganization == null)
            {
                return BaseResponse<int>.Failure("organization not found");
            }

            var healthyDonations = await _donationRepository.GetCountsAsync(
            r => r.IsHealthy == true && r.DonorOrganizationId == donorOrganization.Id);

            if (healthyDonations == 0)
            {
                return BaseResponse<int>.Failure("no healthy donation made");
            }

            return BaseResponse<int>.Success(healthyDonations);
        }

        public async Task<BaseResponse<int>> GetSuccessfulDonationsByDonorIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Models.Enums.Role.Donor)
            {
                return BaseResponse<int>.Failure("user not authenticated");
            }

            var donor = await _donorRepository.GetByUserIdAsync(currentUser.Id);
            if (donor == null)
            {
                return BaseResponse<int>.Failure("donor not found");
            }

            var successfulDonations = await _donationRepository.GetCountsAsync(
            r => r.IsSuccessful == true && r.DonorId == donor.Id);

            if (successfulDonations == 0)
            {
                return BaseResponse<int>.Failure("no successful donation made");
            }

            return BaseResponse<int>.Success(successfulDonations);
        }

        public async Task<BaseResponse<int>> GetSuccessfulDonationsAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Models.Enums.Role.Admin)
            {
                return BaseResponse<int>.Failure("user not autheticated");
            }
            var successfulDonations = await _donationRepository.GetCountsAsync(r => r.IsSuccessful == true);
            if (successfulDonations == 0)
            {
                return BaseResponse<int>.Failure("no successful donation made");
            }
            return BaseResponse<int>.Success(successfulDonations);
        }

        public async Task<BaseResponse<int>> GetSuccessfulDonationsByBankingOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Models.Enums.Role.BankingOrganization)
            {
                return BaseResponse<int>.Failure("user not autheticated");
            }

            var bankingOrganization = await _bankingRepository.GetByUserIdAsync(currentUser.Id);
            if (bankingOrganization == null)
            {
                return BaseResponse<int>.Failure("organization not found");
            }

            var successfulDonations = await _donationRepository.GetCountsAsync(
            r => r.IsSuccessful == true && r.BankingOrganizationId == bankingOrganization.Id);

            if (successfulDonations == 0)
            {
                return BaseResponse<int>.Failure("no healthy donation made");
            }

            return BaseResponse<int>.Success(successfulDonations);
        }

        public async Task<BaseResponse<int>> GetSuccessfulDonationsByDonorOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Models.Enums.Role.DonorOrganization)
            {
                return BaseResponse<int>.Failure("user not autheticated");
            }

            var donorOrganization = await _donorOrganizationRepository.GetByUserIdAsync(currentUser.Id);
            if (donorOrganization == null)
            {
                return BaseResponse<int>.Failure("organization not found");
            }

            var successfulDonations = await _donationRepository.GetCountsAsync(
            r => r.IsSuccessful == true && r.DonorOrganizationId == donorOrganization.Id);

            if (successfulDonations == 0)
            {
                return BaseResponse<int>.Failure("no healthy donation made");
            }

            return BaseResponse<int>.Success(successfulDonations);
        }

        public async Task<BaseResponse<int>> GetTotalDonationsAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser == null)
            {
                return BaseResponse<int>.Failure("user not authenticated");
            }
            var donations = await _donationRepository.GetCountsAsync(r => !r.IsDeleted);
            if(donations == 0)
            {
                return BaseResponse<int>.Failure("no donations is recorded");
            }
            return BaseResponse<int>.Success(donations);
        }

        public async Task<BaseResponse<int>> GetTotalDonationsByBankingOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                return BaseResponse<int>.Failure("user not authenticated");
            }
            var bankingOrganization = await _bankingRepository.GetByUserIdAsync(currentUser.Id);
            if(bankingOrganization == null)
            {
                return BaseResponse<int>.Failure("organization not found");
            }

            var donations = await _donationRepository.GetCountsAsync(r => !r.IsDeleted && 
                                            r.BankingOrganizationId == bankingOrganization.Id);
            if (donations == 0)
            {
                return BaseResponse<int>.Failure("no donations is recorded");
            }
            return BaseResponse<int>.Success(donations);
        }

        public async Task<BaseResponse<int>> GetTotalDonationsByDonorIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                return BaseResponse<int>.Failure("user not authenticated");
            }
            var donor = await _donorRepository.GetByUserIdAsync(currentUser.Id);
            if (donor == null)
            {
                return BaseResponse<int>.Failure("organization not found");
            }

            var donations = await _donationRepository.GetCountsAsync(r => !r.IsDeleted &&
                                            r.DonorId == donor.Id);
            if (donations == 0)
            {
                return BaseResponse<int>.Failure("no donations is recorded");
            }
            return BaseResponse<int>.Success(donations);
        }

        public async Task<BaseResponse<int>> GetTotalDonationsByDonorOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                return BaseResponse<int>.Failure("user not authenticated");
            }
            var donorOrganization = await _donorOrganizationRepository.GetByUserIdAsync(currentUser.Id);
            if (donorOrganization == null)
            {
                return BaseResponse<int>.Failure("organization not found");
            }

            var donations = await _donationRepository.GetCountsAsync(r => !r.IsDeleted &&
                                            r.DonorOrganizationId == donorOrganization.Id);
            if (donations == 0)
            {
                return BaseResponse<int>.Failure("no donations is recorded");
            }
            return BaseResponse<int>.Success(donations);
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            var donationToDelete = await _donationRepository.GetByIdAsync(id);
            if(donationToDelete == null)
            {
                return BaseResponse<bool>.Failure("donation not found");
            }
            donationToDelete.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();
            return BaseResponse<bool>.Success(true, "delete successful");
        }
    }
}
