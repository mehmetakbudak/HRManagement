using System.Collections.Generic;

namespace Chinook.Storage.Entities
{
    public class TaskStatusDmo : BaseModel
    {
        public int TaskCategoryId { get; set; }

        public TaskCategoryDmo TaskCategory { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<TaskDmo> Tasks { get; set; }
    }
}
