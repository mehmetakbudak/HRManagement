namespace Chinook.Storage.Entities
{
    public class MenuItemAccessRightDmo : BaseModel
    {
        public int MenuItemId { get; set; }

        public MenuItemDmo MenuItem { get; set; }

        public int AccessRightId { get; set; }

        public AccessRightDmo AccessRight { get; set; }

        public bool Required { get; set; }
    }
}
