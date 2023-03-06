using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface INoteCategoryService
    {
        IQueryable<NoteCategoryModel> GetAll();
        Task<ServiceResult> Post(NoteCategoryModel model);
        Task<ServiceResult> Put(NoteCategoryModel model);
        Task<ServiceResult> Delete(int id);
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
                var noteCategory = new NoteCategory
                {
                    UserId = AuthTokenContent.Current.UserId,
                    Name = model.Name,
                    InsertDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Deleted = false
                };
                await unitOfWork.Repository<NoteCategory>().Add(noteCategory);
                await unitOfWork.SaveChanges();
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
                var category = await unitOfWork.Repository<NoteCategory>()
                    .Get(x => x.Id == model.Id && x.UserId == AuthTokenContent.Current.UserId);

                if (category != null)
                {
                    category.Name = model.Name;
                    category.UpdateDate = DateTime.Now;
                    await unitOfWork.SaveChanges();
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
                var category = await unitOfWork.Repository<NoteCategory>()
                    .Get(x => x.Id == id && x.UserId == AuthTokenContent.Current.UserId);

                if (category != null)
                {
                    category.Deleted = true;

                    await unitOfWork.SaveChanges();
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
