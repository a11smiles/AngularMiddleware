# AngularMiddleware

[![Build status](https://ci.appveyor.com/api/projects/status/mwbvor2k824iqpss?svg=true)](https://ci.appveyor.com/project/a11smiles/angularmiddleware)

Middleware for facilitating support of true Angular client-side applications in ASP.NET 5.

AngularMiddleware allows the development of Angular (or any other JavaScript framework) client-side application ASP.NET 5 without the need for controllers and views.

## Problem
Typically, with ASP.NET web applications, there are a few requirements for supporting Angular and other JavaScript frameworks.

1. Unnecessary controllers and views that are serving static pages
2. Creating "catch-all" routes:  

   ```  
   routes.MapRoute(  
       "",  
       "{url}",  
       new { controller = "Home", Action = "Index" }
   );
   ```

While this may work in cases where you prefer using partial views and perform additional ViewModel binding, this isn't the cleanest approach to accomplishing a true client-side application.

In reality, one would prefer to utilize the ASP.NET framework for it's WebApi and REST services while leaving all presentation, databinding and routing strictly on the client.

### Push State
Initially, routing was performed by appending a "hash" or "hashbang" to the end of URLs.  These URLs utilize the fragment portion of the URL as if it were an extension of the path name (e.g. `#/user/1234`).  This was great for older browsers (and still should be used if you need to support older browsers), but is no longer necessary due to the arrival of the HTML5 History API.  Continuing to use hashes should also be stopped due to the fact that hashes originally signified an anchor within the document.

Because 1) using hashes and URL fragments should not be used for routing; 2) we don't want to create catch-all routes; 3) we don't need server-side controllers and views; 4) we still wish to use WebApi; and, 5) we need to be able to serve up static files, AngularMiddleware was created.

### Approach
My first attempt at hooking into the request pipeline is shown in a question I posted on [StackOverflow](http://stackoverflow.com/questions/34051586/angularjs-with-mvc-6).  The logic was very elementary to simply ignore any request whose path began with `/api` - allow MVC to serve those.  _All_ other requests would serve _index.html_ in the root directory.  I posted the question to see if anyone knew of anything built-in the MVC 5.  I assumed that because Microsoft was pushing Angular and client-side development so much, they would have provided a mechanism of sorts.  Unfortunately, not.

One of the responses I received pointed me to Angular's `ui-router` documentation on [GitHub](https://github.com/angular-ui/ui-router/wiki/Frequently-Asked-Questions#how-to-configure-your-server-to-work-with-html5mode) for removing the hash (e.g. `html5mode`).  For ASP.NET/IIS, there were two possibilities...

1. Azure IIS Rewrites
2. ASP.Net C# Rewrites

In the case of ASP.NET 5 and DNX, neither are a good fit.  The `web.cofing`, while it can be modified, should be only contain a couple of entries for the `httpPlatformHandler` in order to support the new hosting model.  Similarly, `Global.asax` has been deprecated.  For ASP.NET 5 web applications, almost all configurations have been moved into various _json_ configuration files; and, application events should be managed in the request pipe configured by the `Startup` class.

Therefore, AngularMiddleware provides a solution to the issues listed above and offers additional configurations.

### Installation
AngularMiddleware should be listed in your project's `dependencies` within the _project.json_ configuration file.  NuGet will download the libaries and include them in your project automatically.

```
{
  ...

  "dependencies" : {
    "AngularMiddleware" : "version"
  }

  ...
}
```  

### Basic Usage
**Startup.cs**  
Add a `using` reference for _AngularMiddleware_ at the top of your _StartUp_ class file.  Then, add AngularMiddleware to the `Configure()` method.

```
public void Configure(IApplicationBuilder app, IHostingEnvironment env) 
{
   ...

   app.UseAngular();

   ...
}
```
This is the basic configuration and, assuming that you have `index.html` in your _wwwroot_ directory, all should work.

### Advanced Options  
**Different Default Document**  
In the case you choose a different document as your default (e.g. instead of `index.html`), you can tell AngularMiddleware what that default document should be.

```
app.UseAngular(routes =>
{
    routes.DefaultRoute = "default.html";
});

```
  
**Route/Page Redirection**  
Let's say that you wish to redirect all requests for `~/administrators.html`, `~/hr_users.html`, and `~/marketing_users.html` to `~/users.html`.

```
app.UseAngular(routes =>
{
    routes.MapAngularRoute(new[] { "administrators.html", "hr_users.html", "marketing_users.html" }, "users.html");
});

```  

**Ingore Specific Requests**  
AngularMiddleware also allows you to ignore specific requests.

```
app.UseAngular(routes =>
{
    routes.IgnoreAngularRoute("admin.html");
});

```  

_NOTE: `IgnoreAngularRoute` should only be used in rare cases that the pipeline doesn't service a request properly.  If you are using `MVC` middleware and you've defined routes (e.g. legacy routing or attribute routing), those routes will be ignored by AngularMiddleware and be served, instead, by the MVC middleware.  Likewise, if you are using either/both of the `DefaultFiles` and `StaticFiles` middleware, unless a specific route is mapped (i.e. rewritten) or ignored, these middleware will serve static files - including the default files (if `DefaultFiles` middleware is used). _  
### License
AngularMiddleware is released under the [MIT License](http://www.opensource.org/licenses/MIT).

### Development
The AngularMiddleware project is developed and maintained by [Joshua Davis](http://jdav.is).