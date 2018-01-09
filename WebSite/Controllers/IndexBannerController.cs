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

    public class IndexBannerController : BaseController
    {
        IndexBannerService Service;

        public IndexBannerController()
        {
            Service = new IndexBannerService();
        }

        #region CRUD
        //GET: /IndexBanner/List
        [MyAuthorizeAttribute(Roles = "Index,IndexC,IndexU,IndexR,IndexD,Admin")]
        public ActionResult List(IndexBannerListModel criteria)
        {
            criteria.Rolses = Service.GetHaveRoles(User.Identity.Name);
            try
            {
                IndexBannerListModel model = Service.GetList(User.Identity.Name, criteria);

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("message", ex.Message);
            }

            criteria.Data = new List<IndexBannerModel>();
            return View(criteria);
        }


        [MyAuthorizeAttribute(Roles = "Index,IndexC,Admin")]
        public ActionResult Add(int page = 1)
        {
            IndexBannerModel entity = Service.NewInstance();

            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }

        [HttpPost]
        [MyAuthorizeAttribute(Roles = "Index,IndexC,Admin")]
        public ActionResult Create(IndexBannerModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImgFile != null)
                {
                    try
                    {
                        string ErrMsgs = string.Empty;

                        if (Service.Create(
                            User.Identity.Name,
                            model, out ErrMsgs))
                        {
                            return RedirectToAction("List", new {page = model.page});
                        }

                        ModelState.AddModelError("message", ErrMsgs);

                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("message", ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("Img", "圖片為必填");
                }
                
            }

            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }

        [MyAuthorizeAttribute(Roles = "Index,IndexU,Admin")]
        public ActionResult Edit(int id, string Search, int SStatus, int page = 1)
        {
            IndexBannerModel model = Service.Get(User.Identity.Name, id);
			if (model != null)
            {
                model.page = page;
                model.Search = Search;
                model.SStatus = SStatus;
				model.Mode = EditPageMode.Update;
				return View("Add", model);
			}
			else
			{
				return Content("無此資料");
			}
        }

        [HttpPost]
        [MyAuthorizeAttribute(Roles = "Index,IndexU,Admin")]
        public ActionResult Update(IndexBannerModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;

					string UserAccount = (null != UserManager.User) ? UserManager.User.Account : "N/A";

                    if (Service.Update(User.Identity.Name, UserAccount, model, out ErrMsgs))
                    {
                        return RedirectToAction("List", new { page = model.page, SStatus = model.SStatus, Search = model.Search });
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
        [MyAuthorizeAttribute(Roles = "Index,IndexD,Admin")]
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
