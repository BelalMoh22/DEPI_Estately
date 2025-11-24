namespace Estately.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> GetByIdAsync(int id)
            => await _userManager.FindByIdAsync(id.ToString());

        public IQueryable<ApplicationUser> Query()
            => _userManager.Users;

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
            => await _userManager.Users.ToListAsync();

        public async Task<ApplicationUser?> FindByNameAsync(string username)
            => await _userManager.FindByNameAsync(username);

        public async Task<ApplicationUser> Search(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _userManager.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task<int> Count()
        {
            return await _userManager.Users.CountAsync();
        }

    }
}
