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
    
    public class ContactUsManagerController : BaseController
    {
        ContactUsManagerService Service;

        public ContactUsManagerController()
        {
            Service = new ContactUsManagerService();
        }

        #region CRUD
        //GET: /ContactUsManager/List
        [MyAuthorizeAttribute(Roles = "Contact,ContactC,ContactU,ContactR,ContactD,Admin")]
        public ActionResult List(ContactUsManagerListModel criteria)
        {
            criteria.Rolses = Service.GetHaveRoles(User.Identity.Name);
            try
            {
                ContactUsManagerListModel model = Service.GetList(User.Identity.Name, criteria);

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("message", ex.Message);
            }

            criteria.Data=new List<ContactUsManagerModel>();
            return View(criteria);
        }


        [MyAuthorizeAttribute(Roles = "Contact,ContactC,Admin")]
        public ActionResult Add(int page = 1)
        {
            ContactUsManagerModel entity = Service.NewInstance();

            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }


        [HttpPost]
        [MyAuthorizeAttribute(Roles = "Contact,ContactC,Admin")]
        public ActionResult Create(ContactUsManagerModel model)
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


        [MyAuthorizeAttribute(Roles = "Contact,ContactU,Admin")]
        public ActionResult Edit(int id, int page = 1)
        {
            ContactUsManagerModel model = Service.Get(User.Identity.Name, id);
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
        [MyAuthorizeAttribute(Roles = "Contact,ContactU,Admin")]
        public ActionResult Update(ContactUsManagerModel model)
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
        [MyAuthorizeAttribute(Roles = "Contact,ContactD,Admin")]
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
