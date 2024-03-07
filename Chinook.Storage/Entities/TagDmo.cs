using System.Collections.Generic;

namespace Chinook.Storage.Entities
{
    public class TagDmo : BaseInfoModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public List<SourceTagDmo> SourceTags { get; set; }
    }
}
