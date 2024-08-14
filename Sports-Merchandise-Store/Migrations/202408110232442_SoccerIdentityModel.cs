namespace Sports_Merchandise_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoccerIdentityModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoccerMerchandises",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        ItemType = c.String(),
                        ItemSize = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Teamid = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.Players", t => t.PlayerId, cascadeDelete: false)
                .ForeignKey("dbo.Teams", t => t.Teamid, cascadeDelete: false)
                .Index(t => t.Teamid)
                .Index(t => t.PlayerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoccerMerchandises", "Teamid", "dbo.Teams");
            DropForeignKey("dbo.SoccerMerchandises", "PlayerId", "dbo.Players");
            DropIndex("dbo.SoccerMerchandises", new[] { "PlayerId" });
            DropIndex("dbo.SoccerMerchandises", new[] { "Teamid" });
            DropTable("dbo.SoccerMerchandises");
        }
    }
}
