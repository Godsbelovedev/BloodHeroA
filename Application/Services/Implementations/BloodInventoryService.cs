using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BloodHeroA.Application.Services.Implementations
{
    public class BloodInventoryService : IBloodInventoryService
    {
        private readonly IAuthService _authService;
        private readonly IBankingOrganizationRepository _bankingOrganization;
        private readonly IBloodInventoryRepository _bloodInventoryRepository;
        private readonly IRecipientOrganizationRepository _recipientOrganization;

        public BloodInventoryService(IAuthService authService,
              IBankingOrganizationRepository bankingOrganization,
              IBloodInventoryRepository bloodInventoryRepository,
              IRecipientOrganizationRepository recipientOrganization)
        {
            _authService = authService;
            _bankingOrganization = bankingOrganization;
            _bloodInventoryRepository = bloodInventoryRepository;
            _recipientOrganization = recipientOrganization;
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
         GetAllInventoryForBloodGroupAB_NegativeAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser == null || currentUser.Role != Role.Admin)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }
            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.AB_Negative);
            if(inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.AB_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupAB_NegativeByBankingOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.BankingOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if(organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.AB_Negative 
               && r.BankingOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.AB_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupAB_NegativeByRecipientOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.RecipientOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.AB_Negative
               && r.RecipientOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.AB_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupAB_PositiveAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.Admin)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }
            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.AB_Positive);
            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.AB_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupAB_PositiveByBankingOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.BankingOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.AB_Positive
               && r.BankingOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.AB_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
         GetAllInventoryForBloodGroupAB_PositiveByRecipientOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.RecipientOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.AB_Positive
               && r.RecipientOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.AB_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupA_NegativeAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.Admin)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }
            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.A_Negative);
            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.A_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupA_NegativeByBankingOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.BankingOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.A_Negative
               && r.BankingOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.A_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupA_NegativeByRecipientOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.RecipientOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.A_Negative
               && r.RecipientOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.A_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupA_PositiveAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.Admin)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }
            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.A_Positive);
            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.A_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupA_PositiveByBankingOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.BankingOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.A_Positive
               && r.BankingOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.A_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupA_PositiveByRecipientOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.RecipientOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.A_Positive
               && r.RecipientOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.A_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupB_NegativeAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.Admin)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }
            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.B_Negative);
            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.B_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupB_NegativeByBankingOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.BankingOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.B_Negative
               && r.BankingOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.B_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupB_NegativeByRecipientOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.RecipientOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.B_Negative
               && r.RecipientOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.B_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupB_PositiveAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.Admin)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }
            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.B_Positive);
            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.B_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupB_PositiveByBankingOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.BankingOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.B_Positive
               && r.BankingOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.B_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupB_PositiveByRecipientOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.RecipientOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.B_Positive
               && r.RecipientOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.B_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupO_NegativeAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.Admin)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }
            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.O_Negative);
            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.O_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupO_NegativeByBankingOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.BankingOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.O_Negative
               && r.BankingOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.O_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupO_NegativeByRecipientOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.RecipientOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.O_Negative
               && r.RecipientOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.O_Negative,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupO_PositiveAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.Admin)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }
            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.O_Positive);
            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.O_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupO_PositiveByBankingOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.BankingOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.O_Positive
               && r.BankingOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.O_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }

        public async Task<BaseResponse<BloodInventoryResponseDTO>> 
        GetAllInventoryForBloodGroupO_PositiveByRecipientOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Role != Role.RecipientOrganization)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("user not authenticated");
            }

            var organization = await _recipientOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("organization not found");
            }

            var inventory = await _bloodInventoryRepository.FindAsync
               (r => r.BloodGroup == BloodGroup.O_Positive
               && r.RecipientOrganizationId == organization.Id);

            if (inventory == null)
            {
                return BaseResponse<BloodInventoryResponseDTO>.Failure("no inventory found");
            }
            return new BaseResponse<BloodInventoryResponseDTO>
            {
                Data = new BloodInventoryResponseDTO
                {
                    BloodGroup = BloodGroup.O_Positive,
                    StoredUnits = inventory.StoredUnits,
                    ExpiredUnits = inventory.ExpiredUnits,
                    ReleasedUnits = inventory.ReleasedUnits,
                    AvailableUnits = inventory.UnitsAvailable
                }
            };
        }
    }  
}

      
