namespace Chinook.Storage.Models
{
    public class BlogCategoryFilterModel : FilterModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool? IsActive { get; set; }
    }

    public class BlogCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
    }
}
