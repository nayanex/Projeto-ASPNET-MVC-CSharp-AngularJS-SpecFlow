namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GastosRelacionados : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GastosRelacionados",
                c => new
                    {
                        GastoId = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(),
                        Lote = c.String(),
                        SubLote = c.String(),
                        Documento = c.String(),
                        Linha = c.String(),
                        CentroDeCustoId = c.Int(nullable: false),
                        RubricaId = c.Int(),
                        Descricao = c.String(),
                        HistoricoLancamento = c.String(),
                        Justificativa = c.String(),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.GastoId)
                .ForeignKey("dbo.CentroCusto", t => t.CentroDeCustoId, cascadeDelete: true)
                .ForeignKey("dbo.Rubrica", t => t.RubricaId)
                .Index(t => t.CentroDeCustoId)
                .Index(t => t.RubricaId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.GastosRelacionados", new[] { "RubricaId" });
            DropIndex("dbo.GastosRelacionados", new[] { "CentroDeCustoId" });
            DropForeignKey("dbo.GastosRelacionados", "RubricaId", "dbo.Rubrica");
            DropForeignKey("dbo.GastosRelacionados", "CentroDeCustoId", "dbo.CentroCusto");
            DropTable("dbo.GastosRelacionados");
        }
    }
}
