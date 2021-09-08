using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LED.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
    public static class HtmlHelperExtensions
    {
        public static IHtmlString InlineStyles(this HtmlHelper htmlHelper, string bundleVirtualPath)
        {
            string bundleContent = LoadBundleContent(htmlHelper.ViewContext.HttpContext, bundleVirtualPath);
            string htmlTag = $"<style rel=\"stylesheet\" type=\"text/css\">{bundleContent}</style>";
            return new HtmlString(htmlTag);
        }

        public static IHtmlString InlineScripts(this HtmlHelper htmlHelper, string bundleVirtualPath)
        {
            string bundleContent = LoadBundleContent(htmlHelper.ViewContext.HttpContext, bundleVirtualPath);
            string htmlTag = $"<script type=\"text/javascript\">{bundleContent}</script>";
            return new HtmlString(htmlTag);
        }

        private static string LoadBundleContent(HttpContextBase httpContext, string bundleVirtualPath)
        {
            var bundleContext = new BundleContext(httpContext, BundleTable.Bundles, bundleVirtualPath);
            var bundle = BundleTable.Bundles.Single(b => b.Path == bundleVirtualPath);
            var bundleResponse = bundle.GenerateBundleResponse(bundleContext);
            return bundleResponse.Content;
        }
    }
}
