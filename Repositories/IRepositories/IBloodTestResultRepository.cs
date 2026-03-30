using BloodHeroA.Models.Entities;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface IBloodTestResultRepository
    {
        Task CreateAsync(BloodTestResult bloodTestResult);
        void UpdateAsync(BloodTestResult bloodTestResult);
        Task<IEnumerable<BloodTestResult>> GetAllTestAsync(Expression<Func<BloodTestResult, bool>> exp);
        Task<BloodTestResult?> GetByDonationIdAsync(Guid donationId);
    }
}
