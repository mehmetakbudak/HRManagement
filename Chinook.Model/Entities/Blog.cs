using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Model.Entities
{
    [Table("blogs")]
    public class Blog : BaseInfoModel
    {
        [ForeignKey("BlogCategory")]
        public int BlogCategoryId { get; set; }

        public BlogCategory BlogCategory{ get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(200)]
        public string ShortDefinition { get; set; }

        public string Description { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }

        public int InsertedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public int ReadCount { get; set; }

        public int Sequence { get; set; }

        public bool Published { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}
