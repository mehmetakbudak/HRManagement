using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;

namespace Chinook.Service
{
    public interface INoteService
    {
        IQueryable<NoteModel> GetAll();
        ServiceResult Post(NoteModel model);
        ServiceResult Put(NoteModel model);
        ServiceResult Move(NoteModel model);
        ServiceResult Delete(int id);
    }

    public class NoteService : INoteService
    {
        private readonly IUnitOfWork unitOfWork;

        public NoteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<NoteModel> GetAll()
        {
            var list = unitOfWork.Repository<Note>()
                .GetAll(x => !x.Deleted && x.NoteCategory.UserId == AuthTokenContent.Current.UserId,
                x => x.Include(b => b.NoteCategory))
                .OrderByDescending(x => x.UpdateDate)
                .AsEnumerable()
                .Select(x => new NoteModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    NoteCategoryId = x.NoteCategoryId,
                    Title = x.Title
                }).AsQueryable();

            return list;
        }

        public ServiceResult Post(NoteModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var category = unitOfWork.Repository<NoteCategory>().Get(x => x.Id == model.NoteCategoryId && x.UserId == AuthTokenContent.Current.UserId);

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
                    unitOfWork.Repository<Note>().Add(note);
                    unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public ServiceResult Put(NoteModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var note = unitOfWork.Repository<Note>().Get(x => x.Id == model.Id && x.NoteCategory.UserId == AuthTokenContent.Current.UserId,
                  x => x.Include(a => a.NoteCategory));

                if (note != null)
                {
                    note.Title = model.Title;
                    note.Description = model.Description;
                    note.NoteCategoryId = model.NoteCategoryId;
                    note.UpdateDate = DateTime.Now;
                    note.Deleted = false;
                    unitOfWork.Save();
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

        public ServiceResult Delete(int id)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var note = unitOfWork.Repository<Note>().Get(x => x.Id == id && x.NoteCategory.UserId == AuthTokenContent.Current.UserId,
                    x => x.Include(a => a.NoteCategory));

                if (note != null)
                {
                    note.Deleted = true;
                    unitOfWork.Save();
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

        public ServiceResult Move(NoteModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var note = unitOfWork.Repository<Note>().Get(x => x.Id == model.Id && x.NoteCategory.UserId == AuthTokenContent.Current.UserId,
                  x => x.Include(a => a.NoteCategory));

                if (note != null)
                {
                    note.NoteCategoryId = model.NoteCategoryId;
                    note.UpdateDate = DateTime.Now;
                    unitOfWork.Save();
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
