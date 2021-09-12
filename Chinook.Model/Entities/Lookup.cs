using Chinook.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Model.Entities
{
    public class Lookup : BaseModel
    {
        public string Name { get; set; }
        public LookupType Type { get; set; }
        public bool Deleted { get; set; }
    }
}
