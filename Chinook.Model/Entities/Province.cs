using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Model.Entities
{
    [Table("provinces")]
    public class Province : BaseModel
    {
        public string Name { get; set; }
    }
}
