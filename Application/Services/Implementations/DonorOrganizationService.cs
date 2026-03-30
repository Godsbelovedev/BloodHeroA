using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using BloodHeroA.Repositories.Implementation;
using BloodHeroA.Repositories.IRepositories;

namespace BloodHeroA.Application.Services.Implementations
{
    public class DonorOrganizationService : IDonorOrganizationService
    {
        private readonly IDonorOrganizationRepository _donorOrganizationRepository;
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public DonorOrganizationService(IDonorOrganizationRepository donorOrganizationRepository,
                                        IUnitOfWorkRepository unitOfWork,
                                        IAuthService authService,
                                        IUserRepository userRepository,
                                        INotificationService notificationService)
        {
            _donorOrganizationRepository = donorOrganizationRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }
        public async Task<BaseResponse<DonorOrganizationResponseDto>>
                      AddAsync(DonorOrganizationRequestDto organization)
        {
            var checkOrganization = await _userRepository.GetUserAsync(d => d.Email == organization.Email);
            if(checkOrganization is not null)
            {
                return BaseResponse<DonorOrganizationResponseDto>
                                   .Failure("user already exist");
            }
            var password = BCrypt.Net.BCrypt.HashPassword(organization.Password);
            var user = new User
            {
                FullName = organization.OrganizationName,
                Email = organization.Email,
                PhoneNumber = organization.PhoneNumber,
                HashPassWord = password,
                Role = Role.DonorOrganization
            };
            var donorOrganization = new DonorOrganization
            {
                OrganizationName = organization.OrganizationName,
                Role = Role.DonorOrganization,
                Address = organization.Address,
                Email = organization.Email,
                PhoneNumber = organization.PhoneNumber,
                UserId = user.Id
                
            };
            user.DonorOrganizationId = donorOrganization.Id;

            var notificationDto = new NotificationDTO
            {
                Subject = "Welcome to Blood Hero!",
                ReceiverEmail = organization.Email,
                Message = $"Dear {organization.OrganizationName},\r\n\r\n" +
              $"Welcome to Blood Hero! Your organization has been successfully registered.\r\n\r\n" +
              $"You can now save lives as a hero through donation, and track your activities through our platform.\r\n\r\n" +
              $"We’re excited to have you on board and look forward to supporting your lifesaving efforts.\r\n\r\n" +
              $"Regards,\r\n\r\n Blood Hero Admin"
            };

            await _donorOrganizationRepository.AddAsync(donorOrganization);
            await _userRepository.CreateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(notificationDto);

            return new BaseResponse<DonorOrganizationResponseDto>
            {
                Data = new DonorOrganizationResponseDto
                {
                    Id = donorOrganization.Id,
                    Address = donorOrganization.Address,
                    OrganizationName = donorOrganization.OrganizationName,
                    CreatedAt = donorOrganization.CreatedAt,
                    Email = donorOrganization.Email,
                    IsDeleted = donorOrganization.IsDeleted,
                    PhoneNumber = donorOrganization.PhoneNumber,
                    TotalDonations = donorOrganization.TotalDonations,
                    TotalRegisteredDonors = donorOrganization.TotalRegisteredDonors
                },
                Message = "Create successful",
                Status = true
            };
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            var userToDelete = await _donorOrganizationRepository.GetByIdAsync(id);
            if (userToDelete is null)
            {
                return BaseResponse<bool>.Failure("organization do not exist");
            }
            userToDelete.IsDeleted = true;
            userToDelete.User!.IsDeleted = true;
            userToDelete.User.IsAvailable = false;
            await _unitOfWork.SaveChangesAsync();
            return BaseResponse<bool>.Success(true);
        }

