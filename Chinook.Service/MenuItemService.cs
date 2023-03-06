using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
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
        Task<ServiceResult> Post(MenuItemModel model);
        Task<ServiceResult> Put(MenuItemModel model);
        ServiceResult Up(int id);
        ServiceResult Down(int id);
        ServiceResult Delete(int id);
    }

    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork unitOfWork;

        public MenuItemService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Post(MenuItemModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var lastMenuItem = await unitOfWork.Repository<MenuItem>()
                    .GetAll(x => x.ParentId == model.ParentId && x.IsActive && !x.Deleted)
                    .OrderByDescending(x => x.Order)
                    .FirstOrDefaultAsync();

                var entity = new MenuItem
                {
                    Deleted = false,
                    IsActive = model.IsActive,
                    MenuId = model.MenuId,
                    Name = model.Name,
                    Url = model.Url,
                    ParentId = model.ParentId,
                    Order = lastMenuItem == null ? 1 : (lastMenuItem.Order + 1)
                };
                await unitOfWork.Repository<MenuItem>().Add(entity);
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
                var menuItem = await unitOfWork.Repository<MenuItem>()
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

        public ServiceResult Up(int id)
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

        public ServiceResult Down(int id)
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
