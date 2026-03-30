using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface IDonationRequestRepository
    {
        Task<DonationRequest?> GetByIdAsync(Guid id);
        Task<IEnumerable<DonationRequest>> GetAllAsync();
        Task CreateAsync(DonationRequest donationRequest);
        Task<IEnumerable<DonationRequest>> FindAsync
                          (Expression<Func<DonationRequest, bool>> exp);
        Task<int> FindCountAsync(Expression<Func<DonationRequest, bool>> exp);
    }
}
