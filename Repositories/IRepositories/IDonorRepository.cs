using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface IDonorRepository
    {
        Task<Donor?> GetByIdAsync(Guid id);
        Task<Donor?> GetByUserIdAsync(Guid userId);
        Task CreateAsync(Donor donor);
        public void UpdateAsync(Donor donor);
        //Task<IEnumerable<Donor>> GetAvailableDonorsAsync(Expression<Func<Donor, bool>> exp);
        Task<IEnumerable<Donor>> GetDonorsAsync(Expression<Func<Donor, bool>> exp);
        Task<IEnumerable<Donor>> GetAllAsync(); 
        // Task<IEnumerable<Donor>> GetDonorsAsync(Expression<Func<Donor, bool>> exp);
        //Task<PaginationDTO<Donor>> GetAvailableDonorsAsync(PaginationRequest request);
        //Task<PaginationDTO<Donor>> GetApprovedDonorsAsync(PaginationRequest request);
        //Task<PaginationDTO<Donor>> GetByBloodGroupAsync(PaginationRequest request, BloodGroup bloodGroup);
        //Task<Donor?> GetByUserIdAsync(Guid userId);
    }
}
