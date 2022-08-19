using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThuanPhat.Models;

namespace ThuanPhat.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly DataEntities _context = new DataEntities();
        private GenericRepository<Admin> _adminRepository;
        private GenericRepository<ArticleCategory> _articategoryRepository;
        private GenericRepository<Article> _articleRepository;
        private GenericRepository<Banner> _bannerRepository;
        private GenericRepository<Contact> _contactRepository;
        private GenericRepository<ConfigSite> _configRepository;
        private GenericRepository<Subcribe> _subcribeRepository;
        private GenericRepository<Service> _serviceRepository;
        private GenericRepository<ServiceCategory> _serviceCategoryRepository;
        private GenericRepository<Career> _careerRepository;
        private GenericRepository<Recruit> _recruitRepository;
        private GenericRepository<Language> _languageRepository;

        public GenericRepository<ArticleLang> _articleLangRepository;
        public GenericRepository<ArticleCategoryLang> _articleCategoryLangRepository;
        public GenericRepository<BannerLang> _bannerLangRepository;
        public GenericRepository<CareerLang> _careerLangRepository;
        public GenericRepository<ConfigSiteLang> _configLangRepository;
        public GenericRepository<ServiceLang> _serviceLangRepository;
        public GenericRepository<ServiceCategoryLang> _serviceCategoryLangRepository;

        public GenericRepository<Language> LanguageRepository =>
            _languageRepository ?? (_languageRepository = new GenericRepository<Language>(_context));
        public GenericRepository<ArticleLang> ArticleLangRepository =>
            _articleLangRepository ?? (_articleLangRepository = new GenericRepository<ArticleLang>(_context));
        public GenericRepository<ArticleCategoryLang> ArticleCategoryLangRepository =>
            _articleCategoryLangRepository ?? (_articleCategoryLangRepository = new GenericRepository<ArticleCategoryLang>(_context));
        public GenericRepository<BannerLang> BannerLangRepository =>
            _bannerLangRepository ?? (_bannerLangRepository = new GenericRepository<BannerLang>(_context));
        public GenericRepository<CareerLang> CareerLangRepository =>
            _careerLangRepository ?? (_careerLangRepository = new GenericRepository<CareerLang>(_context));
        public GenericRepository<ConfigSiteLang> ConfigSiteLangRepository =>
            _configLangRepository ?? (_configLangRepository = new GenericRepository<ConfigSiteLang>(_context));
        public GenericRepository<ServiceLang> ServiceLangRepository =>
            _serviceLangRepository ?? (_serviceLangRepository = new GenericRepository<ServiceLang>(_context));
        public GenericRepository<ServiceCategoryLang> ServiceCategoryLangRepository =>
            _serviceCategoryLangRepository ?? (_serviceCategoryLangRepository = new GenericRepository<ServiceCategoryLang>(_context));
        public GenericRepository<Career> CareerRepository =>
            _careerRepository ?? (_careerRepository = new GenericRepository<Career>(_context));
        public GenericRepository<Recruit> RecruitRepository =>
            _recruitRepository ?? (_recruitRepository = new GenericRepository<Recruit>(_context));
        public GenericRepository<Service> ServiceRepository =>
            _serviceRepository ?? (_serviceRepository = new GenericRepository<Service>(_context));
        public GenericRepository<ServiceCategory> ServiceCategoryRepository =>
            _serviceCategoryRepository ?? (_serviceCategoryRepository = new GenericRepository<ServiceCategory>(_context));
        public GenericRepository<Subcribe> SubcribeRepository =>
            _subcribeRepository ?? (_subcribeRepository = new GenericRepository<Subcribe>(_context));
        public GenericRepository<ConfigSite> ConfigSiteRepository =>
            _configRepository ?? (_configRepository = new GenericRepository<ConfigSite>(_context));
        public GenericRepository<Contact> ContactRepository =>
            _contactRepository ?? (_contactRepository = new GenericRepository<Contact>(_context));
        public GenericRepository<Banner> BannerRepository =>
            _bannerRepository ?? (_bannerRepository = new GenericRepository<Banner>(_context));
        public GenericRepository<Article> ArticleRepository =>
            _articleRepository ?? (_articleRepository = new GenericRepository<Article>(_context));
        public GenericRepository<ArticleCategory> ArticleCategoryRepository =>
            _articategoryRepository ?? (_articategoryRepository = new GenericRepository<ArticleCategory>(_context));
        public GenericRepository<Admin> AdminRepository =>
            _adminRepository ?? (_adminRepository = new GenericRepository<Admin>(_context));
        public void Save()
        {
            _context.SaveChanges();
        }
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}