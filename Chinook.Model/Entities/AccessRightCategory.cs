namespace Chinook.Model.Entities
{
    public class AccessRightCategory : BaseModel
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}

