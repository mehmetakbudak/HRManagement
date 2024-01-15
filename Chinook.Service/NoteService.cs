using Chinook.Data.Repository;
using Chinook.Service.Exceptions;
using Chinook.Service.Extensions;
using Chinook.Service.Helpers;
using Chinook.Storage.Entities;
using Chinook.Storage.Models;
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
        PaginationModel<Note> GetByFilter(NoteFilterModel model);
        Task<NoteModel> GetById(int id);
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
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public PaginationModel<Note> GetByFilter(NoteFilterModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var query = _unitOfWork.Repository<Note>()
                .GetAll(x => !x.Deleted && x.NoteCategory.UserId == userId)
                .Include(x => x.NoteCategory)
                .OrderByDescending(x => x.UpdateDate)
                .AsQueryable();

            if (model.NoteCategoryIds != null && model.NoteCategoryIds.Count > 0)
            {
                query = query.Where(x => model.NoteCategoryIds.Contains(x.NoteCategoryId));
            }
            if (!string.IsNullOrEmpty(model.Title))
            {
                query = query.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }

            var list = PaginationHelper<Note>.Paginate(query, model);

            return list;
        }

        public async Task<NoteModel> GetById(int id)
        {
            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var note = await _unitOfWork.Repository<Note>()
                .Get(x => !x.Deleted && x.Id == id && x.NoteCategory.UserId == userId);

            if (note == null)
            {
                throw new NotFoundException("Note not found.");
            }

            return new NoteModel
            {
                Id = note.Id,
                Title = note.Title,
                Description = note.Description,
                NoteCategoryId = note.NoteCategoryId
            };
        }

        public async Task<ServiceResult> Post(NoteModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var category = _unitOfWork.Repository<NoteCategory>()
                .Get(x => x.Id == model.NoteCategoryId && x.UserId == userId);

            if (category == null)
            {
                throw new NotFoundException("Note category not found");
            }

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

            return serviceResult;
        }

        public async Task<ServiceResult> Put(NoteModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var note = await _unitOfWork.Repository<Note>()
                .Get(x => x.Id == model.Id && x.NoteCategory.UserId == userId);

            if (note == null)
            {
                throw new NotFoundException("Note not found.");
            }
            note.Title = model.Title;
            note.Description = model.Description;
            note.NoteCategoryId = model.NoteCategoryId;
            note.UpdateDate = DateTime.Now;
            note.Deleted = false;

            await _unitOfWork.SaveChanges();

            return serviceResult;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var note = await _unitOfWork.Repository<Note>()
                .Get(x => x.Id == id && x.NoteCategory.UserId == userId);

            if (note == null)
            {
                throw new NotFoundException("Note not found.");
            }
            note.Deleted = true;

            await _unitOfWork.SaveChanges();
            return serviceResult;
        }

        public async Task<ServiceResult> Move(NoteModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var note = await _unitOfWork.Repository<Note>()
                .Get(x => x.Id == model.Id && x.NoteCategory.UserId == userId);

            if (note == null)
            {
                throw new NotFoundException("Note not found.");
            }
            note.NoteCategoryId = model.NoteCategoryId;
            note.UpdateDate = DateTime.Now;

            await _unitOfWork.SaveChanges();

            return serviceResult;
        }
    }
}
