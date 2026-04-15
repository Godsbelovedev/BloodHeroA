using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BloodHeroA.Application.Services.Implementations
{
    public class ReleasedBloodService : IReleasedBloodService
    {
        private readonly IReleasedBloodRepository _releasedBlood;
        private readonly IDonationRequestRepository _request;
        private readonly IRecipientOrganizationRepository _recipientOrganization;
        private readonly IBankingOrganizationRepository _bankingOrganization;
        private readonly IAuthService _authService;
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IBloodInventoryRepository _bloodInventoryRepository;
        private readonly IBloodStorageRepository _bloodStorageRepository;
        private readonly INotificationService _notificationService;
        private readonly IBloodStorageService _bloodStorageService;

        public ReleasedBloodService(IReleasedBloodRepository releasedBlood,
                            IDonationRequestRepository request,
                            //IUserRepository userRepository,
                            IBankingOrganizationRepository bankingOrganization,
                            IAuthService authService,
                            IUnitOfWorkRepository unitOfWork,
                            IRecipientOrganizationRepository recipientOrganization,
                            IBloodInventoryRepository bloodInventoryRepository,
                            IBloodStorageRepository bloodStorageRepository,
                            INotificationService notificationService,
                            IBloodStorageService bloodStorageService)
        {
            _releasedBlood = releasedBlood;
            _request = request;
            //_userRepository = userRepository;
            _bankingOrganization = bankingOrganization;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _recipientOrganization = recipientOrganization;
            _bloodInventoryRepository = bloodInventoryRepository;
            _bloodStorageRepository = bloodStorageRepository;
            _notificationService = notificationService;
            _bloodStorageService = bloodStorageService;
        }

        public async Task<BaseResponse<IEnumerable<ReleasedBloodResponseDto>>>
        ReleaseBloodAsync(ReleasedBloodRequestDto releasedBlood)
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser == null)
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
                .Failure("user not authenticated");
            }
            var bankingOrganization = await _bankingOrganization.
                           GetByUserIdAsync(currentUser.Id);
            if(bankingOrganization == null)
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
                .Failure("banking organization not found");
            }
            var checkRequest = await _request.GetByIdAsync
                (releasedBlood.DonationRequestId);
            if(checkRequest == null)
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
                .Failure("request not found");
            }
            if (checkRequest.RequestStatus == Status.Completed)
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
                .Failure("request is fully attended to");
            }
            var recipientOrganization = await _recipientOrganization.
                  GetByIdAsync(checkRequest.RecipientOrganizationId);
            if(recipientOrganization == null)
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
                .Failure("recipient organization not found");
            }
            var storages = await _bloodStorageService
                .GetStoragesForMultiSupplyAsync(checkRequest.BloodTypeNeeded);

            if (storages.Data == null || !storages.Data.Any())
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
                .Failure("no record found");
            }
            var storageToRelease = storages.Data.Take
                (releasedBlood.UnitToRelease).ToList();

            if (storageToRelease == null || storageToRelease.Any() == false)
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
               .Failure("no record found");
            }

            var listOfStorageIds = storageToRelease.Select(r => r.Id).ToList();
            if(listOfStorageIds == null || !listOfStorageIds.Any())
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
                .Failure("no record found");
            }

            var distinctStorages = _bloodStorageRepository.GetAvailableBloods
                                        (b => listOfStorageIds.Contains(b.Id));

            var storageForRelease = await distinctStorages.ToListAsync();

            var bloodGroupsForInventory = storageForRelease.Select(r => r.BloodGroup)
                                          .Distinct().ToList();

            var inventories = await _bloodInventoryRepository.FindInventoriesAsync
            (e => bloodGroupsForInventory.Contains(e.BloodGroup)
            && e.BankingOrganizationId == bankingOrganization.Id);

            if (inventories is null|| inventories.Any() == false)
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
                                .Failure("inventories not found");
            }

            var response = new List<ReleasedBloodResponseDto>();
            foreach (var storage in storageForRelease)
            {
                var inventory = inventories.FirstOrDefault
                (r => r.BloodGroup == storage.BloodGroup);
                if(inventory == null)
                {
                    return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
                               .Failure("inventory not found");
                }
                var release = new ReleasedBlood
                {
                    BankingOrganizationId = bankingOrganization.Id,

                    DonationRequestId = releasedBlood.DonationRequestId,
                    ReasonForRelease = "",
                    UnitsReleased = 1,
                    BloodGroup = storage.BloodGroup,
                    RecipientOrganizationId = recipientOrganization.Id,
                    BloodStorageId = storage.Id
                };
               
                await _releasedBlood.CreateAsync(release);
                response.Add(new ReleasedBloodResponseDto
                {
                    BankingOrganizationName = bankingOrganization.OrganizationName,
                    BloodType = storage.BloodGroup,
                    DonationRequestId = checkRequest.Id,
                    Id = release.Id,
                    Quantity = 1,
                    RecipientOrganizationName = recipientOrganization.OrganizationName,
                    ReleaseDate = DateTime.UtcNow,
                    Status = Status.Completed
                });
                bankingOrganization.TotalRelease += 1;
                recipientOrganization.TotalRecievedBlood += 1;
                storage.IsReleased = true;
                release.Status = Status.Completed;
                inventory.ReleasedUnits += 1;
                inventory.BankingOrganizationId = bankingOrganization.Id;
                inventory.RecipientOrganizationId = recipientOrganization.Id;

                checkRequest.UnitsRemained -= release.UnitsReleased;
                checkRequest.UnitsSupplied += release.UnitsReleased;
                if (checkRequest.UnitsSupplied < checkRequest.UnitsRequested)
                {
                    checkRequest.RequestStatus = Status.InProgress;
                }
                else if (checkRequest.UnitsSupplied >= checkRequest.UnitsRequested)
                {
                    checkRequest.RequestStatus = Status.Completed;
                    break;
                }
            }
           
            await _unitOfWork.SaveChangesAsync();
            var notificationDto = new NotificationDTO
            {
                Subject = "Blood Release Notification",
                ReceiverEmail = recipientOrganization.Email,
                SendererEmail = "admin@bloodhero.com",
                Message = $"Dear {recipientOrganization.OrganizationName},\r\n\r\n" +
                $"Your organization has received {releasedBlood.UnitToRelease} unit(s) " +
                $"fitting the request {checkRequest.BloodTypeNeeded}.\r\n" +
                //$"Reason: {release.ReasonForRelease}\r\n\r\n" +
                $"Regards,\nBlood Hero Admin"
            };
            
            await _notificationService.SendNotificationAsync(notificationDto);
            return new BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
            {
                Data = response,
                Message = "create successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<ReleasedBloodResponseDto>> CreateAsync(ReleasedBloodRequestDto releasedBlood)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return BaseResponse<ReleasedBloodResponseDto>
                                .Failure("user not authenticated");
            }
            var bankingOrganization = await _bankingOrganization.GetByUserIdAsync
                                            (currentUser.Id);
            if (bankingOrganization is null)
            {
                return BaseResponse<ReleasedBloodResponseDto>
                                .Failure("banking organization not found");
            }
            var checkRequest = await _request.GetByIdAsync(releasedBlood.DonationRequestId);
            if (checkRequest == null)
            {
                return BaseResponse<ReleasedBloodResponseDto>.Failure("request not found");
            }

            if (checkRequest.RequestStatus == Status.Completed)
            {
                return BaseResponse<ReleasedBloodResponseDto>.Failure("this request fully attended to");
            }
            var recipientOrganization = await _recipientOrganization.GetByIdAsync(checkRequest.RecipientOrganizationId);
            if (recipientOrganization is null)
            {
                return BaseResponse<ReleasedBloodResponseDto>
                                .Failure("rencipient organization not found");
            }

            var storage = await _bloodStorageRepository.FindAsync(r => !r.IsDeleted
             && !r.IsReleased && r.Id == releasedBlood.BloodStorageId); 
            if (storage is null)
            {
                return BaseResponse<ReleasedBloodResponseDto>
                                .Failure("storage not found");
            }
            var inventory = await _bloodInventoryRepository.FindAsync
             (e => e.BloodGroup == storage.BloodGroup 
             && e.BankingOrganizationId == bankingOrganization.Id);
            if (inventory is null)
            {
                return BaseResponse<ReleasedBloodResponseDto>
                                .Failure("inventory not found");
            }
            var unitsAvailable = inventory.StoredUnits - inventory.ReleasedUnits - inventory.ExpiredUnits;
            if(unitsAvailable < 1)
            {
                return BaseResponse<ReleasedBloodResponseDto>
                .Failure("no blood of this type available. try consider other alternative types");
            }
            var release = new ReleasedBlood
            {
                BankingOrganizationId = bankingOrganization.Id,
                
                DonationRequestId = releasedBlood.DonationRequestId,
                ReasonForRelease = "",
                UnitsReleased = 1,
                BloodGroup = storage.BloodGroup,
                RecipientOrganizationId = recipientOrganization.Id,
                BloodStorageId = releasedBlood.BloodStorageId
            };
            bankingOrganization.TotalRelease += 1;
            recipientOrganization.TotalRecievedBlood += 1;
            storage.IsReleased = true;
            release.Status = Status.Completed;
            inventory.ReleasedUnits += 1;
            inventory.BankingOrganizationId = bankingOrganization.Id;
            inventory.RecipientOrganizationId = recipientOrganization.Id;

            checkRequest.UnitsRemained -=  release.UnitsReleased;
            checkRequest.UnitsSupplied += release.UnitsReleased;
            if (checkRequest.UnitsSupplied < checkRequest.UnitsRequested)
            {
                checkRequest.RequestStatus = Status.InProgress;
            }
            else if (checkRequest.UnitsSupplied >= checkRequest.UnitsRequested)
            {
                checkRequest.RequestStatus = Status.Completed;
            }

            var notificationDto = new NotificationDTO
            {
                Subject = "Blood Release Notification",
                ReceiverEmail = recipientOrganization.Email,
                Message = $"Dear {recipientOrganization.OrganizationName},\r\n\r\n" +
                  $"Your organization has received {release.UnitsReleased} unit(s) of blood type {release.BloodGroup}.\r\n" +
                  //$"Reason: {release.ReasonForRelease}\r\n\r\n" +
                  $"Regards,\nBlood Hero Admin"
            };

            await _releasedBlood.CreateAsync(release);
            await _unitOfWork.SaveChangesAsync();
            await _notificationService.SendNotificationAsync(notificationDto);

            return new BaseResponse<ReleasedBloodResponseDto>
            {
                Data = new ReleasedBloodResponseDto
                {
                    BankingOrganizationName = bankingOrganization.OrganizationName,
                    BloodType = release.BloodGroup,
                   // Essence = release.ReasonForRelease,
                    Id = release.Id,
                    Quantity = 1,
                    RecipientOrganizationName = recipientOrganization.OrganizationName,
                    ReleaseDate = release.ReleasedAt,
                    Status = release.Status                    
                },
                Message = "create successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<ReleasedBloodResponseDto?>> GetByIdAsync(Guid id)
        {
            var releaseBlood = await _releasedBlood.GetByIdAsync(id);
            if(releaseBlood is null)
            {
                    return BaseResponse<ReleasedBloodResponseDto?>
                                    .Failure("released record not found");
            }

            var bankingOrganization = await _bankingOrganization.GetByUserIdAsync(releaseBlood.BankingOrganizationId);
            if (bankingOrganization is null)
            {
                return BaseResponse<ReleasedBloodResponseDto?>
                                .Failure("Banking Organization not found");
            }
            var recipientOrganization = await _recipientOrganization.GetByIdAsync(releaseBlood.RecipientOrganizationId);
            if (recipientOrganization is null)
            {
                return BaseResponse<ReleasedBloodResponseDto?>
                                .Failure("recipient Organization not found");
            }
            return new BaseResponse<ReleasedBloodResponseDto?>
            {
                Data = new ReleasedBloodResponseDto
                {
                    BankingOrganizationName = bankingOrganization.OrganizationName,
                    BloodType = releaseBlood.BloodGroup,
                    //Essence = releaseBlood.ReasonForRelease,
                    Id = releaseBlood.Id,
                    Quantity = 1,
                    RecipientOrganizationName = recipientOrganization.OrganizationName,
                    ReleaseDate = releaseBlood.ReleasedAt,
                    Status = releaseBlood.Status
                },
                Message = " retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<ReleasedBloodResponseDto>>> GetReleasedByTypeAsync(BloodGroup bloodGroup)
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser is null)
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>.Failure("user not authenticated");
            }

            var bankingOrganization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (bankingOrganization is null)
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>.Failure("organization not found");
            }

            var recipientOrganization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (recipientOrganization is null)
            {
                return BaseResponse<IEnumerable<ReleasedBloodResponseDto>>.Failure("organization not found");
            }
            var releasedBloods = await _releasedBlood.GetReleasedByTypeAsync(r => r.BloodGroup == bloodGroup);
            
            if(currentUser is not null)
            {
                if(currentUser.Role == Role.BankingOrganization)
                {
                    releasedBloods = await _releasedBlood.GetReleasedByTypeAsync(r => r.BloodGroup == bloodGroup
                                                        && r.BankingOrganizationId == bankingOrganization.Id);
                }
                if (currentUser.Role == Role.RecipientOrganization)
                {
                    releasedBloods = await _releasedBlood.GetReleasedByTypeAsync(r => r.BloodGroup == bloodGroup
                                                        && r.RecipientOrganizationId == recipientOrganization.Id);
                }
            }
            var dto = releasedBloods.Select(r => new ReleasedBloodResponseDto
            {
                Id = r.Id,
                BankingOrganizationName = r.BankingOrganization.OrganizationName,
                BloodType = r.BloodGroup,
               // Essence = r.ReasonForRelease,
                Quantity = 1,
                RecipientOrganizationName = r.RecipientOrganization.OrganizationName,
                ReleaseDate = r.ReleasedAt,
                Status = r.Status
            });
            return new BaseResponse<IEnumerable<ReleasedBloodResponseDto>>
            {
                Data = dto,
                Message = "retrieve successful",
                Status = true
            };
        }

        
    }
}
