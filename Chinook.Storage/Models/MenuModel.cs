using Chinook.Storage.Enums;
using System.Collections.Generic;

namespace Chinook.Storage.Models
{
    public class MenuModel
    {
        public int Id { get; set; }

        public MenuType Type { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeletable { get; set; }

        public List<MenuModel> Items { get; set; }
    }

    public class MenuItemModel
    {
        public int Id { get; set; }

        public int MenuId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool IsActive { get; set; }

        public MenuType MenuType { get; set; }

        public int DisplayOrder { get; set; }

        public int? ParentId { get; set; }

        public List<MenuItemModel> Items { get; set; }

        public List<int> AccessRightIds { get; set; }
    }

    public class MenuItemGetModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public int? ParentId { get; set; }

        public bool Expanded { get; set; }
    }

    public class MenuTreeModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public MenuType Type { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeletable { get; set; }
        public List<MenuItemModel> Children { get; set; }
    }

    public class MenubarModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string RouterLink { get; set; }
        public int? ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<MenubarModel> Items { get; set; }
    }
}