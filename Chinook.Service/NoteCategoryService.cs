using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
using System;
using System.Linq;
using System.Net;

namespace Chinook.Service
{
    public interface INoteCategoryService
    {
        IQueryable<NoteCategoryModel> GetAll();
        ServiceResult Post(NoteCategoryModel model);
        ServiceResult Put(NoteCategoryModel model);
        ServiceResult Delete(int id);
    }

    public class NoteCategoryService : INoteCategoryService
    {
        private readonly IUnitOfWork unitOfWork;

        public NoteCategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IQueryable<NoteCategoryModel> GetAll()
        {
            var list = unitOfWork.Repository<NoteCategory>()
                .GetAll(x => !x.Deleted && x.UserId == AuthTokenContent.Current.UserId)
                .AsEnumerable()
                .OrderByDescending(x => x.UpdateDate)
                .Select(x => new NoteCategoryModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .AsQueryable();
            return list;
        }

        public ServiceResult Post(NoteCategoryModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var noteCategory = new NoteCategory
                {
                    UserId = AuthTokenContent.Current.UserId,
                    Name = model.Name,
                    InsertDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Deleted = false
                };
                unitOfWork.Repository<NoteCategory>().Add(noteCategory);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public ServiceResult Put(NoteCategoryModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var category = unitOfWork.Repository<NoteCategory>()
                    .Get(x => x.Id == model.Id && x.UserId == AuthTokenContent.Current.UserId);

                if (category != null)
                {
                    category.Name = model.Name;
                    category.UpdateDate = DateTime.Now;
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
                var category = unitOfWork.Repository<NoteCategory>().Get(x => x.Id == id && x.UserId == AuthTokenContent.Current.UserId);

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
