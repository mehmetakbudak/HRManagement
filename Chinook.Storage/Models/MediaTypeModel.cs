using Chinook.Storage.Entities;

namespace Chinook.Storage.Models
{
    public class BlogCategoryModel : BaseModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
    }
}
