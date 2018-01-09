using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Begonia.Toyota.WebSite.Service;
using JamZoo.Project.WebSite.Enums;
using Newtonsoft.Json;

namespace Begonia.Toyota.WebSite.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        BaseService Service = new BaseService();
        
        public class keyModel
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        public class DataModel
        {
            public List<keyModel> data { get; set; }
        }

        [HttpPost]
        public ActionResult reOrder(string para, string fn)
        {
            DataModel d = JsonConvert.DeserializeObject<DataModel>(para);
            var isSuccess = Service.reOrder(d, (FN)Enum.Parse(typeof(FN), fn, false));
            return Json(new { isSuccess  = isSuccess });
        }
    }
}
