using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.Implementation;
using BloodHeroA.Repositories.IRepositories;
using System.Drawing;

namespace BloodHeroA.Application.Services.Implementations
{

    public class BloodTestResultService : IBloodTestResultService
    {
        private readonly IDonationRepository _donation;
        private readonly IAuthService _authService;
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IBankingOrganizationRepository _bankingOrganization;
        private readonly INotificationService _notificationService;
        private readonly IBloodTestResultRepository _bloodTestResult;
        public BloodTestResultService(IAuthService authService,
                                IUnitOfWorkRepository unitOfWork,
                                IBankingOrganizationRepository bankingOrganization,
                                INotificationService notificationService,
                                IDonationRepository donation,
                                IBloodTestResultRepository bloodTestResult)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _bankingOrganization = bankingOrganization;
            _notificationService = notificationService;
            _donation = donation;
            _bloodTestResult = bloodTestResult;
        }

        public async Task<BaseResponse<BloodTestResultResponseDto>> 
                    CreateAsync(BloodTestResultDTO bloodTestResult)
        {
            var currentUser = await _authService.GetCurrentUser();
            if(currentUser is null)
            {
               return BaseResponse<BloodTestResultResponseDto>.Failure("user unauthenticated");
            }

            var bankingOrganization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if(bankingOrganization is null)
            {
               return BaseResponse<BloodTestResultResponseDto>.Failure("organization not found");
            }

            var donationToTest = await _donation.GetByIdAsync(bloodTestResult.DonationId);
            if (donationToTest is null || donationToTest.IsTested)
            {
                return BaseResponse<BloodTestResultResponseDto>.Failure("donation not found or already tested");
            }
            if (donationToTest.Donor is null)
            {
                return BaseResponse<BloodTestResultResponseDto>.Failure("Donor not found");
            }


            var bloodTest = new BloodTestResult
            {
                DonationId = donationToTest.Id,
                BankingOrganizationId = bankingOrganization.Id,
                Syphilis = bloodTestResult.Syphilis,
                Tattoo = bloodTestResult.Tattoo,
                Cancer = bloodTestResult.Cancer,
                ChronicDisease = bloodTestResult.ChronicDisease,
                HeartDisease = bloodTestResult.HeartDisease,
                Hemophilic = bloodTestResult.Hemophilic,
                HepatitisB = bloodTestResult.HepatitisB,
                HIV = bloodTestResult.HIV,
                IVDrugConsumer = bloodTestResult.IVDrugConsumer,
                SevereLungsDisease = bloodTestResult.SevereLungsDisease,
                BloodGroup = donationToTest.DonatedBloodType,
                IsHealthy = bloodTestResult.IsHealthy,
                TestRemark = bloodTestResult.TestRemark,
                BankingOrganization = bankingOrganization,
                Donation = donationToTest
            };

            donationToTest.IsHealthy = bloodTest.IsHealthy;
            donationToTest.IsTested = true;
            donationToTest.Donor.Cancer = bloodTestResult.Cancer;
            donationToTest.Donor.ChronicDisease = bloodTestResult.ChronicDisease;
            donationToTest.Donor.IVDrugConsumer = bloodTestResult.IVDrugConsumer;
            donationToTest.Donor.HIV = bloodTestResult.HIV;
            donationToTest.Donor.HeartDisease = bloodTestResult.HeartDisease;
            donationToTest.Donor.HepatitisB = bloodTestResult.HepatitisB;
            donationToTest.Donor.HepatitisB = bloodTestResult.HepatitisB;
            donationToTest.Donor.SevereLungsDisease = bloodTestResult.SevereLungsDisease;
            donationToTest.Donor.Syphilis = bloodTestResult.Syphilis;
            donationToTest.Donor.Tattoo = bloodTestResult.Tattoo;
            donationToTest.Donor.BloodGroup = bloodTest.BloodGroup;

            await _bloodTestResult.CreateAsync(bloodTest);
            await _unitOfWork.SaveChangesAsync();
            var notificationDto = new NotificationDTO
            {
                Message = $"Dear {donationToTest.Donor?.FirstName} {donationToTest.Donor?.LastName}\r\n\r\n" +
                $"Your blood donation Donated at {donationToTest.CreatedAt} " +
                $"with donation ID {donationToTest.Id}  has been tested. " +
                $"Below are the results:\r\n\r\n" +
                $"Donation ID: {donationToTest.Id}\r\n\r\n" +
                $"Blood Group: {bloodTest.BloodGroup}\r\n\r\n" +

                $"Test Results:\r\n\r\n" +
                $"- HIV Positive: {bloodTest.HIV }\r\n\r\n" +
                $"- Hepatitis B Positive: {bloodTest.HepatitisB}\r\n\r\n" +
                $"- Syphilis Positive: {bloodTest.Syphilis}\r\n\r\n" +
                $"- Cancer Detected: {bloodTest.Cancer}\r\n\r\n" +
                $"- Heart Disease: {bloodTest.HeartDisease}\r\n\r\n" +
                $"- Chronic Disease: {bloodTest.ChronicDisease}\r\n\r\n" +
                $"- Hemophilia: {bloodTest.Hemophilic }\r\n\r\n" +
                $"- IV Drug Use Detected: {bloodTest.IVDrugConsumer}\r\n\r\n" +
                $"- Severe Lung Disease: {bloodTest.SevereLungsDisease}\r\n\r\n" +
                $"- Tattoo Detected: {bloodTest.Tattoo}\r\n\r\n" +
                $"- Healthy Blood Status: {bloodTest.IsHealthy}\r\n\r\n" +


                $"Remark: {bloodTest.TestRemark}\r\n\r\n" +

                $"If you have any concerns, please contact {bankingOrganization.OrganizationName} on " +
                $" {bankingOrganization.User!.Email}\r\n\r\n" +
                $"Regards,\n{bankingOrganization.OrganizationName}",
                        ReceiverEmail = donationToTest.Donor!.Email,
                        Subject = "BLOOD TEST RESULT NOTIFICATION"
            };
            await _notificationService.SendNotificationAsync(notificationDto);
            return new BaseResponse<BloodTestResultResponseDto>
            {
                Data = new BloodTestResultResponseDto
                {
                    Id = bloodTest.Id,
                    BloodGroup = bloodTest.BloodGroup,
                    DonationId = donationToTest.Id,
                    DonorFullName = $"{donationToTest.Donor?.FirstName} {donationToTest.Donor?.LastName}".Trim(),
                    Syphilis = bloodTestResult.Syphilis,
                    Tattoo = bloodTestResult.Tattoo,
                    Cancer = bloodTestResult.Cancer,
                    ChronicDisease = bloodTestResult.ChronicDisease,
                    HeartDisease = bloodTestResult.HeartDisease,
                    Hemophilic = bloodTestResult.Hemophilic,
                    HepatitisB = bloodTestResult.HepatitisB,
                    HIV = bloodTestResult.HIV,
                    IVDrugConsumer = bloodTestResult.IVDrugConsumer,
                    SevereLungsDisease = bloodTestResult.SevereLungsDisease,
                    TestDate = bloodTest.CreatedAt,
                    IsHealthy = bloodTest.IsHealthy,
                    TestRemark = bloodTest.TestRemark
                },
                Message = "blood test conducted, results recorded and notification sent successfully.",
                Status = true
            };
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid donationId)
        {
            var testToDelete = await _bloodTestResult.GetByDonationIdAsync(donationId);
            if (testToDelete == null)
            {
                return BaseResponse<bool>.Failure("donation not found");
            }
            testToDelete.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();
            return BaseResponse<bool>.Success(true, "delete successful");
        }

