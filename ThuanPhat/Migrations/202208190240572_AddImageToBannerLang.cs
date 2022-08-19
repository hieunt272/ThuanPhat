namespace ThuanPhat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageToBannerLang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BannerLangs", "Image", c => c.String(maxLength: 500));
            AlterColumn("dbo.Contacts", "Mobile", c => c.String(nullable: false));
            AlterColumn("dbo.Recruits", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Recruits", "Mobile", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Recruits", "Mobile", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Recruits", "Email", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Contacts", "Mobile", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.BannerLangs", "Image");
        }
    }
}
