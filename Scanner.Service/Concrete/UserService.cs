using Scanner.Core.Abstract;
using Scanner.Core.Models;
using Scanner.Helper.Security.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Service.Concrete
{

    public interface IUserService : IService<User>
    {
        Task<User> GetByMailAddress(string email);
        Task<IEnumerable<User>> GetUsers();
        void SaveRefreshToken(int userId, string refreshToken, DateTime endDate);
    }

    public class UserService : Service<User>, IUserService
    {
        private ITokenHelper _tokenHelper;
        public UserService(IUnitOfWork unitOfWork, IRepository<User> repository, ITokenHelper tokenHelper) : base(unitOfWork, repository)
        {
            _tokenHelper = tokenHelper;
        }

        public async Task<User> GetByMailAddress(string email)
        {
            return await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _unitOfWork.GetRepository<User>().Where(x => x.IsDeleted == false);
        }

        public async void SaveRefreshToken(int userId, string refreshToken, DateTime endDate)
        {
            var user = await GetByIdAsync(userId);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireDate = endDate;
            _unitOfWork.Commit();
        }
    }
}
