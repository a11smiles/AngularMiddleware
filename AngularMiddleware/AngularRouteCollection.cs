using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMiddleware
{
    public class AngularRouteCollection
    {
        private List<AngularRoute> _routes = new List<AngularRoute>();
        private List<string> _ignoredRoutes = new List<string>();
        private string _defaultRoute = "index.html";

        public List<AngularRoute> Routes
        {
            get { return _routes; }
        }
        public List<string> IgnoredRoutes
        {
            get { return _ignoredRoutes; }
        }
        public string DefaultRoute
        {
            get { return _defaultRoute; }
            set { _defaultRoute = value; }
        }

        public void MapAngularRoute(string[] requests, string response)
        {
            _routes.Add(new AngularRoute() { Requests = requests, Response = response });
        }

        public void IgnoreAngularRoute(string route)
        {
            _ignoredRoutes.Add(route);
        }
    }
}
