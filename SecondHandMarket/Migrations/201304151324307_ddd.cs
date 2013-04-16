namespace SecondHandMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ddd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Goods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Single(nullable: false),
                        Description = c.String(),
                        TradePlace = c.String(),
                        Mobile = c.String(),
                        Linkman = c.String(),
                        QQ = c.String(),
                        ReleaseTime = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfo", t => t.User_Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        QQ = c.String(),
                        Mobile = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Release_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goods", t => t.Release_Id)
                .Index(t => t.Release_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Pictures", new[] { "Release_Id" });
            DropIndex("dbo.Goods", new[] { "Category_Id" });
            DropIndex("dbo.Goods", new[] { "User_Id" });
            DropForeignKey("dbo.Pictures", "Release_Id", "dbo.Goods");
            DropForeignKey("dbo.Goods", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Goods", "User_Id", "dbo.UserInfo");
            DropTable("dbo.Pictures");
            DropTable("dbo.UserInfo");
            DropTable("dbo.Goods");
        }
    }
}
