namespace Chinook.Storage.Entities
{
    public class SelectedBlogCategoryDmo : BaseModel
    {
        public int BlogId { get; set; }

        public BlogDmo Blog { get; set; }

        public int BlogCategoryId { get; set; }

        public BlogCategoryDmo BlogCategory { get; set; }
    }
}
