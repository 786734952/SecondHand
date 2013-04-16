namespace SecondHandMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adf : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentAddress_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.ParentAddress_Id)
                .Index(t => t.ParentAddress_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Addresses", new[] { "ParentAddress_Id" });
            DropForeignKey("dbo.Addresses", "ParentAddress_Id", "dbo.Addresses");
            DropTable("dbo.Addresses");
        }
    }
}
