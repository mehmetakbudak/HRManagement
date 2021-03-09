using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Chinook.Service
{
    public interface IMenuItemService
    {
        ServiceResult Post(MenuItemModel model);
        ServiceResult Put(MenuItemModel model);
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

        public ServiceResult Post(MenuItemModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var lastMenuItem = unitOfWork.Repository<MenuItem>()
                    .GetAll(x => x.ParentId == model.ParentId && x.IsActive && !x.Deleted)
                    .OrderByDescending(x => x.Order)
                    .FirstOrDefault();

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
                unitOfWork.Repository<MenuItem>().Add(entity);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public ServiceResult Put(MenuItemModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var menuItem = unitOfWork.Repository<MenuItem>()
                    .Get(x => x.Id == model.Id && !x.Deleted && x.IsActive);

                if (menuItem != null)
                {
                    menuItem.IsActive = model.IsActive;
                    menuItem.Name = model.Name;
                    menuItem.Order = model.Order;
                    menuItem.Url = model.Url;
                    menuItem.MenuId = model.MenuId;
                    unitOfWork.Save();
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
