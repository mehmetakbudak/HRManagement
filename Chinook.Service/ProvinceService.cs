using System.Collections.Generic;
using System.Linq;
using Chinook.Data.Repository;
using Chinook.Storage.Entities;

namespace Chinook.Service
{
    public interface IProvinceService
    {
        List<ProvinceDmo> Get();
    }

    public class ProvinceService : IProvinceService
    {
        public readonly IUnitOfWork _unitOfWork;

        public ProvinceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ProvinceDmo> Get()
        {
            return _unitOfWork.Repository<ProvinceDmo>().GetAll()
                .OrderBy(x => x.Id).ToList();
        }
    }
}

