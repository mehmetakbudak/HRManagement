using System.Linq;
using Chinook.Data.Repository;
using Chinook.Storage.Entities;

namespace Chinook.Service
{
    public interface IProvinceService
    {
        IQueryable<Province> Get();
    }

    public class ProvinceService : IProvinceService
    {
        public readonly IUnitOfWork _unitOfWork;

        public ProvinceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Province> Get()
        {
            return _unitOfWork.Repository<Province>().GetAll();
        }
    }
}

