namespace Chinook.Storage.Entities
{
    public class TaskDmo : BaseInfoModel
    {
        public int TaskCategoryId { get; set; }

        public TaskCategoryDmo TaskCategory { get; set; }

        public int TaskStatusId { get; set; }

        public TaskStatusDmo TaskStatus { get; set; }

        public int? AssignUserId { get; set; }

        public UserDmo AssignUser { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
       
        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}