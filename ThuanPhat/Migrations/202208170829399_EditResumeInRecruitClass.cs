namespace ThuanPhat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditResumeInRecruitClass : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Recruits", "Resume", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Recruits", "Resume", c => c.String());
        }
    }
}
