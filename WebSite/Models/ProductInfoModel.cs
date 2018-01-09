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
    public class ProductInfoListModel : PagerModel
    {
        [Display(Name = "產品類型")]
        public string ProductType { get; set; }
        public List<System.Web.Mvc.SelectListItem> ProductTypeDropDownList { get; set; }

        [Display(Name = "引擎形式")]
        public string EngineId { get; set; }
        public List<System.Web.Mvc.SelectListItem> EngineDropDownList { get; set; }
		public string Search { get; set; }

		public List<ProductInfoModel> Data { get; set; }

        public List<ProductInfoModel> Data1 { get; set; }

        public List<ProductInfoModel> Data2 { get; set; }

        public List<ProductInfoModel> Data3 { get; set; }

        public List<ProductInfoModel> Data4 { get; set; }
        public List<ProductInfoModel> Data5 { get; set; }

        public List<ProductInfoModel> Data6 { get; set; }

        public List<ProductInfoModel> Data7 { get; set; }
    }
    
    public class ProductInfoModel : EditModePage
    {
		[Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "產品名稱(系列)")]
		[Required(ErrorMessage="必填")]
		public string Name { get; set; }

		[Display(Name = "產品次標題")]
		[Required(ErrorMessage="必填")]
		public string SubTitle { get; set; }

        [Display(Name = "產品類型")]
        public string ProductType { get; set; }
        public List<System.Web.Mvc.SelectListItem> ProductTypeDropDownList { get; set; }

        [Display(Name = "引擎形式")]
        public string EngineId { get; set; }
        public List<System.Web.Mvc.SelectListItem> EngineDropDownList { get; set; }

		[Display(Name = "列表圖片")]
		public string Img { get; set; }
        public HttpPostedFileBase ImgFile { get; set; }
        
        public string Logo { get; set; }
        public HttpPostedFileBase LogoFile { get; set; }

        [Display(Name = "Banner圖片")]
		public string Banner { get; set; }
        public HttpPostedFileBase BannerFile { get; set; }

        [Display(Name = "概觀-機型")]
		[Required(ErrorMessage="必填")]
		public string OverviewType { get; set; }


        [Display(Name = "概觀-簡述")]
        public string OverviewTitle { get; set; }

        [Display(Name = "概觀-簡述")]
        public string OverviewIntro { get; set; }
        
        [Display(Name = "概觀-噸數起始")]
        public double OverviewTonnesStart { get; set; }
        public string OverviewTonnesStartSHOW { get; set; }


        [Display(Name = "概觀-噸數起始")]
        public double OverviewTonnesEnd { get; set; }
        public string OverviewTonnesEndSHOW { get; set; }

		[Display(Name = "概觀圖片")]
		public string OverviewImg { get; set; }
        public HttpPostedFileBase OverviewImgFile { get; set; }

        [Display(Name = "概觀-圖片簡述")]
		public string OverviewImgIntro { get; set; }

		[Display(Name = "技術-產品介紹")]
		public string Introduction { get; set; }

		[Display(Name = "詳細規格-汽油")]
		public string DetailGas { get; set; }

		[Display(Name = "詳細規格-柴油")]
		public string DetailDiesel { get; set; }

		[Display(Name = "詳細規格-電動")]
		public string DetailElectric { get; set; }

		[Display(Name = "詳細規格-俯瞰+側面圖")]
		public string DetailOverlookImg { get; set; }
        public HttpPostedFileBase LookImgFile { get; set; }

  //      [Display(Name = "詳細規格-側面圖")]
		//public string DetailSlideImg { get; set; }
  //      public HttpPostedFileBase SlideImgFile { get; set; }

        [Display(Name = "詳細規格-引擎規格表")]
		public string DetailEngineTable { get; set; }

		[Display(Name = "詳細規格-馬達和電瓶規格")]
		public string DetailMotorBatteryTable { get; set; }

		[Display(Name = "meta_keywords")]
		public string MetaKeywords { get; set; }

		[Display(Name = "meta_description")]
		public string MetaDescription { get; set; }

        public List<string> AccessoriesList { get; set; } 

        /// <summary>
        /// 配件管理
        /// </summary>
        public List<SelectListItem> AccessoriesDropDownList { get; set; }

        //上架on-shelf 
        public DateTime? OnShelfDate { get; set; }  

        [Display(Name = "排序")]
        [Required(ErrorMessage = "必填")]
        [RegularExpression("\\d+", ErrorMessage = "格式錯誤")]
        public int Orderby { get; set; }

        [Display(Name = "狀態")]
        [Required(ErrorMessage = "必填")]
        public bool Status { get; set; }

		[Display(Name = "建立時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime CreateTime { get; set; }

		[Display(Name = "建立者")]
		public string CreateId { get; set; }

        public string ProductDesc1 { get; set; }
        public string ProductImg1 { get; set; }
        public HttpPostedFileBase ImgFile1 { get; set; }

        public string ProductDesc2 { get; set; }
        public string ProductImg2 { get; set; }
        public HttpPostedFileBase ImgFile2 { get; set; }

        public string ProductDesc3 { get; set; }
        public string ProductImg3 { get; set; }
        public HttpPostedFileBase ImgFile3 { get; set; }

        public string ProductDesc4 { get; set; }
        public string ProductImg4 { get; set; }
        public HttpPostedFileBase ImgFile4 { get; set; }

        public string ProductDesc5 { get; set; }
        public string ProductImg5 { get; set; }
        public HttpPostedFileBase ImgFile5 { get; set; }

        public string ProductDesc6 { get; set; }
        public string ProductImg6 { get; set; }
        public HttpPostedFileBase ImgFile6 { get; set; }

        public Dictionary<string, string> ProductImgList { get; set; }
        public ProductAccessoriesListModel AccessoriesDataList { get; set; }

        public ProductInfoModel()
        {
            AccessoriesList = new List<string>();
            CreateTime = DateTime.Now;
            Status = true;
            string css = "<link href=\"/content/css/app.css\" rel=\"stylesheet\" />" +
                         "<link href=\"/content/css/hamburger.css\" rel=\"stylesheet\" />";

            Introduction += css;
            Introduction += "<div class=\"sectionTitle\"><h2> 技術</h2> <span class=\"engSub\">Technology</span>";
            Introduction += "<div class=\"row\" id=\"a\"><div class=\"small-12 small-centered medium-8 columns\"><br class=\"hide-for-small-only\">" +
                            "<h3 class=\"red\">承襲豐田一貫的高品質引擎強韌、可靠、環保</h3><p class=\"fz18\">新8系列(8FG/D 35N-80N)擁有最可靠的引擎、動力豐沛、持久耐力及低耗油量，支援所有工況作業，並降低營運成本。</p>" +
                            "<br><br></div></div>";
            Introduction += "<div class=\"row\" id=\"b\"><div class=\"small-12 medium-7 columns\"><img src=\"/Content/images/contect_1_03.jpg\" alt=\"\"></div>" +
                            "<div class=\"small-12 medium-5 columns left\"><h4 class=\"orange left insidetitle\">低油耗和世界一流的發動機清潔</h4><p class=\"left\">用於優化燃料噴射量新發動機，燃燒效率也在電子控制的改善。用於優化燃料噴射量新發動機，燃燒效率也在電子控制的改善。" +
                            "</p><img src=\"/Content/images/contect_2_03.jpg\" alt=\"\"></div></div>";
            Introduction += "<div class=\"dotted\"></div><div class=\"row\" id=\"c\"><div class=\"small-12 medium-7 columns\">" +
                            "<br><h4 class=\"orange left insidetitle\">燃料消耗的降低</h4><p class=\"left\">新的發動機，負荷傳感動力轉向程序（LSP），高效率轉矩，在頂級的裝 備轉換器，它已經實現節油性能，" +
                            "</p></div><div class=\"small-12  medium-5  columns left\"><img src=\"/Content/images/contect_3_03.jpg\" alt=\"\"></div></div>";
            Introduction += "<div class=\"dotted\"></div><div class=\"row\" id=\"d\"><div class=\"small-12 medium-6 columns\"><img src=\"/Content/images/contact4_07.jpg\" alt=\"\">" +
                            "</div><div class=\"small-12 medium-6  columns\"><br class=\"hide-for-medium\"><h4 class=\"orange left insidetitle\">負載感應動力轉向程序（LSP）</h4>" +
                            "<p class=\"left\">它採用負荷傳感液壓系統動力轉向。通過提供根據後橋的負載來調節流向動力 轉向，減少了能量損失。</p><br>" +
                            "<h4 class=\"orange left insidetitle\">第二定子變矩器</h4><p class=\"left\">（4.5-8.0ton標準·3.5-4.0ton可選） 第二定子變矩器可以不順暢行駛換擋衝擊。它還可以防止因過熱無意2高速騰飛 的問題。</p></div> </div>";
            Introduction += "<div class=\"dotted\"></div><div class=\"row\" id=\"e\"><div class=\"small-12 medium-8 small-centered columns\">" +
                            "<br><h3 class=\"red\">TOYOTA原創的安全科技，提升您的工作效能</h3>" +
                            "<p class=\"fz18\">一脈傳承豐田的創新安全配備，如積極式穩定系統(SAS)及操作員離席偵測控制系統(OPS)，<span class=\"orange\">令新８系列(8FG/D 35N-80N)</span>即使在高頻度工作節奏下也能發揮高穩定性及高安全性</p>" +
                            "<br><img src=\"/Content/images/contact5_11.jpg\" alt=\"\"></div></div>";
            Introduction += "<div class=\"dotted\"></div><div class=\"row\" id=\"f\"><div class=\"small-12 medium-6  columns\">" +
                            "<h4 class=\"orange left insidetitle\">在以高揚程貨物裝卸時間過彎，出色的穩定性 <img src=\"/Content/images/product_content_7_15.jpg\" alt=\"\"></h4>" +
                            "<p class=\"left third\" style=\"\">※後輪搖擺鎖定控制</p><p class=\"left\">這樣，當凸起轉動和行李在高如，鎖定後軸鎖芯中，如果需要的話。 通過固定上下擺動後軸以確保車輛的橫向穩定性。" +
                            "<br> ※車前輪輪胎雙（6-8ton標準，3.5-5ton可選）<br>會不會是儀表後輪擺動鎖控制在不在。 </p>" +
                            "<p class=\"left third\" style=\"\">※FHPS旋鈕位置控制</p> <p class=\"left\">這樣，當凸起轉動和行李在高如，鎖定後軸鎖芯中，如果需要的話。 通過固定上下擺動後軸以確保車輛的橫向穩定性。" +
                            "<br> ※車前輪輪胎雙（6-8ton標準，3.5-5ton可選）<br>會不會是儀表後輪擺動鎖控制在不在。" +
                            "</p><br><img src=\"/Content/images/product_content_5_18.jpg\" alt=\"\">" +
                            "<br></div><div class=\"small-9 small-centered medium-6 columns\">" +
                            "<br><img src=\"/Content/images/product_content_5_15.jpg\" alt=\"\"></div></div>";
            Introduction += "<div class=\"dotted\"></div>" +
                            "<div class=\"row\" id=\"g\">" +
                                "<div class=\"small-12 medium-4 columns\">" +
                                    "<h4 class=\"orange left insidetitle\">特約秋季負荷防塌貨 <img src=\"/Content/images/product_content_7_15.jpg\" alt=\"\"></h4>" +
                                    "<br>" +
                                    "<p class=\"left\">當檢測在該操作者是不是在傳感器的正確的駕駛操作位置片的狀態下，停止由電力的行駛和貨物裝卸的操作。這將有助於防止事故，諸如由誤操作事故運行或“壓力”的。" +
                                    "</p>" +
                                    "<p class=\"remind\">※然而，在下車的時候，並不意味著制動器的應用</p>" +
                                "</div>" +
                                "<div class=\"small-12 medium-4 columns\">" +
                                    "<img src=\"/Content/images/product_content_6_23.jpg\" alt=\"\">" +
                                "</div>" +
                                "<div class=\"small-12 medium-4 columns\">" +
                                    "<h4 class=\"orange left insidetitle\">離鍵升降鎖功能 </h4>" +
                                    "<br>" +
                                    "<p class=\"left\">鍵斷時間將有助於防止由升降桿誤操作事故以免落入即使電梯向下運行。" +
                                    "</p>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"dotted\"></div>" +
                        "</div>";

            DetailDiesel += css;
            DetailDiesel += "<table class=\"sep_table\" id=\"sep_1\" style=\"background: #0a0a0a;\">" +
                           "<tr><th scope=\"col\">型式</th><th scope=\"col\">8FG35N 8FD35N</th><th scope=\"col\">8FG35N 8FD35N</th>" +
                           "<th scope=\"col\">8FG35N 8FD35N</th><th scope=\"col\">8FG35N 8FD35N</th> <th scope=\"col\">8FG35N 8FD35N</th>" +
                           "<th scope=\"col\">8FG35N 8FD35N</th><th scope=\"col\">8FG35N 8FD35N</th></tr><tr>" +
                           "<td>引擎型式</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td>" +
                           "</tr><tr><td>荷載能力 (kg)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td>" +
                           "</tr><tr><td>荷載中心 (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td>" +
                           "</tr><tr><td>全寬 A (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td>" +
                           "</tr><tr><td>最小迴轉半徑(外側) B (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td>" +
                           "</tr><tr><td>護頂架高度 C (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td>" +
                           "</tr><tr><td>全長(不含貨叉) D (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td>"+
                            "</tr></table>";

            DetailGas += css;
            DetailGas += "<table class=\"sep_table active\" id=\"sep_0\" style=\"background: #0a0a0a;\">" +
                        "<tr><th scope=\"col\">型式</th><th scope=\"col\">8FD35N 8FG35N </th>" +
                        "<th scope=\"col\">8FD35N 8FG35N </th><th scope=\"col\">8FD35N 8FG35N </th><th scope=\"col\">8FD35N 8FG35N </th><th scope=\"col\">8FD35N 8FG35N </th>" +
                        "<th scope=\"col\">8FD35N 8FG35N </th><th scope=\"col\">8FD35N 8FG35N </th></tr><tr><td>引擎型式</td>" +
                        "<td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td></tr>" +
                        "<tr><td>荷載能力 (kg)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td></tr>" +
                        "<tr><td>荷載中心 (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td></tr>" +
                        "<tr><td>全寬 A (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td></tr>" +
                        "<tr><td>最小迴轉半徑(外側) B (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td>" +
                        "<td>3500</td></tr><tr><td>護頂架高度 C (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td></tr>" +
                        "<tr><td>全長(不含貨叉) D (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td></tr></table>";

            DetailElectric += css;
            DetailElectric += "<table class=\"sep_table active\" id=\"sep_0\" style=\"background: #0a0a0a;\">" +
                        "<tr><th scope=\"col\">型式</th><th scope=\"col\">8FD35N 8FG35N </th>" +
                        "<th scope=\"col\">8FD35N 8FG35N </th><th scope=\"col\">8FD35N 8FG35N </th><th scope=\"col\">8FD35N 8FG35N </th><th scope=\"col\">8FD35N 8FG35N </th>" +
                        "<th scope=\"col\">8FD35N 8FG35N </th><th scope=\"col\">8FD35N 8FG35N </th></tr><tr><td>引擎型式</td>" +
                        "<td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td><td>14Z -II</td></tr>" +
                        "<tr><td>荷載能力 (kg)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td></tr>" +
                        "<tr><td>荷載中心 (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td></tr>" +
                        "<tr><td>全寬 A (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td></tr>" +
                        "<tr><td>最小迴轉半徑(外側) B (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td>" +
                        "<td>3500</td></tr><tr><td>護頂架高度 C (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td></tr>" +
                        "<tr><td>全長(不含貨叉) D (mm)</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td><td>3500</td></tr></table>";
            DetailEngineTable += css;
            DetailEngineTable += "<table width=\"\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"sep_table_2\" style=\"background: #0a0a0a;\">" +
                "<tr><th scope=\"col\" class=\"spec_title\">型式</th><th scope=\"col\"><h3>TOYOTA 4Y</h3> Gasoline"+
                    "</th><th scope=\"col\"><h3>TOYOTA 1DZII</h3> Diesel</th><th scope=\"col\"><h3>TOYOTA 2Z</h3> Diesel</th>"+
                "</tr><tr><td class=\"spec_title\">排氣量 (cc)</td><td>2237</td><td>2237</td><td>2237</td>"+
                "</tr><tr><td class=\"spec_title\">最大馬力 (PS/r.p.m)</td><td>54 / 2400 (58 / 2460)</td>"+
                "<td>54 / 2400 (58 / 2460)</td><td>54 / 2400 (58 / 2460)</td></tr>"+
                "<tr><td class=\"spec_title\">最大扭力 (N-m/r.p.m)</td><td>162 / 16400</td><td>162 / 16400</td><td>162 / 16400</td>"+
                "</tr></table>";
            DetailMotorBatteryTable += css;
            DetailMotorBatteryTable += "<table width=\"90%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"sep_table_2\" style=\"background: #0a0a0a;\">" +
                "<tr><th scope=\"col\" colspan=\"2\" class=\"spec_title\">機型</th><th scope=\"col\">"+
                "<h3>7FBR10.13</h3></th><th scope=\"col\"><h3>7FBR15.18</h3></th><th scope=\"col\">"+
                "<h3>7FBR20.25</h3></th></tr><tr><td class=\"spec_title\">電壓/容量<p class=\"spec_title_vic\">(5小時率)</p>"+
                "</td><td align=\"left\" style=\"text-align: left;\">標準 V/AH</td><td>48/201</td><td>48/280</td><td>48/280</td>"+
                "</tr><tr><td rowspan=\"3\" class=\"spec_title\">電動馬達 (cc)</td><td align=\"left\" style=\"text-align: left;\">行走/KW</td>"+
                "<td>4.9</td><td>4.9</td><td>5.2</td></tr><tr><td align=\"left\" style=\"text-align: left;\">舉升/KW</td>"+
                "<td>6.5</td><td>8.0</td><td>11.0</td></tr><tr><td align=\"left\" style=\"text-align: left;\">動力轉向/KW</td>"+
                "<td>0.26</td><td>0.26</td><td>0.35</td></tr></table>";
        }
}
}