using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using Begonia.Toyota.WebSite.Enums;

namespace Begonia.Toyota.WebSite.Models
{
    /// <summary>
    /// 列表頁
    /// </summary>
    public class AccountListModel : PagerModel
    {
        public string Search { get; set; }

        public List<AccountModel> Data { get; set; }
    }

    /// <summary>
    /// 帳號
    /// </summary>
    public class AccountModel : EditModePage
    {
        public Dictionary<string, string> RolgMapping { get; set; }
        public string IP { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        [StringLength(256, MinimumLength = 5, ErrorMessage = "請至少填入五個字元")]
        [Required(ErrorMessage="必填")]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        
        public string Password { get; set; }

        
        public string RePassword { get; set; }

        [Display(Name = "狀態")]
        public int Status { get; set; }

        public List<System.Web.Mvc.SelectListItem> PermissionDropDownList { get; set; }

        public List<string> PermissionList { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }
        public string CreateUserID { get; set; }
        public string UpdateUserID { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime UpdatePasswordTime { get; set; }

        [Display(Name = "最後登入時間")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 取得 Role
        /// </summary>
        /// <returns></returns>
        [Display(Name = "角色")]
        [Required(ErrorMessage = "必填")]
        public string Role { get; set; }

        public string Email { get; set; }

        public AccountModel()
        {
            Status = 1;
            RolgMapping = new Dictionary<string, string>()
            {
                //{"Admin", "Admin"},
                //{"Account",  "系統管理"},
                {"AccountC",  "系統管理-新增"},
                {"AccountR",  "系統管理-瀏覽"},
                {"AccountU",  "系統管理-更新"},
                {"AccountD",  "系統管理-刪除"},
                //{"Index", "首頁管理"},
                {"IndexC", "首頁管理-新增"},
                {"IndexR", "首頁管理-瀏覽"},
                {"IndexU", "首頁管理-更新"},
                {"IndexD", "首頁管理-刪除"},
                //{"About", "關於台灣豐田管理"},
                {"AboutC", "關於台灣豐田管理-新增"},
                {"AboutR", "關於台灣豐田管理-瀏覽"},
                {"AboutU", "關於台灣豐田管理-更新"},
                {"AboutD", "關於台灣豐田管理-刪除"},
                //{"Product",  "產品管理"},
                {"ProductC",  "產品管理-新增"},
                {"ProductR",  "產品管理-瀏覽"},
                {"ProductU",  "產品管理-更新"},
                {"ProductD",  "產品管理-刪除"},
                //{"Used", "中古車"},
                {"UsedC", "中古車-新增"},
                {"UsedR", "中古車-瀏覽"},
                {"UsedU", "中古車-更新"},
                {"UsedD", "中古車-刪除"},
                //{"Service", "產車服務"},
                {"ServiceC", "產車服務-新增"},
                {"ServiceR", "產車服務-瀏覽"},
                {"ServiceU", "產車服務-更新"},
                {"ServiceD", "產車服務-刪除"},
                //{"Human",  "人才招募"},
                {"HumanC",  "人才招募-新增"},
                {"HumanR",  "人才招募-瀏覽"},
                {"HumanU",  "人才招募-更新"},
                {"HumanD",  "人才招募-刪除"},
                //{"Contact", "聯絡我們"},
                {"ContactC", "聯絡我們-新增"},
                {"ContactR", "聯絡我們-瀏覽"},
                {"ContactU", "聯絡我們-更新"},
                {"ContactD", "聯絡我們-刪除"},
                //{"Faq", "常見問題"},
                {"FaqC", "常見問題-新增"},
                {"FaqR", "常見問題-瀏覽"},
                {"FaqU", "常見問題-更新"},
                {"FaqD", "常見問題-刪除"},
            };

        }
    }
}