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
    
    public partial class log_news
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string Action { get; set; }
        public string AccountId { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
