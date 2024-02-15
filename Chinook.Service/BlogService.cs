using Chinook.Data;
using Chinook.Service.Exceptions;
using Chinook.Service.Extensions;
using Chinook.Service.Helpers;
using Chinook.Storage.Entities;
using Chinook.Storage.Enums;
using Chinook.Storage.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface IBlogService
    {
        IQueryable<BlogDmo> Get();
        PaginationModel<BlogDmo> GetByFilter(BlogFilterModel model);
        Task<List<BlogModel>> GetBlogsByCategoryUrl(string categoryUrl);
        Task<BlogDetailModel> GetById(int id);
        Task<BlogDetailOutputModel> GetDetailById(int id);
        Task<List<MostReadBlogModel>> MostRead(string blogCategoryUrl = null);
        Task<ServiceResult> Post(BlogInputModel model);
        Task<ServiceResult> Put(BlogInputModel model);
        Task<ServiceResult> Seen(int id);   
        Task<ServiceResult> Delete(int id);
        Task<List<BlogModel>> GetTagBlogsByUrl(string url);
    }
    public class BlogService : IBlogService
    {
        private readonly ChinookContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;

        public BlogService(
            ChinookContext context,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public IQueryable<BlogDmo> Get()
        {
            return _context.Blogs.Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public PaginationModel<BlogDmo> GetByFilter(BlogFilterModel model)
        {
            var query = Get();
            if (!string.IsNullOrEmpty(model.Title))
            {
                query = query.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }
            if (!string.IsNullOrEmpty(model.Url))
            {
                query = query.Where(x => x.Url.ToLower().Contains(model.Url.ToLower()));
            }
            if (model.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == model.IsActive.Value);
            }
            var list = PaginationHelper<BlogDmo>.Paginate(query, model);
            return list;
        }

        public async Task<List<BlogModel>> GetBlogsByCategoryUrl(string blogCategoryUrl)
        {
            var blogCategory = await _context.BlogCategories
                .Where(x => x.Url == blogCategoryUrl && !x.Deleted).FirstOrDefaultAsync();

            if (blogCategory is null)
            {
                throw new NotFoundException("Blog category not found.");
            }

            var comments = _context.Comments
                .Where(x => !x.Deleted && x.Status == CommentStatus.Approved && x.SourceType == SourceType.Blog).AsQueryable();

            var blogs = await _context.SelectedBlogCategories
                 .Where(x => !x.Blog.Deleted && x.Blog.Published && x.Blog.IsActive && x.BlogCategoryId == blogCategory.Id)
                 .Include(x => x.Blog)
                 .Include(x => x.BlogCategory)
                 .Select(x => x.Blog)
                 .OrderBy(x => x.DisplayOrder)
                 .Select(x => new BlogModel
                 {
                     Id = x.Id,
                     Url = x.Url,
                     Title = x.Title,
                     Content = x.Content,
                     ImageUrl = x.ImageUrl,
                     Description = x.Description,
                     InsertedDate = x.InsertedDate,
                     FullName = $"{x.User.FirstName} {x.User.LastName}",
                     CommentCount = comments.Count(a => a.SourceId == x.Id)
                 }).ToListAsync();

            return blogs;
        }

        public async Task<BlogDetailModel> GetById(int id)
        {
            BlogDetailModel model = null;
            var blog = await _context.Blogs
                .Where(x => !x.Deleted && x.Id == id)
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.BlogCategory)
                .FirstOrDefaultAsync();

            if (blog is null)
            {
                throw new NotFoundException("Blog not found.");
            }

            var blogTags = await _context.SourceTags
                .Where(x => x.SourceId == blog.Id && x.SourceType == SourceType.Blog)
                .Include(x => x.Tag)
                .ToListAsync();

            model = new BlogDetailModel
            {
                Content = blog.Content,
                Id = blog.Id,
                Description = blog.Description,
                Title = blog.Title,
                Url = blog.Url,
                IsActive = blog.IsActive,
                Published = blog.Published,
                DisplayOrder = blog.DisplayOrder,
                ImageUrl = blog.ImageUrl,
                BlogCategories = blog.SelectedBlogCategories.Select(x => x.BlogCategory.Id).ToList(),
                SelectedTags = blogTags.Select(x => x.Tag.Name).ToList()
            };
            return model;
        }

        public async Task<BlogDetailOutputModel> GetDetailById(int id)
        {
            BlogDetailOutputModel model = null;

            var blog = await _context.Blogs
                .Where(x => x.Id == id && !x.Deleted && x.Published && x.IsActive)
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.BlogCategory)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            if (blog == null)
            {
                throw new NotFoundException("Blog not found.");
            }

            var commentCount = await _context.Comments
                .Where(x => x.SourceType == SourceType.Blog && x.SourceId == id && !x.Deleted && x.Status == CommentStatus.Approved).CountAsync();

            var blogTags = await _context.SourceTags
                .Where(x => x.SourceType == SourceType.Blog && x.SourceId == id)
                .Include(x => x.Tag)
                .Select(x => x.Tag).ToListAsync();

            model = new BlogDetailOutputModel
            {
                Id = blog.Id,
                Url = blog.Url,
                Content = blog.Content,
                NumberOfView = blog.NumberOfView,
                InsertedDate = blog.InsertedDate,
                Title = blog.Title,
                ImageUrl = blog.ImageUrl,
                FullName = $"{blog.User.FirstName} {blog.User.LastName}",
                CommentCount = commentCount,
                BlogCategories = blog.SelectedBlogCategories
                                     .Select(x => x.BlogCategory)
                                     .Select(x => new BlogDetailCategoryModel
                                     {
                                         Id = x.Id,
                                         Name = x.Name,
                                         Url = x.Url
                                     }).ToList(),
                BlogTags = blogTags.Select(x => new BlogDetailTagModel
                {
                    Name = x.Name,
                    Url = x.Url
                }).ToList()
            };
            return model;
        }

        public async Task<List<MostReadBlogModel>> MostRead(string blogCategoryUrl = null)
        {
            IQueryable<BlogDmo> data = null;

            if (!string.IsNullOrEmpty(blogCategoryUrl))
            {
                data = _context.SelectedBlogCategories
                    .Where(x => !x.Blog.Deleted && x.Blog.IsActive && x.Blog.Published && x.BlogCategory.Url == blogCategoryUrl)
                    .Include(x => x.Blog)
                    .Include(x => x.BlogCategory)
                    .Select(x => x.Blog).AsQueryable();
            }
            else
            {
                data = _context.Blogs.Where(x => !x.Deleted && x.IsActive && x.Published);
            }

            var list = await data.OrderByDescending(x => x.NumberOfView).Take(5)
                .Select(x => new MostReadBlogModel()
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Title = x.Title,
                    Url = x.Url,
                    InsertedDate = x.InsertedDate
                }).ToListAsync();

            return list;
        }

        public async Task<ServiceResult> Post(BlogInputModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (model == null || model.Id != 0)
                {
                    throw new NotFoundException("Model null olamaz.");
                }
                if (model.Image == null)
                {
                    throw new BadRequestException("Resim ekleyiniz.");
                }

                #region Add File
                var extension = Path.GetExtension(model.Image.FileName);
                string imageUrl = $"/images/blogs/{Guid.NewGuid()}{extension}";
                var fileUploadUrl = $"{_environment.WebRootPath}{imageUrl}";
                model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));
                #endregion

                var userId = _httpContextAccessor.HttpContext.User.UserId();

                #region Add Blog 
                var blog = new BlogDmo
                {
                    Content = model.Content,
                    Deleted = false,
                    Description = model.Content,
                    DisplayOrder = model.DisplayOrder,
                    InsertedDate = DateTime.Now,
                    IsActive = model.IsActive,
                    Published = model.Published,
                    NumberOfView = 0,
                    Title = model.Title,
                    UserId = userId,
                    ImageUrl = imageUrl,
                    Url = UrlHelper.FriendlyUrl(model.Title)
                };

                await _context.Blogs.AddAsync(blog);
                await _context.SaveChangesAsync();
                #endregion

                #region Add SelectedBlogCategory
                foreach (var blogCategoryId in model.BlogCategories)
                {
                    await _context.SelectedBlogCategories.AddAsync(new SelectedBlogCategoryDmo
                    {
                        BlogCategoryId = blogCategoryId,
                        BlogId = blog.Id
                    });
                }
                #endregion

                await AddNewTags(model.SelectedTags);

                #region Add SourceTag
                var tags = await _context.Tags
                    .Where(x => model.SelectedTags.Contains(x.Name)).ToListAsync();

                foreach (var tag in tags)
                {
                    await _context.SourceTags.AddAsync(new SourceTagDmo
                    {
                        SourceId = blog.Id,
                        TagId = tag.Id,
                        SourceType = SourceType.Blog
                    });
                }
                #endregion

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
            finally
            {
                await transaction.CommitAsync();
            }
            return result;
        }

        private async Task AddNewTags(List<string> tags)
        {
            var tagNames = await _context.Tags.Select(x => x.Name).ToListAsync();

            var addingTagList = tags.Where(x => !string.IsNullOrEmpty(x) && !tagNames.Contains(x)).ToList();

            if (addingTagList != null && addingTagList.Count > 0)
            {
                foreach (var tagName in addingTagList)
                {
                    await _context.Tags.AddAsync(new TagDmo
                    {
                        Name = tagName,
                        Url = UrlHelper.FriendlyUrl(tagName)
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ServiceResult> Put(BlogInputModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == model.Id && !x.Deleted);

                if (blog == null)
                {
                    throw new NotFoundException("Kayıt bulunamadı.");
                }

                #region Update File
                if (model.Image != null)
                {
                    var currentFileUrl = Path.Combine(_environment.WebRootPath, blog.ImageUrl);

                    if (File.Exists(currentFileUrl))
                    {
                        File.Delete(currentFileUrl);
                    }
                    var extension = Path.GetExtension(model.Image.FileName);
                    string imageUrl = $"/images/blogs/{Guid.NewGuid()}{extension}";
                    var fileUploadUrl = $"{_environment.WebRootPath}{imageUrl}";
                    model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

                    blog.ImageUrl = imageUrl;
                }
                #endregion

                #region Update Blog
                blog.Content = model.Content;
                blog.Published = model.Published;
                blog.IsActive = model.IsActive;
                blog.Description = model.Description;
                blog.UpdatedDate = DateTime.Now;
                blog.Title = model.Title;
                blog.DisplayOrder = model.DisplayOrder;
                blog.Url = UrlHelper.FriendlyUrl(model.Title);
                #endregion

                #region Update SelectedBlogCategory
                var selectedBlogCategories = await _context.SelectedBlogCategories
                    .Where(x => x.BlogId == model.Id).ToListAsync();

                var addingBlogCategoryList = model.BlogCategories
                    .Where(x => !selectedBlogCategories.Select(x => x.BlogCategoryId).Contains(x)).ToList();

                if (addingBlogCategoryList != null && addingBlogCategoryList != null)
                {
                    foreach (var blogCategoryId in addingBlogCategoryList)
                    {
                        await _context.SelectedBlogCategories.AddAsync(new SelectedBlogCategoryDmo()
                        {
                            BlogCategoryId = blogCategoryId,
                            BlogId = blog.Id
                        });
                    }
                }

                var deletingBlogCategoryList = selectedBlogCategories
                    .Where(x => !model.BlogCategories.Contains(x.BlogCategoryId)).ToList();

                if (deletingBlogCategoryList != null && deletingBlogCategoryList.Any())
                {
                    _context.SelectedBlogCategories.RemoveRange(deletingBlogCategoryList);
                }
                #endregion

                await AddNewTags(model.SelectedTags);

                #region Update SourceTags
                var sourceTags = await _context.SourceTags
                    .Where(x => x.SourceId == model.Id && x.SourceType == SourceType.Blog)
                    .Include(x => x.Tag)
                    .ToListAsync();

                var addingTagNameList = model.SelectedTags
                    .Where(x => !sourceTags.Select(x => x.Tag.Name).Contains(x)).ToList();

                var addingTagList = await _context.Tags
                    .Where(x => addingTagNameList.Contains(x.Name)).ToListAsync();

                if (addingTagList != null && addingTagList != null)
                {
                    foreach (var tag in addingTagList)
                    {
                        await _context.SourceTags.AddAsync(new SourceTagDmo
                        {
                            SourceId = blog.Id,
                            TagId = tag.Id,
                            SourceType = SourceType.Blog
                        });
                    }
                }

                var deletingSourceTagList = sourceTags.Where(x => !model.SelectedTags.Contains(x.Tag.Name)).ToList();

                if (deletingSourceTagList != null && deletingSourceTagList.Any())
                {
                    _context.SourceTags.RemoveRange(deletingSourceTagList);
                }
                #endregion

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
            finally
            {
                await transaction.CommitAsync();
            }
            return result;
        }

        public async Task<ServiceResult> Seen(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(x => !x.Deleted && x.Published && x.IsActive && x.Id == id);

            if (blog is null)
            {
                throw new NotFoundException("Blog not found.");
            }

            blog.NumberOfView++;
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);

            if (blog is null)
            {
                throw new NotFoundException("Blog not found.");
            }

            blog.Deleted = true;

            await _context.SaveChangesAsync();

            var currentFileUrl = Path.Combine(_environment.WebRootPath, blog.ImageUrl);

            if (File.Exists(currentFileUrl))
            {
                File.Delete(currentFileUrl);
            }

            return result;
        }

        public async Task<List<BlogModel>> GetTagBlogsByUrl(string url)
        {
            var sourceTags = await _context.SourceTags
                .Where(x => x.Tag.Url == url && x.SourceType == SourceType.Blog)
                .Include(x => x.Tag)
                .ToListAsync();

            var comments = _context.Comments
                .Where(x => !x.Deleted && x.Status == CommentStatus.Approved && x.SourceType == SourceType.Blog).AsQueryable();

            var blogs = await _context.Blogs
                .Where(x => sourceTags.Select(a => a.SourceId).Contains(x.Id) && x.Published && !x.Deleted && x.IsActive)
                .OrderBy(x => x.DisplayOrder)
              .Select(x => new BlogModel
              {
                  Id = x.Id,
                  Url = x.Url,
                  Title = x.Title,
                  Content = x.Content,
                  ImageUrl = x.ImageUrl,
                  Description = x.Description,
                  InsertedDate = x.InsertedDate,
                  FullName = $"{x.User.FirstName} {x.User.LastName}",
                  CommentCount = comments.Count(a => a.SourceId == x.Id)
              }).ToListAsync();

            return blogs;
        }
    }
}
