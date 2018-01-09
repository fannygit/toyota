using Begonia.Toyota.WebSite.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Begonia.Toyota.WebSite.Library
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            String[] roles = Roles.Split(',');//取得輸入role清單
            if (!httpContext.User.Identity.IsAuthenticated)//判斷是否已驗證
                return false;
            
            if (roles.Length != 0)
            {
                AccountService Service = new AccountService();
                String account = httpContext.User.Identity.Name.ToString(); //登入的使用者帳號
                return Service.Isright(account, roles);
            }
            return false;
        }
    }
}