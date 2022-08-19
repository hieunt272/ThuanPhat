using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace ThuanPhat.Filters
{
    public class LanguageFilters : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var culture = "vi";
            var absolutePath = filterContext.HttpContext.Request.Url?.AbsolutePath.ToLower();

            if (absolutePath != null && absolutePath.StartsWith("/ja"))
            {
                culture = "ja";
            }
            //else
            //{
            //    var httpCookie = filterContext.HttpContext.Request.Cookies["lang"];
            //    if (httpCookie != null)
            //    {
            //        culture = httpCookie.Value;
            //    }
            //}

            //var lang = new HttpCookie("lang") { Value = culture, Expires = DateTime.Now.AddMonths(1)};
            //filterContext.HttpContext.Response.Cookies.Add(lang);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            base.OnActionExecuting(filterContext);
        }
    }
}