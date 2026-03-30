namespace BloodHeroA.Repositories.IRepositories
{
    public interface IUnitOfWorkRepository
    {
            Task<int> SaveChangesAsync();
    }
}
