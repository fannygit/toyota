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

    public class InfoPrizeController : BaseController
    {
        InfoPrizeService Service;

        public InfoPrizeController()
        {
            Service = new InfoPrizeService();
        }

        #region CRUD

        [MyAuthorizeAttribute(Roles = "About,AboutC,AboutU,AboutR,AboutD,Admin")]
        //GET: /InfoPrize/List
        public ActionResult List(InfoPrizeListModel criteria)
        {
            criteria.Rolses = Service.GetHaveRoles(User.Identity.Name);
            try
            {
                InfoPrizeListModel model = Service.GetList(User.Identity.Name, criteria);

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("message", ex.Message);
            }

            criteria.Data = new List<InfoPrizeModel>();
            return View(criteria);
        }


        [MyAuthorizeAttribute(Roles = "About,AboutC,Admin")]
        public ActionResult Add(int page = 1)
        {
            InfoPrizeModel entity = Service.NewInstance();

            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }


        [MyAuthorizeAttribute(Roles = "About,AboutC,Admin")]
        [HttpPost]
        public ActionResult Create(InfoPrizeModel model)
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
                            return RedirectToAction("List", new { page = model.page });
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
                    ModelState.AddModelError("message", "圖片為必填");
                }
            }

            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        [MyAuthorizeAttribute(Roles = "About,AboutU,Admin")]
        public ActionResult Edit(int id, int page = 1)
        {
            InfoPrizeModel model = Service.Get(User.Identity.Name, id);
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


        [MyAuthorizeAttribute(Roles = "About,AboutU,Admin")]
        [HttpPost]
        public ActionResult Update(InfoPrizeModel model)
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
