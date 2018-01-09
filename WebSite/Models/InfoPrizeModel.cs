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
    public class InfoPrizeListModel : PagerModel
    {
        public string YearsDT { get; set; }

        public string SD { get; set; }
        public string ED { get; set; }

        public string Search { get; set; }

		public List<InfoPrizeModel> Data { get; set; }
        
    }

    public class InfoPrizeModel : EditModePage
    {
        public string SD { get; set; }
        public string ED { get; set; }

        public string YearsDT { get; set; }

        [Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "標題")]
		[Required(ErrorMessage="必填")]
		public string Title { get; set; }

		[Display(Name = "得獎年份")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public string Years { get; set; }

		[Display(Name = "圖片")]
		public string Img { get; set; }
        public HttpPostedFileBase ImgFile { get; set; }

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



        public InfoPrizeModel()
        {
			Status = true;
			CreateTime = DateTime.Now;

        }
    }
}