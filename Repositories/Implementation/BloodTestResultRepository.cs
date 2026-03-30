using BloodHeroA.Data;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.Implementation
{
    public class BloodTestResultRepository : IBloodTestResultRepository
    {
        private readonly AppDbContext _context;

        public BloodTestResultRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(BloodTestResult bloodTestResult)
        {
            await _context.BloodTestResults.AddAsync(bloodTestResult);
        }

        public async Task<BloodTestResult?> GetByDonationIdAsync(Guid donationId)
        {
            return await _context.BloodTestResults.Include(o => o.BankingOrganization)
                .Include(o => o.Donation).ThenInclude(o => o.Donor)
                .FirstOrDefaultAsync(r => r.DonationId == donationId && !r.IsDeleted);
        }

        public async Task <IEnumerable<BloodTestResult>> GetAllTestAsync(Expression<Func<BloodTestResult, bool>> exp)
        {
            return await _context.BloodTestResults
                .Include(o => o.BankingOrganization)
                .Include(o => o.Donation)
                .ThenInclude(o => o.Donor)
            .Where(exp).ToListAsync();
        }

        public void UpdateAsync(BloodTestResult bloodTestResult)
        {
            _context.BloodTestResults.Update(bloodTestResult);
        }
    }
}
