using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Storage.Entities
{
    public class BlogDmo : BaseInfoModel
    {
        public int UserId { get; set; }

        public UserDmo User { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public int NumberOfView { get; set; }

        public bool Published { get; set; }

        public int DisplayOrder { get; set; }
      
        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public List<SelectedBlogCategoryDmo> SelectedBlogCategories { get; set; }
    }
}