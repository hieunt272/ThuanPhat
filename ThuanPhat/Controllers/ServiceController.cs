using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ThuanPhat.DAL;
using ThuanPhat.Models;
using ThuanPhat.ViewModel;

namespace ThuanPhat.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private IEnumerable<ServiceCategory> ServiceCategories => _unitOfWork.ServiceCategoryRepository.Get();

        #region ServiceCategory
        [ChildActionOnly]
        public ActionResult ListCategory()
        {
            var allcats = _unitOfWork.ServiceCategoryRepository.Get(orderBy: q => q.OrderBy(a => a.CategorySort));
            return PartialView(allcats);
        }
        public ActionResult ServiceCategory(string result = "")
        {
            ViewBag.ServiceCat = result;
            ViewBag.RootCats =
                new SelectList(
                    _unitOfWork.ServiceCategoryRepository.Get(a => a.ParentId == null,
                                                              q => q.OrderBy(a => a.CategorySort)), "Id", "CategoryName");

            var model = new ServiceCatViewModel
            {
                ServiceCategory = new ServiceCategory { CategoryActive = true, CategorySort = 1 }
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult ServiceCategory(ServiceCatViewModel category)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["ServiceCategory.Image"];
                if (file != null && file.ContentLength > 0)
                {
                    if (file.ContentType != "image/jpeg" & file.ContentType != "image/png" && file.ContentType != "image/gif")
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                    }
                    else
                    {
                        if (file.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                        }
                        else
                        {
                            var imgPath = "/images/serviceCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                            category.ServiceCategory.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                            var newImage = Image.FromStream(file.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                        }
                    }
                }
                category.ServiceCategory.Url = HtmlHelpers.ConvertToUnSign(null, category.ServiceCategory.Url ?? category.ServiceCategory.CategoryName);
                _unitOfWork.ServiceCategoryRepository.Insert(category.ServiceCategory);
                _unitOfWork.Save();

                var count = _unitOfWork.ServiceCategoryRepository.GetQuery(a => a.Url == category.ServiceCategory.Url).Count();
                if (count > 1)
                {
                    category.ServiceCategory.Url += "-" + category.ServiceCategory.Id;
                    _unitOfWork.Save();
                }

                return RedirectToAction("ServiceCategory", new { result = "success" });
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ServiceCategoryRepository.Get(l => l.ParentId == null, a => a.OrderBy(c => c.CategorySort)), "Id", "CategoryName");
            return View(category);
        }
        public ActionResult UpdateCategory(int catId = 0)
        {
            var category = _unitOfWork.ServiceCategoryRepository.GetById(catId);
            if (category == null)
            {
                return RedirectToAction("ServiceCategory");
            }
            var model = new ServiceCatViewModel
            {
                ServiceCategory = category,
            };
            ViewBag.RootCats = new SelectList(_unitOfWork.ServiceCategoryRepository.Get(l => l.ParentId == null, a => a.OrderBy(c => c.CategorySort)), "Id", "CategoryName");
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateCategory(ServiceCatViewModel model, FormCollection fc)
        {
            var category = _unitOfWork.ServiceCategoryRepository.GetById(model.ServiceCategory.Id);
            if (ModelState.IsValid)
            {
                var isPost = true;

                var file = Request.Files["ServiceCategory.Image"];
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
                            var imgPath = "/images/serviceCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                            category.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                            file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                        }
                    }
                }

                if (isPost)
                {
                    category.Url = HtmlHelpers.ConvertToUnSign(null, category.Url ?? category.CategoryName);
                    category.CategoryName = model.ServiceCategory.CategoryName;
                    category.Description = model.ServiceCategory.Description;
                    category.CategorySort = model.ServiceCategory.CategorySort;
                    category.CategoryActive = model.ServiceCategory.CategoryActive;
                    category.ParentId = model.ServiceCategory.ParentId;
                    category.ShowHome = model.ServiceCategory.ShowHome;
                    category.ShowMenu = model.ServiceCategory.ShowMenu;
                    category.TitleMeta = model.ServiceCategory.TitleMeta;
                    category.DescriptionMeta = model.ServiceCategory.DescriptionMeta;

                    _unitOfWork.ServiceCategoryRepository.Update(category);
                    _unitOfWork.Save();

                    var count = _unitOfWork.ServiceCategoryRepository.GetQuery(a => a.Url == category.Url).Count();
                    if (count > 1)
                    {
                        category.Url += "-" + category.Id;
                        _unitOfWork.Save();
                    }

                    return RedirectToAction("ServiceCategory", new { result = "update" });
                }
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ServiceCategoryRepository.Get(a => a.ParentId == null, q => q.OrderBy(a => a.CategorySort)), "Id", "CategoryName");
            return View(model);
        }
        [HttpPost]
        public bool DeleteCategory(int catId = 0)
        {

            var category = _unitOfWork.ServiceCategoryRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            _unitOfWork.ServiceCategoryRepository.Delete(category);
            _unitOfWork.Save();
            return true;
        }
        public bool UpdateServiceCat(int sort = 1, bool active = false, bool home = false, bool menu = false, int serviceCatId = 0)
        {

            var serviceCat = _unitOfWork.ServiceCategoryRepository.GetById(serviceCatId);
            if (serviceCat == null)
            {
                return false;
            }
            serviceCat.CategorySort = sort;
            serviceCat.CategoryActive = active;
            serviceCat.ShowMenu = menu;
            serviceCat.ShowHome = home;
            _unitOfWork.Save();
            return true;
        }

        public ActionResult UpdateServiceCategoryLang(int catId, int result = 0)
        {
            ViewBag.Result = result;
            var category = _unitOfWork.ServiceCategoryRepository.GetById(catId);
            return View(category);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateServiceCategoryLang(FormCollection fc)
        {
            var catId = Convert.ToInt32(fc["ServiceCategoryId"]);
            var langId = Convert.ToInt32(fc["LangId"]);
            var name = fc["Name"];
            var description = fc["Description"];
            var url = fc["Url"];
            var titleMeta = fc["TitleMeta"];
            var descriptionMeta = fc["DescriptionMeta"];

            var urlConvert = HtmlHelpers.ConvertToUnSign(null, url);

            var conceptCategoryLang = _unitOfWork.ServiceCategoryLangRepository.GetQuery(a => a.ServiceCategoryId == catId && a.LanguageId == langId).SingleOrDefault();

            if (conceptCategoryLang == null)
            {
                _unitOfWork.ServiceCategoryLangRepository.Insert(new ServiceCategoryLang
                {
                    ServiceCategoryId = catId,
                    LanguageId = langId,
                    CategoryName = name,
                    Url = HtmlHelpers.ConvertToUnSign(null, urlConvert ?? conceptCategoryLang.ServiceCategory.Url),
                    TitleMeta = titleMeta,
                    DescriptionMeta = descriptionMeta,
                    Description = description
                });
                _unitOfWork.Save();
                return RedirectToAction("UpdateServiceCategoryLang", new { catId, result = 1 });
            }
            conceptCategoryLang.CategoryName = name;
            conceptCategoryLang.Description = description;
            conceptCategoryLang.TitleMeta = titleMeta;
            conceptCategoryLang.DescriptionMeta = descriptionMeta;
            if (urlConvert == "")
            {
                conceptCategoryLang.Url = conceptCategoryLang.ServiceCategory.Url;
            }
            else
            {
                conceptCategoryLang.Url = urlConvert;
            }

            _unitOfWork.Save();
            return RedirectToAction("UpdateServiceCategoryLang", new { catId, result = 1 });
        }
        #endregion

        #region Service
        public ActionResult ListService(int? page, string name, int? catId, int? childId, string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var service = _unitOfWork.ServiceRepository.GetQuery(orderBy: l => l.OrderByDescending(a => a.Id));

            if (childId.HasValue)
            {
                service = service.Where(l => l.ServiceCategoryId == childId);
            }
            else if (catId.HasValue)
            {
                service = service.Where(l => l.ServiceCategoryId == catId || l.ServiceCategory.ParentId == catId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                service = service.Where(l => l.ServiceName.Contains(name));
            }
            var model = new ListServiceViewModel
            {
                SelectCategories = new SelectList(ServiceCategories.Where(a => a.ParentId == null), "Id", "CategoryName"),
                Services = service.ToPagedList(pageNumber, pageSize),
                CatId = catId,
                ChildId = childId,
                Name = name
            };
            if (catId.HasValue)
            {
                model.ChildCategoryList =
                    new SelectList(ServiceCategories.Where(a => a.ParentId == catId), "Id", "CategoryName");
            }
            return View(model);
        }
        public ActionResult Service()
        {
            var model = new InsertServiceViewModel
            {
                Categories = ServiceCategories,
                Service = new Service { Active = true }
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Service(InsertServiceViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Service.Image"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
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
                            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);
                            var imgPath = "/images/services/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                            model.Service.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                            var newImage = Image.FromStream(file.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 800, 600, false);
                            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                        }
                    }
                }

                if (isPost)
                {
                    model.Service.Url = HtmlHelpers.ConvertToUnSign(null, model.Service.Url ?? model.Service.ServiceName);
                    model.Service.ServiceCategoryId = Convert.ToInt32(fc["CategoryId"]);
                    _unitOfWork.ServiceRepository.Insert(model.Service);
                    _unitOfWork.Save();

                    var count = _unitOfWork.ServiceRepository.GetQuery(a => a.Url == model.Service.Url).Count();
                    if (count > 1)
                    {
                        model.Service.Url += "-" + model.Service.Id;
                        _unitOfWork.Save();
                    }

                    return RedirectToAction("ListService", new { result = "success" });
                }
            }

            model.Categories = ServiceCategories;
            return View(model);
        }
        public ActionResult UpdateService(int serviceId = 0)
        {
            var service = _unitOfWork.ServiceRepository.GetById(serviceId);
            if (service == null)
            {
                return RedirectToAction("ListService");
            }
            var model = new InsertServiceViewModel
            {
                Service = service,
                Categories = ServiceCategories,
                SelectCategories = new SelectList(ServiceCategories, "Id", "CategoryName"),
            };
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateService(InsertServiceViewModel model, FormCollection fc)
        {
            var service = _unitOfWork.ServiceRepository.GetById(model.Service.Id);
            if (service == null)
            {
                return RedirectToAction("ListService");
            }
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Service.Image"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                        isPost = false;
                    }
                    else
                    {
                        var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(file.FileName)) + "-" + DateTime.Now.Millisecond + Path.GetExtension(file.FileName);
                        if (file.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                            isPost = false;
                        }
                        else
                        {
                            var imgPath = "/images/services/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            HtmlHelpers.DeleteFile(Server.MapPath("/images/services/" + service.Image));
                            service.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                            var newImage = Image.FromStream(file.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 800, 600, false);
                            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                        }
                    }
                }

                if (isPost)
                {
                    service.Url = HtmlHelpers.ConvertToUnSign(null, model.Service.Url ?? model.Service.ServiceName);
                    service.ServiceCategoryId = Convert.ToInt32(fc["CategoryId"]);
                    service.ServiceName = model.Service.ServiceName;
                    service.Description = model.Service.Description;
                    service.Body = model.Service.Body;
                    service.Active = model.Service.Active;
                    service.Home = model.Service.Home;
                    service.TitleMeta = model.Service.TitleMeta;
                    service.DescriptionMeta = model.Service.DescriptionMeta;
                    _unitOfWork.Save();

                    var count = _unitOfWork.ServiceRepository.GetQuery(a => a.Url == service.Url).Count();
                    if (count > 1)
                    {
                        service.Url += "-" + service.Id;
                        _unitOfWork.Save();
                    }
                    _unitOfWork.ServiceRepository.Update(service);
                    _unitOfWork.Save();
                    return RedirectToAction("ListService", new { result = "update" });
                }
            }
            model.Categories = ServiceCategories;
            return View(model);
        }
        [HttpPost]
        public bool DeleteService(int serviceId = 0)
        {

            var service = _unitOfWork.ServiceRepository.GetById(serviceId);
            if (service == null)
            {
                return false;
            }
            _unitOfWork.ServiceRepository.Delete(service);
            _unitOfWork.Save();
            return true;
        }
        public JsonResult GetChildCategory(int? parentId)
        {
            var categories = ServiceCategories.Where(a => a.ParentId == parentId);
            return Json(categories.Select(a => new { a.Id, Name = a.CategoryName }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateServiceLang(int serviceId, int result = 0)
        {
            ViewBag.Result = result;
            var service = _unitOfWork.ServiceRepository.GetById(serviceId);
            return View(service);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateServiceLang(FormCollection fc)
        {
            var serviceId = Convert.ToInt32(fc["ServiceId"]);
            var langId = Convert.ToInt32(fc["LangId"]);
            var name = fc["Name"];
            var titleMeta = fc["TitleMeta"];
            var descriptionMeta = fc["DescriptionMeta"];
            var body = fc["Body"];
            var description = fc["Description"];
            var url = fc["Url"];

            var urlConvert = HtmlHelpers.ConvertToUnSign(null, url);

            var conceptLang = _unitOfWork.ServiceLangRepository.GetQuery(a => a.ServiceId == serviceId && a.LanguageId == langId).SingleOrDefault();

            if (conceptLang == null)
            {
                _unitOfWork.ServiceLangRepository.Insert(new ServiceLang
                {
                    ServiceId = serviceId,
                    LanguageId = langId,
                    ServiceName = name,
                    Body = body,
                    Description = description,
                    TitleMeta = titleMeta,
                    DescriptionMeta = descriptionMeta,
                    Url = HtmlHelpers.ConvertToUnSign(null, urlConvert ?? conceptLang.Service.Url),
                });
                _unitOfWork.Save();
                return RedirectToAction("UpdateServiceLang", new { serviceId, result = 1 });
            }

            conceptLang.ServiceName = name;
            conceptLang.Description = description;
            conceptLang.Body = body;
            conceptLang.TitleMeta = titleMeta;
            conceptLang.DescriptionMeta = descriptionMeta;
            if (urlConvert == "")
            {
                conceptLang.Url = conceptLang.Service.Url;
            }
            else
            {
                conceptLang.Url = urlConvert;
            }

            _unitOfWork.Save();

            return RedirectToAction("UpdateServiceLang", new { serviceId, result = 1 });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}