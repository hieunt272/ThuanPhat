using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuanPhat.DAL;
using ThuanPhat.Models;

namespace ThuanPhat.Controllers
{
    public class BaseController : Controller
    {
        public readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public IEnumerable<ArticleCategoryDto> ArticleCategoryDtos()
        {
            IEnumerable<ArticleCategoryDto> articleCategoryDtos;
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "ja":
                    articleCategoryDtos = _unitOfWork.ArticleCategoryLangRepository.GetQuery(a => a.ArticleCategory.CategoryActive && a.LanguageId == 2, q => q.OrderBy(a => a.ArticleCategory.CategorySort)).Select(a => new ArticleCategoryDto
                    {
                        Id = a.ArticleCategoryId,
                        CategorySort = a.ArticleCategory.CategorySort,
                        CategoryName = a.CategoryName,
                        Description = a.Description,
                        Url = a.Url,
                        TitleMeta = a.TitleMeta,
                        DescriptionMeta = a.DescriptionMeta,
                        ParentId = a.ArticleCategory.ParentId,
                        ShowHome = a.ArticleCategory.ShowHome,
                        Image = a.ArticleCategory.Image,
                        ShowMenu = a.ArticleCategory.ShowMenu,
                        CategoryActive = a.ArticleCategory.CategoryActive,
                        TypePost = a.ArticleCategory.TypePost,
                        RootUrl = a.ArticleCategory.ParentCategory.Url,
                        RootName = a.ArticleCategory.ParentCategory.CategoryName,
                        AboutImage = a.AboutImage,
                        FormationImage = a.ArticleCategory.FormationImage,
                        AboutText = a.AboutText,
                        MottoText = a.MottoText,
                        FormationText = a.FormationText,
                    });
                    break;
                default:
                    articleCategoryDtos = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort)).Select(a => new ArticleCategoryDto
                    {
                        Id = a.Id,
                        CategorySort = a.CategorySort,
                        CategoryName = a.CategoryName,
                        Description = a.Description,
                        Url = a.Url,
                        TypePost = a.TypePost,
                        TitleMeta = a.TitleMeta,
                        DescriptionMeta = a.DescriptionMeta,
                        ParentId = a.ParentId,
                        ShowHome = a.ShowHome,
                        Image = a.Image,
                        ShowMenu = a.ShowMenu,
                        CategoryActive = a.CategoryActive,
                        RootName = a.ParentCategory.CategoryName,
                        RootUrl = a.ParentCategory.Url,
                        AboutImage = a.AboutImage,
                        FormationImage = a.FormationImage,
                        AboutText = a.AboutText,
                        MottoText = a.MottoText,
                        FormationText = a.FormationText,
                    });
                    break;
            }
            return articleCategoryDtos;
        }
        public IQueryable<ArticleDto> ArticleDtos()
        {
            IQueryable<ArticleDto> articleDtos;
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "ja":
                    articleDtos = _unitOfWork.ArticleLangRepository.GetQuery(a => a.Article.Active && a.LanguageId == 2, q => q.OrderBy(a => a.Article.Sort).ThenByDescending(a => a.Article.CreateDate)).Select(a => new ArticleDto
                    {
                        Id = a.ArticleId,
                        Subject = a.Subject,
                        Description = a.Description,
                        Image = a.Article.Image,
                        TitleMeta = a.TitleMeta,
                        DescriptionMeta = a.DescriptionMeta,
                        ArticleCategoryId = a.Article.ArticleCategoryId,
                        Body = a.Body,
                        CreateDate = a.Article.CreateDate,
                        Home = a.Article.Home,
                        Active = a.Article.Active,
                        CategoryName = a.Article.ArticleCategory.ArticleCategoryLangs.FirstOrDefault(c => c.LanguageId == 2).CategoryName,
                        CategoryUrl = a.Article.ArticleCategory.ArticleCategoryLangs.FirstOrDefault(c => c.LanguageId == 2).Url,
                        Url = a.Url,
                        ParentId = a.Article.ArticleCategory.ParentId,
                        TypePost = a.Article.ArticleCategory.TypePost,
                    });
                    break;
                default:
                    articleDtos = _unitOfWork.ArticleRepository.GetQuery(a => a.Active, q => q.OrderBy(a => a.Sort).ThenByDescending(a => a.CreateDate)).Select(a => new ArticleDto
                    {
                        Id = a.Id,
                        Subject = a.Subject,
                        Description = a.Description,
                        Image = a.Image,
                        TitleMeta = a.TitleMeta,
                        DescriptionMeta = a.DescriptionMeta,
                        ArticleCategoryId = a.ArticleCategoryId,
                        Body = a.Body,
                        CreateDate = a.CreateDate,
                        Home = a.Home,
                        Active = a.Active,
                        Url = a.Url,
                        CategoryName = a.ArticleCategory.CategoryName,
                        CategoryUrl = a.ArticleCategory.Url,
                        ParentId = a.ArticleCategory.ParentId,
                        TypePost = a.ArticleCategory.TypePost
                    });
                    break;
            }
            return articleDtos;
        }
        public IQueryable<BannerDto> BannerDtos()
        {
            IQueryable<BannerDto> bannerDtos;
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "ja":
                    bannerDtos = _unitOfWork.BannerLangRepository.GetQuery(a => a.Banner.Active && a.LanguageId == 2, q => q.OrderBy(a => a.Banner.Sort)).Select(a => new BannerDto
                    {
                        Id = a.BannerId,
                        BannerName = a.BannerName,
                        Image = a.Image,
                        Slogan = a.Slogan,
                        GroupId = a.Banner.GroupId,
                        Sort = a.Banner.Sort,
                        Content = a.Content,
                        Url = a.Url
                    });
                    if (!bannerDtos.Any())
                    {
                        bannerDtos = _unitOfWork.BannerRepository.GetQuery(a => a.Active, q => q.OrderBy(a => a.Sort)).Select(a => new BannerDto
                        {
                            Id = a.Id,
                            BannerName = a.BannerName,
                            Image = a.Image,
                            Slogan = a.Slogan,
                            GroupId = a.GroupId,
                            Sort = a.Sort,
                            Content = a.Content,
                            Url = a.Url
                        });
                    }
                    break;
                default:
                    bannerDtos = _unitOfWork.BannerRepository.GetQuery(a => a.Active, q => q.OrderBy(a => a.Sort)).Select(a => new BannerDto
                    {
                        Id = a.Id,
                        BannerName = a.BannerName,
                        Image = a.Image,
                        Slogan = a.Slogan,
                        GroupId = a.GroupId,
                        Content = a.Content,
                        Sort = a.Sort,
                        Url = a.Url
                    });
                    break;
            }
            return bannerDtos;
        }
        public IEnumerable<ServiceCategoryDto> ServiceCategoryDtos()
        {
            IEnumerable<ServiceCategoryDto> serviceCategoryDtos;
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "ja":
                    serviceCategoryDtos = _unitOfWork.ServiceCategoryLangRepository.GetQuery(a => a.ServiceCategory.CategoryActive && a.LanguageId == 2, q => q.OrderBy(a => a.ServiceCategory.CategorySort)).Select(a => new ServiceCategoryDto
                    {
                        Id = a.ServiceCategoryId,
                        CategorySort = a.ServiceCategory.CategorySort,
                        CategoryName = a.CategoryName,
                        Description = a.Description,
                        Url = a.Url,
                        TitleMeta = a.TitleMeta,
                        DescriptionMeta = a.DescriptionMeta,
                        ParentId = a.ServiceCategory.ParentId,
                        Image = a.ServiceCategory.Image,
                        ShowMenu = a.ServiceCategory.ShowMenu,
                        RootUrl = a.ServiceCategory.ParentCategory.Url,
                        RootName = a.ServiceCategory.ParentCategory.CategoryName,
                        CategoryActive = a.ServiceCategory.CategoryActive,
                    });
                    break;
                default:
                    serviceCategoryDtos = _unitOfWork.ServiceCategoryRepository.GetQuery(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort)).Select(a => new ServiceCategoryDto
                    {
                        Id = a.Id,
                        CategorySort = a.CategorySort,
                        CategoryName = a.CategoryName,
                        Description = a.Description,
                        Url = a.Url,
                        TitleMeta = a.TitleMeta,
                        DescriptionMeta = a.DescriptionMeta,
                        ParentId = a.ParentId,
                        Image = a.Image,
                        ShowMenu = a.ShowMenu,
                        RootName = a.ParentCategory.CategoryName,
                        RootUrl = a.ParentCategory.Url,
                        CategoryActive = a.CategoryActive
                    });
                    break;
            }
            return serviceCategoryDtos;
        }
        public IQueryable<ServiceDto> ServiceDtos()
        {
            IQueryable<ServiceDto> serviceDtos;
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "ja":
                    serviceDtos = _unitOfWork.ServiceLangRepository.GetQuery(a => a.Service.Active && a.LanguageId == 2, q => q.OrderByDescending(a => a.Service.CreateDate)).Select(a => new ServiceDto
                    {
                        Id = a.ServiceId,
                        ServiceName = a.ServiceName,
                        Description = a.Description,
                        Image = a.Service.Image,
                        TitleMeta = a.TitleMeta,
                        DescriptionMeta = a.DescriptionMeta,
                        Body = a.Body,
                        Active = a.Service.Active,
                        Url = a.Url,
                        CreateDate = a.Service.CreateDate,
                        ServiceCategoryId = a.Service.ServiceCategoryId,
                        CategoryUrl = a.Service.ServiceCategory.Url,
                        ParentId = a.Service.ServiceCategory.ParentId,
                        Home = a.Service.Home,
                        CategoryName = a.Service.ServiceCategory.CategoryName
                    });
                    break;
                default:
                    serviceDtos = _unitOfWork.ServiceRepository.GetQuery(a => a.Active, q => q.OrderByDescending(a => a.CreateDate)).Select(a => new ServiceDto
                    {
                        Id = a.Id,
                        ServiceName = a.ServiceName,
                        Description = a.Description,
                        Image = a.Image,
                        TitleMeta = a.TitleMeta,
                        DescriptionMeta = a.DescriptionMeta,
                        Body = a.Body,
                        Active = a.Active,
                        Url = a.Url,
                        CreateDate = a.CreateDate,
                        ServiceCategoryId = a.ServiceCategoryId,
                        CategoryUrl = a.ServiceCategory.Url,
                        ParentId = a.ServiceCategory.ParentId,
                        Home = a.Home,
                        CategoryName = a.ServiceCategory.CategoryName
                    });
                    break;
            }
            return serviceDtos;
        }
        public IQueryable<CareerDto> CareerDtos()
        {
            IQueryable<CareerDto> careerDtos;
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "ja":
                    careerDtos = _unitOfWork.CareerLangRepository.GetQuery(a => a.Career.Active && a.LanguageId == 2, q => q.OrderBy(a => a.Career.Sort)).Select(a => new CareerDto
                    {
                        Id = a.CareerId,
                        Name = a.Name,
                        Active = a.Career.Active,
                        Sort = a.Career.Sort,
                    });
                    break;
                default:
                    careerDtos = _unitOfWork.CareerRepository.GetQuery(a => a.Active, q => q.OrderBy(a => a.Sort)).Select(a => new CareerDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Active = a.Active,
                        Sort = a.Sort
                    });
                    break;
            }
            return careerDtos;
        }
        public ConfigSiteDto ConfigSiteDto()
        {
            ConfigSiteDto configSiteDto;
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "ja":
                    configSiteDto = (ConfigSiteDto)HttpContext.Application["ConfigSiteLang"] ?? (ConfigSiteDto)HttpContext.Application["ConfigSite"];
                    break;
                default:
                    configSiteDto = (ConfigSiteDto)HttpContext.Application["ConfigSite"];
                    break;
            }
            return configSiteDto;
        }
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}