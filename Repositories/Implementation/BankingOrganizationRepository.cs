using BloodHeroA.Data;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BloodHeroA.Repositories.Implementation
{
    public class BankingOrganizationRepository : IBankingOrganizationRepository
    {
        private readonly AppDbContext _context;

        public BankingOrganizationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(BankingOrganization organization)
        {
           await _context.BankingOrganizations.AddAsync(organization);
        }


        public async Task<IEnumerable<BankingOrganization>> GetAllAsync()
        {
            return await _context.BankingOrganizations.Include(r => r.User)
             .Where(i => !i.IsDeleted).ToListAsync();
        }

        public async Task<BankingOrganization?> GetByIdAsync(Guid id)
        {
            return await _context.BankingOrganizations.Include(r => r.User)
            .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);
        }

        public async Task<BankingOrganization?> GetByUserIdAsync(Guid userId)
        {
            return await _context.BankingOrganizations.Include(r => r.User)
           .FirstOrDefaultAsync(i => i.UserId == userId && !i.IsDeleted);
        }

        //public void UpdateAsync(BankingOrganization organization)
        //{
        //    _context.BankingOrganizations.Update(organization);
        //}
    }
}
