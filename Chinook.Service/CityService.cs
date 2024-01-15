using Chinook.Data.Repository;
using Chinook.Storage.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Chinook.Service
{
    public interface ICityService
    {
        List<City> GetByProvinceId(int provinceId);
    }

    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<City> GetByProvinceId(int provinceId)
        {
            return _unitOfWork.Repository<City>()
                .GetAll(x => x.ProvinceId == provinceId)
                .OrderBy(x => x.Name)
                .ToList();
        }
    }
}
