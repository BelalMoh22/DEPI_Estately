using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Estately.Services.ViewModels;

namespace Estately.Services.Interfaces
{
    //public interface IServiceUser
    //{
    //    //Logical Business
    //    ValueTask<TblUser> GetUserByIDAsync(int? id);    // done
    //    Task AddUserAsync(TblUser user); // made async
    //    Task UpdateUserAsync(TblUser user);  // made async
    //    Task DeleteUserAsync(int id);    // made async
    //    Task<int> GetUserCounterAsync();    // made async
    //    ValueTask<IEnumerable<TblUser>> PaginationUserAsync(int page = 1, int pageSize = 10);   // done
    //    ValueTask<IEnumerable<TblUser>> SearchUserAsync(Expression<Func<TblUser, bool>> predicate); // done
    //    int GetMaxIDAsync();
    //}
    public interface IServiceUser
    {
        Task<UserListViewModel> GetUsersPagedAsync(int page, int pageSize, string? searchTerm);
        Task<UserViewModel?> GetUserVMAsync(int id);
        Task CreateUserAsync(UserViewModel model);
        Task UpdateUserAsync(UserViewModel model);
        Task DeleteUserAsync(int id);
        Task ToggleStatusAsync(int id);
        Task AssignRoleAsync(int userId, int userTypeId);
    }
}