using Chinook.Data.Repository;
using Chinook.Storage.Entities;
using Chinook.Storage.Models;
using Chinook.Service.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface INoteCategoryService
    {
        IQueryable<NoteCategoryModel> Get();
        Task<ServiceResult> Post(NoteCategoryModel model);
        Task<ServiceResult> Put(NoteCategoryModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class NoteCategoryService : INoteCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NoteCategoryService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public IQueryable<NoteCategoryModel> Get()
        {
            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var list = _unitOfWork.Repository<NoteCategory>()
                .GetAll(x => !x.Deleted && x.UserId == userId)
                .OrderByDescending(x => x.UpdateDate)
                .Select(x => new NoteCategoryModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsQueryable();
            return list;
        }

        public async Task<ServiceResult> Post(NoteCategoryModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.UserId();

                var noteCategory = new NoteCategory
                {
                    UserId = userId,
                    Name = model.Name,
                    InsertDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Deleted = false
                };
                await _unitOfWork.Repository<NoteCategory>().Add(noteCategory);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Put(NoteCategoryModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.UserId();

                var category = await _unitOfWork.Repository<NoteCategory>()
                    .Get(x => x.Id == model.Id && x.UserId == userId);

                if (category != null)
                {
                    category.Name = model.Name;
                    category.UpdateDate = DateTime.Now;
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
                var userId = _httpContextAccessor.HttpContext.User.UserId();

                var category = await _unitOfWork.Repository<NoteCategory>()
                    .Get(x => x.Id == id && x.UserId == userId);

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
