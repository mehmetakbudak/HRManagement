using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface IBlogCategoryService
    {
        IQueryable<BlogCategory> Get();
        Task<List<BlogCategoryLookupModel>> GetByLookup();
        Task<ServiceResult> Post(BlogCategoryModel model);
        Task<ServiceResult> Put(BlogCategoryModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BlogCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<BlogCategory> Get()
        {
            try
            {
                var result = _unitOfWork.Repository<BlogCategory>()
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

        public async Task<List<BlogCategoryLookupModel>> GetByLookup()
        {
            return await _unitOfWork.Repository<BlogCategory>()
                .GetAll(x => x.IsActive && !x.Deleted)
                .Select(x => new BlogCategoryLookupModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Url = x.Url
                }).ToListAsync();
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
                await _unitOfWork.Repository<BlogCategory>().Add(category);
                await _unitOfWork.SaveChanges();
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
                var category = await _unitOfWork.Repository<BlogCategory>().Get(x => x.Id == model.Id);
                if (category != null)
                {
                    category.Name = model.Name;
                    category.Url = model.Url;
                    category.IsActive = model.IsActive;
                    await _unitOfWork.SaveChanges();
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
                var category = await _unitOfWork.Repository<BlogCategory>().Get(x => x.Id == id);
                if (category != null)
                {
                    category.Deleted = true;
                    await _unitOfWork.SaveChanges();
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
