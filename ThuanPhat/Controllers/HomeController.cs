using Helpers;
using PagedList;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using ThuanPhat.Models;
using ThuanPhat.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System;
using ThuanPhat.Filters;

namespace ThuanPhat.Controllers
{
    public class HomeController : BaseController
    {
        private static string Email => WebConfigurationManager.AppSettings["email"];
        private static string Password => WebConfigurationManager.AppSettings["password"];

        #region Home
        [HttpGet, LanguageFilters]
        [Route("{culture:regex(^(?!.*(vcms|article|banner|contact|uploader|service)).*$)}", Order = 1)]
        [Route(Order = 2)]
        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                BannerDtos = BannerDtos(),
                ArticleDtos = ArticleDtos().Where(a => a.Active && a.Home && a.TypePost == TypePost.Article).OrderByDescending(a => a.CreateDate).Take(15),
                TrainingDtos = ArticleDtos().Where(a => a.Active && a.Home).OrderByDescending(a => a.CreateDate).Take(5),
                TrainingCatDtos = ArticleCategoryDtos().Where(a => a.CategoryActive && a.ShowHome && a.TypePost == TypePost.Training).OrderBy(a => a.CategorySort),
                ServiceDtos = ServiceDtos().Where(a => a.Active && a.Home).OrderByDescending(a => a.CreateDate),
                ServiceCategoryDtos = ServiceCategoryDtos().Where(a => a.CategoryActive && a.ShowHome && a.ParentId == null).OrderBy(a => a.CategorySort),
                IntroduceDtos = ArticleDtos().Where(a => a.Active && a.Home && a.TypePost == TypePost.Introduce).OrderByDescending(a => a.CreateDate).Take(4),
                RecruitDto = ArticleCategoryDtos().FirstOrDefault(a => a.ShowHome && a.TypePost == TypePost.Recruit),
                ConfigSiteDto = ConfigSiteDto(),
            };
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var model = new HeaderViewModel
            {
                ArticleCategoryDtos = ArticleCategoryDtos().Where(a => a.ShowMenu && a.TypePost != TypePost.Introduce),
                IntroduceCatDto = ArticleCategoryDtos().Where(a => a.ShowMenu && a.TypePost == TypePost.Introduce),
                ServiceCategoryDtos = ServiceCategoryDtos().Where(a => a.ShowMenu),
                ConfigSiteDto = ConfigSiteDto(),
                ArticleDtos = ArticleDtos().Where(a => a.Active && a.ShowMenu)
            };
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var model = new FooteViewModel
            {
                ConfigSiteDto = ConfigSiteDto(),
            };
            return PartialView(model);
        }
        #endregion

        [HttpGet, LanguageFilters]
        [Route("{culture:regex(^(?!.*vi).*$)}/contact-us", Order = 1)]
        [Route("{culture=vi}/lien-he", Order = 2)]
        public ActionResult Contact()
        {
            var model = new ContactViewModel
            {
                ConfigSiteDto = ConfigSiteDto(),
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ContactForm(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Please enter the true format" });
            }
            _unitOfWork.ContactRepository.Insert(model.Contact);
            _unitOfWork.Save();
            var subject = "Email liên hệ từ website: " + Request.Url?.Host;
            var body = $"<p>Tên người liên hệ: {model.Contact.FullName},</p>" +
                       $"<p>Số điện thoại: {model.Contact.Mobile},</p>" +
                       $"<p>Email: {model.Contact.Email},</p>" +
                       $"<p>Nội dung: {model.Contact.Body}</p>" +
                       $"<p>Đây là hệ thống gửi email tự động, vui lòng không phản hồi lại email này.</p>";
            Task.Run(() => HtmlHelpers.SendEmail("gmail", subject, body, ConfigSiteDto().Email, Email, Email, Password, "Thuận Phát"));

            return Json(new { status = true, msg = "We will get back to you as soon as possible !!" });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult SubcribeForm(string email)
        {
            var isEmail = new EmailAddressAttribute().IsValid(email);
            if (!isEmail || string.IsNullOrEmpty(email))
            {
                return Json(new { status = false, msg = "Invalid email, please try again!" });
            }

            Subcribe model = new Subcribe { Email = email };

            _unitOfWork.SubcribeRepository.Insert(model);
            _unitOfWork.Save();
            return Json(new { status = true, msg = "Personal registration you believe successful!" });
        }

        [HttpGet, LanguageFilters]
        [Route("{culture:regex(^(?!.*vi).*$)}/recruit", Order = 1)]
        [Route("{culture=vi}/dang-ky-tuyen-dung", Order = 2)]
        public ActionResult Recruit(string result = "")
        {
            ViewBag.Result = result;
            var model = new RecruitViewModel
            {
                SelectGroup = new SelectList(CareerDtos().Where(a => a.Active).OrderBy(a => a.Sort), "Id", "Name"),
            };
            return View(model);
        }
        [HttpGet, LanguageFilters]
        [Route("{culture:regex(^(?!.*vi).*$)}/recruit", Order = 1)]
        [Route("{culture=vi}/dang-ky-tuyen-dung", Order = 2)]
        [HttpPost, ValidateInput(false)]
        public ActionResult Recruit(RecruitViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Recruit.Resume"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif|svg"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg, svg");
                        isPost = false;
                    }
                    else
                    {
                        if (file.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                            isPost = false;
                        }
                        else
                        {
                            var imgPath = "/images/recruits/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                            model.Recruit.Resume = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                            file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                        }
                    }
                }
                if (isPost)
                {
                    _unitOfWork.RecruitRepository.Insert(model.Recruit);
                    _unitOfWork.Save();
                    return RedirectToAction("Recruit", new { result = "success" });
                }
            }

            return View(model);
        }

        [HttpGet, LanguageFilters]
        [Route("{culture:regex(^(?!.*vi).*$)}/introduce", Order = 1)]
        [Route("{culture=vi}/gioi-thieu", Order = 2)]
        public ActionResult Introduce()
        {
            var introduce = ArticleCategoryDtos().FirstOrDefault(a => a.ParentId == null && a.TypePost == TypePost.Introduce);
            var model = new IntroduceViewModel
            {
                IntroduceDto = introduce,
                RecruitDto = ArticleCategoryDtos().FirstOrDefault(a => a.ShowHome && a.TypePost == TypePost.Recruit),
            };
            return View(model);
        }

        #region Article 
        [HttpGet, LanguageFilters]
        [Route("{culture:regex(^(?!.*vi).*$)}/blog/{url}.html", Order = 1)]
        [Route("{culture=vi}/blog/{url}.html", Order = 2)]
        public ActionResult ArticleDetail(string url)
        {
            var article = ArticleDtos().FirstOrDefault(a => a.Url == url && a.Active);
            var articles = ArticleDtos().Where(
                a => a.Id != article.Id && a.Active && (a.ArticleCategoryId == article.ArticleCategoryId || a.ParentId == article.Id)).OrderByDescending(a => a.CreateDate).Take(12);
            if (article == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ArticleDetailsViewModel
            {
                ArticleDto = article,
                ArticleDtos = articles,
            };
            return View(model);
        }

        [HttpGet, LanguageFilters]
        [Route("{culture:regex(^(?!.*vi).*$)}/blog/{url}", Order = 1)]
        [Route("{culture=vi}/blog/{url}", Order = 2)]
        public ActionResult ArticleCategory(int? page, string url)
        {
            var category = ArticleCategoryDtos().FirstOrDefault(a => a.CategoryActive && a.Url == url);
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var articles = ArticleDtos().Where(
                a => a.Active && (a.ArticleCategoryId == category.Id || a.ParentId == category.Id)).OrderByDescending(a => a.CreateDate);
            var pageNumber = page ?? 1;

            if (articles.Count() == 1)
            {
                var fi = articles.First();
                return RedirectToAction("ArticleDetail", new { url = fi.Url });
            }
            var model = new ArticleCategoryViewModel
            {
                CategoryDto = category,
                ArticleDtos = articles.ToPagedList(pageNumber, 12),
                CategoryDtos = ArticleCategoryDtos().Where(a => a.CategoryActive && a.TypePost != TypePost.Introduce).OrderBy(a => a.CategorySort),
                IntroduceCatDto = ArticleCategoryDtos().FirstOrDefault(a => a.CategoryActive && a.TypePost == TypePost.Introduce),
            };
            return View(model);
        }

        [HttpGet, LanguageFilters]
        [Route("{culture:regex(^(?!.*vi).*$)}/blogs", Order = 1)]
        [Route("{culture=vi}/blogs", Order = 2)]
        public ActionResult AllArticle(int? page)
        {
            var pageNumber = page ?? 1;
            var article = ArticleDtos().Where(a => a.Active).OrderByDescending(a => a.CreateDate);
            var model = new AllArticleViewModel()
            {
                ArticleDtos = article.ToPagedList(pageNumber, 12),
                CategoryDtos = ArticleCategoryDtos().Where(a => a.CategoryActive && a.TypePost != TypePost.Introduce).OrderBy(a => a.CategorySort),
                IntroduceCatDto = ArticleCategoryDtos().FirstOrDefault(a => a.CategoryActive && a.TypePost == TypePost.Introduce),
            };
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult MenuArticle(int rootId = 0, int catId = 0)
        {
            var model = new MenuArticleViewModel
            {
                ArticleDtos = ArticleDtos().Where(l => l.Active && l.TypePost == TypePost.Article).OrderByDescending(a => a.CreateDate).Take(8),
                ArticleCategoryDtos = ArticleCategoryDtos().Where(a => a.TypePost == TypePost.Article || a.TypePost == TypePost.Introduce),
            };
            return PartialView(model);
        }

        [Route("{culture:regex(^(?!.*vi).*$)}/search", Order = 1)]
        [Route("{culture=vi}/tim-kiem", Order = 2)]
        public ActionResult SearchArticle(int? page, string keywords)
        {
            var pageNumber = page ?? 1;
            var pageSize = 12;

            var newkey = keywords.Trim();
            var articles = ArticleDtos().Where(l => l.Active && l.Subject.Contains(newkey)).OrderByDescending(a => a.CreateDate);

            if (string.IsNullOrEmpty(newkey))
            {
                return RedirectToAction("Index");
            }

            var model = new ArticleSearchViewModel
            {
                ArticleDtos = articles.ToPagedList(pageNumber, pageSize),
                Keywords = keywords,
                CategoryDtos = ArticleCategoryDtos(),
            };
            return View(model);
        }
        #endregion

        #region Service
        [HttpGet, LanguageFilters]
        [Route("{culture:regex(^(?!.*vi).*$)}/services", Order = 1)]
        [Route("{culture=vi}/dich-vu", Order = 2)]
        public ActionResult AllService(int? page)
        {
            var pageNumber = page ?? 1;
            var serviceCatDtos = ServiceCategoryDtos().Where(a => a.CategoryActive).OrderBy(a => a.CategorySort);
            var model = new AllServiceViewModel()
            {
                ServiceCategoryDtos = serviceCatDtos.ToPagedList(pageNumber, 12),
                ServiceDtos = ServiceDtos(),
            };
            return View(model);
        }

        [HttpGet, LanguageFilters]
        [Route("{culture:regex(^(?!.*vi).*$)}/services/{url}", Order = 1)]
        [Route("{culture=vi}/dich-vu/{url}", Order = 2)]
        public ActionResult ServiceCategory(int? page, string url)
        {
            var category = ServiceCategoryDtos().FirstOrDefault(a => a.CategoryActive && a.Url == url);
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var services = ServiceDtos().Where(
                a => a.Active && (a.ServiceCategoryId == category.Id || a.ParentId == category.Id)).OrderByDescending(a => a.CreateDate);
            var pageNumber = page ?? 1;

            if (services.Count() == 1)
            {
                var fi = services.First();
                return RedirectToAction("ServiceDetail", new { url = fi.Url });
            }
            var model = new ServiceCategoryViewModel()
            {
                CategoryDto = category,
                ServiceDtos = services.ToPagedList(pageNumber, 12),
            };
            return View(model);
        }

        [HttpGet, LanguageFilters]
        [Route("{culture:regex(^(?!.*vi).*$)}/services/{url}.html", Order = 1)]
        [Route("{culture=vi}/dich-vu/{url}.html", Order = 2)]
        public ActionResult ServiceDetail(string url)
        {
            var service = ServiceDtos().FirstOrDefault(a => a.Url == url && a.Active);
            var services = ServiceDtos().Where(a => a.Id != service.Id && a.Active && 
            (a.ServiceCategoryId == service.ServiceCategoryId || a.ParentId == service.Id)).OrderByDescending(a => a.CreateDate).Take(12);
            if (service == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ServiceDetailViewModel
            {
                ServiceDto = service,
                ServiceDtos = services,
                ServiceCategoryLang = _unitOfWork.ServiceCategoryLangRepository.Get(a => a.ServiceCategoryId == service.ServiceCategoryId).FirstOrDefault(),
            };
            return View(model);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}