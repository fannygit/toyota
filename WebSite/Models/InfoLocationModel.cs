using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Begonia.Toyota.WebSite.Enums;
using Begonia.Toyota.WebSite.Library;

namespace Begonia.Toyota.WebSite.Models
{
    public class InfoLocationListModel : PagerModel
    {
        public string lid { get; set; }
        public string Location { get; set; }
        public List<System.Web.Mvc.SelectListItem> LocationDropDownList { get; set; }
        public string Search { get; set; }

		public List<InfoLocationModel> Data { get; set; }

    }

    public class InfoLocationModel : EditModePage
    {
        public string SearchAddress { get; set; }
        public List<System.Web.Mvc.SelectListItem> LocationDropDownList { get; set; }

		[Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "據點名稱")]
		[Required(ErrorMessage="必填")]
		public string Name { get; set; }

		[Display(Name = "區域")]
		[Required(ErrorMessage="必填")]
		public string Location { get; set; }

        [Display(Name = "地址")]
		[Required(ErrorMessage="必填")]
		public string Address { get; set; }

		[Display(Name = "電話")]
		[Required(ErrorMessage="必填")]
		public string Tel { get; set; }

		[Display(Name = "傳真")]
		[Required(ErrorMessage="必填")]
		public string Fax { get; set; }

		[Display(Name = "排序")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Orderby { get; set; }

		[Display(Name = "狀態")]
		[Required(ErrorMessage="必填")]
		public bool Status { get; set; }

		[Display(Name = "建立時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime CreateTime { get; set; }

		[Display(Name = "建立者")]
		public string CreateId { get; set; }



        public InfoLocationModel()
        {
			Status = true;
			CreateTime = DateTime.Now;

        }
    }
}