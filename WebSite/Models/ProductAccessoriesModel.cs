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
    public class ProductAccessoriesListModel : PagerModel
    {
		public string Search { get; set; }

		public List<ProductAccessoriesModel> Data { get; set; }
    }

    public class ProductAccessoriesModel : EditModePage
    {
		[Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "配件名稱")]
        public string NameR { get; set; }
        [Display(Name = "配件名稱")]
        public string NameB { get; set; }

		[Display(Name = "適應機型")]
		[Required(ErrorMessage="必填")]
		public string Type { get; set; }

		[Display(Name = "圖片")]
		public string Img { get; set; }
        public HttpPostedFileBase ImgFile { get; set; }

        [Display(Name = "內容")]
		[Required(ErrorMessage="必填")]
		public string Detail { get; set; }

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



        public ProductAccessoriesModel()
        {
			Status = true;
			CreateTime = DateTime.Now;

        }
    }
}