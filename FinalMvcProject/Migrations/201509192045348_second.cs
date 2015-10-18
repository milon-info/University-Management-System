namespace FinalMvcProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.Courses", "SemName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "SemName");
            DropColumn("dbo.Courses", "Status");
        }
    }
}
