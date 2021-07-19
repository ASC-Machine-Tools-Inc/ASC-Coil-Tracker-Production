namespace ASC_Coil_Tracker_Production.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.COILTABLE",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    COLOR = c.String(nullable: false, maxLength: 50),
                    TYPE = c.String(nullable: false, maxLength: 50),
                    GAUGE = c.String(nullable: false, maxLength: 50),
                    THICK = c.Double(),
                    WEIGHT = c.Double(),
                    LENGTH = c.Double(),
                    NOTES = c.String(maxLength: 50),
                    YIELD = c.Double(),
                    WIDTH = c.Double(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.COILTABLEHISTORY",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    DATE = c.DateTime(nullable: false, storeType: "smalldatetime"),
                    AMOUNTUSED = c.Int(),
                    JOBNUMBER = c.String(maxLength: 50),
                    NOTES = c.String(maxLength: 50),
                    COILID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.COILTABLE", t => t.COILID, cascadeDelete: true)
                .Index(t => t.COILID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.COILTABLEHISTORY", "COILID", "dbo.COILTABLE");
            DropIndex("dbo.COILTABLEHISTORY", new[] { "COILID" });
            DropTable("dbo.COILTABLEHISTORY");
            DropTable("dbo.COILTABLE");
        }
    }
}