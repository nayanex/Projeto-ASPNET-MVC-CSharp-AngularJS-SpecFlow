namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RubricasGastos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RubricaMesGasto",
                c => new
                    {
                        RubricaMesId = c.Int(nullable: false),
                        NbGasto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.RubricaMesId)
                .ForeignKey("dbo.RubricaMes", t => t.RubricaMesId)
                .Index(t => t.RubricaMesId);
            
            DropColumn("dbo.RubricaMes", "NbGasto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RubricaMes", "NbGasto", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropIndex("dbo.RubricaMesGasto", new[] { "RubricaMesId" });
            DropForeignKey("dbo.RubricaMesGasto", "RubricaMesId", "dbo.RubricaMes");
            DropTable("dbo.RubricaMesGasto");
        }
    }
}
