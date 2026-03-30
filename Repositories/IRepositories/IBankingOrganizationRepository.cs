using BloodHeroA.Models.Entities;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface IBankingOrganizationRepository
    {
        Task CreateAsync(BankingOrganization organization);
        Task<BankingOrganization?> GetByIdAsync(Guid id);
        Task<BankingOrganization?> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<BankingOrganization>> GetAllAsync();
       // void UpdateAsync(BankingOrganization organization);
    }
}
