namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rubricas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rubrica",
                c => new
                    {
                        RubricaId = c.Int(nullable: false, identity: true),
                        TipoRubricaId = c.Int(nullable: false),
                        AditivoId = c.Int(nullable: false),
                        NbTotalPlanejado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NbTotalGasto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.RubricaId)
                .ForeignKey("dbo.TipoRubrica", t => t.TipoRubricaId, cascadeDelete: true)
                .ForeignKey("dbo.Aditivo", t => t.AditivoId, cascadeDelete: true)
                .Index(t => t.TipoRubricaId)
                .Index(t => t.AditivoId);
            
            CreateTable(
                "dbo.TipoRubrica",
                c => new
                    {
                        TipoRubricaId = c.Int(nullable: false, identity: true),
                        TxNome = c.String(),
                        CsClasse = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TipoRubricaId);
            
            CreateTable(
                "dbo.RubricaMes",
                c => new
                    {
                        RubricaMesId = c.Int(nullable: false, identity: true),
                        RubricaId = c.Int(nullable: false),
                        CsMes = c.Int(nullable: false),
                        NbAno = c.Int(nullable: false),
                        NbPlanejado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NbGasto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.RubricaMesId)
                .ForeignKey("dbo.Rubrica", t => t.RubricaId, cascadeDelete: true)
                .Index(t => t.RubricaId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RubricaMes", new[] { "RubricaId" });
            DropIndex("dbo.Rubrica", new[] { "AditivoId" });
            DropIndex("dbo.Rubrica", new[] { "TipoRubricaId" });
            DropForeignKey("dbo.RubricaMes", "RubricaId", "dbo.Rubrica");
            DropForeignKey("dbo.Rubrica", "AditivoId", "dbo.Aditivo");
            DropForeignKey("dbo.Rubrica", "TipoRubricaId", "dbo.TipoRubrica");
            DropTable("dbo.RubricaMes");
            DropTable("dbo.TipoRubrica");
            DropTable("dbo.Rubrica");
        }
    }
}
