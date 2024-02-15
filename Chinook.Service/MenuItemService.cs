using Chinook.Data;
using Chinook.Service.Exceptions;
using Chinook.Storage.Entities;
using Chinook.Storage.Enums;
using Chinook.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface IMenuItemService
    {
        Task<MenuItemModel> GetById(int id);
        Task<ServiceResult> Post(MenuItemModel model);
        Task<ServiceResult> Put(MenuItemModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class MenuItemService : IMenuItemService
    {
        private readonly ChinookContext _context;

        public MenuItemService(ChinookContext context)
        {
            _context = context;
        }

        public async Task<MenuItemModel> GetById(int id)
        {
            var menuItem = await _context.MenuItems
                .Where(x => x.Id == id && !x.Deleted)
                .Include(x => x.Menu)
                .Include(x => x.MenuItemAccessRights)
                .FirstOrDefaultAsync();

            if (menuItem == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            var accessRightIds = new List<int>();

            if (menuItem.Menu.Type == MenuType.Admin)
            {
                accessRightIds = menuItem.MenuItemAccessRights
                    .Select(x => x.AccessRightId).ToList();
            }

            var model = new MenuItemModel
            {
                Id = menuItem.Id,
                Title = menuItem.Title,
                Url = menuItem.Url,
                ParentId = menuItem.ParentId,
                IsActive = menuItem.IsActive,
                MenuType = menuItem.Menu.Type,
                DisplayOrder = menuItem.DisplayOrder,
                AccessRightIds = accessRightIds
            };
            return model;
        }

        public async Task<ServiceResult> Post(MenuItemModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            if (!string.IsNullOrEmpty(model.Url))
            {
                var isExist = await _context.MenuItems
                    .AnyAsync(x => !x.Deleted && x.Url == model.Url && x.Menu.Type == model.MenuType);

                if (isExist)
                {
                    throw new FoundException("Daha önce aynı url'den kaydedilmiş.");
                }
            }

            if (model.MenuType == MenuType.Frontend)
            {
                int menuId = 0;
                var menu = await _context.Menus.FirstOrDefaultAsync(x => x.Type == MenuType.Frontend);

                if (menu == null)
                {
                    var newMenu = new MenuDmo
                    {
                        Deleted = false,
                        IsActive = true,
                        IsDeletable = false,
                        Name = "Ön Arayüz Menü",
                        Type = MenuType.Frontend,
                    };
                    await _context.Menus.AddAsync(newMenu);

                    await _context.SaveChangesAsync();

                    menuId = newMenu.Id;
                }
                else
                {
                    menuId = menu.Id;
                }

                await _context.MenuItems.AddAsync(new MenuItemDmo
                {
                    Deleted = false,
                    DisplayOrder = model.DisplayOrder,
                    IsActive = model.IsActive,
                    ParentId = model.ParentId,
                    Title = model.Title,
                    MenuId = menuId,
                    Url = string.IsNullOrEmpty(model.Url) ? null : model.Url

                });
                await _context.SaveChangesAsync();
            }
            else if (model.MenuType == MenuType.Admin)
            {
                int menuId = 0;
                var menu = await _context.Menus.FirstOrDefaultAsync(x => x.Type == MenuType.Admin);

                if (menu == null)
                {
                    var newMenu = new MenuDmo
                    {
                        Deleted = false,
                        IsActive = true,
                        IsDeletable = false,
                        Name = "Admin Menü",
                        Type = MenuType.Admin
                    };
                    await _context.Menus.AddAsync(newMenu);

                    await _context.SaveChangesAsync();

                    menuId = newMenu.Id;
                }
                else
                {
                    menuId = menu.Id;
                }

                var menuItem = new MenuItemDmo
                {
                    Deleted = false,
                    DisplayOrder = model.DisplayOrder,
                    IsActive = model.IsActive,
                    ParentId = model.ParentId,
                    Title = model.Title,
                    MenuId = menuId,
                    Url = model.Url
                };
                await _context.MenuItems.AddAsync(menuItem);

                await _context.SaveChangesAsync();

                if (model.AccessRightIds != null && model.AccessRightIds.Count > 0)
                {
                    foreach (var accessRightId in model.AccessRightIds)
                    {
                        await _context.MenuItemAccessRights.AddAsync(new MenuItemAccessRightDmo
                        {
                            AccessRightId = accessRightId,
                            MenuItemId = menuItem.Id
                        });
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<ServiceResult> Put(MenuItemModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            if (!string.IsNullOrEmpty(model.Url))
            {
                var isExist = await _context.MenuItems.AnyAsync(x => x.Id != model.Id && !x.Deleted && x.Url == model.Url && x.Menu.Type == model.MenuType);

                if (isExist)
                {
                    throw new FoundException("Daha önce aynı url'den kaydedilmiş.");
                }
            }

            var menuItem = await _context.MenuItems
                .Where(x => x.Id == model.Id && !x.Deleted)
                .Include(x => x.MenuItemAccessRights)
                .FirstOrDefaultAsync();

            if (menuItem == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            menuItem.Title = model.Title;
            menuItem.Url = string.IsNullOrEmpty(model.Url) ? null : model.Url;
            menuItem.ParentId = model.ParentId;
            menuItem.IsActive = model.IsActive;
            menuItem.DisplayOrder = model.DisplayOrder;

            var addingList = model.AccessRightIds.Where(x => !menuItem.MenuItemAccessRights.Select(x => x.AccessRightId).Contains(x)).ToList();

            if (addingList != null && addingList != null)
            {
                foreach (var accessRightId in addingList)
                {
                    await _context.MenuItemAccessRights.AddAsync(new MenuItemAccessRightDmo
                    {
                        AccessRightId = accessRightId,
                        MenuItemId = menuItem.Id
                    });
                }
            }

            var deletingList = menuItem.MenuItemAccessRights.Where(x => !model.AccessRightIds.Contains(x.AccessRightId)).ToList();

            if (deletingList != null && deletingList.Any())
            {
                _context.MenuItemAccessRights.RemoveRange(deletingList);
            }

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var menuItem = await _context.MenuItems.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);

            if (menuItem == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            menuItem.Deleted = true;

            await _context.SaveChangesAsync();

            return result;
        }
    }
}
