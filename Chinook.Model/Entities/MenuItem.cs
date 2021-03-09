using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Model.Entities
{
    [Table("menu_items")]
    public class MenuItem : BaseModel
    {
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
