using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Model.Entities
{
    public class NoteCategory : BaseInfoModel
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public string Name { get; set; }

        public bool Deleted { get; set; }

        public virtual IList<Note> Notes { get; set; }
    }
}
