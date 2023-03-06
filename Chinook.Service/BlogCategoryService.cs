using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface IBlogCategoryService
    {
        IQueryable<BlogCategory> GetAll();
        Task<ServiceResult> Post(BlogCategoryModel model);
        Task<ServiceResult> Put(BlogCategoryModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        public BlogCategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<BlogCategory> GetAll()
        {
            try
            {
                var result = unitOfWork.Repository<BlogCategory>()
                    .GetAll(x => !x.Deleted)
                    .OrderByDescending(x => x.Id)
                    .AsQueryable();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ServiceResult> Post(BlogCategoryModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var category = new BlogCategory
                {
                    Name = model.Name,
                    Url = model.Url,
                    IsActive = model.IsActive,
                    Deleted = false
                };
                await unitOfWork.Repository<BlogCategory>().Add(category);
                await unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Put(BlogCategoryModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var category = await unitOfWork.Repository<BlogCategory>().Get(x => x.Id == model.Id);
                if (category != null)
                {
                    category.Name = model.Name;
                    category.Url = model.Url;
                    category.IsActive = model.IsActive;
                    await unitOfWork.SaveChanges();
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Message = "Kategori bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var category = await unitOfWork.Repository<BlogCategory>().Get(x => x.Id == id);
                if (category != null)
                {
                    category.Deleted = true;
                    await unitOfWork.SaveChanges();
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Message = "Kategori bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }
    }
}
