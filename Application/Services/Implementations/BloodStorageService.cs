using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BloodHeroA.Application.Services.Implementations
{
    public class BloodStorageService : IBloodStorageService
    {
        private static Dictionary<BloodGroup, List<BloodGroup>> _bloodGroups =
            new()
            {
                [BloodGroup.O_Negative] =  [BloodGroup.O_Negative],
                [BloodGroup.A_Negative] =  [BloodGroup.A_Negative, BloodGroup.O_Negative],
                [BloodGroup.B_Negative] =  [BloodGroup.B_Negative, BloodGroup.O_Negative],
                [BloodGroup.AB_Negative] = [BloodGroup.AB_Negative, BloodGroup.A_Negative, BloodGroup.B_Negative, BloodGroup.O_Negative],

                [BloodGroup.O_Positive] =  [BloodGroup.O_Positive, BloodGroup.O_Negative],
                [BloodGroup.A_Positive] =  [BloodGroup.A_Positive, BloodGroup.A_Negative, BloodGroup.O_Positive, BloodGroup.O_Negative],
                [BloodGroup.B_Positive] =  [BloodGroup.B_Positive, BloodGroup.B_Negative, BloodGroup.O_Negative],
                [BloodGroup.AB_Positive] = [BloodGroup.AB_Positive, BloodGroup.AB_Negative, BloodGroup.A_Positive, BloodGroup.A_Negative,
                                             BloodGroup.B_Positive, BloodGroup.B_Negative, BloodGroup.O_Positive, BloodGroup.O_Negative ]
            };


        private readonly IBankingOrganizationRepository _bankingOrganization;
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IBloodStorageRepository _bloodStorageRepository;
        private readonly IBloodInventoryRepository _bloodInventoryRepository;
        private readonly IDonationRepository _donationRepository;

        public BloodStorageService(IAuthService authService,
                                   IUnitOfWorkRepository unitOfWork,
                                   IBloodStorageRepository bloodStorageRepository,
                                   IBankingOrganizationRepository bankingOrganization,
                                   IBloodInventoryRepository bloodInventoryRepository,
                                   IDonationRepository donationRepository)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _bloodStorageRepository = bloodStorageRepository;
            _bankingOrganization = bankingOrganization;
            _bloodInventoryRepository = bloodInventoryRepository;
            _donationRepository = donationRepository;
        }

        public async Task<BaseResponse<BloodStorageResponseDto>>
        CreateStorageAsync(BloodStorageDTO bloodStorage)
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser is null)
            {
                return BaseResponse<BloodStorageResponseDto>.Failure("user not authenticated");
            }
            var organization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);

            if (organization is null)
            {
                return BaseResponse<BloodStorageResponseDto>.Failure("organization not found");
            }
            var donation = await _donationRepository.GetByIdAsync(bloodStorage.DonationId);

            if (donation is null)
            {
                return BaseResponse<BloodStorageResponseDto>.Failure("donation not found");
            }
            if (donation.Donor.Tattoo == HealthStatus.Positive || 
                donation.Donor.IVDrugConsumer == HealthStatus.Positive
                || donation.BloodTestResult?.Tattoo == HealthStatus.Positive
                || donation.BloodTestResult?.IVDrugConsumer == HealthStatus.Positive
                || donation.Donor.HIV == HealthStatus.Positive
                || donation.Donor.HepatitisB == HealthStatus.Positive
                || donation.Donor.Cancer == HealthStatus.Positive
                || donation.Donor.HeartDisease == HealthStatus.Positive
                || donation.Donor.Hemophilic == HealthStatus.Positive
                || donation.Donor.ChronicDisease == HealthStatus.Positive
                || donation.Donor.SevereLungsDisease == HealthStatus.Positive
                )
            {
                return BaseResponse<BloodStorageResponseDto>.Failure("donation not healthy for storage");
            }
        var existingStorage = await _bloodStorageRepository
                .FindAsync(r => !r.IsDeleted && !r.IsReleased && r.DonationId == bloodStorage.DonationId);
            if (existingStorage is not null)
            {
                return BaseResponse<BloodStorageResponseDto>.Failure("storage already exist");
            }


            var storage = new BloodStorage
            {
              BloodGroup = bloodStorage.BloodGroup,
              BankingOrganizationId = organization.Id,
              DonationId = bloodStorage.DonationId,
              StorageLocation = bloodStorage.Location,
              UnitStored = 1,
              ExpiryDate = DateTime.UtcNow.AddMinutes(6)
            };

            storage.IsExpired = storage.ExpiryDate < DateTime.UtcNow;
            donation.IsStored = true;
            organization.TotalStorage += 1;

            await _bloodStorageRepository.CreateStorageAsync(storage);

            var inventory = await _bloodInventoryRepository.FindAsync
                (e => e.BankingOrganizationId == organization.Id && 
                 e.BloodGroup == bloodStorage.BloodGroup);
            if (inventory != null)
            {
                inventory.StoredUnits += 1;
            }
            else
            {
                var createInventory = new BloodInventory
                {
                    BankingOrganizationId = organization.Id,
                    BloodGroup = bloodStorage.BloodGroup,
                    StoredUnits = storage.UnitStored 
                };
                await _bloodInventoryRepository.CreateInventory(createInventory);
            }
 
            await _unitOfWork.SaveChangesAsync();

            return new BaseResponse<BloodStorageResponseDto>
            {
               Data = new BloodStorageResponseDto
               {
                   BankingOrganizationName = organization.OrganizationName,
                   BloodGroup = storage.BloodGroup,
                   Location = storage.StorageLocation,
                   UnitStored = 1,
                   DonationId = bloodStorage.DonationId,
                   DonorFullName = donation.Donor.FirstName + " " + donation.Donor.LastName,
                   DonorId = donation.DonorId,
                   DonorOrganizationName = donation.DonorOrganization?.OrganizationName ?? "nil"
               }
            };
        }

        public async Task<BaseResponse<bool?>> DeleteAsync(Guid id)
        {
            var storageToDelete = await _bloodStorageRepository.FindAsync(r => !r.IsDeleted
             && !r.IsReleased && r.Id == id);
            if (storageToDelete is null)
            {
                return BaseResponse<bool?>.Failure("storage do not exist");
            }

            var inventoryToDelete = await _bloodInventoryRepository.
              FindAsync(r => r.BloodGroup == storageToDelete.BloodGroup && 
              r.BankingOrganizationId == storageToDelete.BankingOrganizationId);

            if (inventoryToDelete is null)
            {
                return BaseResponse<bool?>.Failure("inventory do not exist");
            }

            storageToDelete.IsDeleted = true;
            inventoryToDelete.StoredUnits -= 1;
            await _unitOfWork.SaveChangesAsync();
            return BaseResponse<bool?>.Success(true);
        }

        public async Task<BaseResponse<IEnumerable<BloodStorageResponseDto>>>
                GetStoragesForMultiSupplyAsync(BloodGroup bloodGroup)
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser == null)
            {
                return BaseResponse<IEnumerable<BloodStorageResponseDto>>
                .Failure("user not Authenticated");
            }
            var bankingOrganization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if(bankingOrganization == null)
            {
                return BaseResponse<IEnumerable <BloodStorageResponseDto>>
                .Failure("organization not found");
            }
           
            var query = _bloodStorageRepository.GetAvailableBloods(b => _bloodGroups[bloodGroup].Contains
            (b.BloodGroup) && !b.IsDeleted && !b.IsReleased && !b.IsExpired
            && b.BankingOrganizationId == bankingOrganization.Id);

            var storages = await query.Take(10).ToListAsync();
            var orderedStorage = storages.OrderBy(r => _bloodGroups[bloodGroup].IndexOf(r.BloodGroup))
                                          .ThenBy(r => r.ExpiryDate);

            var listOfStorages = orderedStorage.Select(r => new BloodStorageResponseDto
            {
                BankingOrganizationName = r.BankingOrganization.OrganizationName,
                BloodGroup = r.BloodGroup,
                DonationId = r.DonationId,
                DonorFullName = r.Donation.Donor.FirstName + " " + r.Donation.Donor.LastName,
                DonorId = r.Donation.DonorId,
                Location = r.StorageLocation,
                UnitStored = r.UnitStored,
                Id = r.Id,
                DateStored = r.CreatedAt,
                DonorOrganizationName = r.Donation.DonorOrganization?.OrganizationName ?? "nil"
            }).ToList();

            return new BaseResponse<IEnumerable<BloodStorageResponseDto>>
            {
                Data = listOfStorages,
                Message = "record retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<BloodStorageResponseDto>>> 
                               GetForSupplyAsync(BloodGroup bloodGroup)
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser is null)
            {
                return BaseResponse<IEnumerable
                <BloodStorageResponseDto>>.Failure("user not authenticated");
            }
          
            var organization = await _bankingOrganization
                       .GetByUserIdAsync(currentUser.Id);
            if (organization is null)
            {
                return BaseResponse<IEnumerable
                <BloodStorageResponseDto>>.Failure("organization not found");
            }
            var availableBloodType = (await _bloodStorageRepository
          .GetAvailableBloodsAsync(r => _bloodGroups[bloodGroup].Contains(r.BloodGroup)
           && !r.IsDeleted && !r.IsReleased && r.ExpiryDate > DateTime.UtcNow
           && r.BankingOrganizationId == organization.Id))
          .OrderBy(r => _bloodGroups[bloodGroup].IndexOf(r.BloodGroup));

            if (!availableBloodType.Any())
            {
                return BaseResponse<IEnumerable
                <BloodStorageResponseDto>>.Failure("no record found");
            }
            
            var listOfStorages = new List<BloodStorageResponseDto>();
            foreach (var storage in availableBloodType)
            {
                listOfStorages.Add(new BloodStorageResponseDto
                {
                    BloodGroup = storage.BloodGroup,
                    Location = storage.StorageLocation,
                    Id = storage.Id,
                    DateStored = storage.CreatedAt
                });
            }
            return new BaseResponse<IEnumerable<BloodStorageResponseDto>>
            {
                Data = listOfStorages,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<BloodStorageResponseDto?>> GetByIdAsync(Guid id)
        {
            var bloodStorage = await _bloodStorageRepository.FindAsync(r => !r.IsDeleted
             && !r.IsReleased && r.Id == id);

            if(bloodStorage is null)
            {
                return BaseResponse<BloodStorageResponseDto?>.
                       Failure("no record found");
            }
            return new BaseResponse<BloodStorageResponseDto?>
            {
                Data = new BloodStorageResponseDto
                {

                    BankingOrganizationName = bloodStorage.BankingOrganization.OrganizationName,
                    BloodGroup = bloodStorage.BloodGroup,
                    DonationId = bloodStorage.DonationId,
                    DonorFullName = bloodStorage.Donation.Donor.FirstName + " " + bloodStorage.Donation.Donor.LastName,
                    DonorId = bloodStorage.Donation.DonorId,
                    Location = bloodStorage.StorageLocation,
                    UnitStored = bloodStorage.UnitStored,
                    Id = bloodStorage.Id,
                    DateStored = bloodStorage.CreatedAt,
                    DonorOrganizationName = bloodStorage.Donation.DonorOrganization?.OrganizationName ?? "nil"
                    
                },
                Message = "Retrieved Successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<BloodStorageResponseDto>>> GetExpiredAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return BaseResponse<IEnumerable
                <BloodStorageResponseDto>>.Failure("user not authenticated");
            }
            var expiredBloods = await _bloodStorageRepository
            .GetExpiredAsync(r => r.CreatedAt < DateTime.UtcNow.AddMinutes(-10) 
             && !r.IsDeleted && !r.IsReleased);

            var organization = await _bankingOrganization
                       .GetByUserIdAsync(currentUser.Id);
            if (organization is null)
            {
                return BaseResponse<IEnumerable
                <BloodStorageResponseDto>>.Failure("organization not found");
            }

            if (currentUser.Role == Role.BankingOrganization)
            {
                expiredBloods = await _bloodStorageRepository
               .GetAvailableBloodsAsync(r => r.CreatedAt < DateTime.UtcNow.AddMinutes(-10) && !r.IsDeleted
                && !r.IsReleased && r.BankingOrganizationId == organization.Id);
            }
            if (!expiredBloods.Any())
            {
                return BaseResponse<IEnumerable
                <BloodStorageResponseDto>>.Failure("no record found");
            }

                var dto = expiredBloods.Select(r => new BloodStorageResponseDto
                {
                    BankingOrganizationName = r.BankingOrganization.OrganizationName,
                    BloodGroup = r.BloodGroup,
                    DonationId = r.DonationId,
                    DonorFullName = r.Donation.Donor.User.FullName,
                    DonorId = r.Donation.DonorId,
                    Location = r.StorageLocation,
                    UnitStored = r.UnitStored,
                    Id = r.Id,
                    DonorOrganizationName = r.Donation.DonorOrganization?.OrganizationName ?? "nil"
                }).ToList();
            return new BaseResponse<IEnumerable<BloodStorageResponseDto>>
            {
                Data = dto,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task UpdateExpiredBloodCountAsync()
        {
            var expiredStorages = await _bloodStorageRepository.
            GetExpiredAsync(r => !r.IsDeleted && !r.ExpiryProcess
           && !r.IsReleased && r.CreatedAt < DateTime.UtcNow.AddMinutes(-10));

            var inventories = await _bloodInventoryRepository
                .FindInventoriesAsync(r => !r.IsDeleted);

            foreach (var item in expiredStorages)
            {
                var inventory = inventories.FirstOrDefault
                (r => r.BankingOrganizationId == item.BankingOrganizationId
                && r.BloodGroup == item.BloodGroup);

                if(inventory != null)
                {
                    inventory.ExpiredUnits += 1;
                }
                item.IsExpired = true;
                item.ExpiryProcess = true;
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

