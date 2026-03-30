using BloodHeroA.Data;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.Implementation
{
    public class DonationRequestRepository : IDonationRequestRepository
    {
        private readonly AppDbContext _context;

        public DonationRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(DonationRequest donationRequest)
        {
            await _context.DonationRequests.AddAsync(donationRequest);
        }

        public async Task<IEnumerable<DonationRequest>> GetAllAsync()
        {
            return await _context.DonationRequests
            .Include(o => o.RecipientOrganization)
            .Include(o => o.BankingOrganization)
            .Where(i => !i.IsDeleted).ToListAsync();
        }

        public async Task<DonationRequest?> GetByIdAsync(Guid id)
        {
            return await _context.DonationRequests
            .Include(o => o.RecipientOrganization)
            .Include(o => o.BankingOrganization)
            .SingleOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<IEnumerable<DonationRequest>> FindAsync(Expression<Func<DonationRequest, bool>> exp)
        {
           return await _context.DonationRequests
                .Include(o => o.RecipientOrganization)
                .Include(o => o.BankingOrganization)
                .Where(exp).ToListAsync();
        }

        public async Task<int> FindCountAsync(Expression<Func<DonationRequest, bool>> exp)
        {
            return await _context.DonationRequests.CountAsync(exp);
        }
    }
}
