using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using BloodHeroA.Repositories.Implementation;
using BloodHeroA.Repositories.IRepositories;

namespace BloodHeroA.Application.Services.Implementations
{
    public class BankingOrganizationService : IBankingOrganizationService
    {
        private readonly IBankingOrganizationRepository _bankingOrganization;
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IBloodStorageRepository _bloodStorageRepository;

        public BankingOrganizationService(IBankingOrganizationRepository bankingOrganization,
                                          IUnitOfWorkRepository unitOfWork,
                                          IAuthService authService,
                                          IUserRepository userRepository,
                                          INotificationService notificationService,
                                          IBloodStorageRepository bloodStorageRepository)
        {
            _bankingOrganization = bankingOrganization;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _bloodStorageRepository = bloodStorageRepository;
        }

        public async Task<BaseResponse<BankingOrganizationResponseDto>> AddAsync(BankingOrganizationDTO bankingOrganizationDTO)
        {
            var checkOrganization = await _userRepository.GetUserAsync
               (u => u.Email == bankingOrganizationDTO.Email);
            if (checkOrganization is not null)
            {
                return BaseResponse<BankingOrganizationResponseDto>.
                    Failure("organization already exist, create fail");
            }

            if (bankingOrganizationDTO.Password != bankingOrganizationDTO.ConfirmPassword)
            {
                return BaseResponse<BankingOrganizationResponseDto>.
                   Failure("mismatch password");
            }
            var user = new User
            {
                FullName = bankingOrganizationDTO.OrganizationName,
                Email = bankingOrganizationDTO.Email,
                HashPassWord = BCrypt.Net.BCrypt.HashPassword
                (bankingOrganizationDTO.Password),
                PhoneNumber = bankingOrganizationDTO.PhoneNumber,
                Role = Role.BankingOrganization
            };

            var organization = new BankingOrganization
            {
                OrganizationName = user.FullName,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                IsDeleted = user.IsDeleted,
                Address = bankingOrganizationDTO.Address,
                Email = user.Email,
                UserId = user.Id
                
            };

            user.BankingOrganizationId = organization.Id;
            //user.FullName = organization.OrganizationName;

            var notificationDto = new NotificationDTO
            {
                Subject = "Welcome to Blood Hero!",
                ReceiverEmail = organization.Email,
                SendererEmail = "admin@bloodhero.com",
                Message = $"Dear {organization.OrganizationName},\n\n" +
               $"Welcome to Blood Hero! Your organization has been successfully registered.\n\n" +
               $"You can now manage blood donations, request blood units, and track your inventory through our platform.\n\n" +
               $"We’re excited to have you on board and look forward to supporting your lifesaving efforts.\n\n" +
               $"Regards,\nBlood Hero Admin"
            };

           
            await _userRepository.CreateUserAsync(user);
            await _bankingOrganization.CreateAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(notificationDto);

            return new BaseResponse<BankingOrganizationResponseDto>
            {
                Data = new BankingOrganizationResponseDto
                {
                    Address = organization.Address,
                    Email = organization.Email,
                    Id = organization.Id,
                    OrganizationName = organization.OrganizationName,
                    PhoneNumber = organization.PhoneNumber
                },
                Message = "Create successfull",
                Status = true
            };
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            var userToDelete = await _bankingOrganization.GetByIdAsync(id);
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


        public async Task<BaseResponse<IEnumerable<BankingOrganizationResponseDto>>> GetAllAsync()
        {
            var allOrganizations = await _bankingOrganization.GetAllAsync();

            var organizationsList = new List<BankingOrganizationResponseDto>();
            if (!allOrganizations.Any())
            {
                return BaseResponse<IEnumerable<BankingOrganizationResponseDto>>.
                                       Failure("no record found");
            }
            
            foreach (var organization in allOrganizations)
            {
                //var expiriredBloods = await _bloodStorageRepository.GetExpiredAsync
                //    (r => r.IsExpired && !r.IsDeleted && r.BankingOrganizationId == organization.Id);
                organizationsList.Add(new BankingOrganizationResponseDto
                {
                    Address = organization.Address,
                    Email = organization.Email,
                    Id = organization.Id,
                    OrganizationName = organization.OrganizationName,
                    PhoneNumber = organization.PhoneNumber
                });
            }
            return new BaseResponse<IEnumerable<BankingOrganizationResponseDto>>
            {
                Data = organizationsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<BankingOrganizationResponseDto?>> GetByIdAsync(Guid id)
        {
            var organization = await _bankingOrganization.GetByIdAsync(id);
            if (organization is null)
            {
                return new BaseResponse<BankingOrganizationResponseDto?>
                {
                    Data = null,
                    Message = "organization not found",
                    Status = false
                };
            }
            return new BaseResponse<BankingOrganizationResponseDto?>
            {
                Data = new BankingOrganizationResponseDto
                {
                    Address = organization.Address,
                    Email = organization.Email,
                    Id = organization.Id,
                    OrganizationName = organization.OrganizationName,
                    PhoneNumber = organization.PhoneNumber,
                    CreatedAt = organization.CreatedAt,
                    TotalDonations = organization.TotalDonations,
                    TotalRelease = organization.TotalRelease,
                    TotalStorage = organization.TotalStorage,

                },
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<BankingOrganizationResponseDto?>> GetByUserIdAsync(Guid userId)
        {
            var organization = await _bankingOrganization.GetByUserIdAsync(userId);
            if (organization is null)
            {
                return new BaseResponse<BankingOrganizationResponseDto?>
                {
                    Data = null,
                    Message = "organization not found",
                    Status = false
                };
            }
            var expiriredBloods = await _bloodStorageRepository.GetExpiredAsync
                (r => r.IsExpired && !r.IsDeleted && r.BankingOrganizationId == organization.Id);
            return new BaseResponse<BankingOrganizationResponseDto?>
            {
                Data = new BankingOrganizationResponseDto
                {
                    Address = organization.Address,
                    Email = organization.Email,
                    Id = organization.Id,
                    OrganizationName = organization.OrganizationName,
                    PhoneNumber = organization.PhoneNumber,
                    CreatedAt = organization.CreatedAt,
                    TotalDonations = organization.TotalDonations,
                    TotalRelease = organization.TotalRelease,
                    TotalStorage = organization.TotalStorage,
                    TotalExpired = expiriredBloods.Count()
                },
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<BankingOrganizationResponseDto>> UpdateAsync(BankingOrganizationUpdateDto updateDto)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return new BaseResponse<BankingOrganizationResponseDto>
                {
                    Data = null,
                    Message = "login required",
                    Status = false
                };
            }
            var userToUpdate = await _bankingOrganization
                              .GetByUserIdAsync(currentUser.Id);
            if (userToUpdate is null)
            {
                return new BaseResponse<BankingOrganizationResponseDto>
                {
                    Data = null,
                    Message = "user unknown",
                    Status = false
                };
            }
            if (!string.IsNullOrWhiteSpace(updateDto.Address))
            {
                userToUpdate.Address = updateDto.Address;
                userToUpdate.User!.BankingOrganization!.Address = updateDto.Address;
            }
            if (!string.IsNullOrWhiteSpace(updateDto.PhoneNumber))
            {
                userToUpdate.PhoneNumber = updateDto.PhoneNumber;
            }
            if (!string.IsNullOrWhiteSpace(updateDto.OrganizationName))
            {
                userToUpdate.OrganizationName = updateDto.OrganizationName;
            }
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse<BankingOrganizationResponseDto>
            {
                Data = new BankingOrganizationResponseDto
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
