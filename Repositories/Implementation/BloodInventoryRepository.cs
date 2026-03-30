using BloodHeroA.Data;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.Implementation
{
    public class BloodInventoryRepository : IBloodInventoryRepository
    {
        private readonly AppDbContext _context;

        public BloodInventoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateInventory(BloodInventory bloodInventory)
        {
            await _context.BloodInventories.AddAsync(bloodInventory);
        }

        public async Task<BloodInventory?> FindAsync(Expression<Func<BloodInventory, bool>> exp)
        {
            return await _context.BloodInventories.FirstOrDefaultAsync(exp);
        }

        public async Task<IEnumerable<BloodInventory>> FindInventoriesAsync
                             (Expression<Func<BloodInventory, bool>> exp)
        {
            return await _context.BloodInventories.Where(exp).ToListAsync();
        }
        //public async Task<BloodInventory?> GetByStorageIdAsync(Guid storageId)
        //{
        //    return await _context.BloodInventories.FirstOrDefaultAsync(r => r.Id == storageId);
        //}

    }
}