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
    public class ContactUsManagerListModel : PagerModel
    {
        public string SD { get; set; }

        public string Search { get; set; }

		public List<ContactUsManagerModel> Data { get; set; }

        public ContactUsManagerListModel()
        {
            SStatus = 0;
        }
    }

    public class ContactUsManagerModel1
    {
        public string Namea { get; set; }
        public string ServiceUnit { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Detail { get; set; }
    }

    public class ContactUsManagerModel : EditModePage
    {
        public string SD { get; set; }

        [Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "姓名")]
		public string Name { get; set; }

		[Display(Name = "電子信箱")]
		public string Email { get; set; }

		[Display(Name = "來信日期")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime? AnswerDate { get; set; }

		[Display(Name = "服務單位")]
		public string ServiceUnits { get; set; }

		[Display(Name = "聯絡電話")]
		public string Tel { get; set; }

		[Display(Name = "問題描述")]
		public string ProblemInfo { get; set; }

		[Display(Name = "回覆內容")]
		public string Answer { get; set; }

        [Display(Name = "回覆人員")]
        public string AnswerName { get; set; }

		[Display(Name = "狀態")]
		public int Status { get; set; }

		[Display(Name = "建立時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime CreateDate { get; set; }

		[Display(Name = "建立者")]
		public string CreateId { get; set; }



        public ContactUsManagerModel()
        {
			CreateDate = DateTime.Now;

        }
    }
}