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

namespace Chinook.Service
{
    public interface ITaskStatusService
    {
        Task<List<TaskStatusDmo>> GetAll();
        Task<TaskStatusModel> GetById(int id);
        Task<List<TaskStatusDmo>> GetByTaskCategoryId(int categoryId);
        Task<ServiceResult> Post(TaskStatusModel model);
        Task<ServiceResult> Put(TaskStatusModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class TaskStatusService : ITaskStatusService
    {
        private readonly ChinookContext _context;

        public TaskStatusService(ChinookContext context)
        {            
            _context = context;
        }

        public async Task<List<TaskStatusDmo>> GetAll()
        {
            return await _context.TaskStatuses
                .Where(x => !x.Deleted)
                .Include(o => o.TaskCategory)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<TaskStatusModel> GetById(int id)
        {
            var taskStatus = await _context.TaskStatuses.FirstOrDefaultAsync(x => x.Id == id);

            if (taskStatus == null)
            {
                throw new NotFoundException();
            }

            return new TaskStatusModel
            {
                DisplayOrder = taskStatus.DisplayOrder,
                Id = taskStatus.Id,
                IsActive = taskStatus.IsActive,
                Name = taskStatus.Name,
                TaskCategoryId = taskStatus.TaskCategoryId
            };
        }

        public async Task<List<TaskStatusDmo>> GetByTaskCategoryId(int taskCategoryId)
        {
            return await _context.TaskStatuses
                .Where(x => !x.Deleted && x.TaskCategoryId == taskCategoryId)
                .Include(o => o.TaskCategory)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<ServiceResult> Post(TaskStatusModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskStatus = new TaskStatusDmo
            {
                Deleted = false,
                IsActive = model.IsActive,
                TaskCategoryId = model.TaskCategoryId,
                Name = model.Name,
                DisplayOrder = model.DisplayOrder
            };

            await _context.TaskStatuses.AddAsync(taskStatus);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<ServiceResult> Put(TaskStatusModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskStatus = await _context.TaskStatuses.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (taskStatus == null)
            {
                throw new NotFoundException();
            }

            taskStatus.IsActive = model.IsActive;
            taskStatus.Name = model.Name;
            taskStatus.TaskCategoryId = model.TaskCategoryId;
            taskStatus.DisplayOrder = model.DisplayOrder;

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskStatus = await _context.TaskStatuses.FirstOrDefaultAsync(x => x.Id == id);

            if (taskStatus == null)
            {
                throw new NotFoundException();
            }

            taskStatus.Deleted = true;
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
