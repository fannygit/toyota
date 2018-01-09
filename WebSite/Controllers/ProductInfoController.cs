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

    public class ProductInfoController : BaseController
    {
        ProductInfoService Service;

        public ProductInfoController()
        {
            Service = new ProductInfoService();
        }

        #region CRUD
        //GET: /ProductInfo/List
        [MyAuthorizeAttribute(Roles = "Product,ProductC,ProductR,ProductU,ProductD,Admin")]
        public ActionResult List(ProductInfoListModel criteria)
        {
            criteria.Rolses = Service.GetHaveRoles(User.Identity.Name);
            try
            {
                ProductInfoListModel model = Service.GetList(User.Identity.Name, criteria);
                model.ProductTypeDropDownList = Service.GetProductTypeList(null);
                model.EngineDropDownList = Service.GetEngineList(null, true);

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("message", ex.Message);
            }

            return View(criteria);
        }

        [MyAuthorizeAttribute(Roles = "Product,ProductC,Admin")]
        public ActionResult Add(int page = 1)
        {
            ProductInfoModel entity = Service.NewInstance();
            entity.ProductTypeDropDownList = Service.GetProductTypeList(null);
            entity.EngineDropDownList = Service.GetEngineList(null);
            entity.AccessoriesDropDownList = Service.GetAccessories(new List<string>());
            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }


        [ValidateInput(false)]
        [HttpPost]
        [MyAuthorizeAttribute(Roles = "Product,ProductC,Admin")]
        public ActionResult Create(ProductInfoModel model)
        {
            if (model.EngineId == null)
                model.EngineId = "";

            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;
                    if (model.OverviewImgFile != null)
                    {
                        if (Service.Create(User.Identity.Name, model, out ErrMsgs))
                        {
                            return RedirectToAction("List", new { page = model.page });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("OverviewImg", "請選擇概觀圖片");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
            }
            model.ProductTypeDropDownList = Service.GetProductTypeList(model.ProductType);
            model.EngineDropDownList = Service.GetEngineList(model.EngineId);
            model.AccessoriesDropDownList = Service.GetAccessories(model.AccessoriesList);
            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        [MyAuthorizeAttribute(Roles = "Product,ProductU,Admin")]
        public ActionResult Edit(int id, int page = 1)
        {
            ProductInfoModel model = Service.Get(User.Identity.Name, id);
			if (model != null)
			{

                model.ProductTypeDropDownList = Service.GetProductTypeList(model.ProductType);
                model.EngineDropDownList = Service.GetEngineList(model.EngineId);
                model.AccessoriesDropDownList = Service.GetAccessories(model.AccessoriesList);
                model.Mode = EditPageMode.Update;
				return View("Add", model);
			}
			else
			{
				return Content("無此資料");
			}
        }


        [ValidateInput(false)]
        [HttpPost]
        [MyAuthorizeAttribute(Roles = "Product,ProductU,Admin")]
        public ActionResult Update(ProductInfoModel model)
        {
            if (model.EngineId == null)
                model.EngineId = "";

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

            model.ProductTypeDropDownList = Service.GetProductTypeList(model.ProductType);
            model.EngineDropDownList = Service.GetEngineList(model.EngineId);
            model.AccessoriesDropDownList = Service.GetAccessories(model.AccessoriesList);
            model.Mode = EditPageMode.Update;
            return View("Add", model);
        }

        [HttpPost]
        [MyAuthorizeAttribute(Roles = "Product,ProductD,Admin")]
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
