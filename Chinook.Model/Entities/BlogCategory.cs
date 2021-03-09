using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Model.Entities
{
    [Table("blog_categories")]
    public class BlogCategory : BaseModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public bool IsActive { get; set; }
        public bool Deleted { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}
