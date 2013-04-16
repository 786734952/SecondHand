namespace SecondHandMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adfs1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Releases", "Title", c => c.String());
            DropColumn("dbo.Releases", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Releases", "Name", c => c.String());
            DropColumn("dbo.Releases", "Title");
        }
    }
}
