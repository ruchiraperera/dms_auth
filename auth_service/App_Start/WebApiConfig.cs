﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi.Jwt.Filters;

namespace auth_service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();
            config.Filters.Add(new JwtAuthenticationAttribute());
            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
