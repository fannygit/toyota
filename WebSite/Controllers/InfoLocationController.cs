﻿/*
ModelName
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Begonia.Toyota.WebSite.Models;
using Begonia.Toyota.WebSite.Service;
using Begonia.Toyota.WebSite.ViewModels;
using Begonia.Toyota.WebSite.Enums;
using Begonia.Toyota.WebSite.Library.Principal;
using Begonia.Toyota.WebSite.Library;

namespace Begonia.Toyota.WebSite.Controllers
{

    public class InfoLocationController : BaseController
    {
        InfoLocationService Service;

        public InfoLocationController()
        {
            Service = new InfoLocationService();
        }

        #region CRUD
        //GET: /InfoLocation/List

        [MyAuthorizeAttribute(Roles = "About,AboutC,AboutU,AboutR,AboutD,Admin")]
        public ActionResult List(InfoLocationListModel criteria)
        {
            criteria.Rolses = Service.GetHaveRoles(User.Identity.Name);
            try
            {
                InfoLocationListModel model = Service.GetList(User.Identity.Name, criteria);

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("message", ex.Message);
            }

            criteria.Data = new List<InfoLocationModel>();
            return View(criteria);
        }

        [MyAuthorizeAttribute(Roles = "About,AboutC,Admin")]
        public ActionResult Add(int page = 1)
        {
            InfoLocationModel entity = Service.NewInstance();

            entity.page = page;
            entity.Mode = EditPageMode.Create;
            entity.LocationDropDownList = Service.GetLocation("");
            return View(entity);
        }

        [MyAuthorizeAttribute(Roles = "About,AboutC,Admin")]
        [HttpPost]
        public ActionResult Create(InfoLocationModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;

                    if (Service.Create(
                        User.Identity.Name, 
                        model, out ErrMsgs))
                    {
                        return RedirectToAction("List", new { page = model.page });
                    }

                    ModelState.AddModelError("message", ErrMsgs);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
            }
            
            model.LocationDropDownList = Service.GetLocation(model.Location);
            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        [MyAuthorizeAttribute(Roles = "About,AboutU,Admin")]
        public ActionResult Edit(int id, int page = 1)
        {
            InfoLocationModel model = Service.Get(User.Identity.Name, id);
			if (model != null)
			{
                model.LocationDropDownList = Service.GetLocation(model.Location);
                model.Mode = EditPageMode.Update;
				return View("Add", model);
			}
			else
			{
				return Content("無此資料");
			}
        }

        [MyAuthorizeAttribute(Roles = "About,AboutU,Admin")]
        [HttpPost]
        public ActionResult Update(InfoLocationModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;

					string UserAccount = (null != UserManager.User) ? UserManager.User.Account : "N/A";

                    if (Service.Update(User.Identity.Name, UserAccount, model, out ErrMsgs))
                    {
                        return RedirectToAction("List", new { page = model.page });
                    }
                    else
                    {
                        ModelState.AddModelError("message", "修改失敗:" + ErrMsgs);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
            }


            model.LocationDropDownList = Service.GetLocation(model.Location);
            model.Mode = EditPageMode.Update;
            return View("Add", model);
        }

        [MyAuthorizeAttribute(Roles = "About,AboutD,Admin")]
        [HttpPost]
        public ActionResult Delete(int id, int page = 1)
        {
            string errorMsg = string.Empty;
            try
            {
                if (Service.Delete(User.Identity.Name, id, out errorMsg))
                {
                    return Json("成功修改");
                }
            }
            catch (Exception ex)
            {
                return Json("刪除失敗，原因:" + ex.Message);
            }
            return Json("刪除失敗，原因:" + errorMsg);
        }
        #endregion


    }
}
