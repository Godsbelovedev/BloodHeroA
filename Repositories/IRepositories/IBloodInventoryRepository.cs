using BloodHeroA.Models.Entities;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface IBloodInventoryRepository
    {
        Task<BloodInventory?> FindAsync(Expression<Func<BloodInventory, bool>> exp);
        Task<IEnumerable<BloodInventory>> FindInventoriesAsync(Expression<Func<BloodInventory, bool>> exp);
        //Task<BloodInventory?> GetByStorageIdAsync(Guid storageId);
        Task CreateInventory(BloodInventory bloodInventory);
    }
}
