using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Begonia.Toyota.WebSite.Enums;
using Begonia.Toyota.WebSite.Models;
using Begonia.Toyota.WebSite.Service;
using JamZoo.Project.WebSite.Enums;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Configuration;
using Begonia.Toyota.WebSite.Library;

namespace Begonia.Toyota.WebSite.Controllers
{
    public class WebSiteController : Controller
    {
        public FixedDataService htmlService = new FixedDataService();

        public string domain = ConfigurationManager.AppSettings["domain"];
        public string secretKey = ConfigurationManager.AppSettings["secretKey"];
        // GET: /WebSite/
        

        /// <summary>
        /// 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Home | 首頁";
            InfoNewsService infoNewsService = new InfoNewsService();
            IndexModel model = new IndexModel();
            model.NewsList = new InfoNewsListModel();
            model.NewsList.PageSize = 99;
            model.NewsList = infoNewsService.GetList("", model.NewsList, true);
            
            IndexBannerService indexBannerService = new IndexBannerService();
            model.BannerList = new IndexBannerListModel();
            model.BannerList.PageSize = 100;
            model.BannerList = indexBannerService.GetList("", model.BannerList, true);

            IndexVideoService indexVideosService = new IndexVideoService();
            model.Video = indexVideosService.Get("",0, true);
            if (model.Video == null) model.Video = new IndexVideoModel();
            return View(model);
        }

        /// <summary>
        /// 關於
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - About | 關於台灣豐田";
            AboutModel model = new AboutModel();
            InfoPrizeService prizeService = new InfoPrizeService();
            model.YearsList = prizeService.GetYears1("");
            string key = model.YearsList.FirstOrDefault();
            model.PrizeList = prizeService.GetList1(key, true).OrderBy(p=>p.Orderby).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult GetPrizeList(string yy)
        {
            InfoPrizeService prizeService = new InfoPrizeService();
            List<InfoPrizeModel> PrizeList = prizeService.GetList1(yy, true).OrderBy(p => p.Orderby).ToList();
            return Json(PrizeList);
        }

        /// <summary>
        /// 據點查詢
        /// </summary>
        /// <returns></returns>
        public ActionResult Branch()
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Branch | 據點查詢";
            InfoLocationService Service = new InfoLocationService();
            InfoLocationListModel list = new InfoLocationListModel();
            list.PageSize = 1000;
            list = Service.GetList("", list, true);

