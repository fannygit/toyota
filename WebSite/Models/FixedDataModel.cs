using Begonia.Toyota.WebSite.Models;
using JamZoo.Project.WebSite.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Begonia.Toyota.WebSite.Models
{
    /// <summary>
    /// 列表頁
    /// </summary>
    public class FixedDataListModel : PagerModel
    {
        public string Search { get; set; }

        public List<FixedDataModel> Data { get; set; }
    }

    /// <summary>
    /// 帳號
    /// </summary>
    public class FixedDataModel : EditModePage
    {

        [Display(Name = "編號")]
        public string Id { get; set; }

        [Display(Name = "HTML")]
        public string Html { get; set; }

        public string HumanBankUrl { get; set; }
        public string MailBoxSender { get; set; }
        public string MailBoxReceiver { get; set; }

        [Display(Name = "功能名稱")]
        [Required(ErrorMessage = "必填")]
        public string FunctionName { get; set; }


        [Display(Name = "建立時間")]
        [Required(ErrorMessage = "必填")]
        [DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "建立者")]
        public string CreateId { get; set; }

    }
}