using Chinook.Data.Repository;
using Chinook.Model.Entities;
using System.Linq;

namespace Chinook.Service
{
    public interface ILookupService
    {
        IQueryable<Lookup> Get();
        IQueryable<Province> GetProvinces();
        IQueryable<City> GetCities();
    }

    public class LookupService : ILookupService
    {
        private readonly IUnitOfWork unitOfWork;

        public LookupService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Lookup> Get()
        {
            return unitOfWork.Repository<Lookup>().GetAll(x => !x.Deleted);
        }

        public IQueryable<Province> GetProvinces()
        {
            return unitOfWork.Repository<Province>().GetAll();
        }

        public IQueryable<City> GetCities()
        {
            return unitOfWork.Repository<City>().GetAll();
        }
    }
}
