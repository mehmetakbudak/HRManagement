using Chinook.Storage.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chinook.Storage.Models
{
    public class NoteModel : BaseModel
    {
        public int NoteCategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
