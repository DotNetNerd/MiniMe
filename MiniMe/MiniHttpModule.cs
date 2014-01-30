using System;
using System.Linq;
using System.Web;

namespace MiniMe
{
    public class MiniHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += ApplicationBeginRequest;
        }

        public void Dispose() { }

        static void ApplicationBeginRequest(object sender, EventArgs e)
        {
            var extentions = new string[] {".js", ".css", ".png", ".gif", ".jpg"};
            string url = HttpContext.Current.Request.RawUrl;

            if (!HttpContext.Current.Response.ContentType.Equals("text/html", StringComparison.OrdinalIgnoreCase) || extentions.Any(ext => url.EndsWith(ext, StringComparison.OrdinalIgnoreCase))) return;

            HttpContext current = HttpContext.Current;
            current.Response.Filter = new MiniHtmlFilter(current.Response.Filter);
        }
    }
}