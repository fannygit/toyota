//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Begonia.Toyota.WebSite.DbContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class info_news
    {
        public int Id { get; set; }
        public string title { get; set; }
        public System.DateTime title_date { get; set; }
        public string list_img { get; set; }
        public string list_intro { get; set; }
        public string detail { get; set; }
        public int orderfield { get; set; }
        public bool status { get; set; }
        public System.DateTime create_time { get; set; }
        public string create_id { get; set; }
    }
}
