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
using System.Drawing;
using Begonia.Toyota.WebSite.Library;

namespace Begonia.Toyota.WebSite.Controllers
{
    public class AccountController : Controller
    {
        AccountService Service = new AccountService();
        PermissionService permissionService = new PermissionService();

        public ActionResult INIT()
        {
            string errMsg = string.Empty;

            if (string.IsNullOrEmpty(errMsg))
                permissionService.InitPermission(out errMsg);//加入權限列表
            
            if (string.IsNullOrEmpty(errMsg))
                Service.InitAdmin(out errMsg);//加入預設使用者

            if (string.IsNullOrEmpty(errMsg))
                errMsg = "init is ok";

            return Content(errMsg);
        }

        public ActionResult INITP()
        {
            string errMsg = string.Empty;

            if (string.IsNullOrEmpty(errMsg))
                permissionService.InitPermission(out errMsg);//加入權限列表

            return Content(errMsg);
        }

        public AccountController()
        {
            Service = new AccountService();
        }

        [HttpPost]
        public ActionResult GetRoles()
        {
            List<string> list = Service.GetHaveRoles(User.Identity.Name);
            return Json(list);
        }

        #region CRUD
        //GET: /Account/List
        [MyAuthorizeAttribute(Roles = "Account,AccountC,AccountU,AccountR,AccountD,Admin")]
        [Authorize]
        public ActionResult List(AccountListModel criteria)
        {
            criteria.Rolses = Service.GetHaveRoles(User.Identity.Name);
            try
            {
                AccountListModel model = Service.GetList(User.Identity.Name, criteria);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("message", ex.Message);
            }

            criteria.Data = new List<AccountModel>();
            return View(criteria);
        }


        [MyAuthorizeAttribute(Roles = "Account,AccountC,Admin")]
        [Authorize]
        public ActionResult Add(int page = 1)
        {
            AccountModel entity = Service.NewInstance();
            entity.PermissionDropDownList = Service.GetPermission(null);
            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }

