using BloodHeroA.DTOs;
using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using System.Linq.Expressions;

namespace BloodHeroA.Repositories.IRepositories
{
    public interface IReleasedBloodRepository
    {
        Task CreateAsync(ReleasedBlood releasedBlood);
        Task<ReleasedBlood?> GetByIdAsync(Guid id);
        Task<IEnumerable<ReleasedBlood>> GetReleasedByTypeAsync
                            (Expression<Func<ReleasedBlood, bool>> exp);
    }
}
