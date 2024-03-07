using Chinook.Storage.Enums;

namespace Chinook.Storage.Entities
{
    public class SourceTagDmo : BaseModel
    {
        public int TagId { get; set; }

        public TagDmo Tag { get; set; }

        public SourceType SourceType { get; set; }

        public int SourceId { get; set; }
    }
}
