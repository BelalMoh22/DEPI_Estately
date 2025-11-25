using Estately.Services.ViewModels;

namespace Estately.Services.Interfaces
{
    public interface IServiceUser
    {
        Task<ApplicationUserListViewModel> GetUsersPagedAsync(int page, int pageSize, string? search);
        Task<ApplicationUserViewModel?> GetUserByIdAsync(int id);
        Task UpdateUserAsync(ApplicationUserViewModel model);
        Task DeleteUserAsync(int id);
    }
}
