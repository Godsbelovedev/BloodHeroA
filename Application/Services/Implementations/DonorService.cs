using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using BloodHeroA.Repositories.Implementation;
using BloodHeroA.Repositories.IRepositories;
using System.Drawing;

namespace BloodHeroA.Application.Services.Implementations
{
    public class DonorService : IDonorService
    {
        private readonly IDonorRepository _donorRepository;
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IDonorOrganizationRepository _donorOrganization;
        private readonly INotificationService _notificationService;
        public DonorService
        (IDonorRepository donorRepository,
         IUnitOfWorkRepository unitOfWork,
         IAuthService authService,
         IUserRepository userRepository,
         IDonorOrganizationRepository donorOrganization,
         INotificationService notificationService)
        {
            _donorRepository = donorRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _userRepository = userRepository;
            _donorOrganization = donorOrganization;
            _notificationService = notificationService;
        }
        private static int CheckAge(DateTime dateOfBirth)
        {
            DateTime today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if(dateOfBirth > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        public async Task<BaseResponse<DonorResponseDto>> 
        RegisterDonorByOrganizationAsync(DonorRequestDto donorDto)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser == null)
            {
                return BaseResponse<DonorResponseDto>.
                 Failure("no organization logged in");
            }
            var donorOrganization = await _donorOrganization
                        .GetByUserIdAsync(currentUser.Id);

            if (donorOrganization == null)
            {
                return BaseResponse<DonorResponseDto>.
                 Failure("no organization found");
            }

            var checkDonor = await _userRepository.GetUserAsync
             (u => u.Email == donorDto.Email);
            if (checkDonor != null)
            {
                return BaseResponse<DonorResponseDto>.
                 Failure("Donor already exist, create fail");
            }
            var password = BCrypt.Net.BCrypt.HashPassword(donorDto.Password);
            var user = new User
            {
                PhoneNumber = donorDto.PhoneNumber,
                Email = donorDto.Email,
                HashPassWord = password,
                Role = Role.Donor
            };

            if (donorDto.Tattoo == HealthStatus.Positive || donorDto.IVDrugConsumer == HealthStatus.Positive)
            {
                return BaseResponse<DonorResponseDto>.
                Failure("not a qualified donor");
            }
            if (DonorService.CheckAge(donorDto.DateOfBirth) < 18)
            {
                return BaseResponse<DonorResponseDto>.
                Failure("age below requirement, create fail");
            }
            var donor = new Donor
            {
                Role = user.Role,
                DateOfBirth = donorDto.DateOfBirth,
                BloodGroup = donorDto.BloodGroup,
                Email = user.Email,
                FirstName = donorDto.FirstName,
                MiddleName = donorDto.MiddleName,
                LastName = donorDto.LastName,
                Gender = donorDto.Gender,
                IVDrugConsumer = donorDto.IVDrugConsumer,
                MaritalStatus = donorDto.MaritalStatus,
                PasswordHash = user.HashPassWord,
                PhoneNumber = user.PhoneNumber,
                StateOfOrigin = donorDto.StateOfOrigin,
                User = user,
                UserId = user.Id,
                DonorOrganization = donorOrganization,
                DonorOrganizationId = donorOrganization.Id,
                //BankingOrganization = donorOrganization.BankingOrganization,
                //BankingOrganizationId = donorOrganization.BankingOrganizationId
            };
            user.FullName = $"{donor.FirstName} {donor.MiddleName} {donor.LastName}";
            user.DonorId = donor.Id;

            var notificationDto = new NotificationDTO
            {
                Subject = "Welcome to Blood Hero!",
                ReceiverEmail = donorDto.Email,
                Message = $"Dear {user.FullName},\r\n\r\n" +
             $"Welcome to Blood Hero! You have been successfully registered.\r\n\r\n" +
             $"You can now make donations, and track your activities through our platform.\r\n\r\n" +
             $"We’re excited to have you on board and look forward to supporting your lifesaving efforts.\r\n\r\n" +
             $"Regards,\r\n\r\nBlood Hero Admin"
            };

           
            await _userRepository.CreateUserAsync(user);
            await _donorRepository.CreateAsync(donor);
            donorOrganization.TotalRegisteredDonors += 1;
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(notificationDto);

            return new BaseResponse<DonorResponseDto>
            {
                Data = new DonorResponseDto
                {
                    FirstName = donor.FirstName,
                    MiddleName = donor.MiddleName,
                    LastName = donor.LastName,
                    Id = donor.Id,
                    BloodGroup = donor.BloodGroup,
                    Gender = donor.Gender,
                    DateOfBirth = donor.DateOfBirth,
                    Email = donor.Email,
                    MaritalStatus = donor.MaritalStatus,
                    PhoneNumber = donor.PhoneNumber,
                    StateOfOrigin = donor.StateOfOrigin,
                    DonorOrganizationName = donor.DonorOrganization?.OrganizationName ?? "no organization"
                },
                Message = "Create successfull",
                Status = true
            };
        }