        [MyAuthorizeAttribute(Roles = "Account,AccountC,Admin")]
        [Authorize]
        [HttpPost]
        public ActionResult Create(AccountModel model)
        {

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("Password", "密碼為必填");
                }
                else if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("RePassword", "確認密碼為必填");
                }
                else if (!model.Password.Equals(model.RePassword))
                {
                    ModelState.AddModelError("RePassword", "密碼需一致");
                }
                else if (model.Password.Length < 6 || model.Password.Length > 16)
                {
                    ModelState.AddModelError("Password", "密碼需6-16字元");
                    ModelState.AddModelError("RePassword", "密碼需6-16字元");
                }
                else if (model.PermissionList!=null && model.PermissionList.Any())
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
                    ModelState.AddModelError("PermissionDropDownList", "請至少勾選一權限");
                }
            }

            model.PermissionDropDownList = Service.GetPermission(null);
            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        [MyAuthorizeAttribute(Roles = "Account,AccountU,Admin")]
        [Authorize]
        public ActionResult Edit(string id, string Search, int SStatus, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
            { 
                id = User.Identity.Name;
            }
            AccountModel model = Service.Get(User.Identity.Name, id);
            model.Mode = EditPageMode.Update;
            model.page = page;
            model.Search = Search;
            model.SStatus = SStatus;
            return View("Add", model);
        }


        [MyAuthorizeAttribute(Roles = "Account,AccountU,Admin")]
        [Authorize]
        [HttpPost]
        public ActionResult Update(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.PermissionList != null && model.PermissionList.Any())
                {
                    if (!string.IsNullOrEmpty(model.RePassword))
                    {
                        if (model.RePassword.Length > 16 || model.RePassword.Length < 6)
                        {
                            ModelState.AddModelError("RePassword", "密碼需6-16字元");
                        }
                        else
                        {
                            model.Password = model.RePassword;
                            model.UpdatePasswordTime = DateTime.Now;
                        }
                    }
                    else
                    {
                        model.UpdatePasswordTime = model.UpdateDate;//依舊沒改
                    }

                    try
                    {
                        string ErrMsgs = string.Empty;

                        if (Service.Update(User.Identity.Name, model, out ErrMsgs))
                        {
                            return RedirectToAction("List", new {page = model.page, SStatus = model.SStatus, Search = model.Search});
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
            }

            model.PermissionDropDownList = Service.GetPermission(model.Id);
            model.Mode = EditPageMode.Update;
            return View("Add", model);
        }


        [MyAuthorizeAttribute(Roles = "Account,AccountD,Admin")]
        [HttpPost]
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


        public ActionResult Info()
        {
            return View();
        }

        #region 認證模組

        // 登入頁
        // GET: /Account/Login
        public ActionResult Login()
        {
            LoginPage Model = new LoginPage();

            bool iserror = false;
            try
            {
                GerImg();
            }
            catch (Exception e)
            {
                iserror = true;
                throw;
            }
            if (iserror) GerImg();

            return View(Model);
        }


        [HttpPost]
        public ActionResult GerImgGo()
        {
            string fn = string.Empty;
            try
            {
                fn = GerImg(true);
            }
            catch (Exception e)
            {
                fn = "";
            }
            
            return Json(fn);
        }

        private string GerImg(bool isrand= false)
        {
            int NumCount = 5;//預設產生5位亂數
            if (Session["NumCount"] != null)
            {
                //有指定產生幾位數
                string NC = Session["NumCount"].ToString();
                //字串轉數字，轉型成功的話儲存到 NumCount，不成功的話，NumCount會是0
                Int32.TryParse(NC.Replace("'", "''"), out NumCount);
            }

            if (NumCount == 0) NumCount = 5;
            //取得亂數
            string str_ValidateCode = this.GetRandomNumberString(NumCount);
            /*用於驗證的Session*/
            Session["ValidateNumber"] = str_ValidateCode;

            //取得圖片物件
            System.Drawing.Image image = this.CreateCheckCodeImage(str_ValidateCode);

            string fp = string.Empty;
            string fn = string.Empty;
            if (!isrand)
            {
                fp = Server.MapPath("~/App_Data/UploadFile/ttt.jpeg");
                fn = "ttt.jpeg";
            }
            else
            {
                fn = Utils.GetObjectId() + ".jpeg";
                fp = Server.MapPath("~/App_Data/UploadFile/" + fn);
            }
            image.Save(fp, System.Drawing.Imaging.ImageFormat.Jpeg);
            return fn;
        }

        // 登入驗證
        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(LoginPage Model, string returnUrl)
        {
            string errorMsg = string.Empty;
            if (ModelState.IsValid)
            {
                if (Session["ValidateNumber"] == null)
                {
                    Response.Redirect("~/Account/Login");
                }
                string vn = Session["ValidateNumber"].ToString();
                if (Model.textfield2 == vn)
                {
                    AccountModel user = null;
                    bool isSuccess = Service.Authentication(
                        Model.Account,
                        Model.Password,
                        out user,
                        out errorMsg);

                    if (isSuccess)
                    {
                        Service.CreateLog(null, user.Id, true, true, null);
                        //要存在 cookie 的 user data
                        WebSiteUser userData = new WebSiteUser()
                        {
                            Id = user.Id,
                            Account = user.Account,
                            Roles = user.PermissionDropDownList.Select(p => p.Text).ToArray(),
                        };

                        bool isCookiePersistent = false;
                        System.Web.Script.Serialization.JavaScriptSerializer serializer =
                            new System.Web.Script.Serialization.JavaScriptSerializer();
                        string jsonAccountModel = serializer.Serialize(userData);

                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                            user.Id, DateTime.Now, DateTime.Now.AddMinutes(2880), isCookiePersistent, jsonAccountModel);

                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                        if (true == isCookiePersistent)
                            authCookie.Expires = authTicket.Expiration;
                        Response.Cookies.Add(authCookie);
                        if (user.UpdateDate.AddDays(90) <= DateTime.Now)
                        {
                            TempData["isneedshowchangepwd"] = "yes";
                        }
                        //Response.Redirect(FormsAuthentication.GetRedirectUrl(Model.Account, false));
                        Response.Redirect("~/Account/Info");
                    }
                    else
                    {
                        errorMsg = "帳密錯誤";
                    }
                }
                else
                {
                    errorMsg = "輸入驗證碼錯誤";
                    Service.CreateLog(Model.Account, Model.Account, true, false, errorMsg);
                }
            }
            else
            {
                errorMsg = "請輸入必填參數";
                Service.CreateLog(Model.Account, Model.Account, true, false, errorMsg);
            }

            ModelState.AddModelError("message", errorMsg);

            return View();
        }


        // 登出
        // GET: /Account/Signout
        public ActionResult Signout()
        {
            Service.CreateLog(null, User.Identity.Name, false, true, null);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        #endregion

        #region 產生數字亂數
        private string GetRandomNumberString(int int_NumberLength)
        {
            System.Text.StringBuilder str_Number = new System.Text.StringBuilder();//字串儲存器
            Random rand = new Random(Guid.NewGuid().GetHashCode());//亂數物件

            for (int i = 1; i <= int_NumberLength; i++)
            {
                str_Number.Append(rand.Next(0, 10).ToString());//產生0~9的亂數
            }

            return str_Number.ToString();
        }
        #endregion

        #region 產生圖片
        private System.Drawing.Image CreateCheckCodeImage(string checkCode)
        {

            System.Drawing.Bitmap image = new System.Drawing.Bitmap((checkCode.Length * 20), 40);//產生圖片，寬20*位數，高40像素
            System.Drawing.Graphics g = Graphics.FromImage(image);


            //生成隨機生成器
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int int_Red = 0;
            int int_Green = 0;
            int int_Blue = 0;
            int_Red = random.Next(256);//產生0~255
            int_Green = random.Next(256);//產生0~255
            int_Blue = (int_Red + int_Green > 400 ? 0 : 400 - int_Red - int_Green);
            int_Blue = (int_Blue > 255 ? 255 : int_Blue);

            //清空圖片背景色
            g.Clear(Color.FromArgb(int_Red, int_Green, int_Blue));

            //畫圖片的背景噪音線
            for (int i = 0; i <= 24; i++)
            {


                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);

                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);

                g.DrawEllipse(new Pen(Color.DarkViolet), new System.Drawing.Rectangle(x1, y1, x2, y2));
            }

            Font font = new System.Drawing.Font("Arial", 20, (System.Drawing.FontStyle.Bold));
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2F, true);

            g.DrawString(checkCode, font, brush, 2, 2);
            for (int i = 0; i <= 99; i++)
            {

                //畫圖片的前景噪音點
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);

                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }

            //畫圖片的邊框線
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);


            return image;

        }
        #endregion
    }
}
