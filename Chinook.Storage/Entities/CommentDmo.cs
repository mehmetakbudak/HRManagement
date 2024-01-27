using Chinook.Storage.Enums;

namespace Chinook.Storage.Entities
{
    public class CommentDmo : BaseInfoModel
    {
        public int? ParentId { get; set; }

        public SourceType SourceType { get; set; }

        public int SourceId { get; set; }

        public int UserId { get; set; }

        public virtual UserDmo User { get; set; }

        public CommentStatus Status { get; set; }

        public string Description { get; set; }       

        public bool Deleted { get; set; }
    }
}
