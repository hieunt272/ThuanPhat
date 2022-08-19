namespace ThuanPhat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAboutImageToArticleCatLangClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleCategoryLangs", "AboutImage", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArticleCategoryLangs", "AboutImage");
        }
    }
}
