using Chinook.Storage.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Storage.Entities
{
    public class TitleDmo : BaseModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
