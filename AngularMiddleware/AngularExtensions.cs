using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;

namespace AngularMiddleware
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public static class AngularExtensions
    {
        public static IApplicationBuilder UseAngular(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AngularRequestMiddleware>(new AngularRouteCollection());
        }

        public static IApplicationBuilder UseAngular(this IApplicationBuilder builder, Action<AngularRouteCollection> routes)
        {
            var rc = new AngularRouteCollection();

            routes(rc);

            return builder.UseMiddleware<AngularRequestMiddleware>(rc);
        }
    }
}
