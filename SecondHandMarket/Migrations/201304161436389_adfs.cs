namespace SecondHandMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adfs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Goods", "User_Id", "dbo.UserInfo");
            DropForeignKey("dbo.Goods", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Pictures", "Release_Id", "dbo.Goods");
            DropIndex("dbo.Goods", new[] { "User_Id" });
            DropIndex("dbo.Goods", new[] { "Category_Id" });
            DropIndex("dbo.Pictures", new[] { "Release_Id" });
            CreateTable(
                "dbo.Releases",
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
            
            AddForeignKey("dbo.Pictures", "Release_Id", "dbo.Releases", "Id");
            CreateIndex("dbo.Pictures", "Release_Id");
            DropTable("dbo.Goods");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.Pictures", new[] { "Release_Id" });
            DropIndex("dbo.Releases", new[] { "Category_Id" });
            DropIndex("dbo.Releases", new[] { "User_Id" });
            DropForeignKey("dbo.Pictures", "Release_Id", "dbo.Releases");
            DropForeignKey("dbo.Releases", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Releases", "User_Id", "dbo.UserInfo");
            DropTable("dbo.Releases");
            CreateIndex("dbo.Pictures", "Release_Id");
            CreateIndex("dbo.Goods", "Category_Id");
            CreateIndex("dbo.Goods", "User_Id");
            AddForeignKey("dbo.Pictures", "Release_Id", "dbo.Goods", "Id");
            AddForeignKey("dbo.Goods", "Category_Id", "dbo.Categories", "Id");
            AddForeignKey("dbo.Goods", "User_Id", "dbo.UserInfo", "Id");
        }
    }
}
