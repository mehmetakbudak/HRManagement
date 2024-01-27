using Chinook.Data.Repository;
using Chinook.Storage.Entities;
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
        List<MenuItemModel> GetByMenuId(int id);
        Task<ServiceResult> Post(MenuItemModel model);
        Task<ServiceResult> Put(MenuItemModel model);        
        ServiceResult Delete(int id);
    }

    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork unitOfWork;

        public MenuItemService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<MenuItemModel> GetByMenuId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> Post(MenuItemModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var lastMenuItem = await unitOfWork.Repository<MenuItemDmo>()
                    .GetAll(x => x.ParentId == model.ParentId && x.IsActive && !x.Deleted)
                    .OrderByDescending(x => x.Order)
                    .FirstOrDefaultAsync();

                var entity = new MenuItemDmo
                {
                    Deleted = false,
                    IsActive = model.IsActive,
                    MenuId = model.MenuId,
                    Name = model.Name,
                    Url = model.Url,
                    ParentId = model.ParentId,
                    Order = lastMenuItem == null ? 1 : (lastMenuItem.Order + 1)
                };
                await unitOfWork.Repository<MenuItemDmo>().Add(entity);
                await unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Put(MenuItemModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var menuItem = await unitOfWork.Repository<MenuItemDmo>()
                    .Get(x => x.Id == model.Id && !x.Deleted && x.IsActive);

                if (menuItem != null)
                {
                    menuItem.IsActive = model.IsActive;
                    menuItem.Name = model.Name;
                    menuItem.Order = model.Order;
                    menuItem.Url = model.Url;
                    menuItem.MenuId = model.MenuId;

                    await unitOfWork.SaveChanges();
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Message = "Menü elemanı bulunamadı.";
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
