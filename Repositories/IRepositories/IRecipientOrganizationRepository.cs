using BloodHeroA.Models.Entities;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface IRecipientOrganizationRepository
    {
            Task<IEnumerable<RecipientOrganization>> GetAllAsync();
            Task<RecipientOrganization?> GetByIdAsync(Guid id);
            Task AddAsync(RecipientOrganization organization);
            void UpdateAsync(RecipientOrganization recipientOrganization);
             Task<RecipientOrganization?> GetByUserIdAsync(Guid userId);
    }
}