        public async Task<BaseResponse<IEnumerable<BloodTestResultResponseDto>>> GetAllTestByBankingOrganizationAsync()
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return BaseResponse<IEnumerable<BloodTestResultResponseDto>>.Failure("user unauthenticated");
            }

            var bankingOrganization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (bankingOrganization is null)
            {
                return BaseResponse<IEnumerable<BloodTestResultResponseDto>>.Failure("organization not found");
            }
            var allBloodTests = await _bloodTestResult.GetAllTestAsync
                (i => !i.IsDeleted && i.BankingOrganizationId == bankingOrganization.Id);

            if (!allBloodTests.Any())
            {
                return BaseResponse<IEnumerable<BloodTestResultResponseDto>>.
                                       Failure("no record found");
            }

            var bloodTestList = new List<BloodTestResultResponseDto>();
            foreach (var allBloodTest in allBloodTests)
            {
                bloodTestList.Add(new BloodTestResultResponseDto
                {
                    Id = allBloodTest.Id,
                    BloodGroup = allBloodTest.BloodGroup,
                    DonationId = allBloodTest.DonationId,
                    DonorFullName = $"{allBloodTest.Donation.Donor.FirstName} {allBloodTest.Donation.Donor.LastName}".Trim(),
                    Syphilis = allBloodTest.Syphilis,
                    Tattoo = allBloodTest.Tattoo,
                    Cancer = allBloodTest.Cancer,
                    ChronicDisease = allBloodTest.ChronicDisease,
                    HeartDisease = allBloodTest.HeartDisease,
                    Hemophilic = allBloodTest.Hemophilic,
                    HepatitisB = allBloodTest.HepatitisB,
                    HIV = allBloodTest.HIV,
                    IVDrugConsumer = allBloodTest.IVDrugConsumer,
                    SevereLungsDisease = allBloodTest.SevereLungsDisease,
                    TestDate = allBloodTest.CreatedAt
                });
            }
            return BaseResponse<IEnumerable<BloodTestResultResponseDto>>
                .Success(bloodTestList, "record(s) retrieved successfully");
        }

        public async Task<BaseResponse<BloodTestResultResponseDto?>> GetByDonationIdAsync(Guid donationId)
        {
            var bloodTest = await _bloodTestResult.GetByDonationIdAsync(donationId);
            if (bloodTest is null)
            {
                return BaseResponse<BloodTestResultResponseDto?>.Failure("no record found");
            }
            var bloodTestResponse = new BloodTestResultResponseDto
            {
                Id = bloodTest.Id,
                BloodGroup = bloodTest.BloodGroup,
                DonationId = bloodTest.DonationId,
                DonorFullName = $"{bloodTest.Donation.Donor.FirstName} {bloodTest.Donation.Donor.LastName}".Trim(),
                Syphilis = bloodTest.Syphilis,
                Tattoo = bloodTest.Tattoo,
                Cancer = bloodTest.Cancer,
                ChronicDisease = bloodTest.ChronicDisease,
                HeartDisease = bloodTest.HeartDisease,
                Hemophilic = bloodTest.Hemophilic,
                HepatitisB = bloodTest.HepatitisB,
                HIV = bloodTest.HIV,
                IVDrugConsumer = bloodTest.IVDrugConsumer,
                SevereLungsDisease = bloodTest.SevereLungsDisease,
                TestDate = bloodTest.CreatedAt,
                TestRemark = bloodTest.TestRemark,
                IsHealthy = bloodTest.IsHealthy
            };

            return BaseResponse<BloodTestResultResponseDto?>
            .Success(bloodTestResponse, "record retrieved successfully");
        }

        public async Task<BaseResponse<BloodTestResultResponseDto>> UpdateAsync(BloodTestResultUpdateDTO bloodTestResultUpdate)
        {
            var currentUser = await _authService.GetCurrentUser();
            if (currentUser is null)
            {
                return BaseResponse<BloodTestResultResponseDto>.Failure("user unauthenticated");
            }

            var bankingOrganization = await _bankingOrganization.GetByUserIdAsync(currentUser.Id);
            if (bankingOrganization is null)
            {
                return BaseResponse<BloodTestResultResponseDto>.Failure("organization not found");
            }

            var bloodTestToUpdate = await _bloodTestResult.GetByDonationIdAsync(bloodTestResultUpdate.Id);
            if (bloodTestToUpdate is null)
            {
                return BaseResponse<BloodTestResultResponseDto>.Failure("no record found");
            }
        
            bloodTestToUpdate.BankingOrganizationId = bankingOrganization.Id;
            bloodTestToUpdate.Cancer = bloodTestResultUpdate.Cancer ?? bloodTestToUpdate.Cancer;
            bloodTestToUpdate.ChronicDisease = bloodTestResultUpdate.ChronicDisease ?? bloodTestToUpdate.ChronicDisease;
            bloodTestToUpdate.IVDrugConsumer = bloodTestResultUpdate.IVDrugConsumer ?? bloodTestToUpdate.IVDrugConsumer;
            bloodTestToUpdate.HIV = bloodTestResultUpdate.HIV ?? bloodTestToUpdate.HIV;
            bloodTestToUpdate.HeartDisease = bloodTestResultUpdate.HeartDisease ?? bloodTestToUpdate.HeartDisease;
            bloodTestToUpdate.HepatitisB = bloodTestResultUpdate.HepatitisB ?? bloodTestToUpdate.HepatitisB;
            bloodTestToUpdate.Hemophilic= bloodTestResultUpdate.Hemophilic ?? bloodTestToUpdate.Hemophilic;
            bloodTestToUpdate.SevereLungsDisease = bloodTestResultUpdate.SevereLungsDisease ?? bloodTestToUpdate.SevereLungsDisease;
            bloodTestToUpdate.Tattoo = bloodTestResultUpdate.Tattoo ?? bloodTestToUpdate.Tattoo;
            bloodTestToUpdate.BloodGroup = bloodTestResultUpdate.BloodGroup ?? bloodTestToUpdate.BloodGroup;
            bloodTestToUpdate.Tattoo = bloodTestResultUpdate.Tattoo ?? bloodTestToUpdate.Tattoo;
            bloodTestToUpdate.IsHealthy = bloodTestResultUpdate.IsHealthy ?? bloodTestToUpdate.IsHealthy;
            await _unitOfWork.SaveChangesAsync();

            var bloodTestResponse = new BloodTestResultResponseDto
            {
                Id = bloodTestToUpdate.Id,
                BloodGroup = bloodTestToUpdate.BloodGroup,
                DonationId = bloodTestToUpdate.DonationId,
                DonorFullName = $"{bloodTestToUpdate.Donation.Donor.FirstName} {bloodTestToUpdate.Donation.Donor.LastName}".Trim(),
                Syphilis = bloodTestToUpdate.Syphilis,
                Tattoo = bloodTestToUpdate.Tattoo,
                Cancer = bloodTestToUpdate.Cancer,
                ChronicDisease = bloodTestToUpdate.ChronicDisease,
                HeartDisease = bloodTestToUpdate.HeartDisease,
                Hemophilic = bloodTestToUpdate.Hemophilic,
                HepatitisB = bloodTestToUpdate.HepatitisB,
                HIV = bloodTestToUpdate.HIV,
                IVDrugConsumer = bloodTestToUpdate.IVDrugConsumer,
                SevereLungsDisease = bloodTestToUpdate.SevereLungsDisease,
                TestDate = bloodTestToUpdate.CreatedAt,
                TestRemark = bloodTestToUpdate.TestRemark,
                IsHealthy = bloodTestToUpdate.IsHealthy
            };
           
            return BaseResponse<BloodTestResultResponseDto>
            .Success(bloodTestResponse, "record successfully updated");
        }
    }
}
