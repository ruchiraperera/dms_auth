//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace auth_service.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_profile
    {
        public int profileId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string imageurl { get; set; }
        public int userId { get; set; }
    }
}
