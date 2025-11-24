namespace Estately.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(int id);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        IQueryable<ApplicationUser> Query();
        Task<ApplicationUser?> FindByNameAsync(string username);
        Task<ApplicationUser> Search(Expression<Func<ApplicationUser, bool>> predicate);
        Task<int> Count();
    }
}