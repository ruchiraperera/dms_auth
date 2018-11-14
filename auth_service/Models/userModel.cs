using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace auth_service.Models
{
    public class userModel
    {
        public tbl_user user { get; set; }
        public tbl_permission permission { get; set; }

    }
}