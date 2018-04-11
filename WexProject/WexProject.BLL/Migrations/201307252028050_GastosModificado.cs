namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GastosModificado : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RubricaMesGasto", "RubricaMesId", "dbo.RubricaMes");
            DropIndex("dbo.RubricaMesGasto", new[] { "RubricaMesId" });
            AddColumn("dbo.RubricaMes", "NbReplanejado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.RubricaMes", "NbGasto", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Rubrica", "NbTotalGasto");
            DropTable("dbo.RubricaMesGasto");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RubricaMesGasto",
                c => new
                    {
                        RubricaMesId = c.Int(nullable: false),
                        NbGasto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.RubricaMesId);
            
            AddColumn("dbo.Rubrica", "NbTotalGasto", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.RubricaMes", "NbGasto");
            DropColumn("dbo.RubricaMes", "NbReplanejado");
            CreateIndex("dbo.RubricaMesGasto", "RubricaMesId");
            AddForeignKey("dbo.RubricaMesGasto", "RubricaMesId", "dbo.RubricaMes", "RubricaMesId");
        }
    }
}
