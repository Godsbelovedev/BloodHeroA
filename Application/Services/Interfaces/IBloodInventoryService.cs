using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using System.Linq.Expressions;

namespace BloodHeroA.Application.Services.Interfaces
{
    public interface IBloodInventoryService
    {
        Task<BaseResponse<BloodInventoryResponseDTO>>
          GetAllInventoryForBloodGroupA_PositiveAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupB_PositiveAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
         GetAllInventoryForBloodGroupAB_PositiveAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupO_PositiveAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupA_NegativeAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
         GetAllInventoryForBloodGroupB_NegativeAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupAB_NegativeAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupO_NegativeAsync();


        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupA_PositiveByBankingOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupB_PositiveByBankingOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupAB_PositiveByBankingOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupO_PositiveByBankingOrganizationIdAsync();


        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupA_NegativeByBankingOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupB_NegativeByBankingOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupAB_NegativeByBankingOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupO_NegativeByBankingOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupA_PositiveByRecipientOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupB_PositiveByRecipientOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupAB_PositiveByRecipientOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupO_PositiveByRecipientOrganizationIdAsync();


        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupA_NegativeByRecipientOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupB_NegativeByRecipientOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupAB_NegativeByRecipientOrganizationIdAsync();

        Task<BaseResponse<BloodInventoryResponseDTO>>
        GetAllInventoryForBloodGroupO_NegativeByRecipientOrganizationIdAsync();
    }
}