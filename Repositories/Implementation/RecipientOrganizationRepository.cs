using BloodHeroA.Data;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BloodHeroA.Repositories.Implementation
{
    public class RecipientOrganizationRepository : IRecipientOrganizationRepository
    {
        private readonly AppDbContext _context;

        public RecipientOrganizationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RecipientOrganization recipientOrganization)
        {
            await _context.RecipientOrganizations.AddAsync(recipientOrganization);
        }


        public async Task<IEnumerable<RecipientOrganization>> GetAllAsync()
        {
            return await _context.RecipientOrganizations.Include(r => r.User)
            .Where(i => !i.IsDeleted).ToListAsync();
        }

        public async Task<RecipientOrganization?> GetByIdAsync(Guid id)
        {
            return await _context.RecipientOrganizations.Include(r => r.User)
            .SingleOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<RecipientOrganization?> GetByUserIdAsync(Guid userId)
        {
            return await _context.RecipientOrganizations.Include(r => r.User)
            .FirstOrDefaultAsync(r => r.UserId == userId && !r.IsDeleted);
        }

        public void UpdateAsync(RecipientOrganization recipientOrganization)
        {
            _context.RecipientOrganizations.Update(recipientOrganization);
        }
    }
}
