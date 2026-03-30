using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface IDonationRepository
    {
        Task<Donation?> GetByIdAsync(Guid id);
        Task<IEnumerable<Donation>> GetAllAsync();
        Task<IEnumerable<Donation>> GetDonationsByDonorIdAsync(Guid donorId);
        Task<IEnumerable<Donation>> GetDonationsByDonorOrganizationIdAsync
                                                 (Guid donorOrganizatioIdId);
        Task<IEnumerable<Donation>> GetDonationsByBankingOrganizationIdAsync
                                                 (Guid BankingOrganizatioId);
        Task CreateAsync(Donation donation);
        Task<IEnumerable<Donation>> FindAsync
                         (Expression<Func<Donation, bool>> exp);

        Task<int> GetCountsAsync(Expression<Func<Donation, bool>> exp);
    }
}
