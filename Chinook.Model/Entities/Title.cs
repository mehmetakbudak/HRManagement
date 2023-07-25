using Chinook.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Model.Entities
{
    public class Title : BaseModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
