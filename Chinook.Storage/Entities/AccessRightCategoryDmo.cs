namespace Chinook.Storage.Entities
{
    public class AccessRightCategoryDmo : BaseModel
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
