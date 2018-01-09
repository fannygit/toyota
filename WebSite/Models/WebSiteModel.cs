using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Begonia.Toyota.WebSite.Models
{
    public class AboutModel
    {
        public List<InfoPrizeModel> PrizeList { get; set; }
        public List<string> YearsList { get; set; }
    }

    public class IndexModel
    {
        public IndexBannerListModel BannerList { get; set; }
        public InfoNewsListModel NewsList { get; set; }

        public IndexVideoModel Video { get; set; }
    }
}