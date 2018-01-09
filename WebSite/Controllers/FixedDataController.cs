using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Begonia.Toyota.WebSite.Enums;
using Begonia.Toyota.WebSite.Library.Principal;
using Begonia.Toyota.WebSite.Models;
using Begonia.Toyota.WebSite.Service;
using JamZoo.Project.WebSite.Enums;

namespace Begonia.Toyota.WebSite.Controllers
{
    public class FixedDataController : Controller
    {
        FixedDataService Service;

        public FixedDataController()
        {
            Service = new FixedDataService();
        }


        public ActionResult TEST(string cid)
        {
            Service.UpdateALL(cid);
            return Content("ok");
        }

        #region CRUD
        //GET: /FixedData/List

        public ActionResult Add(string fn, int page = 1)
        {
            try
            {
                string roles = "Admin";
                switch ((FixedData)Enum.Parse(typeof(FixedData), fn, true))
                {
                    case FixedData.公司福利管理:
                        roles += ",Human,HumanC,HumanU,HumanR,HumanD";
                        break;
                    case FixedData.租賃服務:
                        roles += ",Service,ServiceC,ServiceR,ServiceU,ServiceD";
                        break;
                    case FixedData.售後服務:
                        roles += ",Service,ServiceC,ServiceR,ServiceU,ServiceD";
                        break;
                    case FixedData.職涯規劃管理:
                        roles += ",Human,HumanC,HumanU,HumanR,HumanD";
                        break;
                }
                AccountService accService = new AccountService();
                String account = User.Identity.Name; //登入的使用者帳號
                if (accService.Isright(account, roles.Split(',')))
                {
                    FixedDataModel model = Service.Get(User.Identity.Name, fn);

                    #region default

                    string hkurl = "";
                    var m2 = Service.Get2("", FixedData2.現有職缺設定.ToString());
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
                    string htmldefault = "";
                    switch ((FixedData)Enum.Parse(typeof(FixedData), fn, true))
                    {
                        case FixedData.公司福利管理:
                            htmldefault = "<section class=\"vision banner-welfare short\">" +
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
                             "<div class=\"text-center\"><a href = \"" + hkurl + "\" target=\"_blank\" class=\"more_box animall\">瀏覽我們的職缺</a></div>" +
                             "</div>" +
                             "</div> " +
                             "</section>";
                            break;
                        case FixedData.租賃服務:
                            htmldefault = "<section class=\"vision banner-lease short\">" +
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
                            break;
                        case FixedData.售後服務:
                            htmldefault = "<section class=\"vision banner-warranty short\">" +
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
                            break;
                        case FixedData.職涯規劃管理:
                            htmldefault = "<section class=\"vision banner-career short\">" +
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
                             "<div class=\"text-center\"><a href = \"" + hkurl + "\" target=\"_blank\" class=\"more_box animall\">瀏覽我們的職缺</a></div>" +
                             "</div>" +
                             "</div>" +
                             "</section>";
                            break;
                    }

                    #endregion

                    if (model != null)
                    {
                        model.Mode = EditPageMode.Update;
                        if (string.IsNullOrEmpty(model.Html)) model.Html = htmldefault;
                        return View(model);
                    }
                    else
                    {
                        model = Service.NewInstance();
                        model.Html = htmldefault;
                        model.page = page;
                        model.Mode = EditPageMode.Create;
                        model.FunctionName = fn;
                        return View(model);
                    }
                }
                else
                {
                    return RedirectToAction("Signout", "Account");
                }
            }
            catch (Exception e)
            {
                string errormsg = string.Empty;
                errormsg += e.InnerException != null ? e.InnerException.Message : e.Message;
                return Content(errormsg);
            }
            
        }

        public ActionResult Add2(string fn, int page = 1)
        {
            //fn=聯絡我們信箱 Human
            //fn=索取型錄信箱 Contact
            //fn=現有職缺設定 Contact
            string roles = "Admin";
            switch ((FixedData2)Enum.Parse(typeof(FixedData2), fn, true))
            {
                case FixedData2.聯絡我們信箱:
                    roles += ",Contact,ContactC,ContactU,ContactR,ContactD";
                    break;
                case FixedData2.現有職缺設定:
                    roles += ",Human,HumanC,HumanU,HumanR,HumanD";
                    break;
                case FixedData2.索取型錄信箱:
                    roles += ",Contact,ContactC,ContactU,ContactR,ContactD";
                    break;
            }
            AccountService accService = new AccountService();
            String account = User.Identity.Name; //登入的使用者帳號
            if (accService.Isright(account, roles.Split(',')))
            {
                FixedDataModel model = Service.Get2(User.Identity.Name, fn);
                if (model != null)
                {
                    model.Mode = EditPageMode.Update;
                    return View(model);
                }
                else
                {
                    model = Service.NewInstance();
                    model.page = page;
                    model.Mode = EditPageMode.Create;
                    model.FunctionName = fn;
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Signout", "Account");
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(FixedDataModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;
                    if (Service.Create(
                        User.Identity.Name,
                        model, out ErrMsgs))
                    {
                        return RedirectToAction("Add", new { fn = model.FunctionName, page = model.page });
                    }

                    ModelState.AddModelError("message", ErrMsgs);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
            }

            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create2(FixedDataModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;
                    if (Service.Create2(
                        User.Identity.Name,
                        model, out ErrMsgs))
                    {
                        return RedirectToAction("Add2", new { fn = model.FunctionName, page = model.page });
                    }

                    ModelState.AddModelError("message", ErrMsgs);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
            }

            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Update(FixedDataModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;

                    string UserAccount = (null != UserManager.User) ? UserManager.User.Account : "N/A";

                    if (Service.Update(User.Identity.Name, UserAccount, model, out ErrMsgs))
                    {
                        return RedirectToAction("Add", new { fn = model.FunctionName, page = model.page });
                    }
                    else
                    {
                        ModelState.AddModelError("message", "修改失敗:" + ErrMsgs);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
            }

            model.Mode = EditPageMode.Update;
            return View("Add", model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Update2(FixedDataModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;

                    string UserAccount = (null != UserManager.User) ? UserManager.User.Account : "N/A";

                    if (Service.Update2(User.Identity.Name, UserAccount, model, out ErrMsgs))
                    {
                        TempData["msg"] = "success";
                        return RedirectToAction("Add2", new { fn = model.FunctionName, page = model.page });
                    }
                    else
                    {
                        ModelState.AddModelError("message", "修改失敗:" + ErrMsgs);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
            }

            model.Mode = EditPageMode.Update;
            return View("Add", model);
        }

        #endregion


    }
}
