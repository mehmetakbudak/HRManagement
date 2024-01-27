using System.Collections.Generic;

namespace Chinook.Storage.Entities
{
    public class TaskCategoryDmo : BaseModel
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<TaskDmo> Tasks { get; set; }
    }
}
