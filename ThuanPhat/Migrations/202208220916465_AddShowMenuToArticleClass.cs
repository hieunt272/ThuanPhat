namespace ThuanPhat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShowMenuToArticleClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "ShowMenu", c => c.Boolean(nullable: false));
            AddColumn("dbo.ServiceCategories", "ShowHome", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceCategories", "ShowHome");
            DropColumn("dbo.Articles", "ShowMenu");
        }
    }
}