        public async Task <BaseResponse<DonorResponseDto>> 
        SelfRegisterDonorAsync(DonorRequestDto donorDto)
        {
            var checkDonor = await _userRepository.GetUserAsync
              (u => u.Email == donorDto.Email);
            if (checkDonor is not null)
            {
                return BaseResponse<DonorResponseDto>.
                 Failure("Donor already exist, create fail");
            }
            var password = BCrypt.Net.BCrypt.HashPassword(donorDto.Password);
            var user = new User
            {
                PhoneNumber = donorDto.PhoneNumber,
                Email = donorDto.Email,
                HashPassWord = password,
                Role = Role.Donor
            };

            if (donorDto.Tattoo == HealthStatus.Positive || donorDto.IVDrugConsumer == HealthStatus.Positive)
            {
                return BaseResponse<DonorResponseDto>.
                Failure("not a qualified donor");
            }

            if (DonorService.CheckAge(donorDto.DateOfBirth) < 18)
            {
                return BaseResponse<DonorResponseDto>.
                Failure("age below requirement, create fail");
            }
            var donor = new Donor
            {
                Role = user.Role,
                DateOfBirth = donorDto.DateOfBirth,
                BloodGroup = donorDto.BloodGroup,
                Email = user.Email,
                FirstName = donorDto.FirstName,
                MiddleName = donorDto.MiddleName,
                LastName = donorDto.LastName,
                Gender = donorDto.Gender,
                IVDrugConsumer = donorDto.IVDrugConsumer,
                Tattoo = donorDto.Tattoo,
                MaritalStatus = donorDto.MaritalStatus,
                PasswordHash = user.HashPassWord,
                PhoneNumber = user.PhoneNumber,
                StateOfOrigin = donorDto.StateOfOrigin,
                User = user,
                UserId = user.Id,
               // BankingOrganization = donorDto.BankingOrganization ,
               // BankingOrganizationId = donorDto.BankingOrganizationId,
                IsDeleted = user.IsDeleted
            };
            user.FullName = $"{donor.FirstName} {donor.MiddleName} {donor.LastName}";
            user.DonorId = donor.Id;
            var notificationDto = new NotificationDTO
            {
                Subject = "Welcome to Blood Hero!",
                ReceiverEmail = donorDto.Email,
                Message = $"Dear {user.FullName},\r\n\r\n" +
               $"Welcome to Blood Hero! You have been successfully registered.\r\n\r\n" +
               $"You can now make donations, and track your activities through our platform.\r\n\r\n" +
               $"We’re excited to have you on board and look forward to supporting your lifesaving efforts.\r\n\r\n" +
               $"Regards,\r\n\r\n Blood Hero Admin"
            };
           
            await _userRepository.CreateUserAsync(user);
            await _donorRepository.CreateAsync(donor);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(notificationDto);

            return new BaseResponse<DonorResponseDto>
            {
                Data = new DonorResponseDto
                {
                   
                    FirstName = donor.FirstName,
                    MiddleName = donor.MiddleName,
                    LastName = donor.LastName,
                    Id = donor.Id,
                    BloodGroup = donor.BloodGroup,
                    Gender = donor.Gender,
                    DateOfBirth = donor.DateOfBirth,
                    Email = donor.Email,
                    Syphilis = donor.Syphilis,
                    Tattoo = donor.Tattoo,
                    Cancer = donor.Cancer,
                    ChronicDisease = donor.ChronicDisease,
                    HeartDisease = donor.HeartDisease,
                    Hemophilic = donor.Hemophilic,
                    HepatitisB = donor.HepatitisB,
                    HIV = donor.HIV,
                    IVDrugConsumer = donor.IVDrugConsumer,
                    SevereLungsDisease = donor.SevereLungsDisease,
                    MaritalStatus = donor.MaritalStatus,
                    PhoneNumber = donor.PhoneNumber,
                    StateOfOrigin = donor.StateOfOrigin,
                    DonorOrganizationName = donor.DonorOrganization?.OrganizationName ?? "Nil",
                    RegisterdDate = donor.CreatedAt
                },
                Message = "Create successfull",
                Status = true
            };
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            var userToDelete = await _donorRepository.GetByIdAsync(id);
            if (userToDelete is null)
            {
                return BaseResponse<bool>.Failure("organization do not exist");
            }
            userToDelete.IsDeleted = true;
            userToDelete.User.IsDeleted = true;
            userToDelete.User.IsAvailable = false;
            await _unitOfWork.SaveChangesAsync();
            return BaseResponse<bool>.Success(true);
        }

