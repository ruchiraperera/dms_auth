using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApi.Jwt.Filters
{
    public class AllowOptionsHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (request.Method == HttpMethod.Options &&
                response.StatusCode == HttpStatusCode.MethodNotAllowed)
            {
                response = new HttpResponseMessage(HttpStatusCode.OK);
            }

            return response;
        }
    }
}