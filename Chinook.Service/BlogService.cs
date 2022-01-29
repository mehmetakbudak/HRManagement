using Chinook.Data;
using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Chinook.Service
{
    public interface IBlogService
    {
        IQueryable<Blog> GetAll();
        List<Blog> GetAllByCategoryUrl(string categoryUrl);
        Blog GetById(int id);
        ServiceResult Post(BlogModel model);
        ServiceResult Put(BlogModel model);
        ServiceResult Delete(int id);
    }
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Blog> GetAll()
        {
            return _unitOfWork.Repository<Blog>()
                .GetAll(x => !x.Deleted)
                .Include(x => x.BlogCategory);
        }

        public List<Blog> GetAllByCategoryUrl(string categoryUrl)
        {
            var list = _unitOfWork.Repository<Blog>().GetAll(x => !x.Deleted && x.Published && x.BlogCategory.Url == categoryUrl).ToList();
            return list;
        }

        public Blog GetById(int id)
        {
            return _unitOfWork.Repository<Blog>().Get(x => x.Id == id && !x.Deleted);
        }

        public ServiceResult Post(BlogModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var entity = new Blog()
                {
                    BlogCategoryId = model.BlogCategoryId,
                    Deleted = false,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                    InsertDate = DateTime.Now,
                    InsertedBy = AuthTokenContent.Current.UserId,
                    Published = model.Published,
                    IsActive = model.IsActive,
                    ReadCount = 0,
                    Sequence = model.Sequence,
                    ShortDefinition = model.ShortDefinition,
                    Title = model.Title,
                    Url = model.Url
                };
                _unitOfWork.Repository<Blog>().Add(entity);
                _unitOfWork.Save();

            }
            catch (Exception ex)
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public ServiceResult Put(BlogModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var blog = GetById(model.Id);
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
                    blog.UpdatedBy = AuthTokenContent.Current.UserId;

                    _unitOfWork.Save();
                }

            }
            catch (Exception ex)
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public ServiceResult Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var blog = GetById(id);
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
    }
}
