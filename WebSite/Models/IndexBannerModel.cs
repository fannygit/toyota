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
    public class IndexBannerListModel : PagerModel
    {
		public string Search { get; set; }

		public List<IndexBannerModel> Data { get; set; }
    }

    public class IndexBannerModel : EditModePage
    {
		[Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "Banner大標題")]
		[Required(ErrorMessage="必填")]
		public string BannerTitle { get; set; }

		[Display(Name = "Banner簡述")]
		public string BannerIntro { get; set; }

		[Display(Name = "按鈕連結")]
		public string BtnUrl { get; set; }

        [Display(Name = "圖片建議尺寸: 1700x900")]
        public string Img { get; set; }
        public HttpPostedFileBase ImgFile { get; set; }

		[Display(Name = "排序")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Orderby { get; set; }

		[Display(Name = "狀態")]
        public bool Status { get; set; }
        public int StatusValue { get; set; }

		[Display(Name = "建立時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime CreateTime { get; set; }

		[Display(Name = "建立者")]
		public string CreateId { get; set; }



        public IndexBannerModel()
        {
			Status = true;
			CreateTime = DateTime.Now;

        }
    }
}