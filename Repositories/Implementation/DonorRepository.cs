using BloodHeroA.Data;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.Implementation
{
    public class DonorRepository : IDonorRepository
    {
        private readonly AppDbContext _context;

        public DonorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Donor donor)
        {
            await _context.Donors.AddAsync(donor);
        }


        public async Task<IEnumerable<Donor>> GetAllAsync()
        {

            return await _context.Donors.Include(d => d.DonorOrganization)
             .Where(i => !i.IsDeleted).ToListAsync();
        }

        //public async Task<IEnumerable<Donor>> GetAvailableDonorsAsync(Expression<Func<Donor, bool>> exp)
        //{

        //    return await _context.Donors.Include(d => d.DonorOrganization)
        //    .Where(exp).ToListAsync();
        //}

        public async Task<Donor?> GetByIdAsync(Guid id)
        {
            return await _context.Donors.Include(d => d.DonorOrganization)
            .Include(d => d.User).SingleOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<Donor?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Donors.Include(d => d.DonorOrganization)
             .Include(d => d.User).FirstOrDefaultAsync(r => r.UserId == userId && !r.IsDeleted);
        }
        
        public async Task<IEnumerable<Donor>> GetDonorsAsync(Expression<Func<Donor, bool>> exp)
        {
            return await _context.Donors
            .Include(d => d.User)
            .Include(d => d.DonorOrganization) 
            .Where(exp).ToListAsync();
        }

        public void UpdateAsync(Donor donor)
        {
            _context.Donors.Update(donor);
        }
    }
}
