using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface ILookupService
    {
        IQueryable<Lookup> Get();
        IQueryable<Province> GetProvinces();
        IQueryable<City> GetCitiesByProvinceId(int provinceId);
        Task<List<BlogCategoryLookupModel>> GetBlogCategories();
    }

    public class LookupService : ILookupService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LookupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Lookup> Get()
        {
            return _unitOfWork.Repository<Lookup>().GetAll(x => !x.Deleted);
        }

        public IQueryable<Province> GetProvinces()
        {
            return _unitOfWork.Repository<Province>().GetAll();
        }

        public IQueryable<City> GetCitiesByProvinceId(int provinceId)
        {
            return _unitOfWork.Repository<City>()
                .GetAll(x => x.ProvinceId == provinceId);
        }

        public async Task<List<BlogCategoryLookupModel>> GetBlogCategories()
        {
            return await _unitOfWork.Repository<BlogCategory>()
                .GetAll(x => x.IsActive && !x.Deleted)
                .Select(x => new BlogCategoryLookupModel
                {
                    Name = x.Name,
                    Url = x.Url
                }).ToListAsync();
        }
    }
}
