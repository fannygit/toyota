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
    public class ContactCatalogManagerListModel : PagerModel
    {
        public string CDate { get; set; }
        public string RDate { get; set; }

        public string Search { get; set; }

		public List<ContactCatalogManagerModel> Data { get; set; }

        public ContactCatalogManagerListModel()
        {
            SStatus = 0;
        }
    }

    public class ContactCatalogManagerModel1
    {
        public List<string> ck { get; set; }
        public List<SelectListItem> TypeList { get; set; }
        public List<string> NameList { get; set; }
        public List<string> CityList { get; set; }
        public List<string> AreaList { get; set; }
        public string Namea { get; set; }
        public string ServiceUnit { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Detail { get; set; }
        public string ReMarks { get; set; }
        public string Address { get; set; }

        public string selectCity { get; set; }
        public string selectArea { get; set; }
        public string selectPt { get; set; }
        public string selectName { get; set; }
    }

    public class ContactCatalogManagerModel : EditModePage
    {

        public string CDate { get; set; }
        public string RDate { get; set; }

        [Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "姓名")]
		public string Name { get; set; }

		[Display(Name = "電子信箱")]
		public string Email { get; set; }

		[Display(Name = "回覆日期")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime? AnswerDate { get; set; }

		[Display(Name = "服務單位")]
		public string ServiceUnits { get; set; }

		[Display(Name = "聯絡電話")]
		public string Tel { get; set; }

		[Display(Name = "地址")]
		public string Address { get; set; }

		[Display(Name = "目前擁有車輛品牌")]
		public string HaveBrand { get; set; }

		[Display(Name = "型錄索取")]
		public string WantCatalog { get; set; }

		[Display(Name = "備註")]
		public string Remark { get; set; }

		[Display(Name = "回覆內容")]
		public string Answer { get; set; }

		[Display(Name = "建立時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime CreateDate { get; set; }

		[Display(Name = "建立者")]
		public string CreateId { get; set; }

		[Display(Name = "回覆人員")]
		public string AnswerName { get; set; }

        public int Status { get;set;}



        public ContactCatalogManagerModel()
        {
			CreateDate = DateTime.Now;

        }
    }
}