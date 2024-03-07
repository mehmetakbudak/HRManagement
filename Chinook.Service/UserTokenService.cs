using Chinook.Data.Repository;
using Chinook.Storage.Entities;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface IUserTokenService
    {
        Task<UserTokenDmo> GetByUserId(int userId);
    }

    public class UserTokenService : IUserTokenService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserTokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<UserTokenDmo> GetByUserId(int userId)
        {
            return _unitOfWork.Repository<UserTokenDmo>().Get(x => x.UserId == userId);
        }
    }
}
