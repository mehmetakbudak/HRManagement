using Chinook.Model.Entities;
using Chinook.Model.Enums;
using System.Collections.Generic;

namespace Chinook.Model.Models
{
    public class MenuModel : BaseModel
    {
        public string Label { get; set; }
        public MenuType Type { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeletable { get; set; }
        public List<MenuItemModel> Items { get; set; }
    }

    public class MenuItemModel : BaseModel
    {
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public int MenuId { get; set; }
        public int Order { get; set; }
        public List<MenuItemModel> Items { get; set; }
    }    
}
