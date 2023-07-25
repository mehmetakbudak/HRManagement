using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface ICityService
    {
        IQueryable<City> GetByProvinceId(int provinceId);
    }

    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<City> GetByProvinceId(int provinceId)
        {
            return _unitOfWork.Repository<City>()
                .GetAll(x => x.ProvinceId == provinceId);
        }
    }
}
