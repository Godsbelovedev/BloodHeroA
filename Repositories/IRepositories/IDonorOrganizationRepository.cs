using BloodHeroA.Models.Entities;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface IDonorOrganizationRepository
    {
        Task<IEnumerable<DonorOrganization>> GetAllAsync();
        Task<DonorOrganization?> GetByIdAsync(Guid id);
        Task<DonorOrganization?> GetByUserIdAsync(Guid userId);
        Task AddAsync(DonorOrganization organization);
        void UpdateAsync(DonorOrganization organization);
    }
}
