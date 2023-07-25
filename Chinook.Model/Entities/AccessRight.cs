namespace Chinook.Model.Entities
{
    public class AccessRight : BaseModel
    {
        public int AccessRightCategoryId { get; set; }
        public AccessRightCategory AccessRightCategory { get; set; }

        public string Definition { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}

