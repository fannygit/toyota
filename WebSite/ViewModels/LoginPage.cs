using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Begonia.Toyota.WebSite.Enums;

namespace Begonia.Toyota.WebSite.ViewModels
{
    /// <summary>
    /// 登入頁面
    /// </summary>
    public class LoginPage
    {
        [Required(ErrorMessage="必填")]
        public string Account { get; set; }

        [Required(ErrorMessage="必填")]
        public string Password { get; set; }

        [Required(ErrorMessage = "必填")]
        public string textfield2 { get;set;}
    }
}