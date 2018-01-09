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
    public class AUsedCarInfoListModel : PagerModel
    {
        [Display(Name = "產品類型")]
        public string ProductType { get; set; }
        public List<System.Web.Mvc.SelectListItem> ProductTypeDropDownList { get; set; }
		public string Search { get; set; }

		public List<AUsedCarInfoModel> Data { get; set; }

        public List<string> ImgList { get; set; }

        public AUsedCarInfoModel AUsedCarInfo { get; set; }

        public List<SelectListItem> TypeList { get; set; }

        public List<SelectListItem> EngineList { get; set; }

        public List<string> TonnesList { get; set; }
        public List<string> YearsList { get; set; }
        public string type { get; set; }
        public string showtype { get; set; }
        public string eng { get; set; }
        public string showeng { get; set; }
        public string ton { get; set; }
        public string showton { get; set; }
        public string yy { get; set; }
        public string showyy { get; set; }
    }

    public class AUsedCarInfoModel : EditModePage
    {
		[Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "產品機型")]
		[Required(ErrorMessage="必填")]
		public string Type { get; set; }

		[Display(Name = "產品類型")]
        public string ProductType { get; set; }
        public List<System.Web.Mvc.SelectListItem> ProductTypeDropDownList { get; set; }

        [Display(Name = "引擎形式")]
        public string EngineId { get; set; }
        public List<System.Web.Mvc.SelectListItem> EngineDropDownList { get; set; }

		[Display(Name = "列表圖片")]
		public string ListImg { get; set; }
        public HttpPostedFileBase ImgFile { get; set; }

        [Display(Name = "序號")]
		public string Number { get; set; }


		[Display(Name = "屬具")]
		public string Tool { get; set; }

		[Display(Name = "噸數")]
		[Required(ErrorMessage="必填")]
		public string Tonnes { get; set; }

		[Display(Name = "貨叉長")]
		public string Length { get; set; }

		[Display(Name = "引擎號碼")]
		public string EngineNumber { get; set; }

		[Display(Name = "車身號碼")]
		public string CarNumber { get; set; }

		[Display(Name = "年式")]
		[Required(ErrorMessage="必填")]
		public string Years { get; set; }

		[Display(Name = "使用小時數")]
		public string UsedHours { get; set; }

		[Display(Name = "售價")]
		[Required(ErrorMessage="必填")]
		public string Price { get; set; }

		[Display(Name = "聯絡人")]
		public string ContactPerson { get; set; }

		[Display(Name = "聯絡方式")]
		public string ContactMethod { get; set; }

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

        public string ProductImg1 { get; set; }
        public HttpPostedFileBase ImgFile1 { get; set; }

        public string ProductImg2 { get; set; }
        public HttpPostedFileBase ImgFile2 { get; set; }

        public string ProductImg3 { get; set; }
        public HttpPostedFileBase ImgFile3 { get; set; }

        public string ProductImg4 { get; set; }
        public HttpPostedFileBase ImgFile4 { get; set; }

        public string ProductImg5 { get; set; }
        public HttpPostedFileBase ImgFile5 { get; set; }

        public string ProductImg6 { get; set; }
        public HttpPostedFileBase ImgFile6 { get; set; }

        public string ProductImg7 { get; set; }
        public HttpPostedFileBase ImgFile7 { get; set; }

        public string ProductImg8 { get; set; }
        public HttpPostedFileBase ImgFile8 { get; set; }

        public string ProductImg9 { get; set; }
        public HttpPostedFileBase ImgFile9 { get; set; }
        public string ProductImg10 { get; set; }
        public HttpPostedFileBase ImgFile10 { get; set; }

        public AUsedCarInfoModel()
        {
			Status = true;
			CreateTime = DateTime.Now;

        }
    }
}