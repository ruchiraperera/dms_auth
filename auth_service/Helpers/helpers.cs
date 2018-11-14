using auth_service.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace auth_service.Helpers
{
    public class helpers
    {
    }
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {

        string uniqueFilename = null;
        public CustomMultipartFormDataStreamProvider(string path, string filename) : base(path)
        {

            this.uniqueFilename = filename;
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            //headers.ContentDisposition.FileName.Replace("\"", string.Empty);

            // var j = base.RootPath;

            //if (File.Exists(base.RootPath + "/" + headers.ContentDisposition.FileName))
            //{

            //}
            return headers.ContentDisposition.FileName = uniqueFilename;

            //return headers.ContentDisposition.FileName =  string.Concat(Guid.NewGuid().ToString(),headers.ContentDisposition.FileName);
            //return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
    }
}