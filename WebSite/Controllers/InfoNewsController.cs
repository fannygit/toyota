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

    public class InfoNewsController : BaseController
    {
        InfoNewsService Service;

        public InfoNewsController()
        {
            Service = new InfoNewsService();
        }

        #region CRUD
        //GET: /InfoNews/List

        [MyAuthorizeAttribute(Roles = "About,AboutC,AboutU,AboutR,AboutD,Admin")]
        public ActionResult List(InfoNewsListModel criteria)
        {
            criteria.Rolses = Service.GetHaveRoles(User.Identity.Name);
            try
            {
                InfoNewsListModel model = Service.GetList(User.Identity.Name, criteria);

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("message", ex.Message);
            }
            criteria.Data = new List<InfoNewsModel>();
            return View(criteria);
        }


        [MyAuthorizeAttribute(Roles = "About,AboutC,Admin")]
        public ActionResult Add(int page = 1)
        {
            InfoNewsModel entity = Service.NewInstance();

            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }


        [MyAuthorizeAttribute(Roles = "About,AboutC,Admin")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(InfoNewsModel model)
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
                    ModelState.AddModelError("message", "請選擇列表圖片");
                }
            }

            ModelState.AddModelError("message", "請填入必填參數");
            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        [MyAuthorizeAttribute(Roles = "About,AboutU,Admin")]
        public ActionResult Edit(int id, int SStatus, string Search, string SD, string ED, int page = 1)
        {
            InfoNewsModel model = Service.Get(User.Identity.Name, id);
            if (model != null)
            {
                model.SStatus = SStatus;
                model.Search = Search;
                model.Mode = EditPageMode.Update;
                return View("Add", model);
            }
            else
            {
                return Content("無此資料");
            }
        }


        [MyAuthorizeAttribute(Roles = "About,AboutU,Admin")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Update(InfoNewsModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;

                    string UserAccount = (null != UserManager.User) ? UserManager.User.Account : "N/A";

                    if (Service.Update(User.Identity.Name, UserAccount, model, out ErrMsgs))
                    {
                        Service.CreateLog(User.Identity.Name, "編輯", model.Id, model.Title);
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

        [MyAuthorizeAttribute(Roles = "About,AboutD,Admin")]
        [HttpPost]
        public ActionResult Delete(int id, int page = 1)
        {
            string errorMsg = string.Empty;
            try
            {
                if (Service.Delete(User.Identity.Name, id, out errorMsg))
                {
                    Service.CreateLog(User.Identity.Name, "刪除", id, "");
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
