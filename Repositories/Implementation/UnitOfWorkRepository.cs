using BloodHeroA.Data;
using BloodHeroA.Repositories.IRepositories;

namespace BloodHeroA.Repositories.Implementation
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly AppDbContext _context;

        public UnitOfWorkRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