        public async Task<BaseResponse<IEnumerable<DonorResponseDto>>> GetAllAsync()
        {
            var allDonors = await _donorRepository.GetAllAsync();

            if (!allDonors.Any())
            {
                return BaseResponse<IEnumerable<DonorResponseDto>>.
                                       Failure("no record found");
            }

            var donorsList = new List<DonorResponseDto>();
            foreach (var donor in allDonors)
            {
                donorsList.Add(new DonorResponseDto
                {

                    FirstName = donor.FirstName,
                    MiddleName = donor.MiddleName,
                    LastName = donor.LastName,
                    Id = donor.Id,
                    BloodGroup = donor.BloodGroup,
                    Gender = donor.Gender,
                    DateOfBirth = donor.DateOfBirth,
                    Email = donor.Email,
                    Syphilis = donor.Syphilis,
                    Tattoo = donor.Tattoo,
                    Cancer = donor.Cancer,
                    ChronicDisease = donor.ChronicDisease,
                    HeartDisease = donor.HeartDisease,
                    Hemophilic = donor.Hemophilic,
                    HepatitisB = donor.HepatitisB,
                    HIV = donor.HIV,
                    IVDrugConsumer = donor.IVDrugConsumer,
                    SevereLungsDisease = donor.SevereLungsDisease,
                    MaritalStatus = donor.MaritalStatus,
                    PhoneNumber = donor.PhoneNumber,
                    StateOfOrigin = donor.StateOfOrigin,
                    DonorOrganizationName = donor.DonorOrganization?.OrganizationName ?? "no organization"
                });
            }

            return new BaseResponse<IEnumerable<DonorResponseDto>>
            {
                Data = donorsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<DonorResponseDto>>> GetAvailableDonorsAsync()
        {
            var availableDonors = await _donorRepository.GetDonorsAsync
                           (d => d.NextDueDonationDate <= DateTime.UtcNow &&
                           !d.IsDeleted && d.DonorOrganization == null);
            if (!availableDonors.Any())
            {
                return BaseResponse<IEnumerable<DonorResponseDto>>.
                                       Failure("no Donor Available for Donation");
            }


            var donorsList = new List<DonorResponseDto>();
            foreach (var donor in availableDonors)
            {
                donorsList.Add(new DonorResponseDto
                {

                    FirstName = donor.FirstName,
                    MiddleName = donor.MiddleName,
                    LastName = donor.LastName,
                    Id = donor.Id,
                    BloodGroup = donor.BloodGroup,
                    Gender = donor.Gender,
                    DateOfBirth = donor.DateOfBirth,
                    Email = donor.Email,
                    RegisterdDate = donor.CreatedAt,
                    Syphilis = donor.Syphilis,
                    Tattoo = donor.Tattoo,
                    Cancer = donor.Cancer,
                    ChronicDisease = donor.ChronicDisease,
                    HeartDisease = donor.HeartDisease,
                    Hemophilic = donor.Hemophilic,
                    HepatitisB = donor.HepatitisB,
                    HIV = donor.HIV,
                    IVDrugConsumer = donor.IVDrugConsumer,
                    SevereLungsDisease = donor.SevereLungsDisease,
                    MaritalStatus = donor.MaritalStatus,
                    PhoneNumber = donor.PhoneNumber,
                    StateOfOrigin = donor.StateOfOrigin,
                    DonorOrganizationName = donor.DonorOrganization?.OrganizationName ?? "no organization"
                });
            }
            
            return new BaseResponse<IEnumerable<DonorResponseDto>>
            {
                Data = donorsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<DonorResponseDto>>> 
            GetAvailableDonorsByDonorOrganizationIdAsync(Guid id)
        {
           
            var organization = await _donorOrganization.GetByIdAsync(id);
            if (organization is null)
            {
                return new BaseResponse<IEnumerable<DonorResponseDto>>
                {
                    Message = "organization not found",
                    Status = false
                };
            }
            var availableDonors = await _donorRepository.
            GetDonorsAsync(u => u.DonorOrganizationId == organization.Id
            && !u.IsDeleted && u.NextDueDonationDate <= DateTime.UtcNow);

            if(!availableDonors.Any())
            {
                return new BaseResponse<IEnumerable<DonorResponseDto>>
                {
                    Message = "No donor from this organization " +
                    "available for donation",
                    Status = false
                };
            }
            var donorsList = new List<DonorResponseDto>();
            foreach (var donor in availableDonors)
            {
                donorsList.Add(new DonorResponseDto
                {

                    FirstName = donor.FirstName,
                    MiddleName = donor.MiddleName,
                    LastName = donor.LastName,
                    Id = donor.Id,
                    BloodGroup = donor.BloodGroup,
                    Gender = donor.Gender,
                    DateOfBirth = donor.DateOfBirth,
                    Email = donor.Email,
                    RegisterdDate = donor.CreatedAt,
                    Syphilis = donor.Syphilis,
                    Tattoo = donor.Tattoo,
                    Cancer = donor.Cancer,
                    ChronicDisease = donor.ChronicDisease,
                    HeartDisease = donor.HeartDisease,
                    Hemophilic = donor.Hemophilic,
                    HepatitisB = donor.HepatitisB,
                    HIV = donor.HIV,
                    IVDrugConsumer = donor.IVDrugConsumer,
                    SevereLungsDisease = donor.SevereLungsDisease,
                    MaritalStatus = donor.MaritalStatus,
                    PhoneNumber = donor.PhoneNumber,
                    StateOfOrigin = donor.StateOfOrigin,
                    donorOrganizationId = donor.DonorOrganizationId,
                    DonorOrganizationName = donor.DonorOrganization?.OrganizationName ?? "no organization"
                });
            }

            return new BaseResponse<IEnumerable<DonorResponseDto>>
            {
                Data = donorsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<DonorResponseDto?>> GetByIdAsync(Guid id)
        {
            var donor = await _donorRepository.GetByIdAsync(id);
            if (donor is null)
            {
                return new BaseResponse<DonorResponseDto?>
                {
                    Data = null,
                    Message = "organization not found",
                    Status = false
                };
            }
            return new BaseResponse<DonorResponseDto?>
            {
                Data = new DonorResponseDto
                {

                    FirstName = donor.FirstName,
                    MiddleName = donor.MiddleName,
                    LastName = donor.LastName,
                    Id = donor.Id,
                    BloodGroup = donor.BloodGroup,
                    Gender = donor.Gender,
                    DateOfBirth = donor.DateOfBirth,
                    Email = donor.Email,
                    Syphilis = donor.Syphilis,
                    Tattoo = donor.Tattoo,
                    Cancer = donor.Cancer,
                    ChronicDisease = donor.ChronicDisease,
                    HeartDisease = donor.HeartDisease,
                    Hemophilic = donor.Hemophilic,
                    HepatitisB = donor.HepatitisB,
                    HIV = donor.HIV,
                    IVDrugConsumer = donor.IVDrugConsumer,
                    SevereLungsDisease = donor.SevereLungsDisease,
                    MaritalStatus = donor.MaritalStatus,
                    RegisterdDate = donor.CreatedAt,
                    PhoneNumber = donor.PhoneNumber,
                    StateOfOrigin = donor.StateOfOrigin,
                    LastDonationDate = donor.LastDonationDate,
                    TotalDonations = donor.TotalDonations,
                    DonorOrganizationName = donor.DonorOrganization?.OrganizationName ?? "no organization"
                },
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<IEnumerable<DonorResponseDto>>> GetDonorsByDonorOrganizationIdAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return new BaseResponse<IEnumerable<DonorResponseDto>>
                {
                    Message = "no login found",
                    Status = false
                };
            }

            var organization = await _donorOrganization.GetByUserIdAsync(currentUser.Id);
            if (organization is null)
            {
                return new BaseResponse<IEnumerable<DonorResponseDto>>
                {
                    Message = "organization not found",
                    Status = false
                };
            }
            var availableDonors = await _donorRepository.
            GetDonorsAsync(u => u.DonorOrganization != null
            && u.DonorOrganizationId == organization.Id
            && !u.IsDeleted);

            if (!availableDonors.Any())
            {
                return new BaseResponse<IEnumerable<DonorResponseDto>>
                {
                    Message = "No donors found for this organization",
                    Status = false
                };
            }
            var donorsList = new List<DonorResponseDto>();
            foreach (var donor in availableDonors)
            {
                donorsList.Add(new DonorResponseDto
                {
                    FirstName = donor.FirstName,
                    MiddleName = donor.MiddleName,
                    LastName = donor.LastName,
                    Id = donor.Id,
                    BloodGroup = donor.BloodGroup,
                    Gender = donor.Gender,
                    DateOfBirth = donor.DateOfBirth,
                    Email = donor.Email,
                    Syphilis = donor.Syphilis,
                    Tattoo = donor.Tattoo,
                    Cancer = donor.Cancer,
                    ChronicDisease = donor.ChronicDisease,
                    HeartDisease = donor.HeartDisease,
                    Hemophilic = donor.Hemophilic,
                    HepatitisB = donor.HepatitisB,
                    HIV = donor.HIV,
                    IVDrugConsumer = donor.IVDrugConsumer,
                    SevereLungsDisease = donor.SevereLungsDisease,
                    MaritalStatus = donor.MaritalStatus,
                    PhoneNumber = donor.PhoneNumber,
                    RegisterdDate = donor.CreatedAt,
                    StateOfOrigin = donor.StateOfOrigin,
                    DonorOrganizationName = donor.DonorOrganization?.OrganizationName ?? "no organization"
                });
            }

            return new BaseResponse<IEnumerable<DonorResponseDto>>
            {
                Data = donorsList,
                Message = "retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<DonorResponseDto?>> UpdateAsync(DonorUpdateDto donorDto)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return new BaseResponse<DonorResponseDto?>
                {
                    Data = null,
                    Message = "login required",
                    Status = false
                };
            }
            var userToUpdate = await _donorRepository
                              .GetByUserIdAsync(currentUser.Id);
            if (userToUpdate is null)
            {
                return new BaseResponse<DonorResponseDto?>
                {
                    Data = null,
                    Message = "user unknown",
                    Status = false
                };
            }
           
            if(donorDto.DateOfBirth != null)
            {
                if(DonorService.CheckAge(donorDto.DateOfBirth.Value) < 18)
                {
                    return new BaseResponse<DonorResponseDto?>
                    {
                        Data = null,
                        Message = "age below requirement, update fail",
                        Status = false
                    };
                }
            }
            userToUpdate.FirstName = donorDto.FirstName ?? userToUpdate.FirstName;
            userToUpdate.LastName = donorDto.LastName ?? userToUpdate.LastName;
            userToUpdate.MiddleName = donorDto.MiddleName ?? userToUpdate.MiddleName;
            userToUpdate.PhoneNumber = donorDto.PhoneNumber ?? userToUpdate.PhoneNumber;
            userToUpdate.User.PhoneNumber = donorDto.PhoneNumber ?? userToUpdate.PhoneNumber;
            userToUpdate.User.FullName = $"{donorDto.FirstName} {donorDto.MiddleName} {donorDto.LastName}";
            userToUpdate.MaritalStatus = donorDto.MaritalStatus ?? userToUpdate.MaritalStatus;
            
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse<DonorResponseDto?>
            {
                Data = new DonorResponseDto
                {
                    FirstName = userToUpdate.FirstName,
                    MiddleName = userToUpdate.MiddleName,
                    LastName = userToUpdate.LastName,
                    Id = userToUpdate.Id,
                    BloodGroup = userToUpdate.BloodGroup,
                    Gender = userToUpdate.Gender,
                    DateOfBirth = userToUpdate.DateOfBirth,
                    Email = userToUpdate.Email,
                    Syphilis = userToUpdate.Syphilis,
                    Tattoo = userToUpdate.Tattoo,
                    Cancer = userToUpdate.Cancer,
                    ChronicDisease = userToUpdate.ChronicDisease,
                    HeartDisease = userToUpdate.HeartDisease,
                    Hemophilic = userToUpdate.Hemophilic,
                    HepatitisB = userToUpdate.HepatitisB,
                    HIV = userToUpdate.HIV,
                    IVDrugConsumer = userToUpdate.IVDrugConsumer,
                    SevereLungsDisease = userToUpdate.SevereLungsDisease,
                    MaritalStatus = userToUpdate.MaritalStatus,
                    PhoneNumber = userToUpdate.PhoneNumber,
                    StateOfOrigin = userToUpdate.StateOfOrigin,
                    DonorOrganizationName = userToUpdate.DonorOrganization?.OrganizationName ?? "no organization"
                },
                Message = "details updated successfully",
                Status = true
    };
        }

        public async Task<BaseResponse<DonorResponseDto?>> GetByUserIdAsync(Guid userId)
        {
            var donor = await _donorRepository.GetByUserIdAsync(userId);
            if (donor is null)
            {
                return new BaseResponse<DonorResponseDto?>
                {
                    Data = null,
                    Message = "organization not found",
                    Status = false
                };
            }
            return new BaseResponse<DonorResponseDto?>
            {
                Data = new DonorResponseDto
                {
                    FirstName = donor.FirstName,
                    MiddleName = donor.MiddleName,
                    LastName = donor.LastName,
                    Id = donor.Id,
                    BloodGroup = donor.BloodGroup,
                    Gender = donor.Gender,
                    DateOfBirth = donor.DateOfBirth,
                    Email = donor.Email,
                    RegisterdDate = donor.CreatedAt,
                    Syphilis = donor.Syphilis,
                    Tattoo = donor.Tattoo,
                    Cancer = donor.Cancer,
                    ChronicDisease = donor.ChronicDisease,
                    HeartDisease = donor.HeartDisease,
                    Hemophilic = donor.Hemophilic,
                    HepatitisB = donor.HepatitisB,
                    HIV = donor.HIV,
                    IVDrugConsumer = donor.IVDrugConsumer,
                    SevereLungsDisease = donor.SevereLungsDisease,
                    MaritalStatus = donor.MaritalStatus,
                    PhoneNumber = donor.PhoneNumber,
                    StateOfOrigin = donor.StateOfOrigin,
                    DonorOrganizationName = donor.DonorOrganization?.OrganizationName ?? "no organization"

                },
                Message = "retrieved successfully",
                Status = true
            };
        }
    }

    
}
