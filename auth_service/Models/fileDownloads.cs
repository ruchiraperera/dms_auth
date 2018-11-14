using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace auth_service.Models
{
    public class fileDownloads
    {
        public List<fileInfo> GetFile()
        {
            List<fileInfo> listFiles = new List<fileInfo>();
            //Path For download From Network Path.  
            string fileSavePath = @"";
            DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                listFiles.Add(new fileInfo()
                {
                    FileId = i + 1,
                    FileName = item.Name,
                    FilePath = dirInfo.FullName + @"\" + item.Name  
                });

                i = i + 1;
            }
            return listFiles;
        }
    }
}