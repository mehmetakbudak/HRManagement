using Chinook.Data.Repository;
using Chinook.Service.Exceptions;
using Chinook.Service.Extensions;
using Chinook.Service.Helpers;
using Chinook.Storage.Entities;
using Chinook.Storage.Models;
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
        Task<BlogModel> GetById(int id);
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
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public List<Blog> GetAllByCategoryUrl(string categoryUrl)
        {
            var list = _unitOfWork.Repository<Blog>()
                .GetAll(x => !x.Deleted && x.Published && x.BlogCategory.Url == categoryUrl).ToList();
            return list;
        }

        public async Task<BlogModel> GetById(int id)
        {
            var data = await _unitOfWork.Repository<Blog>()
                .Get(x => x.Id == id && !x.Deleted);

            if (data is null)
            {
                throw new NotFoundException("Blog not found.");
            }
            return new BlogModel
            {
                Id = data.Id,
                BlogCategoryId = data.BlogCategoryId,
                Description = data.Description,
                ImageUrl = data.ImageUrl,
                IsActive = data.IsActive,
                Published = data.Published,
                Sequence = data.Sequence,
                ShortDefinition = data.ShortDefinition,
                Title = data.Title,
                Url = data.Url
            };
        }

        public PaginationModel<Blog> GetByFilter(BlogFilterModel model)
        {
            var query = Get();
            if (!string.IsNullOrEmpty(model.Title))
            {
                query = query.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }
            if (!string.IsNullOrEmpty(model.Url))
            {
                query = query.Where(x => x.Url.ToLower().Contains(model.Url.ToLower()));
            }
            if (model.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == model.IsActive.Value);
            }
            var list = PaginationHelper<Blog>.Paginate(query, model);
            return list;
        }

        public async Task<ServiceResult> Post(BlogModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

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
            return result;
        }

        public async Task<ServiceResult> Put(BlogModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var blog = await _unitOfWork.Repository<Blog>().Get(x => x.Id == model.Id && !x.Deleted);

            if (blog is null)
            {
                throw new NotFoundException("Blog not found.");
            }

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
            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var blog = await _unitOfWork.Repository<Blog>().Get(x => x.Id == id && !x.Deleted);

            if (blog is null)
            {
                throw new NotFoundException("Blog not found.");
            }
            blog.Deleted = true;
            blog.UpdateDate = DateTime.Now;

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
