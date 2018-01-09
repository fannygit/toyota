/*
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

    public class AUsedCarEngineController : BaseController
    {
        AUsedCarEngineService Service;

        public AUsedCarEngineController()
        {
            Service = new AUsedCarEngineService();
        }

        #region CRUD
        //GET: /AUsedCarEngine/List
        [MyAuthorizeAttribute(Roles = "Used,UsedC,UsedU,UsedR,UsedD,Admin")]
        public ActionResult List(AUsedCarEngineListModel criteria)
        {
            criteria.Rolses = Service.GetHaveRoles(User.Identity.Name);
            try
            {
                AUsedCarEngineListModel model = Service.GetList(User.Identity.Name, criteria);

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("message", ex.Message);
            }

            criteria.Data = new List<AUsedCarEngineModel>(); 
            return View(criteria);
        }

        [MyAuthorizeAttribute(Roles = "Used,UsedC,Admin")]
        public ActionResult Add(int page = 1)
        {
            AUsedCarEngineModel entity = Service.NewInstance();

            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }

        [HttpPost]
        [MyAuthorizeAttribute(Roles = "Used,UsedC,Admin")]
        public ActionResult Create(AUsedCarEngineModel model)
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

            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        [MyAuthorizeAttribute(Roles = "Used,UsedU,Admin")]
        public ActionResult Edit(string id, int page = 1)
        {
            AUsedCarEngineModel model = Service.Get(User.Identity.Name, id);
			if (model != null)
			{
				model.Mode = EditPageMode.Update;
				return View("Add", model);
			}
			else
			{
				return Content("無此資料");
			}
        }

        [HttpPost]
        [MyAuthorizeAttribute(Roles = "Used,UsedU,Admin")]
        public ActionResult Update(AUsedCarEngineModel model)
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

            model.Mode = EditPageMode.Update;
            return View("Add", model);
        }

        [HttpPost]
        [MyAuthorizeAttribute(Roles = "Used,UsedD,Admin")]
        public ActionResult Delete(string id, int page = 1)
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
