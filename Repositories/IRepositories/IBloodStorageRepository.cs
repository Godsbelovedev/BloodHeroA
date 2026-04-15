using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface IBloodStorageRepository
    {

        Task CreateStorageAsync(BloodStorage bloodStorage);

        Task<BloodStorage?> FindAsync(Expression<Func<BloodStorage, bool>> exp);

        Task<IEnumerable<BloodStorage>> GetExpiredAsync
             (Expression<Func<BloodStorage, bool>> exp);

        Task<IEnumerable<BloodStorage>> GetAvailableBloodsAsync
                            (Expression<Func<BloodStorage, bool>> exp);
        public IQueryable<BloodStorage> GetAvailableBloods
                                   (Expression<Func<BloodStorage, bool>> exp);
    }
}
