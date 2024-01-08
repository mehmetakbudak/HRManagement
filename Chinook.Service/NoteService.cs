using Chinook.Data.Repository;
using Chinook.Storage.Entities;
using Chinook.Storage.Models;
using Chinook.Service.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface INoteService
    {
        IQueryable<NoteModel> Get();
        IQueryable<NoteModel> GetByCategoryId(int categoryId);
        Task<ServiceResult> Post(NoteModel model);
        Task<ServiceResult> Put(NoteModel model);
        Task<ServiceResult> Move(NoteModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NoteService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            this._unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public IQueryable<NoteModel> Get()
        {
            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var list = _unitOfWork.Repository<Note>()
                .GetAll(x => !x.Deleted && x.NoteCategory.UserId == userId)
                .OrderByDescending(x => x.UpdateDate)
                .Select(x => new NoteModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    NoteCategoryId = x.NoteCategoryId,
                    Title = x.Title
                }).AsQueryable();

            return list;
        }

        public IQueryable<NoteModel> GetByCategoryId(int categoryId)
        {
            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var list = _unitOfWork.Repository<Note>()
                .GetAll(x => !x.Deleted && x.NoteCategoryId == categoryId && x.NoteCategory.UserId == userId)
                .OrderByDescending(x => x.UpdateDate)
                .Select(x => new NoteModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    NoteCategoryId = x.NoteCategoryId,
                    Title = x.Title
                }).AsQueryable();

            return list;
        }

        public async Task<ServiceResult> Post(NoteModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.UserId();

                var category = _unitOfWork.Repository<NoteCategory>()
                    .Get(x => x.Id == model.NoteCategoryId && x.UserId == userId);

                if (category != null)
                {
                    var note = new Note
                    {
                        Title = model.Title,
                        NoteCategoryId = model.NoteCategoryId,
                        Description = model.Description,
                        UpdateDate = DateTime.Now,
                        InsertDate = DateTime.Now,
                        Deleted = false
                    };
                    await _unitOfWork.Repository<Note>().Add(note);
                    await _unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Put(NoteModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.UserId();

                var note = await _unitOfWork.Repository<Note>()
                    .Get(x => x.Id == model.Id && x.NoteCategory.UserId == userId);

                if (note != null)
                {
                    note.Title = model.Title;
                    note.Description = model.Description;
                    note.NoteCategoryId = model.NoteCategoryId;
                    note.UpdateDate = DateTime.Now;
                    note.Deleted = false;

                    await _unitOfWork.SaveChanges();
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Message = "Not bulunamadı.";
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

                var note = await _unitOfWork.Repository<Note>()
                    .Get(x => x.Id == id && x.NoteCategory.UserId == userId);

                if (note != null)
                {
                    note.Deleted = true;

                    await _unitOfWork.SaveChanges();
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Message = "Not bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Move(NoteModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.UserId();

                var note = await _unitOfWork.Repository<Note>()
                    .Get(x => x.Id == model.Id && x.NoteCategory.UserId == userId);

                if (note != null)
                {
                    note.NoteCategoryId = model.NoteCategoryId;
                    note.UpdateDate = DateTime.Now;

                    await _unitOfWork.SaveChanges();
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Message = "Not bulunamadı.";
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
