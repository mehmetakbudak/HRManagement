using Chinook.Storage.Entities;
using System.Collections.Generic;

namespace Chinook.Storage.Models
{
    public class NoteModel
    {
        public int Id { get; set; }
        public int NoteCategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class NoteFilterModel : FilterModel
    {
        public List<int> NoteCategoryIds { get; set; }
        public string? Title { get; set; }
    }
}
