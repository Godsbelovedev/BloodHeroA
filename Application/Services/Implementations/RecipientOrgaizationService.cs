using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using BloodHeroA.Repositories.IRepositories;

namespace BloodHeroA.Application.Services.Implementations
{
    public class RecipientOrgaizationService : IRecipientOrganizationService
    {
        private readonly IRecipientOrganizationRepository _recipientOrganization;
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        public RecipientOrgaizationService
            (
            IRecipientOrganizationRepository recipientOrganization,
            IUnitOfWorkRepository unitOfWork, IAuthService authService,
            IUserRepository userRepository, INotificationService notificationService)
        {
            _recipientOrganization = recipientOrganization;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        public async Task<BaseResponse<RecipientResponseDto>> AddAsync(RecipientOrganizationDTO recipientOrganizationDTO)
        {
            var checkOrganization = await _userRepository.GetUserAsync
               (u => u.Email == recipientOrganizationDTO.Email);
            if (checkOrganization is not null)
            {
                return BaseResponse<RecipientResponseDto>.
                    Failure("organization already exist, create fail");
            }
            var user = new User
            {
                FullName = recipientOrganizationDTO.OrganizationName,
                Email = recipientOrganizationDTO.Email,
                HashPassWord = BCrypt.Net.BCrypt.HashPassword
                (recipientOrganizationDTO.Password),
                PhoneNumber = recipientOrganizationDTO.PhoneNumber,
                Role = Role.RecipientOrganization
            };

            var organization = new RecipientOrganization
            {
                OrganizationName = recipientOrganizationDTO.OrganizationName,
                Role = Role.RecipientOrganization,
                PhoneNumber = user.PhoneNumber,
                IsDeleted = user.IsDeleted,
                Address = recipientOrganizationDTO.Address,
                Email = user.Email,
                User = user,
                UserId = user.Id
            };
            user.RecipientOrganization = organization;
            user.RecipientOrganizationId = organization.Id;
            user.FullName = organization.OrganizationName;

            var notificationDto = new NotificationDTO
            {
                Subject = "Welcome to Blood Hero!",
                ReceiverEmail = organization.Email,
                Message = $"Dear {organization.OrganizationName},\r\n\r\n" +
             $"Welcome to Blood Hero! Your organization has been successfully registered.\r\n\r\n" +
             $"You can now save lives as a hero, and track your activities through our platform.\r\n\r\n" +
             $"We’re excited to have you on board and look forward to supporting your lifesaving efforts.\r\n\r\n" +
             $"Regards,\n\n Blood Hero Admin"
            };

           
            await _userRepository.CreateUserAsync(user);
            await _recipientOrganization.AddAsync(organization);
            await _unitOfWork.SaveChangesAsync();
            await _notificationService.SendNotificationAsync(notificationDto);

            return new BaseResponse<RecipientResponseDto>
            {
                Data = new RecipientResponseDto
                {
                    Address = organization.Address,
                    Email = organization.Email,
                    Id = organization.Id,
                    OrganizationName = organization.OrganizationName,
                    PhoneNumber = organization.PhoneNumber,
                    RegisteredDate = organization.CreatedAt,
                    TotalRecivedBlood = organization.TotalRecievedBlood
                },
                Message = "Create successfull",
                Status = true
            };
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            var userToDelete = await _recipientOrganization.GetByIdAsync(id);
            if (userToDelete is null)
            {
                return BaseResponse<bool>.Failure("organization do not exist, delete fail");
            }
            userToDelete.IsDeleted = true;
            userToDelete.User!.IsDeleted = true;
            userToDelete.User.IsAvailable = false;
            await _unitOfWork.SaveChangesAsync();
            return BaseResponse<bool>.Success(true, "delete successful");
        }

        public async Task<BaseResponse<IEnumerable<RecipientResponseDto>>> GetAllAsync()
        {
            var allOrganizations = await _recipientOrganization.GetAllAsync();

            var organizationsList = new List<RecipientResponseDto>();
            if (!allOrganizations.Any())
            {
                return BaseResponse<IEnumerable<RecipientResponseDto>>.
                                       Failure("no record found");
            }
            foreach (var organization in allOrganizations)
            {
                organizationsList.Add( new RecipientResponseDto
                {
                    Address = organization.Address,
                    Email = organization.Email,
                    Id = organization.Id,
                    OrganizationName = organization.OrganizationName,
                    PhoneNumber = organization.PhoneNumber,
                    RegisteredDate = organization.CreatedAt,
                    TotalRecivedBlood = organization.TotalRecievedBlood
                });
            }
            return new BaseResponse<IEnumerable<RecipientResponseDto>>
            {
                Data = organizationsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<RecipientResponseDto?>> GetByIdAsync(Guid id)
        {
            var organization = await _recipientOrganization.GetByIdAsync(id);
            if (organization is null)
            {
                return new BaseResponse<RecipientResponseDto?>
                {
                    Data = null,
                    Message = "organization not found",
                    Status = false
                };
            }
            return new BaseResponse<RecipientResponseDto?>
            {
                Data = new RecipientResponseDto
                {
                    Address = organization.Address,
                    Email = organization.Email,
                    Id = organization.Id,
                    OrganizationName = organization.OrganizationName,
                    PhoneNumber = organization.PhoneNumber,
                    RegisteredDate = organization.CreatedAt,
                    TotalRecivedBlood = organization.TotalRecievedBlood
                },
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<RecipientResponseDto?>> GetByUserIdAsync(Guid userId)
        {
            var organization = await _recipientOrganization.GetByUserIdAsync(userId);
            if (organization is null)
            {
                return new BaseResponse<RecipientResponseDto?>
                {
                    Data = null,
                    Message = "organization not found",
                    Status = false
                };
            }
            return new BaseResponse<RecipientResponseDto?>
            {
                Data = new RecipientResponseDto
                {
                    Address = organization.Address,
                    Email = organization.Email,
                    Id = organization.Id,
                    OrganizationName = organization.OrganizationName,
                    PhoneNumber = organization.PhoneNumber,
                    RegisteredDate = organization.CreatedAt,
                    TotalRecivedBlood = organization.TotalRecievedBlood
                },
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<RecipientResponseDto>> UpdateAsync(RecipientUpdateDto updateDto)
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser is null)
            {
                return new BaseResponse<RecipientResponseDto>
                {
                    Data = null,
                    Message = "login required",
                    Status = false
                };
            }
            var userToUpdate = await _recipientOrganization
                              .GetByUserIdAsync(currentUser.Id);
            if(userToUpdate is null)
            {
                return new BaseResponse<RecipientResponseDto>
                {
                    Data = null,
                    Message = "user unknown",
                    Status = false
                };
            }
           if(!string.IsNullOrWhiteSpace(updateDto.Address))
            {
                userToUpdate.Address = updateDto.Address;
              
            }
            if (!string.IsNullOrWhiteSpace(updateDto.PhoneNumber))
            {
                userToUpdate.PhoneNumber = updateDto.PhoneNumber;
            }
            if (!string.IsNullOrWhiteSpace(updateDto.OrganizationName))
            {
                userToUpdate.OrganizationName = updateDto.OrganizationName;
                currentUser.FullName = updateDto.OrganizationName;
            }
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse<RecipientResponseDto>
            {
                Data = new RecipientResponseDto
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