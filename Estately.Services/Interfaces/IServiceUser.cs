using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Estately.Services.ViewModels;

namespace Estately.Services.Interfaces
{
    public interface IServiceUser
    {
        Task<UserListViewModel> GetUsersPagedAsync(int page, int pageSize, string? search);
        Task<UserViewModel?> GetUserByIdAsync(int id);
        Task CreateUserAsync(UserViewModel model);
        Task UpdateUserAsync(UserViewModel model);
        Task DeleteUserAsync(int id);
        //Task ToggleStatusAsync(int id);
        Task AssignRoleAsync(int userId, int userTypeId);
        Task<int> GetUserCounterAsync();
        int GetMaxIDAsync();
        ValueTask<IEnumerable<TblUser>> SearchUserAsync(Expression<Func<TblUser, bool>> predicate);
        Task<IEnumerable<UserTypeViewModel>> GetAllUserTypesAsync();
    }
}