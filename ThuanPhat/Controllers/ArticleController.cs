﻿using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using ThuanPhat.DAL;
using ThuanPhat.Models;
using ThuanPhat.ViewModel;

namespace ThuanPhat.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private IEnumerable<ArticleCategory> ArticleCategories => _unitOfWork.ArticleCategoryRepository.Get();

        #region ArticleCategory
        [ChildActionOnly]
        public ActionResult ListCategory()
        {
            var allcats = _unitOfWork.ArticleCategoryRepository.Get(orderBy: q => q.OrderBy(a => a.CategorySort));
            return PartialView(allcats);
        }
        public ActionResult ArticleCategory(string result = "")
        {
            ViewBag.ArticleCat = result;
            ViewBag.RootCats =
                new SelectList(
                    _unitOfWork.ArticleCategoryRepository.Get(a => a.ParentId == null,
                                                              q => q.OrderBy(a => a.CategorySort)), "Id", "CategoryName");
            var articleCat = new ArticleCategory { CategorySort = 1, CategoryActive = true };
            return View(articleCat);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ArticleCategory(ArticleCategory category)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/articleCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Image")
                    {
                        category.Image = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "AboutImage")
                    {
                        category.AboutImage = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "FormationImage")
                    {
                        category.FormationImage = imgFile;
                    }
                }
                category.Url = HtmlHelpers.ConvertToUnSign(null, category.Url ?? category.CategoryName);
                _unitOfWork.ArticleCategoryRepository.Insert(category);
                _unitOfWork.Save();
                return RedirectToAction("ArticleCategory", new { result = "success" });
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(l => l.ParentId == null, a => a.OrderBy(c => c.CategorySort)), "Id", "CategoryName");
            return View(category);
        }
        public ActionResult UpdateCategory(int catId = 0)
        {
            var category = _unitOfWork.ArticleCategoryRepository.GetById(catId);
            if (category == null)
            {
                return RedirectToAction("ArticleCategory");
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(l => l.ParentId == null, a => a.OrderBy(c => c.CategorySort)), "Id", "CategoryName");
            return View(category);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateCategory(ArticleCategory model, FormCollection fc)
        {
            var category = _unitOfWork.ArticleCategoryRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/articleCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Image")
                    {
                        category.Image = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "AboutImage")
                    {
                        category.AboutImage = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "FormationImage")
                    {
                        category.FormationImage = imgFile;
                    }
                }
                category.Url = HtmlHelpers.ConvertToUnSign(null, category.Url ?? category.CategoryName);
                category.CategoryName = model.CategoryName;
                category.Description = model.Description;
                category.CategorySort = model.CategorySort;
                category.CategoryActive = model.CategoryActive;
                category.ParentId = model.ParentId;
                category.ShowMenu = model.ShowMenu;
                category.ShowHome = model.ShowHome;
                category.TitleMeta = model.TitleMeta;
                category.DescriptionMeta = model.DescriptionMeta;
                category.TypePost = model.TypePost;
                category.AboutText = model.AboutText;
                category.MottoText = model.MottoText;
                category.FormationText = model.FormationText;
                _unitOfWork.ArticleCategoryRepository.Update(category);
                _unitOfWork.Save();
                return RedirectToAction("ArticleCategory", new { result = "update" });
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(a => a.ParentId == null, q => q.OrderBy(a => a.CategorySort)), "Id", "CategoryName");
            return View(category);
        }
        [HttpPost]
        public bool DeleteCategory(int catId = 0)
        {

            var category = _unitOfWork.ArticleCategoryRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            _unitOfWork.ArticleCategoryRepository.Delete(category);
            _unitOfWork.Save();
            return true;
        }
        public bool UpdateArticleCat(int sort = 1, bool active = false, bool menu = false, bool home = false, int articleCatId = 0)
        {
            var articleCat = _unitOfWork.ArticleCategoryRepository.GetById(articleCatId);
            if (articleCat == null)
            {
                return false;
            }
            articleCat.CategorySort = sort;
            articleCat.CategoryActive = active;
            articleCat.ShowMenu = menu;
            articleCat.ShowHome = home;

            _unitOfWork.Save();
            return true;
        }
        public ActionResult UpdateArticleCategoryLang(int catId, int result = 0)
        {
            ViewBag.Result = result;
            var category = _unitOfWork.ArticleCategoryRepository.GetById(catId);
            return View(category);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateArticleCategoryLang(FormCollection fc)
        {
            var catId = Convert.ToInt32(fc["ArticleCategoryId"]);
            var langId = Convert.ToInt32(fc["LangId"]);
            var name = fc["Name"];
            var description = fc["Description"];
            var url = fc["Url"];
            var titleMeta = fc["TitleMeta"];
            var descriptionMeta = fc["DescriptionMeta"];
            var aboutText = fc["AboutText"];
            var mottoText = fc["MottoText"];
            var formationText = fc["FormationText"];
            var aboutImage = "";

            var file = Request.Files["AboutImage"];
            if (file != null && file.ContentLength > 0)
            {
                if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif|svg"))
                {
                    ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg, svg");
                }
                else
                {
                    if (file.ContentLength > 4000 * 1024)
                    {
                        ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                    }
                    else
                    {
                        var imgPath = "/images/articleCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                        HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                        var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                        aboutImage = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                        file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                    }
                }
            }

            var conceptCategoryLang = _unitOfWork.ArticleCategoryLangRepository.GetQuery(a => a.ArticleCategoryId == catId && a.LanguageId == langId).SingleOrDefault();

            if (conceptCategoryLang == null)
            {
                _unitOfWork.ArticleCategoryLangRepository.Insert(new ArticleCategoryLang
                {
                    ArticleCategoryId = catId,
                    LanguageId = langId,
                    CategoryName = name,
                    Url = HtmlHelpers.ConvertToUnSign(null, url ?? name),
                    TitleMeta = titleMeta,
                    DescriptionMeta = descriptionMeta,
                    Description = description,
                    AboutText = aboutText,
                    MottoText = mottoText,
                    FormationText = formationText,
                    AboutImage = aboutImage,
                });
                _unitOfWork.Save();
                return RedirectToAction("UpdateArticleCategoryLang", new { catId, result = 1 });
            }
            conceptCategoryLang.CategoryName = name;
            conceptCategoryLang.Description = description;
            conceptCategoryLang.Url = HtmlHelpers.ConvertToUnSign(null, url ?? name);
            conceptCategoryLang.TitleMeta = titleMeta;
            conceptCategoryLang.DescriptionMeta = descriptionMeta;
            conceptCategoryLang.AboutText = aboutText;
            conceptCategoryLang.MottoText = mottoText;
            conceptCategoryLang.FormationText = formationText;
            if (aboutImage != "")
            {
                conceptCategoryLang.AboutImage = aboutImage;
            }
            else
            {
                conceptCategoryLang.AboutImage = null;
            }

            _unitOfWork.Save();
            return RedirectToAction("UpdateArticleCategoryLang", new { catId, result = 1 });
        }
        #endregion

        #region Article
        public ActionResult ListArticle(int? page, string name, int? catId, int? childId, string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var article = _unitOfWork.ArticleRepository.GetQuery(orderBy: l => l.OrderByDescending(a => a.Id));

            if (childId.HasValue)
            {
                article = article.Where(l => l.ArticleCategoryId == childId);
            }
            else if (catId.HasValue)
            {
                article = article.Where(l => l.ArticleCategoryId == catId || l.ArticleCategory.ParentId == catId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                article = article.Where(l => l.Subject.ToLower().Contains(name.ToLower()));
            }
            var model = new ListArticleViewModel
            {
                SelectCategories = new SelectList(ArticleCategories.Where(a => a.ParentId == null), "Id", "CategoryName"),
                Articles = article.ToPagedList(pageNumber, pageSize),
                CatId = catId,
                ChildId = childId,
                Name = name
            };

            if (catId.HasValue)
            {
                model.ChildCategoryList =
                    new SelectList(ArticleCategories.Where(a => a.ParentId == catId), "Id", "CategoryName");
            }
            return View(model);
        }
        public ActionResult Article()
        {
            var model = new InsertArticleViewModel
            {
                Categories = ArticleCategories,
                Article = new Article { Active = true, Sort = 1 }
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Article(InsertArticleViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Article.Image"];
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
                            //var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);
                            var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(file.FileName)) + "-" + DateTime.Now.Millisecond + Path.GetExtension(file.FileName);
                            var imgPath = "/images/articles/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                            model.Article.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                            //file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));

                            var newImage = Image.FromStream(file.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 800, 600, false);
                            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                        }
                    }
                }

                if (isPost)
                {
                    model.Article.Url = HtmlHelpers.ConvertToUnSign(null, model.Article.Url ?? model.Article.Subject);
                    var articles = _unitOfWork.ArticleRepository.GetQuery().AsNoTracking();
                    if (articles.Any(p => p.Url.ToLower().Trim() == model.Article.Url.ToLower().Trim()))
                    {
                        model.Article.Url += "-" + DateTime.Now.Millisecond;
                    }
                    model.Article.ArticleCategoryId = Convert.ToInt32(fc["CategoryId"]);
                    _unitOfWork.ArticleRepository.Insert(model.Article);
                    _unitOfWork.Save();
                    return RedirectToAction("ListArticle", new { result = "success" });
                }
            }

            model.Categories = ArticleCategories;
            return View(model);
        }
        public ActionResult UpdateArticle(int articleId = 0)
        {
            var article = _unitOfWork.ArticleRepository.GetById(articleId);
            if (article == null)
            {
                return RedirectToAction("ListArticle");
            }
            var model = new InsertArticleViewModel
            {
                Article = article,
                Categories = ArticleCategories,
                SelectCategories = new SelectList(ArticleCategories, "Id", "CategoryName")

            };
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateArticle(InsertArticleViewModel model, FormCollection fc)
        {
            var article = _unitOfWork.ArticleRepository.GetById(model.Article.Id);
            if (article == null)
            {
                return RedirectToAction("ListArticle");
            }
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Article.Image"];
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
                            var imgPath = "/images/articles/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            HtmlHelpers.DeleteFile(Server.MapPath("/images/articles/" + article.Image));
                            article.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                            var newImage = Image.FromStream(file.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 800, 600, false);
                            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                        }
                    }
                }
                else
                {
                    article.Image = fc["CurrentFile"] == "" ? null : fc["CurrentFile"];
                }

                if (isPost)
                {
                    article.Url = HtmlHelpers.ConvertToUnSign(null, model.Article.Url ?? model.Article.Subject);
                    article.ArticleCategoryId = Convert.ToInt32(fc["CategoryId"]);
                    article.Subject = model.Article.Subject;
                    article.Description = model.Article.Description;
                    article.Body = model.Article.Body;
                    article.Active = model.Article.Active;
                    article.Home = model.Article.Home;
                    article.ShowMenu = model.Article.ShowMenu;
                    article.TitleMeta = model.Article.TitleMeta;
                    article.DescriptionMeta = model.Article.DescriptionMeta;


                    var articles = _unitOfWork.ArticleRepository.GetQuery().AsNoTracking();
                    if (articles.Any(p => p.Url.ToLower().Trim() == model.Article.Url.ToLower().Trim() && p.Id != model.Article.Id))
                    {
                        model.Article.Url += "-" + DateTime.Now.Millisecond;
                    }
                    _unitOfWork.Save();
                    return RedirectToAction("ListArticle", new { result = "update" });
                }
            }
            model.Categories = ArticleCategories;
            return View(model);
        }
        [HttpPost]
        public bool DeleteArticle(int articleId = 0)
        {

            var article = _unitOfWork.ArticleRepository.GetById(articleId);
            if (article == null)
            {
                return false;
            }
            _unitOfWork.ArticleRepository.Delete(article);
            _unitOfWork.Save();
            return true;
        }
        public JsonResult GetChildCategory(int? parentId)
        {
            var categories = ArticleCategories.Where(a => a.ParentId == parentId);
            return Json(categories.Select(a => new { a.Id, Name = a.CategoryName }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateArticleLang(int artId, int result = 0)
        {
            ViewBag.Result = result;
            var article = _unitOfWork.ArticleRepository.GetById(artId);
            return View(article);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateArticleLang(FormCollection fc)
        {
            var artId = Convert.ToInt32(fc["ArticleId"]);
            var langId = Convert.ToInt32(fc["LangId"]);
            var name = fc["Name"];
            var titleMeta = fc["TitleMeta"];
            var descriptionMeta = fc["DescriptionMeta"];
            var body = fc["Body"];
            var description = fc["Description"];
            var url = fc["Url"];

            var conceptLang = _unitOfWork.ArticleLangRepository.GetQuery(a => a.ArticleId == artId && a.LanguageId == langId).SingleOrDefault();

            if (conceptLang == null)
            {
                _unitOfWork.ArticleLangRepository.Insert(new ArticleLang
                {
                    ArticleId = artId,
                    LanguageId = langId,
                    Subject = name,
                    Body = body,
                    Description = description,
                    TitleMeta = titleMeta,
                    DescriptionMeta = descriptionMeta,
                    Url = HtmlHelpers.ConvertToUnSign(null, url ?? name)
                });
                _unitOfWork.Save();
                return RedirectToAction("UpdateArticleLang", new { artId, result = 1 });
            }

            conceptLang.Subject = name;
            conceptLang.Description = description;
            conceptLang.Body = body;
            conceptLang.TitleMeta = titleMeta;
            conceptLang.DescriptionMeta = descriptionMeta;
            conceptLang.Url = HtmlHelpers.ConvertToUnSign(null, url ?? name);

            _unitOfWork.Save();

            return RedirectToAction("UpdateArticleLang", new { artId, result = 1 });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}