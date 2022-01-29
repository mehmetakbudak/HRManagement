using System;
using System.Net;
using System.Linq;
using Chinook.Model.Models;
using Chinook.Model.Entities;
using Chinook.Data.Repository;
using Chinook.Data;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Service
{
    public interface IBlogCategoryService
    {
        IQueryable<BlogCategory> GetAll();
        ServiceResult Post(BlogCategoryModel model);
        ServiceResult Put(BlogCategoryModel model);
        ServiceResult Delete(int id);
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

        public ServiceResult Post(BlogCategoryModel model)
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
                unitOfWork.Repository<BlogCategory>().Add(category);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public ServiceResult Put(BlogCategoryModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var category = unitOfWork.Repository<BlogCategory>().Get(x => x.Id == model.Id);
                if (category != null)
                {
                    category.Name = model.Name;
                    category.Url = model.Url;
                    category.IsActive = model.IsActive;
                    unitOfWork.Save();
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

        public ServiceResult Delete(int id)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var category = unitOfWork.Repository<BlogCategory>().Get(x => x.Id == id);
                if (category != null)
                {
                    category.Deleted = true;
                    unitOfWork.Save();
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
