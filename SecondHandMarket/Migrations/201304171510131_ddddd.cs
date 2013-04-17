namespace SecondHandMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ddddd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Releases", "User_Id", "dbo.UserInfo");
            DropIndex("dbo.Releases", new[] { "User_Id" });
            AddColumn("dbo.Releases", "UserName", c => c.String());
            DropColumn("dbo.Releases", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Releases", "User_Id", c => c.Int());
            DropColumn("dbo.Releases", "UserName");
            CreateIndex("dbo.Releases", "User_Id");
            AddForeignKey("dbo.Releases", "User_Id", "dbo.UserInfo", "Id");
        }
    }
}
