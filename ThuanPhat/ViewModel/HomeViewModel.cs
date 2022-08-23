using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuanPhat.Models;
using System.ComponentModel.DataAnnotations;

namespace ThuanPhat.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<BannerDto> BannerDtos { get; set; }
        public IEnumerable<ArticleDto> ArticleDtos { get; set; }
        public IEnumerable<ArticleDto> IntroduceDtos { get; set; }
        public IEnumerable<ArticleDto> TrainingDtos { get; set; }
        public IEnumerable<ServiceDto> ServiceDtos { get; set; }
        public IEnumerable<ServiceCategoryDto> ServiceCategoryDtos { get; set; }
        public ArticleCategoryDto RecruitDto { get; set; }
        public ConfigSiteDto ConfigSiteDto { get; set; }
        public IEnumerable<ArticleCategoryDto> TrainingCatDtos { get; set; } 
    }

    public class HeaderViewModel 
    { 
        public IEnumerable<ArticleCategoryDto> ArticleCategoryDtos { get; set; }
        public IEnumerable<ArticleCategoryDto> IntroduceCatDto { get; set; }
        public IEnumerable<ServiceCategoryDto> ServiceCategoryDtos { get; set; }
        public ConfigSiteDto ConfigSiteDto { get; set; }
        public IEnumerable<ArticleDto> ArticleDtos { get; set; } 
    }

    public class FooteViewModel
    {
        public ConfigSiteDto ConfigSiteDto { get; set; }
    }

    public class AllArticleViewModel
    {
        public IPagedList<ArticleDto> ArticleDtos { get; set; }
        public IEnumerable<ArticleCategoryDto> CategoryDtos { get; set; }
        public ConfigSiteDto ConfigSiteDto { get; set; }
    }
    public class ArticleCategoryViewModel
    {
        public ArticleCategoryDto CategoryDto { get; set; }
        public IPagedList<ArticleDto> ArticleDtos { get; set; }
        public IEnumerable<ArticleCategoryDto> CategoryDtos { get; set; }
    }
    public class ArticleDetailsViewModel 
    {
        public ArticleDto ArticleDto { get; set; }
        public IEnumerable<ArticleDto> ArticleDtos { get; set; }
    }
    public class MenuArticleViewModel
    {
        public IEnumerable<ArticleDto> ArticleDtos { get; set; }
        public IEnumerable<ArticleCategoryDto> ArticleCategoryDtos { get; set; }
    }
    public class ArticleSearchViewModel
    {
        public string Keywords { get; set; }
        public IPagedList<ArticleDto> ArticleDtos { get; set; }
        public IEnumerable<ArticleCategoryDto> CategoryDtos { get; set; }
    }
    public class IntroduceViewModel
    {
        public ArticleCategoryDto IntroduceDto { get; set; }
        public ArticleCategoryDto RecruitDto { get; set; }
    }
    public class AllServiceViewModel
    {
        public IPagedList<ServiceDto> ServiceDtos { get; set; }
    }
    public class ServiceCategoryViewModel
    {
        public ServiceCategoryDto CategoryDto { get; set; }
        public IPagedList<ServiceDto> ServiceDtos { get; set; }
    }
    public class ServiceDetailViewModel
    {
        public ServiceDto ServiceDto { get; set; }
        public IEnumerable<ServiceDto> ServiceDtos { get; set; }
    }

    public class RecruitViewModel
    {
        public Recruit Recruit { get; set; }
        public SelectList SelectGroup { get; set; }
    }

    public class ContactViewModel
    {
        public Contact Contact { get; set; }
        public ConfigSiteDto ConfigSiteDto { get; set; }
    }
}