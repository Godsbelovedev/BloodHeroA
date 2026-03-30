using BloodHeroA.Data;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.Implementation
{
    public class BloodStorageRepository : IBloodStorageRepository
    {
        private readonly AppDbContext _context;

        public BloodStorageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateStorageAsync(BloodStorage bloodStorage)
        {
            await _context.BloodStorages.AddAsync(bloodStorage);
        }
       
        public async Task<IEnumerable<BloodStorage>> GetAvailableBloodsAsync
                            (Expression<Func<BloodStorage, bool>> exp)
        {
            return await _context.BloodStorages.Include(r => r.BankingOrganization)
                                                .Include(r => r.Donation)
                                                .ThenInclude(r => r.Donor)
                                                .ThenInclude(r => r.User)
                                                .Where(exp).OrderBy(r => r.ExpiryDate)
                                                .ToListAsync();
        }

        public async Task<BloodStorage?> FindAsync(Expression<Func<BloodStorage, bool>> exp)
        {
            return await _context.BloodStorages.Include(r => r.BankingOrganization)
                                                .Include(r => r.Donation)
                                                .ThenInclude(r => r.Donor)
                                                .FirstOrDefaultAsync(exp); 
        }

        public async Task<IEnumerable<BloodStorage>> GetExpiredAsync(Expression<Func<BloodStorage, bool>> exp)
        {
            return await _context.BloodStorages
            .Include(r => r.BankingOrganization)
            .Include(r => r.Donation)
            .ThenInclude(r => r.Donor)
            .ThenInclude(r => r.User)
            .Where(exp).ToListAsync();
        }
    }
}
