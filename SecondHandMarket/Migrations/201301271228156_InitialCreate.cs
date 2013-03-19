namespace SecondHandMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        a = c.Int(nullable: false),
                        SubCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.SubCategory_Id)
                .Index(t => t.SubCategory_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Categories", new[] { "SubCategory_Id" });
            DropForeignKey("dbo.Categories", "SubCategory_Id", "dbo.Categories");
            DropTable("dbo.Categories");
        }
    }
}
