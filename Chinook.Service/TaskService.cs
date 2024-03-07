using Chinook.Data;
using Chinook.Service.Exceptions;
using Chinook.Storage.Entities;
using Chinook.Storage.Model;
using Chinook.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface ITaskService
    {
        Task<List<TaskGetModel>> Get();
        IQueryable<TaskGetModel> GetUserTasks(int userId);
        Task<TaskModel> GetById(int id);
        Task<ServiceResult> Post(TaskModel model);
        Task<ServiceResult> Put(TaskModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class TaskService : ITaskService
    {
        private readonly ChinookContext _context;

        public TaskService(ChinookContext context)
        {
            _context = context;
        }

        public async Task<List<TaskGetModel>> Get()
        {
            var list = await _context.Tasks
                .Where(x => !x.Deleted)
                .Include(x => x.TaskCategory)
                .Include(x => x.TaskStatus)
                .Include(x => x.AssignUser)
                .Select(x => new TaskGetModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    InsertedDate = x.InsertedDate,
                    IsActive = x.IsActive,
                    Title = x.Title,
                    TaskCategoryId = x.TaskCategoryId,
                    TaskCategoryName = x.TaskCategory.Name,
                    TaskStatusId = x.TaskStatusId,
                    TaskStatusName = $"{x.TaskStatus.Name} ({x.TaskCategory.Name})",
                    UpdatedDate = x.UpdatedDate,
                    AssignUserId = x.AssignUserId,
                    UserNameSurname = x.AssignUser.Surname + " " + x.AssignUser.Name
                }).ToListAsync();

            return list;
        }

        public IQueryable<TaskGetModel> GetUserTasks(int userId)
        {
            return _context.Tasks
                .Where(x => !x.Deleted && x.IsActive && x.AssignUserId == userId)
                .Include(x => x.TaskCategory)
                .Include(x => x.TaskStatus)
                .Include(x => x.AssignUser)
                .Select(x => new TaskGetModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    InsertedDate = x.InsertedDate,
                    IsActive = x.IsActive,
                    Title = x.Title,
                    TaskCategoryId = x.TaskCategoryId,
                    TaskCategoryName = x.TaskCategory.Name,
                    TaskStatusId = x.TaskStatusId,
                    TaskStatusName = x.TaskStatus.Name,
                    UpdatedDate = x.UpdatedDate,
                    AssignUserId = x.AssignUserId,
                    UserNameSurname = x.AssignUser.Surname + " " + x.AssignUser.Name
                })
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public async Task<TaskModel> GetById(int id)
        {
            var task = await _context.Tasks
                .Where(x => x.Id == id)
                .Select(x => new TaskModel
                {
                    Description = x.Description,
                    IsActive = x.IsActive,
                    Id = x.Id,
                    TaskCategoryId = x.TaskCategoryId,
                    TaskStatusId = x.TaskStatusId,
                    Title = x.Title,
                    AssignUserId = x.AssignUserId
                }).FirstOrDefaultAsync();
            return task;
        }

        public async Task<ServiceResult> Post(TaskModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            if (model.Id == 0)
            {
                var task = new TaskDmo
                {
                    Deleted = false,
                    Description = model.Description,
                    InsertedDate = DateTime.Now,
                    IsActive = model.IsActive,
                    TaskCategoryId = model.TaskCategoryId,
                    Title = model.Title,
                    TaskStatusId = model.TaskStatusId,
                    AssignUserId = model.AssignUserId
                };

                await _context.AddAsync(task);

                await _context.SaveChangesAsync();
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Put(TaskModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (task == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            task.Description = model.Description;
            task.IsActive = model.IsActive;
            task.Title = model.Title;
            task.TaskCategoryId = model.TaskCategoryId;
            task.TaskStatusId = model.TaskStatusId;
            task.UpdatedDate = DateTime.Now;
            task.AssignUserId = model.AssignUserId;

            await _context.SaveChangesAsync();

            return serviceResult;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            task.Deleted = true;

            await _context.SaveChangesAsync();

            return serviceResult;
        }
    }
}
