namespace Sports_Merchandise_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F1DRIVERnew : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        DriverId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        F1TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DriverId)
                .ForeignKey("dbo.F1_Team", t => t.F1TeamId, cascadeDelete: true)
                .Index(t => t.F1TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "F1TeamId", "dbo.F1_Team");
            DropIndex("dbo.Drivers", new[] { "F1TeamId" });
            DropTable("dbo.Drivers");
        }
    }
}
