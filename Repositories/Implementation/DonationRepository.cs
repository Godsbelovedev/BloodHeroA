using BloodHeroA.Data;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.Implementation
{
    public class DonationRepository : IDonationRepository
    {
        private readonly AppDbContext _context;

        public DonationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Donation donation)
        {
            await _context.Donations.AddAsync(donation);
        }

        public async Task<IEnumerable<Donation>> GetAllAsync()
        {
            return await _context.Donations
            .Include(o => o.BankingOrganization)
            .Include(o => o.DonorOrganization)
            .Include(o => o.BloodTestResult)
            .Include(o => o.Donor)
           .Where(i => !i.IsDeleted).ToListAsync();
        }

        public async Task<Donation?> GetByIdAsync(Guid id)
        {
            return await _context.Donations
            .Include(o => o.BankingOrganization)
            .Include(o => o.DonorOrganization)
            .Include(o => o.BloodTestResult)
            .Include(o => o.Donor)
            .SingleOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<IEnumerable<Donation>> GetDonationsByBankingOrganizationIdAsync(Guid bankingOrganizatioId)
        {
            return await _context.Donations
            .Include(o => o.BankingOrganization)
            .Include(o => o.DonorOrganization)
            .Include(o => o.BloodTestResult)
            .Include(o => o.Donor)
           .Where(i => i.BankingOrganizationId == bankingOrganizatioId && !i.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Donation>> GetDonationsByDonorIdAsync(Guid donorId)
        {
            return await _context.Donations
            .Include(o => o.BankingOrganization)
            .Include(o => o.DonorOrganization)
            .Include(o => o.BloodTestResult)
            .Include(o => o.Donor)
          .Where(i => i.DonorId == donorId && !i.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Donation>> GetDonationsByDonorOrganizationIdAsync(Guid donorOrganizatioIdId)
        {

            return await _context.Donations
            .Include(o => o.BankingOrganization)
            .Include(o => o.DonorOrganization)
            .Include(o => o.BloodTestResult)
            .Include(o => o.Donor)
          .Where(i => i.DonorOrganizationId == donorOrganizatioIdId && !i.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Donation>> FindAsync
                                       (Expression<Func<Donation, bool>> exp)
        {
            return await _context.Donations
                .Include(o => o.BankingOrganization)
            .Include(o => o.DonorOrganization)
            .Include(o => o.BloodTestResult)
            .Include(o => o.Donor)
           .Where(exp).ToListAsync();
        }

        public async Task<int> GetCountsAsync(Expression<Func<Donation, bool>> exp)
        {
            return await _context.Donations.CountAsync(exp);
        }
    }
}
