using Chinook.Storage.Models;
using System;

namespace Chinook.Storage.Model
{
    public class TaskModel
    {
        public int Id { get; set; }
        public int TaskCategoryId { get; set; }
        public int TaskStatusId { get; set; }
        public int? AssignUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class TaskGetModel : TaskModel
    {
        public string UserNameSurname { get; set; }
        public string TaskStatusName { get; set; }
        public string TaskCategoryName { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class TaskFilterModel : FilterModel
    {
        public int? TaskCategoryId { get; set; }
        public string Title { get; set; }
        public int? AssignUserId { get; set; }
        public int? TaskStatusId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }

    public class TaskCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class TaskStatusModel
    {
        public int Id { get; set; }
        public int TaskCategoryId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
