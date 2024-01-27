using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Storage.Entities
{
    public class ProvinceDmo : BaseModel
    {
        public string Name { get; set; }
    }
}
