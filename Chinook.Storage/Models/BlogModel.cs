using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Chinook.Storage.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public DateTime InsertedDate { get; set; }
        public int CommentCount { get; set; }
    }

    public class MostReadBlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public DateTime InsertedDate { get; set; }
    }

    public class BlogDetailModel
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public int NumberOfView { get; set; }
        public int CommentCount { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public DateTime InsertedDate { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public bool Published { get; set; }
        public List<int> BlogCategories { get; set; }
        public List<string> SelectedTags { get; set; }
    }

    public class BlogDetailOutputModel
    {
        public int Id { get; set; }
        public int NumberOfView { get; set; }
        public int CommentCount { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime InsertedDate { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string Url { get; set; }
        public List<BlogDetailCategoryModel> BlogCategories { get; set; }
        public List<BlogDetailTagModel> BlogTags { get; set; }
    }

    public class BlogDetailCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class BlogInputModel : BlogDetailModel
    {
        public IFormFile Image { get; set; }
    }

    public class BlogDetailTagModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class BlogTagCountModel
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class BlogFilterModel : FilterModel
    {
        public int? BlogCategoryId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public bool? IsActive { get; set; }
    }
}