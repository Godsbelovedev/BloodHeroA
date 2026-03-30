using BloodHeroA.Data;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace BloodHeroA.Repositories.Implementation
{
    public class DonorOrganizationRepository : IDonorOrganizationRepository
    {

        private readonly AppDbContext _context;

        public DonorOrganizationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DonorOrganization organization)
        {
            await _context.DonorOrganizations.AddAsync(organization);
        }

        public async Task<IEnumerable<DonorOrganization>> GetAllAsync()
        {
            return await _context.DonorOrganizations.Include(r => r.User)
            .Where(i => !i.IsDeleted).ToListAsync();
        }

        public async Task<DonorOrganization?> GetByIdAsync(Guid id)
        {
            return await _context.DonorOrganizations.Include(r => r.User)
            .SingleOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<DonorOrganization?> GetByUserIdAsync(Guid userId)
        {
            return await _context.DonorOrganizations.Include(r => r.User)
           .FirstOrDefaultAsync(r => r.UserId == userId && !r.IsDeleted);
        }

        public void UpdateAsync(DonorOrganization organization)
        {
            _context.DonorOrganizations.Update(organization);
        }
    }
}
