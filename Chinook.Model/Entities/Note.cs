using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Model.Entities
{
    [Table("notes")]
    public class Note : BaseInfoModel
    {
        public int NoteCategoryId { get; set; }

        public NoteCategory NoteCategory { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Deleted { get; set; }
    }
}
