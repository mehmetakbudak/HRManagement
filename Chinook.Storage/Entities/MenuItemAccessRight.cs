namespace Chinook.Storage.Entities
{
    public class MenuItemAccessRight : BaseModel
    {
        public int MenuItemId { get; set; }

        public MenuItem MenuItem { get; set; }

        public int AccessRightId { get; set; }

        public AccessRight AccessRight { get; set; }

        public bool Required { get; set; }
    }
}
