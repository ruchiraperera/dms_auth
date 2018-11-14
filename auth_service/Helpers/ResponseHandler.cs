using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace auth_service.Helpers
{
    public class ResponseHandler
    {
        public bool status { get; set; }
        public Object data { get; set; }
        public Object error { get; set; }
    }
}