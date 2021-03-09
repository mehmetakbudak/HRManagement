using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Enums;
using Chinook.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Chinook.Service
{
    public interface IMenuService
    {
        List<MenuModel> GetAll();
        MenuModel GetById(int id);
        MenuModel GetByType(MenuType type);
        ServiceResult Post(MenuModel model);
        ServiceResult Put(MenuModel model);
        ServiceResult Delete(int id);
    }

    public class MenuService : IMenuService
    {
        readonly IUnitOfWork unitOfWork;

        public MenuService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<MenuModel> GetAll()
        {
            var list = unitOfWork.Repository<Menu>()
                .GetAll(x => !x.Deleted, x => x.Include(a => a.MenuItems))
                .AsEnumerable()
                .Select(x => new MenuModel
                {
                    Id = x.Id,
                    Label = x.Name,
                    Type = x.Type,
                    IsActive = x.IsActive,
                    IsDeletable = x.IsDeletable,
                    Items = GetSubMenu(x.MenuItems, x.Id, null)
                }).ToList();

            return list;
        }

        public MenuModel GetById(int id)
        {
            var menu = unitOfWork.Repository<Menu>()
                  .GetAll(x => !x.Deleted && x.Id == id, x => x.Include(a => a.MenuItems))
                  .AsEnumerable()
                  .Select(x => new MenuModel
                  {
                      Id = x.Id,
                      Label = x.Name,
                      Type = x.Type,
                      IsActive = x.IsActive,
                      IsDeletable = x.IsDeletable,
                      Items = GetSubMenu(x.MenuItems, x.Id, null)
                  }).FirstOrDefault();
            return menu;
        }

        public MenuModel GetByType(MenuType type)
        {
            var model = new MenuModel();
            var menu = unitOfWork.Repository<Menu>()
                  .GetAll(x => !x.Deleted && x.Type == type,
                  x => x.Include(a => a.MenuItems)).FirstOrDefault();

            if (menu != null)
            {
                model.Id = menu.Id;
                model.IsActive = menu.IsActive;
                model.IsDeletable = menu.IsDeletable;
                model.Label = menu.Name;
                model.Type = menu.Type;

                if (menu.MenuItems.Any())
                {
                    model.Items = GetSubMenu(menu.MenuItems, menu.Id, null);
                }
                else
                {
                    model.Items = null;
                }
            }
            return model;
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
                    model.Items = items;
                }

                list.Add(model);
            }
            return list;
        }

        public ServiceResult Post(MenuModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var entity = new Menu
                {
                    Deleted = false,
                    IsActive = true,
                    IsDeletable = true,
                    Name = model.Label,
                    Type = MenuType.Other
                };
                unitOfWork.Repository<Menu>().Add(entity);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public ServiceResult Put(MenuModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var menu = unitOfWork.Repository<Menu>().Get(x => x.Id == model.Id);
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
                    unitOfWork.Save();
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

        public ServiceResult Delete(int id)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var menu = unitOfWork.Repository<Menu>()
                    .Get(x => x.Id == id,
                    x => x.Include(a => a.MenuItems));

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
                    unitOfWork.Save();
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
