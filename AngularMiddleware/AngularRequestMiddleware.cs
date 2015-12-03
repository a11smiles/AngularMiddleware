using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMiddleware
{
    public class AngularRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AngularRouteCollection _routes;

        public AngularRequestMiddleware(RequestDelegate next, AngularRouteCollection routes)
        {
            _next = next;
            _routes = routes;
        }

        public async Task Invoke(HttpContext context)
        {
            // Filter out ignored routes
            if (_routes.IgnoredRoutes.Any() && !_routes.IgnoredRoutes.Select(r => r.ToLower()).Contains(context.Request.Path.Value.Substring(1).ToLower()))
                return;

            // Rewrite mapped routes
            foreach (var route in _routes.Routes)
            {
                // Request.Path starts with forward slash (i.e. "/")
                // So, add the slash to the rewrite paths if they don't include it
                if (route.Requests.Select(r => r.ToLower().StartsWith("/") ? r : "/" + r).Contains(context.Request.Path.Value.ToLower()))
                {
                    // Serve rewritten route
                    await ServeRoute(context, route.Response);
                    return;
                }
            }

            // Serve direct file requests
            if (context.Request.Path.Value.Contains('.'))
                await ServeRoute(context, context.Request.Path.Value);
            else
                await ServeRoute(context, _routes.DefaultRoute);

            return;
        }

        private async Task ServeRoute(HttpContext context, string path)
        {
            // Remove leading forward slash, otherwise OWIN will look in root directory (i.e. C:\)
            if (path.StartsWith("/"))
                path = path.Substring(1);
            
            // If rewritten file exists, return it. Otherwise, return 404.
            if (System.IO.File.Exists(path))
                await context.Response.WriteAsync(System.IO.File.ReadAllText(path));
            else
            {
                context.Response.StatusCode = 404;
                return;
            }
        }
    }
}
