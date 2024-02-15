using Chinook.Data;
using Chinook.Service.Exceptions;
using Chinook.Storage.Entities;
using Chinook.Storage.Model;
using Chinook.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ITaskCategoryService
    {
        Task<List<TaskCategoryDmo>> GetAll();
        Task<TaskCategoryModel> GetById(int id);
        Task<ServiceResult> Post(TaskCategoryModel model);
        Task<ServiceResult> Put(TaskCategoryModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class TaskCategoryService : ITaskCategoryService
    {
        private readonly ChinookContext _context;

        public TaskCategoryService(ChinookContext context)
        {
            _context = context;
        }

        public async Task<List<TaskCategoryDmo>> GetAll()
        {
            return await _context.TaskCategories
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<TaskCategoryModel> GetById(int id)
        {
            var taskCategory = await _context.TaskCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (taskCategory == null)
            {
                throw new NotFoundException();
            }

            return new TaskCategoryModel
            {
                Id = taskCategory.Id,
                IsActive = taskCategory.IsActive,
                Name = taskCategory.Name
            };
        }

        public async Task<ServiceResult> Post(TaskCategoryModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskCategory = new TaskCategoryDmo
            {
                Deleted = false,
                IsActive = model.IsActive,
                Name = model.Name
            };

            await _context.TaskCategories.AddAsync(taskCategory);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<ServiceResult> Put(TaskCategoryModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskCategory = await _context.TaskCategories.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (taskCategory == null)
            {
                throw new NotFoundException();
            }

            taskCategory.Name = model.Name;
            taskCategory.IsActive = model.IsActive;
            
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskCategory = await _context.TaskCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (taskCategory == null)
            {
                throw new NotFoundException();
            }

            taskCategory.Deleted = true;
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
