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
    public class IndexVideoListModel : PagerModel
    {
		public string Search { get; set; }

		public List<IndexVideoModel> Data { get; set; }
    }

    public class IndexVideoModel : EditModePage
    {
		[Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "影片標題")]
		[Required(ErrorMessage="必填")]
		public string VideoTitle { get; set; }

		[Display(Name = "影片簡述")]
		[Required(ErrorMessage="必填")]
		public string VideoIntro { get; set; }

		[Display(Name = "預覽影片連結")]
		[Required(ErrorMessage="必填")]
		public string ReviewUrl { get; set; }

		[Display(Name = "完整影片連結")]
		[Required(ErrorMessage="必填")]
		public string FullUrl { get; set; }

		[Display(Name = "圖片(mobile)")]
        public string MobileImg { get; set; }
        public HttpPostedFileBase ImgFile { get; set; }

		[Display(Name = "狀態")]
		[Required(ErrorMessage="必填")]
		public bool Status { get; set; }

		[Display(Name = "建立時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime CreateTime { get; set; }

		[Display(Name = "建立者")]
		public string CreateId { get; set; }



        public IndexVideoModel()
        {
			Status = true;
			CreateTime = DateTime.Now;

        }
    }
}