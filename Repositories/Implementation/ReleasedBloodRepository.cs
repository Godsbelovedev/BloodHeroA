using BloodHeroA.Data;
using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.Implementation
{
    public class ReleasedBloodRepository : IReleasedBloodRepository
    {
        private readonly AppDbContext _context;

        public ReleasedBloodRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ReleasedBlood releasedBlood)
        {
            await _context.ReleasedBloods.AddAsync(releasedBlood);
        }

        public async Task<ReleasedBlood?> GetByIdAsync(Guid id)
        {
            return await _context.ReleasedBloods
            .Include(r => r.BankingOrganization)
            .Include(r => r.RecipientOrganization)
            .SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<ReleasedBlood>> GetReleasedByTypeAsync
                           (Expression<Func<ReleasedBlood, bool>> exp)
        {
            return await _context.ReleasedBloods.Include(r => r.BankingOrganization)
                                                .Include(r => r.RecipientOrganization)
                                                .Where(exp).ToListAsync();
        }
    }
}
