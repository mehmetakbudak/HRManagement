using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Storage.Entities
{
    public class Province : BaseModel
    {
        public string Name { get; set; }
    }
}
