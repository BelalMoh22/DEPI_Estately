using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.Implementations
{
    public class ServiceUser : IServiceUser
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceUser(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddUserAsync(TblUser user)
        {
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user != null)
            {
                await _unitOfWork.UserRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async ValueTask<TblUser> GetUserByIDAsync(int? id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<int> GetUserCounterAsync()
        {
            return await _unitOfWork.UserRepository.CounterAsync();
        }

        public int GetMaxIDAsync()
        {
            return _unitOfWork.UserRepository.GetMaxId();
        }

        public async ValueTask<IEnumerable<TblUser>> PaginationUserAsync(int page = 1, int pageSize = 10)
        {
            var userslist = await _unitOfWork.UserRepository.ReadWithPagination(page, pageSize);
            return userslist;
        }

        public async ValueTask<IEnumerable<TblUser>> SearchUserAsync(Expression<Func<TblUser, bool>> predicate)
        {
            var searchList = await _unitOfWork.UserRepository.Search(predicate);
            return searchList;
        }

        public async Task UpdateUserAsync(TblUser user)
        {
            var oldUser = await _unitOfWork.UserRepository.GetByIdAsync(user.UserID);
            if (oldUser != null)
            {
                oldUser.UserTypeID = user.UserTypeID;
                await _unitOfWork.UserRepository.UpdateAsync(oldUser);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}