            foreach (var i in list.Data)
            {
                try
                {
                    if (!string.IsNullOrEmpty(i.Address))
                    {
                        var address = i.Address;
                        var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}", Uri.EscapeDataString(address));

                        var request = WebRequest.Create(requestUri);
                        var response = request.GetResponse();
                        var xdoc = XDocument.Load(response.GetResponseStream());

                        var result = xdoc.Element("GeocodeResponse").Element("result");
                        var locationElement = result.Element("geometry").Element("location");
                        var lat = locationElement.Element("lat");
                        var lng = locationElement.Element("lng");
                        i.SearchAddress = "http://maps.google.com/?q=" + lat.Value + "," + lng.Value;
                    }
                }
                catch (Exception e)
                {
                    i.SearchAddress = "http://maps.google.com";
                }
                
            }

            return View(list);
        }

        /// <summary>
        /// 最新消息列表
        /// </summary>
        /// <returns></returns>
        public ActionResult NewsList(string yy)
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - News | 最新消息";
            InfoNewsService Service = new InfoNewsService();
            InfoNewsListModel list = new InfoNewsListModel();
            list.yy = yy;
            list.PageSize = 100;
            list = Service.GetDicList("", list);
            return View(list);
        }


        public string RemoveHTMLTag(string htmlSource)
        {
            //移除  javascript code.
            htmlSource = Regex.Replace(htmlSource, @"<script[\d\D]*?>[\d\D]*?</script>", String.Empty);

            //移除html tag.
            htmlSource = Regex.Replace(htmlSource, @"<[^>]*>", String.Empty);
            htmlSource = htmlSource.Replace("&amp;", "");
            htmlSource = htmlSource.Replace("nbsp;", "");

            return htmlSource;
        }

        /// <summary>
        /// 最新消息內容頁
        /// </summary>
        /// <returns></returns>
        public ActionResult News(int nid)
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - News | 最新消息";

            InfoNewsService Service = new InfoNewsService();
            InfoNewsModel info = new InfoNewsModel();
            info = Service.Get1("", nid);
            TempData["ogtitle"] = info.Title;
            TempData["ogdescription"] = RemoveHTMLTag(info.Detail.Length >= 100 ? info.Detail.Substring(0, 100)+"..." : info.Detail);
            TempData["ogurl"] = domain + "WebSite/news?nid=" + info.Id;
            TempData["ogimage"] = domain + "File/Get?FileId=" + info.ListImg;
            return View(info);
        }

        /// <summary>
        /// 租賃服務
        /// </summary>
        /// <returns></returns>
        public ActionResult LeaseService()
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Lease Service | 租賃服務";
            var m = htmlService.Get("", FixedData.租賃服務.ToString());
            if (m == null)
            {
                m = new FixedDataModel();
            }
            else
            {
                if (string.IsNullOrEmpty(m.Html))
                {
                    m.Html = "<section class=\"vision banner-lease short\">" +
                             "<div class=\"text-box\">" +
                             "<h2><span class=\"red\">租賃</span> 服務</h2>" +
                             "<h2 class=\"eng\">Lease Service</h2>" +
                             "<p class=\"text description\">台灣豐田所提供之租賃服務常被認誤為是「長期的租賃」， 但其實並不正確。本公司提供的租賃服務，企業不需有資本支出，原廠設備品質保修服務絕對可信賴，並且針對企業不同需求做計量管理。租賃方案可因應企業未來不同需求彈性調整，使企業成本為可預期。</p>" +
                             "</div>" +
                             "</section>" +
                             "<section class=\"lease-01 lease-content\">" +
                             "<div class=\"row\">" +
                             "<div class=\"small-12 medium-5 large-5 columns\">" +
                             "<h2><span class=\"red\">租賃</span> 對企業有什麼好處?</h2>" +
                             "<h2 class=\"eng\">Benefits of Leasing</h2>" +
                             "<ul class=\"list-sq\">" +
                             "<li>避免持有的風險</li>" +
                             "<li>無需負擔資本支出</li>" +
                             "<li>使用上更有彈性</li>" +
                             "<li>原廠技師專業維護</li>" +
                             "<li>可更專注於本業</li>" +
                             "</ul>" +
                             "</div>" +
                             "<div class=\"small-12 medium-7 large-7 columns\">" +
                             "<img src = \"/content/images/lease/img_01@2x.png\" width=\"100%\" class=\"lease-img\" alt=\"\">" +
                             "</div>" +
                             "</div>" +
                             "</section>" +
                             "<section class=\"lease-02 lease-content\">" +
                             "<div class=\"row\">" +
                             "<div class=\"small-12 columns\">" +
                             "<div class=\"text-box\">" +
                             "<h2><span class=\"red\">租賃</span> 比購買更好!</h2>" +
                             "<h2 class=\"eng\">Leasing is Better Than Buying!</h2>" +
                             "<p>企業與銀行、融資公司之間，一般所簽訂的產業機械設備租賃契約（係指融資契約）彈性較小。</p>" +
                             "<p class=\"separated\"><img src = \"/content/images/lease/icon_dim@2x.png\" width=\"10\" alt=\"\"></p>" +
                             "<p>售後服務通常為兩種方式：</p>" +
                             "<ul class=\"way\">" +
                             "<li>1. 由外包第三方公司負責</li>" +
                             "<li>2. 由一般修理業者維護</li>" +
                             "</ul>" +
                             "<p>企業付款對象為銀行或是融資公司。</p>" +
                             "<p class=\"separated\"><img src = \"/content/images/lease/icon_dim@2x.png\" width=\"10\" alt=\"\"></p>" +
                             "<p>這類分割的服務契約方式，服務範圍可能：</p> " +
                             "<table class=\"table-line\">" +
                             "<tbody>" +
                             "<tr>" +
                             "<td>不涵蓋預期外的損壞</td>" +
                             "<td>維修項目不包含所有零件</td>" +
                             "</tr>" +
                             "<tr>" +
                             "<td>限制機器的工作時數</td>" +
                             "<td>其他的限制</td>" +
                             "</tr>" +
                             "<tr>" +
                             "<td>故障時間有其他的擔保</td>" +
                             "<td>彈性相對較低</td>" +
                             "</tr>" +
                             "</tbody>" +
                             "</table> " +
                             "</div>" +
                             "</div>" +
                             "</div>" +
                             "</section>" +
                             "<section class=\"lease-03 lease-content\">" +
                             "<div class=\"row\">" +
                             "<div class=\"small-12 medium-6 large-6 columns\">" +
                             "<h2><span class=\"red\">原廠</span> 租賃優勢！</h2>" +
                             "<h2 class=\"eng\">Original Leasing Advantage!</h2>" +
                             "<ol class=\"list-num\">" +
                             "<li>" +
                             "<span>提供完善的租賃服務：</span>" +
                             "<ul class=\"list-sq\">" +
                             "<li>無需固定資產管理</li>" +
                             "<li>費用支出更可控</li>" +
                             "</ul>" +
                             "</li>" +
                             "<li>" +
                             "<span>完善協助企業堆高機稼動數量管理：</span>" +
                             "<ul class=\"list-sq\">" +
                             "<li>消除作業量變動帶來的設備利用率不平均</li>" +
                             "<li>降低需求變動導致設備閒置</li>" +
                             "</ul>" +
                             "</li>" +
                             "<li><span>台灣豐田租賃服務可具備應變未來的靈活性，以及可預見所有成本。</span></li>" +
                             "<li><span>企業資金專注於核心業務，提升獲利水平。</span></li>" +
                             "<li><span>擴大企業業務規模，提高企業競爭力。</span></li>" +
                             "</ol>" +
                             "</div>" +
                             "<div class=\"small-12 medium-6 large-6 columns\">" +
                             "<img src = \"/content/images/lease/img_02@2x.png\" alt=\"\" class=\"\">" +
                             "</div>" +
                             "</div>" +
                             "</section>";
                }
            }
            return View(m);
        }

        /// <summary>
        /// 售後服務
        /// </summary>
        /// <returns></returns>
        public ActionResult Warranty()
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Warranty Service | 售後服務";
            var m = htmlService.Get("", FixedData.售後服務.ToString());
            if (m == null)
            {
                m = new FixedDataModel();
            }
            else
            {
                if (string.IsNullOrEmpty(m.Html))
                {
                    m.Html = "<section class=\"vision banner-warranty short\">" +
                             "<div class=\"text-box\">" +
                             "<h2><span class=\"red\">售後</span> 服務</h2>" +
                             "<h2 class=\"eng\">Warranty Service</h2>" +
                             "<p class=\"text description\">TOYOTA為業界最可靠、保障之品牌，優於勞動部職業安全衛生法規定，本公司所有車型皆取得「國家核可的型式證書」及「線上登錄」，並以友善環境精神，遵守國際環境標準規範，每年通過ISO14001國際環境管理系統認證。</p>" +
                             "</div>" +
                             "</section>" +
                             "<section class=\"page-content warranty-content\">" +
                             "<div class=\"row\">" +
                             "<div class=\"small-12 columns\">" +
                             "<div class=\"warranty-text\">" +
                             "<h2><span class=\"red\">原廠</span> 保固</h2>" +
                             "<h2 class=\"eng\">Original Warrantya</h2>" +
                             "<p class=\"text description\"> 提供業界最優的保固條件，凡新車自交車日起，提供12個月或 2,000工作小時（以先到者為準）保固。保固範圍內所引起故障（消耗品及人為因素造成除外），經由本公司認定後，即提供免費修理服務。</p>" +
                             "</div>" +
                             "</div>" +
                             "<div class=\"small-12 columns\">" +
                             "<div class=\"warranty-text last\">" +
                             "<h2><span class=\"red\">原廠</span> 零件</h2>" +
                             "<h2 class=\"eng\">Original Parts</h2>" +
                             "<p class=\"text description\">本公司為日本豐田在台灣唯一授權總代理，堅持使用品質最佳、可靠之原廠零件，讓您的堆高機發揮最大的工作價值及確保操作安全。</p>" +
                             "<p class=\"text description\">業界唯一遍佈全台的零件倉庫， 備有完整的零件庫存及每日配送。今日下單，明日即可到府服務；若有緊急需求亦可向日本進行緊急訂貨（空運） ，最快可在7日內取得零件，有效縮短您的等待時間。</p>" +
                             "</div>" +
                             "</div>" +
                             "<img src = \"/content/images/warranty/warranty_01@2x.png\" class=\"warranty-img\" alt=\"\">" +
                             "</div>" +
                             "<br>" +
                             "<div class=\"row\">" +
                             "<div class=\"small-12 columns\">" +
                             "<div class=\"warranty-text last full\">" +
                             "<h2><span class=\"red\">專業</span> 技術</h2>" +
                             "<h2 class=\"eng\">Professional Technique</h2>" +
                             "<p class=\"text description\"> 本公司依循日本豐田原廠訂定之標準作業流程，以提升服務品質，保障堆高機操作及行駛安全，讓每位客戶享有最完善的保養、維修服務。服務技師皆取得日本豐田原廠的「技術認證證書」及「國內堆高機駕照」，每年接受8小時以上的回廠技術教育訓練。每位服務人員皆配有專屬巡迴服務車， 只要一通電話預約，即可為您提供專業即時的售後服務及堆高機操作安全指導。</p>" +
                             "</div>" +
                             "</div>" +
                             "<div class=\"small-12 columns\">" +
                             "<img src = \"/content/images/warranty/warranty_02@2x.png\" width=\"100%\" class=\"warranty-img-full\" alt=\"\">" +
                             "</div>" +
                             "</div>" +
                             "</section>";
                }
            }
            return View(m);
        }

        /// <summary>
        /// 現有職缺
        /// </summary>
        /// <returns></returns>
        public ActionResult NowCareer()
        {
            var m = htmlService.Get2("", FixedData2.現有職缺設定.ToString());
            if (m == null)
            { 
                m = new FixedDataModel();
                m.HumanBankUrl = "https://www.104.com.tw/";
            }

            return Redirect(m.HumanBankUrl);
        }

        /// <summary>
        /// 職涯規劃
        /// </summary>
        /// <returns></returns>
        public ActionResult Career()
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Career Planning | 職涯規劃";

            var m = htmlService.Get("", FixedData.職涯規劃管理.ToString());
            var m2 = htmlService.Get2("", FixedData2.現有職缺設定.ToString());
            string hkurl = "";
            if (m2 != null)
            {
                if (!string.IsNullOrEmpty(m2.HumanBankUrl))
                {
                    hkurl = m2.HumanBankUrl;
                }
                else
                {
                    hkurl = "https://www.104.com.tw/";
                }
            }
            else
            {
                hkurl = "https://www.104.com.tw/";
            }

            if (m == null)
            {
                m = new FixedDataModel();
            }
            else
            {
                if (string.IsNullOrEmpty(m.Html))
                {
                    m.Html = "<section class=\"vision banner-career short\">" +
                             "<div class=\"text-box\">" +
                             "<h2><span class=\"red\">職涯</span> 規劃</h2>" +
                             "<h2 class=\"eng\">Career Planning</h2>" +
                             "<p class=\"text description\">台灣豐田長期以來透過公開管道，不斷招募相關領域的專業人才，秉持「適才適所」與「人盡其才」的理念，提供員工職涯發展機會、工作生活平衡，及提升能力，促進員工持續留任，建立互信、互愛、互助之團結精神以謀公司之業務發展。以用心經營事業的態度幫助員工在各自的工作崗位上，貢獻心力。</p>" +
                             "</div>" +
                             "</section>" +
                             "<section class=\"page-content\">" +
                             "<div class=\"row\">" +
                             "<div class=\"small-12 columns\">" +
                             "<ul class=\"welfare-list\">" +
                             "<li><div class=\"img\"><img src = \"/content/images/career/img_01@2x.png\" alt=\"\"></div><div class=\"text-box\"><h2>業務類</h2><h2 class=\"eng\">Business</h2><p class=\"text description\">身為開發新客戶的一線人員，公司定期舉辦產品知識課程，藉由了解產品之後，更能夠替客戶推薦最適合客戶的產品，解決客戶的問題，進而將本公司的產品推向各地。</p></div></li>" +
                             "<li><div class=\"img\"><img src = \"/content/images/career/img_02@2x.png\" alt=\"\"></div><div class=\"text-box\"><h2>修護類</h2><h2 class=\"eng\">Repair</h2><p class=\"text description\">以基層修護經驗為發展之重要基礎，持續學習，是台灣豐田協助員工發展的基石。我們重視員工的職能與潛力發展，並且不定期舉辦技術檢定考試、產品說明會等教育訓練課程，達到強化員工職能之目標。</p></div></li>" +
                             "<li><div class=\"img\"><img src = \"/content/images/career/img_03@2x.png\" alt=\"\"></div><div class=\"text-box\"><h2>內勤類</h2><h2 class=\"eng\">Internal</h2><p class=\"text description\">考量個人工作潛能、專長與意願，提供完善之教育訓練課程，配合必要的部門輪調，累積多方面實務經驗，並鼓勵員工進修與學習。公司另會不定期安排教育訓練課程，或替員工報名與其他關係企業一同受訓、進修，例如：PDCA、Toyota Way、TBP、進階excel課程等，促進同仁持續學習，以期未來應用於公司業務上。</p></div></li>" +
                             "</ul>" +
                             "<div class=\"text-center\"><a href = \""+ hkurl + "\" target=\"_blank\" class=\"more_box animall\">瀏覽我們的職缺</a></div>" +
                             "</div>" +
                             "</div>" +
                             "</section>";
                }
            }
            return View(m);
        }

        /// <summary>
        /// 公司福利
        /// </summary>
        /// <returns></returns>
        public ActionResult Welfare()
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Welfare System | 公司福利";
            var m = htmlService.Get("", FixedData.公司福利管理.ToString());
            var m2 = htmlService.Get2("", FixedData2.現有職缺設定.ToString());
            string hkurl = "";
            if (m2 != null)
            {
                if (!string.IsNullOrEmpty(m2.HumanBankUrl))
                {
                    hkurl = m2.HumanBankUrl;
                }
                else
                {
                    hkurl = "https://www.104.com.tw/";
                }
            }
            else
            {
                hkurl = "https://www.104.com.tw/";
            }

            if (m == null)
            {
                m = new FixedDataModel();
            }
            else
            {
                if (string.IsNullOrEmpty(m.Html))
                {
                    m.Html = "<section class=\"vision banner-welfare short\">" +
                             "<div class=\"text-box\">" +
                             "<h2><span class=\"red\">公司</span> 福利</h2>" +
                             "<h2 class=\"eng\">Welfare System</h2>" +
                             "<p class=\"text description\">員工是公司最重要的資產，尊重員工、創造和諧的工作氣氛、打造安全衛生的工作環境、關心員工與家庭間的情感聯繫，是我們努力的方向。在這裡，每一位員工在自己的工作崗位上認真工作、努力打拼，為公司的發展盡最大的努力。我們歡迎你，成為台灣豐田這個大家庭的一員。</p>" +
                             "</div>" +
                             "</section>" +
                             "<section class=\"page-content\">" +
                             "<div class=\"row\">" +
                             "<div class=\"small-12 columns\">" +
                             "<ul class=\"welfare-list\">" +
                             "<li><div class=\"img\"><img src = \"/content/images/welfare/img_01@2x.png\" alt=\"\"></div><div class=\"text-box\"><h2>工作健康與安全</h2><h2 class=\"eng\">Health and Safety</h2><p class=\"text description\">安全不僅是工作的基本條件，亦是我們不斷努力的目標。公司除替員工投保勞健保以外，每年依照需求安排員工參加健康檢查。在工作職場環境方面，不僅依照相關法令建置安全舒適的工作環境，更落實防災與演練，定期舉辦消防講座。</p></div></li>" +
                             "<li><div class=\"img\"><img src = \"/content/images/welfare/img_02@2x.png\" alt=\"\"></div><div class=\"text-box\"><h2>休閒活動</h2><h2 class=\"eng\">Leisure Activities</h2><p class=\"text description\">為了增進員工及其眷屬對本公司之了解與向心力，以及強化員工家庭間交流，我們會不定期舉辦各項活動，例如：慶生會、家庭日、年終尾牙等共好活動，拉近每位成員間的距離，一同齊心合作。</p></div></li>" +
                             "</ul>" +
                             "<div class=\"text-center\"><a href = \""+ hkurl + "\" target=\"_blank\" class=\"more_box animall\">瀏覽我們的職缺</a></div>" +
                             "</div>" +
                             "</div> " +
                             "</section>";
                }
            }

            return View(m);
        }

        #region ContactUs

        public ActionResult reCAPTCHATest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult reCAPTCHATestPOST()
        {
            string msg = "";
            if (Request.Form["captchaToken"] == "")
            {
                msg = "請確認是否為機器人";
            }
            else
            {
                // 建立一個HttpWebRequest網址指向Google的驗證API
                var req = (HttpWebRequest)HttpWebRequest.Create("https://www.google.com/recaptcha/api/siteverify");
                // Post的資料
                // secret:secret_key
                // response:回傳的Token
                // remoteip:設定的Domain Name

                string posStr = "secret=" + secretKey + "&response=" + Request.Form["captchaToken"] + "&remoteip=" + Request.Url.Host;
                byte[] byteStr = Encoding.UTF8.GetBytes(posStr);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                // 把要Post資料寫進HttpWebRequest
                using (Stream streamArr = req.GetRequestStream())
                {
                    streamArr.Write(byteStr, 0, byteStr.Length);
                }
                // 取得回傳資料
                using (var res = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader getJson = new StreamReader(res.GetResponseStream()))
                    {
                        string json = getJson.ReadToEnd();
                        msg = json;
                    }
                }
            }
            if (msg.Contains("success") && msg.Contains("true"))
            {
                return RedirectToAction("ContactUs");
            }
            else
            {
                return Content(msg);
            }
        }


        /// <summary>
        /// 聯絡我們
        /// </summary>
        /// <returns></returns>
        public ActionResult ContactUs()
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Contact Us | 聯絡我們";

            return View();
        }

        [HttpPost]
        public ActionResult ContactUsCreate(ContactUsManagerModel1 model1)
        {
            string msg = "";
            if (Request.Form["captchaToken"] == "")
            {
                msg = "請確認是否為機器人";
            }
            else
            {
                // 建立一個HttpWebRequest網址指向Google的驗證API
                var req = (HttpWebRequest)HttpWebRequest.Create("https://www.google.com/recaptcha/api/siteverify");
                // Post的資料
                // secret:secret_key
                // response:回傳的Token
                // remoteip:設定的Domain Name
                string posStr = "secret=" + secretKey + "&response=" + Request.Form["captchaToken"] + "&remoteip=" + Request.Url.Host;
                byte[] byteStr = Encoding.UTF8.GetBytes(posStr);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                // 把要Post資料寫進HttpWebRequest
                using (Stream streamArr = req.GetRequestStream())
                {
                    streamArr.Write(byteStr, 0, byteStr.Length);
                }
                // 取得回傳資料
                using (var res = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader getJson = new StreamReader(res.GetResponseStream()))
                    {
                        string json = getJson.ReadToEnd();
                        msg = json;
                    }
                }
            }

            if (msg.Contains("success") && msg.Contains("true"))
            {
                ContactUsManagerService Service = new ContactUsManagerService();
                if (Service.Create1(model1))
                {
                    TempData["msg"] = "送出成功";
                    string body = "姓名: " + model1.Namea + "<br />";
                    body += "服務單位: " + model1.ServiceUnit + "<br />";
                    body += "Email: " + model1.Email + "<br />";
                    body += "聯絡電話: " + model1.Tel + "<br />";
                    body += "問題描述: " + model1.Detail;
                   
                    TempData["msg"] = "送出成功";
                    var m = htmlService.Get2("", FixedData2.聯絡我們信箱.ToString());
                    if (!string.IsNullOrEmpty(m.MailBoxReceiver))
                    {
                        model1.Email = model1.Email + "," + m.MailBoxReceiver;
                    }
                    Utils.SendMailByGmail(model1.Email, m.MailBoxSender,"台灣豐田產業機械 聯絡我們", body);
                    return RedirectToAction("ContactUs");
                }
            }
            else
            {
                TempData["msg"] = "此表單防止機器人傳送";
                return RedirectToAction("ContactUs");
            }
            return null;
        }

        #endregion

        #region RequestCatalogs

        /// <summary>
        /// 索取型錄
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestCatalogs()
        {

            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Contact Us | 型錄索取";

            ProductInfoService productInfoService = new ProductInfoService();
            ContactCatalogManagerModel1 model1 = new ContactCatalogManagerModel1();
            model1.TypeList = Enum.GetValues(typeof(ProductType)).Cast<ProductType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            if (model1.TypeList.Any())
            {
                model1.NameList = productInfoService.GetList1(model1.TypeList.FirstOrDefault().Value);
            }

            model1.CityList = GetTWCityList();
            if (model1.CityList.Any())
            {
                model1.AreaList = GetTWAreaList(model1.CityList.FirstOrDefault());
            }
            return View(model1);
        }

        [HttpPost]
        public ActionResult GetAreaByKey(string key)
        {
            List<string> objcity = GetTWAreaList(key);
            return Json(objcity);
        }

        [HttpPost]
        public ActionResult GetProductByKey(string key)
        {
            ProductInfoService productInfoService = new ProductInfoService();
            var st = (ProductType)Enum.Parse(typeof(ProductType), key, true);
            int kk = (int)st;
            List<string> objcity = productInfoService.GetList1(kk.ToString());
            return Json(objcity);
        }

        [HttpPost]
        public ActionResult RequestCatalogsCreate(ContactCatalogManagerModel1 model1)
        {
            string msg = "";
            if (Request.Form["captchaToken"] == "")
            {
                msg = "請確認是否為機器人";
            }
            else
            {
                // 建立一個HttpWebRequest網址指向Google的驗證API
                var req = (HttpWebRequest)HttpWebRequest.Create("https://www.google.com/recaptcha/api/siteverify");
                // Post的資料
                // secret:secret_key
                // response:回傳的Token
                // remoteip:設定的Domain Name
                string posStr = "secret=" + secretKey + "&response=" + Request.Form["captchaToken"] + "&remoteip=" + Request.Url.Host;
                byte[] byteStr = Encoding.UTF8.GetBytes(posStr);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                // 把要Post資料寫進HttpWebRequest
                using (Stream streamArr = req.GetRequestStream())
                {
                    streamArr.Write(byteStr, 0, byteStr.Length);
                }
                // 取得回傳資料
                using (var res = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader getJson = new StreamReader(res.GetResponseStream()))
                    {
                        string json = getJson.ReadToEnd();
                        msg = json;
                    }
                }
            }
            if (msg.Contains("success") && msg.Contains("true"))
            {
                ContactCatalogManagerService Service = new ContactCatalogManagerService();
                if (Service.Create1(model1))
                {
                    string body = "姓名: " + model1.Namea + "<br />";
                    body += "服務單位: " + model1.ServiceUnit + "<br />";
                    body += "Email: " + model1.Email + "<br />";
                    body += "聯絡電話: " + model1.Tel + "<br />";
                    body += "地址: " + model1.selectCity + model1.selectArea + model1.Address +"<br />";
                    if (model1.ck!=null)
                    {
                        if (model1.ck.Any())
                            body += "產車品牌: " + string.Join(",", model1.ck) + "<br />";
                    }
                    body += "型錄索取: " + model1.selectPt + "/" + model1.selectName + "<br />";
                    body += "備註: " + model1.ReMarks + "<br />";
                    TempData["msg"] = "送出成功";
                    var m = htmlService.Get2("", FixedData2.索取型錄信箱.ToString());
                    if (!string.IsNullOrEmpty(m.MailBoxReceiver))
                    {
                        model1.Email = model1.Email + "," + m.MailBoxReceiver;
                    }
                    Utils.SendMailByGmail(model1.Email, m.MailBoxSender, "台灣豐田產業機械 索取型錄", body);
                    return RedirectToAction("RequestCatalogs");
                }
            }
            else
            {
                TempData["msg"] = "此表單防止機器人傳送";
                return RedirectToAction("RequestCatalogs");
            }
            return null;
        }

        #region TW

        private string twString = "{\"taiwan\":[{\"city\":\"臺北市\",\"area\":[{\"zip\":\"100\",\"text\":\"中正區\"},{\"zip\":\"103\",\"text\":\"大同區\"},{\"zip\":\"104\",\"text\":\"中山區\"},{\"zip\":\"105\",\"text\":\"松山區\"},{\"zip\":\"106\",\"text\":\"大安區\"},{\"zip\":\"108\",\"text\":\"萬華區\"},{\"zip\":\"110\",\"text\":\"信義區\"},{\"zip\":\"111\",\"text\":\"士林區\"},{\"zip\":\"112\",\"text\":\"北投區\"},{\"zip\":\"114\",\"text\":\"內湖區\"},{\"zip\":\"115\",\"text\":\"南港區\"},{\"zip\":\"116\",\"text\":\"文山區\"}]},{\"city\":\"新北市\",\"area\":[{\"zip\":\"207\",\"text\":\"萬里區\"},{\"zip\":\"208\",\"text\":\"金山區\"},{\"zip\":\"220\",\"text\":\"板橋區\"},{\"zip\":\"221\",\"text\":\"汐止區\"},{\"zip\":\"222\",\"text\":\"深坑區\"},{\"zip\":\"223\",\"text\":\"石碇區\"},{\"zip\":\"224\",\"text\":\"瑞芳區\"},{\"zip\":\"226\",\"text\":\"平溪區\"},{\"zip\":\"227\",\"text\":\"雙溪區\"},{\"zip\":\"228\",\"text\":\"貢寮區\"},{\"zip\":\"231\",\"text\":\"新店區\"},{\"zip\":\"232\",\"text\":\"坪林區\"},{\"zip\":\"233\",\"text\":\"烏來區\"},{\"zip\":\"234\",\"text\":\"永和區\"},{\"zip\":\"235\",\"text\":\"中和區\"},{\"zip\":\"236\",\"text\":\"土城區\"},{\"zip\":\"237\",\"text\":\"三峽區\"},{\"zip\":\"238\",\"text\":\"樹林區\"},{\"zip\":\"239\",\"text\":\"鶯歌區\"},{\"zip\":\"241\",\"text\":\"三重區\"},{\"zip\":\"242\",\"text\":\"新莊區\"},{\"zip\":\"243\",\"text\":\"泰山區\"},{\"zip\":\"244\",\"text\":\"林口區\"},{\"zip\":\"247\",\"text\":\"蘆洲區\"},{\"zip\":\"248\",\"text\":\"五股區\"},{\"zip\":\"249\",\"text\":\"八里區\"},{\"zip\":\"251\",\"text\":\"淡水區\"},{\"zip\":\"252\",\"text\":\"三芝區\"}]},{\"city\":\"基隆市\",\"area\":[{\"zip\":\"200\",\"text\":\"仁愛區\"},{\"zip\":\"201\",\"text\":\"信義區\"},{\"zip\":\"202\",\"text\":\"中正區\"},{\"zip\":\"203\",\"text\":\"中山區\"},{\"zip\":\"204\",\"text\":\"安樂區\"},{\"zip\":\"205\",\"text\":\"暖暖區\"},{\"zip\":\"206\",\"text\":\"七堵區\"}]},{\"city\":\"宜蘭縣\",\"area\":[{\"zip\":\"260\",\"text\":\"宜蘭市\"},{\"zip\":\"261\",\"text\":\"頭城鎮\"},{\"zip\":\"262\",\"text\":\"礁溪鄉\"},{\"zip\":\"263\",\"text\":\"壯圍鄉\"},{\"zip\":\"264\",\"text\":\"員山鄉\"},{\"zip\":\"265\",\"text\":\"羅東鎮\"},{\"zip\":\"266\",\"text\":\"三星鄉\"},{\"zip\":\"267\",\"text\":\"大同鄉\"},{\"zip\":\"268\",\"text\":\"五結鄉\"},{\"zip\":\"269\",\"text\":\"冬山鄉\"},{\"zip\":\"270\",\"text\":\"蘇澳鎮\"},{\"zip\":\"272\",\"text\":\"南澳鄉\"}]},{\"city\":\"新竹市\",\"area\":[{\"zip\":\"300\",\"text\":\"東區\"},{\"zip\":\"300\",\"text\":\"北區\"},{\"zip\":\"300\",\"text\":\"香山區\"}]},{\"city\":\"新竹縣\",\"area\":[{\"zip\":\"302\",\"text\":\"竹北市\"},{\"zip\":\"303\",\"text\":\"湖口鄉\"},{\"zip\":\"304\",\"text\":\"新豐鄉\"},{\"zip\":\"305\",\"text\":\"新埔鎮\"},{\"zip\":\"306\",\"text\":\"關西鎮\"},{\"zip\":\"307\",\"text\":\"芎林鄉\"},{\"zip\":\"308\",\"text\":\"寶山鄉\"},{\"zip\":\"310\",\"text\":\"竹東鎮\"},{\"zip\":\"311\",\"text\":\"五峰鄉\"},{\"zip\":\"312\",\"text\":\"橫山鄉\"},{\"zip\":\"313\",\"text\":\"尖石鄉\"},{\"zip\":\"314\",\"text\":\"北埔鄉\"},{\"zip\":\"315\",\"text\":\"峨眉鄉\"}]},{\"city\":\"桃園縣\",\"area\":[{\"zip\":\"320\",\"text\":\"中壢市\"},{\"zip\":\"324\",\"text\":\"平鎮市\"},{\"zip\":\"325\",\"text\":\"龍潭鄉\"},{\"zip\":\"326\",\"text\":\"楊梅鎮\"},{\"zip\":\"327\",\"text\":\"新屋鄉\"},{\"zip\":\"328\",\"text\":\"觀音鄉\"},{\"zip\":\"330\",\"text\":\"桃園市\"},{\"zip\":\"333\",\"text\":\"龜山鄉\"},{\"zip\":\"334\",\"text\":\"八德市\"},{\"zip\":\"335\",\"text\":\"大溪鎮\"},{\"zip\":\"336\",\"text\":\"復興鄉\"},{\"zip\":\"337\",\"text\":\"大園鄉\"},{\"zip\":\"338\",\"text\":\"蘆竹鄉\"}]},{\"city\":\"苗栗縣\",\"area\":[{\"zip\":\"350\",\"text\":\"竹南鎮\"},{\"zip\":\"351\",\"text\":\"頭份鎮\"},{\"zip\":\"352\",\"text\":\"三灣鄉\"},{\"zip\":\"353\",\"text\":\"南莊鄉\"},{\"zip\":\"354\",\"text\":\"獅潭鄉\"},{\"zip\":\"356\",\"text\":\"後龍鎮\"},{\"zip\":\"357\",\"text\":\"通霄鎮\"},{\"zip\":\"358\",\"text\":\"苑裡鎮\"},{\"zip\":\"360\",\"text\":\"苗栗市\"},{\"zip\":\"361\",\"text\":\"造橋鄉\"},{\"zip\":\"362\",\"text\":\"頭屋鄉\"},{\"zip\":\"363\",\"text\":\"公館鄉\"},{\"zip\":\"364\",\"text\":\"大湖鄉\"},{\"zip\":\"365\",\"text\":\"泰安鄉\"},{\"zip\":\"366\",\"text\":\"銅鑼鄉\"},{\"zip\":\"367\",\"text\":\"三義鄉\"},{\"zip\":\"368\",\"text\":\"西湖鄉\"},{\"zip\":\"369\",\"text\":\"卓蘭鎮\"}]},{\"city\":\"臺中市\",\"area\":[{\"zip\":\"400\",\"text\":\"中區\"},{\"zip\":\"401\",\"text\":\"東區\"},{\"zip\":\"402\",\"text\":\"南區\"},{\"zip\":\"403\",\"text\":\"西區\"},{\"zip\":\"404\",\"text\":\"北區\"},{\"zip\":\"406\",\"text\":\"北屯區\"},{\"zip\":\"407\",\"text\":\"西屯區\"},{\"zip\":\"408\",\"text\":\"南屯區\"},{\"zip\":\"411\",\"text\":\"太平區\"},{\"zip\":\"412\",\"text\":\"大裡區\"},{\"zip\":\"413\",\"text\":\"霧峰區\"},{\"zip\":\"414\",\"text\":\"烏日區\"},{\"zip\":\"420\",\"text\":\"豐原區\"},{\"zip\":\"421\",\"text\":\"後裡區\"},{\"zip\":\"422\",\"text\":\"石岡區\"},{\"zip\":\"423\",\"text\":\"東勢區\"},{\"zip\":\"424\",\"text\":\"和平區\"},{\"zip\":\"426\",\"text\":\"新社區\"},{\"zip\":\"427\",\"text\":\"潭子區\"},{\"zip\":\"428\",\"text\":\"大雅區\"},{\"zip\":\"429\",\"text\":\"神岡區\"},{\"zip\":\"432\",\"text\":\"大肚區\"},{\"zip\":\"433\",\"text\":\"沙鹿區\"},{\"zip\":\"434\",\"text\":\"龍井區\"},{\"zip\":\"435\",\"text\":\"梧棲區\"},{\"zip\":\"436\",\"text\":\"清水區\"},{\"zip\":\"437\",\"text\":\"大甲區\"},{\"zip\":\"438\",\"text\":\"外埔區\"},{\"zip\":\"439\",\"text\":\"大安區\"}]},{\"city\":\"彰化縣\",\"area\":[{\"zip\":\"500\",\"text\":\"彰化市\"},{\"zip\":\"502\",\"text\":\"芬園鄉\"},{\"zip\":\"503\",\"text\":\"花壇鄉\"},{\"zip\":\"504\",\"text\":\"秀水鄉\"},{\"zip\":\"505\",\"text\":\"鹿港鎮\"},{\"zip\":\"506\",\"text\":\"福興鄉\"},{\"zip\":\"507\",\"text\":\"線西鄉\"},{\"zip\":\"508\",\"text\":\"和美鎮\"},{\"zip\":\"509\",\"text\":\"伸港鄉\"},{\"zip\":\"510\",\"text\":\"員林鎮\"},{\"zip\":\"511\",\"text\":\"社頭鄉\"},{\"zip\":\"512\",\"text\":\"永靖鄉\"},{\"zip\":\"513\",\"text\":\"埔心鄉\"},{\"zip\":\"514\",\"text\":\"溪湖鎮\"},{\"zip\":\"515\",\"text\":\"大村鄉\"},{\"zip\":\"516\",\"text\":\"埔鹽鄉\"},{\"zip\":\"520\",\"text\":\"田中鎮\"},{\"zip\":\"521\",\"text\":\"北斗鎮\"},{\"zip\":\"522\",\"text\":\"田尾鄉\"},{\"zip\":\"523\",\"text\":\"埤頭鄉\"},{\"zip\":\"524\",\"text\":\"溪州鄉\"},{\"zip\":\"525\",\"text\":\"竹塘鄉\"},{\"zip\":\"526\",\"text\":\"二林鎮\"},{\"zip\":\"527\",\"text\":\"大城鄉\"},{\"zip\":\"528\",\"text\":\"芳苑鄉\"},{\"zip\":\"530\",\"text\":\"二水鄉\"}]},{\"city\":\"南投縣\",\"area\":[{\"zip\":\"540\",\"text\":\"南投市\"},{\"zip\":\"541\",\"text\":\"中寮鄉\"},{\"zip\":\"542\",\"text\":\"草屯鎮\"},{\"zip\":\"544\",\"text\":\"國姓鄉\"},{\"zip\":\"545\",\"text\":\"埔里鎮\"},{\"zip\":\"546\",\"text\":\"仁愛鄉\"},{\"zip\":\"551\",\"text\":\"名間鄉\"},{\"zip\":\"552\",\"text\":\"集集鎮\"},{\"zip\":\"553\",\"text\":\"水裡鄉\"},{\"zip\":\"555\",\"text\":\"魚池鄉\"},{\"zip\":\"556\",\"text\":\"信義鄉\"},{\"zip\":\"557\",\"text\":\"竹山鎮\"},{\"zip\":\"558\",\"text\":\"鹿谷鄉\"}]},{\"city\":\"嘉義市\",\"area\":[{\"zip\":\"600\",\"text\":\"東區\"},{\"zip\":\"600\",\"text\":\"西區\"}]},{\"city\":\"嘉義縣\",\"area\":[{\"zip\":\"602\",\"text\":\"番路鄉\"},{\"zip\":\"603\",\"text\":\"梅山鄉\"},{\"zip\":\"604\",\"text\":\"竹崎鄉\"},{\"zip\":\"605\",\"text\":\"阿里山\"},{\"zip\":\"606\",\"text\":\"中埔鄉\"},{\"zip\":\"607\",\"text\":\"大埔鄉\"},{\"zip\":\"608\",\"text\":\"水上鄉\"},{\"zip\":\"611\",\"text\":\"鹿草鄉\"},{\"zip\":\"612\",\"text\":\"太保市\"},{\"zip\":\"613\",\"text\":\"朴子市\"},{\"zip\":\"614\",\"text\":\"東石鄉\"},{\"zip\":\"615\",\"text\":\"六腳鄉\"},{\"zip\":\"616\",\"text\":\"新港鄉\"},{\"zip\":\"621\",\"text\":\"民雄鄉\"},{\"zip\":\"622\",\"text\":\"大林鎮\"},{\"zip\":\"623\",\"text\":\"溪口鄉\"},{\"zip\":\"624\",\"text\":\"義竹鄉\"},{\"zip\":\"625\",\"text\":\"布袋鎮\"}]},{\"city\":\"雲林縣\",\"area\":[{\"zip\":\"630\",\"text\":\"斗南鎮\"},{\"zip\":\"631\",\"text\":\"大埤鄉\"},{\"zip\":\"632\",\"text\":\"虎尾鎮\"},{\"zip\":\"633\",\"text\":\"土庫鎮\"},{\"zip\":\"634\",\"text\":\"褒忠鄉\"},{\"zip\":\"635\",\"text\":\"東勢鄉\"},{\"zip\":\"636\",\"text\":\"台西鄉\"},{\"zip\":\"637\",\"text\":\"崙背鄉\"},{\"zip\":\"638\",\"text\":\"麥寮鄉\"},{\"zip\":\"640\",\"text\":\"斗六市\"},{\"zip\":\"643\",\"text\":\"林內鄉\"},{\"zip\":\"646\",\"text\":\"古坑鄉\"},{\"zip\":\"647\",\"text\":\"莿桐鄉\"},{\"zip\":\"648\",\"text\":\"西螺鎮\"},{\"zip\":\"649\",\"text\":\"二崙鄉\"},{\"zip\":\"651\",\"text\":\"北港鎮\"},{\"zip\":\"652\",\"text\":\"水林鄉\"},{\"zip\":\"653\",\"text\":\"口湖鄉\"},{\"zip\":\"654\",\"text\":\"四湖鄉\"},{\"zip\":\"655\",\"text\":\"元長鄉\"}]},{\"city\":\"臺南市\",\"area\":[{\"zip\":\"700\",\"text\":\"中西區\"},{\"zip\":\"701\",\"text\":\"東區\"},{\"zip\":\"702\",\"text\":\"南區\"},{\"zip\":\"704\",\"text\":\"北區\"},{\"zip\":\"708\",\"text\":\"安平區\"},{\"zip\":\"709\",\"text\":\"安南區\"},{\"zip\":\"710\",\"text\":\"永康區\"},{\"zip\":\"711\",\"text\":\"歸仁區\"},{\"zip\":\"712\",\"text\":\"新化區\"},{\"zip\":\"713\",\"text\":\"左鎮區\"},{\"zip\":\"714\",\"text\":\"玉井區\"},{\"zip\":\"715\",\"text\":\"楠西區\"},{\"zip\":\"716\",\"text\":\"南化區\"},{\"zip\":\"717\",\"text\":\"仁德區\"},{\"zip\":\"718\",\"text\":\"關廟區\"},{\"zip\":\"719\",\"text\":\"龍崎區\"},{\"zip\":\"720\",\"text\":\"官田區\"},{\"zip\":\"721\",\"text\":\"麻豆區\"},{\"zip\":\"722\",\"text\":\"佳里區\"},{\"zip\":\"723\",\"text\":\"西港區\"},{\"zip\":\"724\",\"text\":\"七股區\"},{\"zip\":\"725\",\"text\":\"將軍區\"},{\"zip\":\"726\",\"text\":\"學甲區\"},{\"zip\":\"727\",\"text\":\"北門區\"},{\"zip\":\"730\",\"text\":\"新營區\"},{\"zip\":\"731\",\"text\":\"後壁區\"},{\"zip\":\"732\",\"text\":\"白河區\"},{\"zip\":\"733\",\"text\":\"東山區\"},{\"zip\":\"734\",\"text\":\"六甲區\"},{\"zip\":\"735\",\"text\":\"下營區\"},{\"zip\":\"736\",\"text\":\"柳營區\"},{\"zip\":\"737\",\"text\":\"鹽水區\"},{\"zip\":\"741\",\"text\":\"善化區\"},{\"zip\":\"742\",\"text\":\"大內區\"},{\"zip\":\"743\",\"text\":\"山上區\"},{\"zip\":\"744\",\"text\":\"新市區\"},{\"zip\":\"745\",\"text\":\"安定區\"}]},{\"city\":\"高雄市\",\"area\":[{\"zip\":\"800\",\"text\":\"新興區\"},{\"zip\":\"801\",\"text\":\"前金區\"},{\"zip\":\"802\",\"text\":\"苓雅區\"},{\"zip\":\"803\",\"text\":\"鹽埕區\"},{\"zip\":\"804\",\"text\":\"鼓山區\"},{\"zip\":\"805\",\"text\":\"旗津區\"},{\"zip\":\"806\",\"text\":\"前鎮區\"},{\"zip\":\"807\",\"text\":\"三民區\"},{\"zip\":\"811\",\"text\":\"楠梓區\"},{\"zip\":\"812\",\"text\":\"小港區\"},{\"zip\":\"813\",\"text\":\"左營區\"},{\"zip\":\"814\",\"text\":\"仁武區\"},{\"zip\":\"815\",\"text\":\"大社區\"},{\"zip\":\"820\",\"text\":\"岡山區\"},{\"zip\":\"821\",\"text\":\"路竹區\"},{\"zip\":\"822\",\"text\":\"阿蓮區\"},{\"zip\":\"823\",\"text\":\"田寮區\"},{\"zip\":\"824\",\"text\":\"燕巢區\"},{\"zip\":\"825\",\"text\":\"橋頭區\"},{\"zip\":\"826\",\"text\":\"梓官區\"},{\"zip\":\"827\",\"text\":\"彌陀區\"},{\"zip\":\"828\",\"text\":\"永安區\"},{\"zip\":\"829\",\"text\":\"湖內區\"},{\"zip\":\"830\",\"text\":\"鳳山區\"},{\"zip\":\"831\",\"text\":\"大寮區\"},{\"zip\":\"832\",\"text\":\"林園區\"},{\"zip\":\"833\",\"text\":\"鳥松區\"},{\"zip\":\"840\",\"text\":\"大樹區\"},{\"zip\":\"842\",\"text\":\"旗山區\"},{\"zip\":\"843\",\"text\":\"美濃區\"},{\"zip\":\"844\",\"text\":\"六龜區\"},{\"zip\":\"845\",\"text\":\"內門區\"},{\"zip\":\"846\",\"text\":\"杉林區\"},{\"zip\":\"847\",\"text\":\"甲仙區\"},{\"zip\":\"848\",\"text\":\"桃源區\"},{\"zip\":\"849\",\"text\":\"三民區\"},{\"zip\":\"851\",\"text\":\"茂林區\"},{\"zip\":\"852\",\"text\":\"茄萣區\"}]},{\"city\":\"澎湖縣\",\"area\":[{\"zip\":\"880\",\"text\":\"馬公市\"},{\"zip\":\"881\",\"text\":\"西嶼鄉\"},{\"zip\":\"882\",\"text\":\"望安鄉\"},{\"zip\":\"883\",\"text\":\"七美鄉\"},{\"zip\":\"884\",\"text\":\"白沙鄉\"},{\"zip\":\"885\",\"text\":\"湖西鄉\"}]},{\"zip\":\"T\",\"city\":\"屏東縣\",\"area\":[{\"zip\":\"900\",\"text\":\"屏東市\"},{\"zip\":\"901\",\"text\":\"三地門\"},{\"zip\":\"902\",\"text\":\"霧台鄉\"},{\"zip\":\"903\",\"text\":\"瑪家鄉\"},{\"zip\":\"904\",\"text\":\"九如鄉\"},{\"zip\":\"905\",\"text\":\"裡港鄉\"},{\"zip\":\"906\",\"text\":\"高樹鄉\"},{\"zip\":\"907\",\"text\":\"鹽埔鄉\"},{\"zip\":\"908\",\"text\":\"長治鄉\"},{\"zip\":\"909\",\"text\":\"麟洛鄉\"},{\"zip\":\"911\",\"text\":\"竹田鄉\"},{\"zip\":\"912\",\"text\":\"內埔鄉\"},{\"zip\":\"913\",\"text\":\"萬丹鄉\"},{\"zip\":\"920\",\"text\":\"潮州鎮\"},{\"zip\":\"921\",\"text\":\"泰武鄉\"},{\"zip\":\"922\",\"text\":\"來義鄉\"},{\"zip\":\"923\",\"text\":\"萬巒鄉\"},{\"zip\":\"924\",\"text\":\"崁頂鄉\"},{\"zip\":\"925\",\"text\":\"新埤鄉\"},{\"zip\":\"926\",\"text\":\"南州鄉\"},{\"zip\":\"927\",\"text\":\"林邊鄉\"},{\"zip\":\"928\",\"text\":\"東港鎮\"},{\"zip\":\"929\",\"text\":\"琉球鄉\"},{\"zip\":\"931\",\"text\":\"佳冬鄉\"},{\"zip\":\"932\",\"text\":\"新園鄉\"},{\"zip\":\"940\",\"text\":\"枋寮鄉\"},{\"zip\":\"941\",\"text\":\"枋山鄉\"},{\"zip\":\"942\",\"text\":\"春日鄉\"},{\"zip\":\"943\",\"text\":\"獅子鄉\"},{\"zip\":\"944\",\"text\":\"車城鄉\"},{\"zip\":\"945\",\"text\":\"牡丹鄉\"},{\"zip\":\"946\",\"text\":\"恆春鎮\"},{\"zip\":\"947\",\"text\":\"滿州鄉\"}]},{\"city\":\"臺東縣\",\"area\":[{\"zip\":\"950\",\"text\":\"台東市\"},{\"zip\":\"951\",\"text\":\"綠島鄉\"},{\"zip\":\"952\",\"text\":\"蘭嶼鄉\"},{\"zip\":\"953\",\"text\":\"延平鄉\"},{\"zip\":\"954\",\"text\":\"卑南鄉\"},{\"zip\":\"955\",\"text\":\"鹿野鄉\"},{\"zip\":\"956\",\"text\":\"關山鎮\"},{\"zip\":\"957\",\"text\":\"海端鄉\"},{\"zip\":\"958\",\"text\":\"池上鄉\"},{\"zip\":\"959\",\"text\":\"東河鄉\"},{\"zip\":\"961\",\"text\":\"成功鎮\"},{\"zip\":\"962\",\"text\":\"長濱鄉\"},{\"zip\":\"963\",\"text\":\"太麻裡\"},{\"zip\":\"964\",\"text\":\"金峰鄉\"},{\"zip\":\"965\",\"text\":\"大武鄉\"},{\"zip\":\"966\",\"text\":\"達仁鄉\"}]},{\"city\":\"花蓮縣\",\"area\":[{\"zip\":\"970\",\"text\":\"花蓮市\"},{\"zip\":\"971\",\"text\":\"新城鄉\"},{\"zip\":\"972\",\"text\":\"秀林鄉\"},{\"zip\":\"973\",\"text\":\"吉安鄉\"},{\"zip\":\"974\",\"text\":\"壽豐鄉\"},{\"zip\":\"975\",\"text\":\"鳳林鎮\"},{\"zip\":\"976\",\"text\":\"光復鄉\"},{\"zip\":\"977\",\"text\":\"豐濱鄉\"},{\"zip\":\"978\",\"text\":\"瑞穗鄉\"},{\"zip\":\"979\",\"text\":\"萬榮鄉\"},{\"zip\":\"981\",\"text\":\"玉裡鎮\"},{\"zip\":\"982\",\"text\":\"卓溪鄉\"},{\"zip\":\"983\",\"text\":\"富裡鄉\"}]},{\"city\":\"金門縣\",\"area\":[{\"zip\":\"890\",\"text\":\"金沙鎮\"},{\"zip\":\"891\",\"text\":\"金湖鎮\"},{\"zip\":\"892\",\"text\":\"金寧鄉\"},{\"zip\":\"893\",\"text\":\"金城鎮\"},{\"zip\":\"894\",\"text\":\"烈嶼鄉\"},{\"zip\":\"896\",\"text\":\"烏坵鄉\"}]}]}"
            .Replace("桃園市", "桃園區")
            .Replace("桃園縣", "桃園市")
            .Replace("中壢市", "中壢區")
            .Replace("平鎮市", "平鎮區")
            .Replace("龍潭鄉", "龍潭區")
            .Replace("楊梅鎮", "楊梅區")
            .Replace("新屋鄉", "新屋區")
            .Replace("觀音鄉", "觀音區")
            .Replace("龜山鄉", "龜山區")
            .Replace("八德市", "八德區")
            .Replace("大溪鎮", "大溪區")
            .Replace("復興鄉", "復興區")
            .Replace("大園鄉", "大園區")
            .Replace("蘆竹鄉", "蘆竹區");

        #endregion

        public class areas
        {
            public string zip { get; set; }
            public string text { get; set; }
        }

        public class citys
        {
            public string city { get; set; }
            public List<areas> area { get; set; }
        }

        public class tw
        {
            public List<citys> taiwan { get;set;}
        }

        private List<string> GetTWCityList()
        {
            tw model = JsonConvert.DeserializeObject<tw>(twString);
            return model.taiwan.Select(p => p.city).ToList();
        }

        private List<string> GetTWAreaList(string city)
        {
            tw model = JsonConvert.DeserializeObject<tw>(twString);

            var d = model.taiwan.Where(p => p.city == city).LastOrDefault();
            if (d != null)
            {
                return d.area.Select(p => p.text).ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        #endregion


       
        /// <summary>
        /// 常見問題
        /// </summary>
        /// <returns></returns>
        public ActionResult Faq()
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Contact Us | 常見問題";

            QuestionAndAnswerService Service = new QuestionAndAnswerService();
            QuestionAndAnswerListModel list = new QuestionAndAnswerListModel();
            list.PageSize = 100;
            list = Service.GetList("", list, true);
            return View(list);
        }

        public ActionResult Privacy()
        {

            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Welfare System | 隱私權聲音";

            QuestionAndAnswerService Service = new QuestionAndAnswerService();
            QuestionAndAnswerListModel list = new QuestionAndAnswerListModel();
            list.PageSize = 100;
            list = Service.GetList("", list, true);
            return View(list);
        }

        public ActionResult UsedOverview(string id ,string type, string eng, string ton, string yy)
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Used Car | 中古車";

            AUsedCarInfoService carService = new AUsedCarInfoService();
            AUsedCarInfoListModel list = new AUsedCarInfoListModel();
            list.type = type;
            list.eng = eng;
            list.yy = yy;
            list.ton = ton;
            //if (!string.IsNullOrEmpty(ton) && !ton.Equals("所有噸數"))
            //{
            //    try
            //    {
            //        list.ton = (!ton.Contains(".0") && Convert.ToDouble(ton) % 1 == 0) ? ton.ToString() + ".0" : ton.ToString();
            //    }
            //    catch (Exception e)
            //    {
            //        list.ton = ton.ToString();
            //    }
            //}

            //搜尋資料
            list = carService.GetList1("", list, true);

            //產車種類
            list.TypeList = new List<SelectListItem>();
            list.TypeList.Add(new SelectListItem() { Text = "所有產車", Value = "所有產車" });
            list.TypeList.AddRange(Enum.GetValues(typeof(ProductType)).Cast<ProductType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList());

            //引擎種類
            list.EngineList = new List<SelectListItem>();
            list.EngineList.Add(new SelectListItem() { Text = "所有引擎", Value = "所有引擎" });
            list.EngineList.AddRange(carService.GetEngineList(list.Data.Select(p => p.EngineId).Distinct().ToList()));

            //選擇噸數
            list.TonnesList = new List<string>();
            list.TonnesList.Add("所有噸數");
            list.TonnesList.AddRange(list.Data.Select(p => p.Tonnes).Distinct().ToList());

            //選擇年份
            list.YearsList = new List<string>();
            list.YearsList.Add("所有年份");
            list.YearsList.AddRange(list.Data.Select(p => p.Years).Distinct().ToList());


            list.showtype = string.IsNullOrEmpty(type) ? "產車種類" : list.TypeList.Where(p=>p.Value==type).FirstOrDefault().Text;
            if (list.EngineList.Count > 1)
            {
                if (eng == "所有引擎")
                {
                    list.showeng = "所有引擎";
                }
                else
                {
                    list.showeng = string.IsNullOrEmpty(eng) ? "引擎種類" : list.EngineList.Where(p => p.Value == eng).FirstOrDefault().Text;
                }
            }
            else
            {
                list.showeng = "引擎種類";
            }

            if (list.TonnesList.Count > 1)
            {
                if (ton == "所有噸數")
                {
                    list.showton = "所有噸數";
                }
                else
                {
                    list.showton = string.IsNullOrEmpty(ton) ? "選擇噸數" : list.TonnesList.Where(p => p == list.ton).FirstOrDefault();
                }
            }
            else
            {
                list.showton = "選擇噸數";
            }

            if (list.YearsList.Count > 1)
            {
                if (yy == "所有年份")
                {
                    list.showyy = "所有年份";
                }
                else
                {
                    list.showyy = string.IsNullOrEmpty(yy) ? "選擇年份" : list.YearsList.Where(p => p == yy).FirstOrDefault();
                }
            }
            else
            {
                list.showyy = "選擇年份";
            }
            

            //單筆資料
            if (!string.IsNullOrEmpty(id))
            {
                int ii = Convert.ToInt32(id);
                list.ImgList = carService.GetImgList(ii);
                list.AUsedCarInfo = carService.Get1(ii);
                TempData["msg"] = "wwww";
            }
            else
            {
                list.ImgList = new List<string>();
                list.AUsedCarInfo = new AUsedCarInfoModel();
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult GetUsedOverviewById(string id)
        {
            //單筆資料
            int ii = Convert.ToInt32(id);

            AUsedCarInfoService carService = new AUsedCarInfoService();
            var ImgList = carService.GetImgList(ii);
            var AUsedCarInfo = carService.Get1(ii);
            return Json(new { ImgList = ImgList, AUsedCarInfo = AUsedCarInfo });
        }

        [HttpPost]
        public ActionResult GetProductsList()
        {
            ProductInfoService Service = new ProductInfoService();
            ProductInfoListModel list = new ProductInfoListModel();
            list = Service.GetList1(list, true);
            return Json(list);
        }

        public ActionResult ProductsContent(string id)
        {
            TempData["title"] = "台灣豐田 TOYOTA Material Handling Taiwan - Products | 產品介紹";

            ProductInfoService Service = new ProductInfoService();
            ProductInfoModel info = new ProductInfoModel();
            info.ProductImgList = new Dictionary<string, string>();
            info = Service.Get("", Convert.ToInt16(id), true);
            return View(info);
        }

        public ActionResult SearchResult()
        {
            return View();
        }
    }
}
