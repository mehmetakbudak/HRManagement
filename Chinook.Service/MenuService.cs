using Chinook.Data.Repository;
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
    public interface IMenuService
    {
        IQueryable<MenuModel> Get();
        Task<MenuModel> GetById(int id);
        Task<List<MenubarModel>> GetFrontendMenu(int? parentId = null, List<MenubarModel> children = null);
        Task<MenuTreeModel> GetByType(MenuType type);
        Task<List<LookupModel>> GetByLookup();
        Task<ServiceResult> Post(MenuModel model);
        Task<ServiceResult> Put(MenuModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<MenuModel> Get()
        {
            var list = _unitOfWork.Repository<Menu>()
                .GetAll(x => !x.Deleted)
                .Select(x => new MenuModel
                {
                    Id = x.Id,
                    Label = x.Name,
                    Type = x.Type,
                    IsActive = x.IsActive,
                    IsDeletable = x.IsDeletable,
                }).OrderByDescending(x => x.Id).AsQueryable();

            return list;
        }

        public async Task<MenuModel> GetById(int id)
        {
            var menu = await _unitOfWork.Repository<Menu>()
                  .GetAll(x => !x.Deleted && x.Id == id)
                  .Include(a => a.MenuItems)
                  .Select(x => new MenuModel
                  {
                      Id = x.Id,
                      Label = x.Name,
                      Type = x.Type,
                      IsActive = x.IsActive,
                      IsDeletable = x.IsDeletable,
                  }).FirstOrDefaultAsync();
            return menu;
        }

        public async Task<MenuTreeModel> GetByType(MenuType type)
        {
            var model = new MenuTreeModel();

            var menu = await _unitOfWork.Repository<Menu>()
                  .GetAll(x => !x.Deleted && x.Type == type)
                  .Include(a => a.MenuItems)
                  .FirstOrDefaultAsync();

            if (menu != null)
            {
                model.Id = menu.Id;
                model.IsActive = menu.IsActive;
                model.IsDeletable = menu.IsDeletable;
                model.Label = menu.Name;
                model.Type = menu.Type;

                if (menu.MenuItems.Any())
                {
                    model.Children = GetSubMenu(menu.MenuItems, menu.Id, null);
                }
                else
                {
                    model.Children = null;
                }
            }
            return model;
        }

        public async Task<List<MenubarModel>> GetFrontendMenu(int? parentId = null, List<MenubarModel> children = null)
        {
            var menuItems = new List<MenubarModel>();

            var list = await _unitOfWork.Repository<MenuItem>()
                .GetAll(x => x.Menu.Type == MenuType.Frontend && !x.Deleted && x.IsActive && x.ParentId == parentId)
                .Include(x => x.Menu)
                .OrderBy(x => x.Order)
                .Select(x => new MenubarModel
                {
                    Id = x.Id,
                    Label = x.Name,
                    RouterLink = x.Url,
                    DisplayOrder = x.Order,
                    ParentId = x.ParentId,
                    IsActive = x.IsActive
                }).ToListAsync();

            menuItems.AddRange(list);

            foreach (var menuItem in list)
            {
                var items = await GetFrontendMenu(menuItem.Id, list);
                if (items != null && items.Count > 0)
                {
                    menuItem.Items = items;
                }
            }
            return menuItems;
        }

        private List<MenuItemModel> GetSubMenu(IList<MenuItem> menuItems, int menuId, int? parentId)
        {
            var list = new List<MenuItemModel>();

            var data = menuItems.Where(x => x.MenuId == menuId && x.ParentId == parentId && !x.Deleted)
                .OrderBy(x => x.Order).ToList();

            foreach (var item in data)
            {
                var items = GetSubMenu(menuItems, item.MenuId, item.Id);
                var model = new MenuItemModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsActive = item.IsActive,
                    MenuId = item.MenuId,
                    ParentId = item.ParentId,
                    Order = item.Order,
                    Url = item.Url
                };

                if (items.Any())
                {
                    model.Children = items;
                }

                list.Add(model);
            }
            return list;
        }

        public async Task<List<LookupModel>> GetByLookup()
        {
            return await _unitOfWork.Repository<Menu>().GetAll(x => x.IsActive && !x.Deleted)
                .Select(a => new LookupModel
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToListAsync();
        }


        public async Task<ServiceResult> Post(MenuModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var entity = new Menu
                {
                    Deleted = false,
                    IsActive = model.IsActive,
                    IsDeletable = true,
                    Name = model.Label,
                    Type = MenuType.Other
                };
                await _unitOfWork.Repository<Menu>().Add(entity);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Put(MenuModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var menu = await _unitOfWork.Repository<Menu>().Get(x => x.Id == model.Id);
                if (menu == null)
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Message = "Menü bulunamadı.";
                    return serviceResult;
                }
                if (menu.Type == MenuType.Other)
                {
                    menu.Name = model.Label;
                    menu.IsActive = model.IsActive;
                    await _unitOfWork.SaveChanges();
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.BadRequest;
                    serviceResult.Message = "Menü güncellenemez.";
                    return serviceResult;
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var menu = await _unitOfWork.Repository<Menu>()
                    .GetAll(x => x.Id == id)
                    .Include(a => a.MenuItems)
                    .FirstOrDefaultAsync();

                if (menu == null)
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Message = "Menü bulunamadı.";
                    return serviceResult;
                }
                if (menu.IsDeletable)
                {
                    menu.Deleted = true;
                    foreach (var menuItem in menu.MenuItems)
                    {
                        menuItem.Deleted = true;
                    }
                    await _unitOfWork.SaveChanges();
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.BadRequest;
                    serviceResult.Message = "Menü silinemez.";
                    return serviceResult;
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }
    }
}
