using Chinook.Data.Repository;
using Chinook.Storage.Entities;
using Chinook.Storage.Models;
using Chinook.Service.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface IBlogService
    {
        IQueryable<Blog> Get();
        PaginationModel<Blog> GetByFilter(BlogFilterModel model);
        List<Blog> GetAllByCategoryUrl(string categoryUrl);
        Task<Blog> GetById(int id);
        Task<ServiceResult> Post(BlogModel model);
        Task<ServiceResult> Put(BlogModel model);
        Task<ServiceResult> Delete(int id);
        Task<List<Blog>> GetBlogsByCategoryUrl(string categoryUrl);
    }
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public IQueryable<Blog> Get()
        {
            return _unitOfWork.Repository<Blog>()
                .GetAll(x => !x.Deleted)
                .Include(x => x.BlogCategory)
                .AsQueryable();
        }

        public List<Blog> GetAllByCategoryUrl(string categoryUrl)
        {
            var list = _unitOfWork.Repository<Blog>()
                .GetAll(x => !x.Deleted && x.Published && x.BlogCategory.Url == categoryUrl).ToList();
            return list;
        }

        public async Task<Blog> GetById(int id)
        {
            return await _unitOfWork.Repository<Blog>()
                .Get(x => x.Id == id && !x.Deleted);
        }

        public PaginationModel<Blog> GetByFilter(BlogFilterModel model)
        {
            var list = Get();

            var result = new PaginationModel<Blog>
            {
                Count = list.Count(),
                List = list.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToList()
            };

            return result;
        }

        public async Task<ServiceResult> Post(BlogModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.UserId();
                var entity = new Blog()
                {
                    BlogCategoryId = model.BlogCategoryId,
                    Deleted = false,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                    InsertDate = DateTime.Now,
                    InsertedBy = userId,
                    Published = model.Published,
                    IsActive = model.IsActive,
                    ReadCount = 0,
                    Sequence = model.Sequence,
                    ShortDefinition = model.ShortDefinition,
                    Title = model.Title,
                    Url = model.Url
                };
                await _unitOfWork.Repository<Blog>().Add(entity);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ServiceResult> Put(BlogModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.UserId();

                var blog = await GetById(model.Id);

                if (blog != null)
                {
                    blog.Title = model.Title;
                    blog.Url = model.Url;
                    blog.Published = model.Published;
                    blog.IsActive = model.IsActive;
                    blog.Description = model.Description;
                    blog.ImageUrl = model.ImageUrl;
                    blog.Sequence = model.Sequence;
                    blog.ShortDefinition = model.ShortDefinition;
                    blog.UpdateDate = DateTime.Now;
                    blog.UpdatedBy = userId;

                    await _unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var blog = await GetById(id);
                if (blog != null)
                {
                    blog.Deleted = true;
                    blog.UpdateDate = DateTime.Now;
                }
                else
                {
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<List<Blog>> GetBlogsByCategoryUrl(string categoryUrl)
        {
            var list = await _unitOfWork.Repository<Blog>()
                .GetAll(x => !x.Deleted && x.IsActive && x.Published && x.BlogCategory.Url == categoryUrl).ToListAsync();

            return list;
        }
    }
}
