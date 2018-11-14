using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace auth_service.Models
{

    public class sendEmail {
        public string subject { get; set; }
        public string body { get; set; }
        public List<string> to { get; set; }
        public List<string> cc { get; set; }
        public string filename { get; set; }
    }

    public class fileObj {

        public string filename { get; set; }
        public string uniqueFileName { get; set; }
    }
    public class multipleFile
    {

        public List<fileObj> fileNames { get; set; }
        public List<string> folderNames { get; set; }
        public string currentFolder { get; set; }
        public int userId { get; set; }
    }

    public class starredfiles {

        public List<int> files { get; set; }
    }

    public class deleteItems {
        public List<int> files { get; set; }
        public List<int> folders { get; set; }
    }
}