        public async Task<BaseResponse<IEnumerable<DonorOrganizationResponseDto>>> GetAllAsync()
        {
            var allOrganizations = await _donorOrganizationRepository.GetAllAsync();

            var organizationsList = new List<DonorOrganizationResponseDto>();
            if (!allOrganizations.Any())
            {
                return BaseResponse<IEnumerable<DonorOrganizationResponseDto>>.
                                       Failure("no record found");
            }
            foreach (var organization in allOrganizations)
            {
                organizationsList.Add(new DonorOrganizationResponseDto
                {
                    Id = organization.Id,
                    Address = organization.Address,
                    OrganizationName = organization.OrganizationName,
                    CreatedAt = organization.CreatedAt,
                    Email = organization.Email,
                    IsDeleted = organization.IsDeleted,
                    PhoneNumber = organization.PhoneNumber,
                    TotalDonations = organization.TotalDonations,
                    TotalRegisteredDonors = organization.TotalRegisteredDonors
                });
            }
            return new BaseResponse<IEnumerable<DonorOrganizationResponseDto>>
            {
                Data = organizationsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<DonorOrganizationResponseDto>>> GetAllForDonationAsync()
        {
            var allOrganizations = await _donorOrganizationRepository.GetAllAsync();

            var organizationsList = new List<DonorOrganizationResponseDto>();
            if (!allOrganizations.Any())
            {
                return BaseResponse<IEnumerable<DonorOrganizationResponseDto>>.
                                       Failure("no record found");
            }
            foreach (var organization in allOrganizations)
            {
                organizationsList.Add(new DonorOrganizationResponseDto
                {
                    Id = organization.Id,
                    Address = organization.Address,
                    OrganizationName = organization.OrganizationName,
                    CreatedAt = organization.CreatedAt,
                    Email = organization.Email,
                    IsDeleted = organization.IsDeleted,
                    PhoneNumber = organization.PhoneNumber,
                    TotalDonations = organization.TotalDonations,
                    TotalRegisteredDonors = organization.TotalRegisteredDonors
                });
            }
            return new BaseResponse<IEnumerable<DonorOrganizationResponseDto>>
            {
                Data = organizationsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<DonorOrganizationResponseDto?>> GetByIdAsync(Guid id)
        {
            var organization = await _donorOrganizationRepository.GetByIdAsync(id);
            if (organization is null)
            {
                return new BaseResponse<DonorOrganizationResponseDto?>
                {
                    Data = null,
                    Message = "organization not found",
                    Status = false
                };
            }
            return new BaseResponse<DonorOrganizationResponseDto?>
            {
                Data = new DonorOrganizationResponseDto
                {
                    Id = organization.Id,
                    Address = organization.Address,
                    OrganizationName = organization.OrganizationName,
                    CreatedAt = organization.CreatedAt,
                    Email = organization.Email,
                    IsDeleted = organization.IsDeleted,
                    PhoneNumber = organization.PhoneNumber,
                    TotalDonations = organization.TotalDonations,
                    TotalRegisteredDonors = organization.TotalRegisteredDonors
                },
                Message = "retrieved successfully",
                Status = true
            };
        }

      
        public async Task<BaseResponse<DonorOrganizationResponseDto?>> GetByUserIdAsync(Guid userId)
        {
            var organization = await _donorOrganizationRepository.GetByUserIdAsync(userId);
            if (organization is null)
            {
                return new BaseResponse<DonorOrganizationResponseDto?>
                {
                    Data = null,
                    Message = "organization not found",
                    Status = false
                };
            }
            return new BaseResponse<DonorOrganizationResponseDto?>
            {
                Data = new DonorOrganizationResponseDto
                {
                    Address = organization.Address,
                    Email = organization.Email,
                    Id = organization.Id,
                    OrganizationName = organization.OrganizationName,
                    PhoneNumber = organization.PhoneNumber,
                    CreatedAt = organization.CreatedAt,
                    TotalDonations = organization.TotalDonations,
                    TotalRegisteredDonors = organization.TotalRegisteredDonors
                },
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<DonorOrganizationResponseDto>> UpdateAsync(DonorOrganizationUpdateDto organization)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return new BaseResponse<DonorOrganizationResponseDto>
                {
                    Data = null,
                    Message = "login required",
                    Status = false
                };
            }
            var userToUpdate = await _donorOrganizationRepository
                              .GetByUserIdAsync(currentUser.Id);
            if (userToUpdate is null)
            {
                return new BaseResponse<DonorOrganizationResponseDto>
                {
                    Data = null,
                    Message = "user unknown",
                    Status = false
                };
            }
            if (!string.IsNullOrWhiteSpace(organization.Address))
            {
                userToUpdate.Address = organization.Address;

            }
            if (!string.IsNullOrWhiteSpace(organization.PhoneNumber))
            {
                userToUpdate.PhoneNumber = organization.PhoneNumber;
            }
            if (!string.IsNullOrWhiteSpace(organization.OrganizationName))
            {
                userToUpdate.OrganizationName = organization.OrganizationName;
                currentUser.FullName = organization.OrganizationName;
            }
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse<DonorOrganizationResponseDto>
            {
                Data = new DonorOrganizationResponseDto
                {
                    OrganizationName = userToUpdate.OrganizationName,
                    PhoneNumber = userToUpdate.PhoneNumber,
                    Address = userToUpdate.Address
                },
                Message = "details update successfully",
                Status = true
            };
        }
    }
}
