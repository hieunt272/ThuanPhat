namespace ThuanPhat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLanguage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleCategoryLangs",
                c => new
                    {
                        ArticleCategoryId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        CategoryName = c.String(maxLength: 100),
                        Description = c.String(maxLength: 500),
                        Url = c.String(maxLength: 500),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                        AboutText = c.String(),
                        MottoText = c.String(),
                        FormationText = c.String(),
                    })
                .PrimaryKey(t => new { t.ArticleCategoryId, t.LanguageId })
                .ForeignKey("dbo.ArticleCategories", t => t.ArticleCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.ArticleCategoryId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Sort = c.Int(nullable: false),
                        Culture = c.String(maxLength: 10),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleLangs",
                c => new
                    {
                        ArticleId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Subject = c.String(maxLength: 150),
                        Description = c.String(),
                        Body = c.String(),
                        Url = c.String(maxLength: 300),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => new { t.ArticleId, t.LanguageId })
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.BannerLangs",
                c => new
                    {
                        BannerId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        BannerName = c.String(maxLength: 100),
                        Slogan = c.String(maxLength: 500),
                        Url = c.String(maxLength: 500),
                        Content = c.String(),
                    })
                .PrimaryKey(t => new { t.BannerId, t.LanguageId })
                .ForeignKey("dbo.Banners", t => t.BannerId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.BannerId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.CareerLangs",
                c => new
                    {
                        CareerId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => new { t.CareerId, t.LanguageId })
                .ForeignKey("dbo.Careers", t => t.CareerId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.CareerId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.ConfigSiteLangs",
                c => new
                    {
                        ConfigSiteId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Place = c.String(),
                        Title = c.String(maxLength: 200),
                        Description = c.String(maxLength: 500),
                        AboutText = c.String(),
                        InfoContact = c.String(),
                        InfoFooter = c.String(),
                    })
                .PrimaryKey(t => new { t.ConfigSiteId, t.LanguageId })
                .ForeignKey("dbo.ConfigSites", t => t.ConfigSiteId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.ConfigSiteId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.ServiceCategoryLangs",
                c => new
                    {
                        ServiceCategoryId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        CategoryName = c.String(maxLength: 100),
                        Description = c.String(maxLength: 500),
                        Url = c.String(maxLength: 500),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => new { t.ServiceCategoryId, t.LanguageId })
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.ServiceCategories", t => t.ServiceCategoryId, cascadeDelete: true)
                .Index(t => t.ServiceCategoryId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.ServiceLangs",
                c => new
                    {
                        ServiceId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        ServiceName = c.String(maxLength: 150),
                        Description = c.String(),
                        Body = c.String(),
                        Url = c.String(maxLength: 300),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => new { t.ServiceId, t.LanguageId })
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId)
                .Index(t => t.LanguageId);
            
            AddColumn("dbo.Articles", "Sort", c => c.Int(nullable: false));
            AlterColumn("dbo.ArticleCategories", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.Careers", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.ServiceCategories", "CategoryName", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.ArticleCategories", "ParentId");
            CreateIndex("dbo.ServiceCategories", "ParentId");
            AddForeignKey("dbo.ArticleCategories", "ParentId", "dbo.ArticleCategories", "Id");
            AddForeignKey("dbo.ServiceCategories", "ParentId", "dbo.ServiceCategories", "Id");
            DropColumn("dbo.ConfigSites", "AboutUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConfigSites", "AboutUrl", c => c.String(maxLength: 500));
            DropForeignKey("dbo.ServiceLangs", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.ServiceLangs", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ServiceCategoryLangs", "ServiceCategoryId", "dbo.ServiceCategories");
            DropForeignKey("dbo.ServiceCategoryLangs", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ServiceCategories", "ParentId", "dbo.ServiceCategories");
            DropForeignKey("dbo.ConfigSiteLangs", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ConfigSiteLangs", "ConfigSiteId", "dbo.ConfigSites");
            DropForeignKey("dbo.CareerLangs", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.CareerLangs", "CareerId", "dbo.Careers");
            DropForeignKey("dbo.BannerLangs", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.BannerLangs", "BannerId", "dbo.Banners");
            DropForeignKey("dbo.ArticleCategories", "ParentId", "dbo.ArticleCategories");
            DropForeignKey("dbo.ArticleLangs", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ArticleLangs", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.ArticleCategoryLangs", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ArticleCategoryLangs", "ArticleCategoryId", "dbo.ArticleCategories");
            DropIndex("dbo.ServiceLangs", new[] { "LanguageId" });
            DropIndex("dbo.ServiceLangs", new[] { "ServiceId" });
            DropIndex("dbo.ServiceCategoryLangs", new[] { "LanguageId" });
            DropIndex("dbo.ServiceCategoryLangs", new[] { "ServiceCategoryId" });
            DropIndex("dbo.ServiceCategories", new[] { "ParentId" });
            DropIndex("dbo.ConfigSiteLangs", new[] { "LanguageId" });
            DropIndex("dbo.ConfigSiteLangs", new[] { "ConfigSiteId" });
            DropIndex("dbo.CareerLangs", new[] { "LanguageId" });
            DropIndex("dbo.CareerLangs", new[] { "CareerId" });
            DropIndex("dbo.BannerLangs", new[] { "LanguageId" });
            DropIndex("dbo.BannerLangs", new[] { "BannerId" });
            DropIndex("dbo.ArticleLangs", new[] { "LanguageId" });
            DropIndex("dbo.ArticleLangs", new[] { "ArticleId" });
            DropIndex("dbo.ArticleCategoryLangs", new[] { "LanguageId" });
            DropIndex("dbo.ArticleCategoryLangs", new[] { "ArticleCategoryId" });
            DropIndex("dbo.ArticleCategories", new[] { "ParentId" });
            AlterColumn("dbo.ServiceCategories", "CategoryName", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Careers", "Name", c => c.String());
            AlterColumn("dbo.ArticleCategories", "Description", c => c.String());
            DropColumn("dbo.Articles", "Sort");
            DropTable("dbo.ServiceLangs");
            DropTable("dbo.ServiceCategoryLangs");
            DropTable("dbo.ConfigSiteLangs");
            DropTable("dbo.CareerLangs");
            DropTable("dbo.BannerLangs");
            DropTable("dbo.ArticleLangs");
            DropTable("dbo.Languages");
            DropTable("dbo.ArticleCategoryLangs");
        }
    }
}
