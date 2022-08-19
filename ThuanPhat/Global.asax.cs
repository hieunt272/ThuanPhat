using ThuanPhat.DAL;
using ThuanPhat.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ThuanPhat.Migrations;

namespace ThuanPhat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataEntities, Configuration>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (var unitofWork = new UnitOfWork())
            {
                Application["ConfigSite"] = unitofWork.ConfigSiteRepository.GetQuery().Select(a => new ConfigSiteDto
                {
                    Title = a.Title,
                    Description = a.Description,
                    Image = a.Image,
                    Email = a.Email,
                    Facebook = a.Facebook,
                    GoogleAnalytics = a.GoogleAnalytics,
                    GoogleMap = a.GoogleMap,
                    Hotline = a.Hotline,
                    InfoFooter = a.InfoFooter,
                    Instagram = a.Instagram,
                    LiveChat = a.LiveChat,
                    Place = a.Place,
                    Twitter = a.Twitter,
                    InfoContact = a.InfoContact,
                    AboutText = a.AboutText,
                    AboutImage = a.AboutImage,
                    Youtube = a.Youtube,
                    TaxCode = a.TaxCode,
                    Favicon = a.Favicon,
                }).FirstOrDefault();
                Application["ConfigSiteLang"] = unitofWork.ConfigSiteLangRepository.GetQuery(a => a.LanguageId == 2).Select(a => new ConfigSiteDto
                {
                    Title = a.Title,
                    Description = a.Description,
                    Image = a.ConfigSite.Image,
                    Email = a.ConfigSite.Email,
                    Facebook = a.ConfigSite.Facebook,
                    GoogleAnalytics = a.ConfigSite.GoogleAnalytics,
                    GoogleMap = a.ConfigSite.GoogleMap,
                    Hotline = a.ConfigSite.Hotline,
                    InfoFooter = a.InfoFooter,
                    Instagram = a.ConfigSite.Instagram,
                    LiveChat = a.ConfigSite.LiveChat,
                    Place = a.Place,
                    Twitter = a.ConfigSite.Twitter,
                    InfoContact = a.InfoContact,
                    AboutImage = a.ConfigSite.AboutImage,
                    Favicon = a.ConfigSite.Favicon,
                    AboutText = a.AboutText,
                    TaxCode = a.ConfigSite.TaxCode,
                    Youtube = a.ConfigSite.Youtube,
                }).FirstOrDefault();
            }
        }
    }
}
