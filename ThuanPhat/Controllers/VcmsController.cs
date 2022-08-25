using Helpers;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Drawing;
using ThuanPhat.DAL;
using ThuanPhat.Models;
using ThuanPhat.ViewModel;

namespace ThuanPhat.Controllers
{
    [Authorize]
    public class VcmsController : Controller
    {
        public readonly UnitOfWork _unitOfWork = new UnitOfWork();

        #region Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(AdminLoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.Get(a => a.Username == model.Username && a.Active).SingleOrDefault();

                if (admin != null && HtmlHelpers.VerifyHash(model.Password, "SHA256", admin.Password))
                {
                    var ticket = new FormsAuthenticationTicket(1, model.Username.ToLower(), DateTime.Now, DateTime.Now.AddDays(30), true,
                        admin.ToString(), FormsAuthentication.FormsCookiePath);

                    var encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Vcms");
                }
                ModelState.AddModelError("", @"Tên đăng nhập hoặc mật khẩu không chính xác.");
            }
            return View(model);
        }
        public RedirectToRouteResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Vcms");
        }
        #endregion

        #region Admin
        public ActionResult Index()
        {
            var model = new InfoAdminViewModel
            {
                Admins = _unitOfWork.AdminRepository.GetQuery().Count(),
                Articles = _unitOfWork.ArticleRepository.GetQuery().Count(),
                Contacts = _unitOfWork.ContactRepository.GetQuery().Count(),
                Banners = _unitOfWork.BannerRepository.GetQuery().Count(),
                Services = _unitOfWork.ServiceRepository.GetQuery().Count(),
                Recruits = _unitOfWork.RecruitRepository.GetQuery().Count(),
            };
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult ListAdmin()
        {
            var admins = _unitOfWork.AdminRepository.Get();
            return PartialView("ListAdmin", admins);
        }
        public ActionResult CreateAdmin(string result = "")
        {
            ViewBag.Result = result;
            return View();
        }
        [HttpPost]
        public ActionResult CreateAdmin(Admin model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                if (admin != null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                }
                else
                {
                    var hashPass = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.AdminRepository.Insert(new Admin { Username = model.Username, Password = hashPass, Active = model.Active });
                    _unitOfWork.Save();
                    return RedirectToAction("CreateAdmin", new { result = "success" });
                }
            }
            return View();
        }
        public ActionResult EditAdmin(int adminId = 0)
        {
            var admin = _unitOfWork.AdminRepository.GetById(adminId);
            if (admin == null)
            {
                return RedirectToAction("CreateAdmin");
            }

            var model = new EditAdminViewModel
            {
                Id = admin.Id,
                Username = admin.Username,
                Active = admin.Active,
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult EditAdmin(EditAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetById(model.Id);
                if (admin == null)
                {
                    return RedirectToAction("CreateAdmin");
                }
                if (admin.Username != model.Username)
                {
                    var exists = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                    if (exists != null)
                    {
                        ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                        return View(model);
                    }
                    admin.Username = model.Username;
                }
                admin.Active = model.Active;
                if (model.Password != null)
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                }
                _unitOfWork.Save();
                return RedirectToAction("CreateAdmin", new { result = "update" });
            }
            return View(model);
        }
        public bool DeleteAdmin(string username)
        {
            var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(username)).SingleOrDefault();
            if (admin == null)
            {
                return false;
            }
            if (username == "admin")
            {
                return false;
            }
            _unitOfWork.AdminRepository.Delete(admin);
            _unitOfWork.Save();
            return true;
        }
        public ActionResult ChangePassword(int result = 0)
        {
            ViewBag.Result = result;
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(User.Identity.Name,
                StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (admin == null)
                {
                    return HttpNotFound();
                }
                if (HtmlHelpers.VerifyHash(model.OldPassword, "SHA256", admin.Password))
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.Save();
                    return RedirectToAction("ChangePassword", new { result = 1 });
                }
                else
                {
                    ModelState.AddModelError("", @"Mật khẩu hiện tại không đúng!");
                    return View();
                }
            }
            return View(model);
        }
        #endregion

        #region ConfigSite
        public ActionResult ConfigSite(string result = "")
        {
            ViewBag.Result = result;
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            return View(config);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ConfigSite(ConfigSite model, FormCollection fc)
        {
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            if (config == null)
            {
                _unitOfWork.ConfigSiteRepository.Insert(model);
            }
            else
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/configs/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Image")
                    {
                        config.Image = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "AboutImage")
                    {
                        config.AboutImage = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "Favicon")
                    {
                        config.Favicon = imgFile;
                    }
                }

                config.Facebook = model.Facebook;
                config.GoogleMap = model.GoogleMap;
                config.Youtube = model.Youtube;
                config.Twitter = model.Twitter;
                config.Instagram = model.Instagram;
                config.Title = model.Title;
                config.Description = model.Description;
                config.GoogleAnalytics = model.GoogleAnalytics;
                config.Hotline = model.Hotline;
                config.Email = model.Email;
                config.LiveChat = model.LiveChat;
                config.Place = model.Place;
                config.AboutText = model.AboutText;
                config.InfoFooter = model.InfoFooter;
                config.InfoContact = model.InfoContact;
                config.TaxCode = model.TaxCode;

                _unitOfWork.Save();
                HttpContext.Application["ConfigSite"] = new ConfigSiteDto
                {
                    Title = config.Title,
                    Description = config.Description,
                    Image = config.Image,
                    Email = config.Email,
                    Facebook = config.Facebook,
                    GoogleAnalytics = config.GoogleAnalytics,
                    GoogleMap = config.GoogleMap,
                    Hotline = config.Hotline,
                    InfoFooter = config.InfoFooter,
                    Instagram = config.Instagram,
                    LiveChat = config.LiveChat,
                    Place = config.Place,
                    Twitter = config.Twitter,
                    InfoContact = config.InfoContact,
                    AboutText = config.AboutText,
                    AboutImage = config.AboutImage,
                    Youtube = config.Youtube,
                    TaxCode = config.TaxCode,
                    Favicon = config.Favicon,
                };
                return RedirectToAction("ConfigSite", "Vcms", new { result = "success" });
            }
            return View("ConfigSite", model);
        }

        public ActionResult UpdateConfigSiteLang(int configSiteId, int result = 0)
        {
            ViewBag.Result = result;
            var configSite = _unitOfWork.ConfigSiteRepository.GetById(configSiteId);
            return View(configSite);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateConfigSiteLang(FormCollection fc)
        {
            var configSiteId = Convert.ToInt32(fc["ConfigSiteId"]);
            var langId = Convert.ToInt32(fc["LangId"]);
            var title = fc["Title"];
            var description = fc["Description"];
            var infoFooter = fc["InfoFooter"];
            var infoContact = fc["InfoContact"];
            var place = fc["Place"];
            var aboutText = fc["AboutText"];
            var configSiteLang = _unitOfWork.ConfigSiteLangRepository.GetQuery(a => a.ConfigSiteId == configSiteId && a.LanguageId == langId).SingleOrDefault();
            if (configSiteLang == null)
            {
                _unitOfWork.ConfigSiteLangRepository.Insert(new ConfigSiteLang
                {
                    ConfigSiteId = configSiteId,
                    LanguageId = langId,
                    Title = title,
                    Description = description,
                    InfoFooter = infoFooter,
                    Place = place,
                    InfoContact = infoContact,
                    AboutText = aboutText
                });
                _unitOfWork.Save();
                return RedirectToAction("UpdateConfigSiteLang", new { configSiteId, result = 1 });
            }

            configSiteLang.Title = title;
            configSiteLang.Description = description;
            configSiteLang.Place = place;
            configSiteLang.InfoFooter = infoFooter;
            configSiteLang.InfoContact = infoContact;
            configSiteLang.AboutText = aboutText;

            _unitOfWork.Save();
            HttpContext.Application["ConfigSiteLang"] = new ConfigSiteDto
            {
                Title = configSiteLang.Title,
                Description = configSiteLang.Description,
                Image = configSiteLang.ConfigSite.Image,
                Email = configSiteLang.ConfigSite.Email,
                Facebook = configSiteLang.ConfigSite.Facebook,
                GoogleAnalytics = configSiteLang.ConfigSite.GoogleAnalytics,
                GoogleMap = configSiteLang.ConfigSite.GoogleMap,
                Hotline = configSiteLang.ConfigSite.Hotline,
                InfoFooter = configSiteLang.InfoFooter,
                Instagram = configSiteLang.ConfigSite.Instagram,
                Youtube = configSiteLang.ConfigSite.Youtube,
                LiveChat = configSiteLang.ConfigSite.LiveChat,
                Place = configSiteLang.Place,
                Twitter = configSiteLang.ConfigSite.Twitter,
                AboutText = configSiteLang.AboutText,
                TaxCode = configSiteLang.ConfigSite.TaxCode,
                Favicon = configSiteLang.ConfigSite.Favicon,
                AboutImage = configSiteLang.ConfigSite.AboutImage,
                InfoContact = configSiteLang.InfoContact
            };
            return RedirectToAction("UpdateConfigSiteLang", new { configSiteId, result = 1 });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}