namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustosMaoDeObra : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lote",
                c => new
                    {
                        LoteId = c.Int(nullable: false, identity: true),
                        Numero = c.Int(nullable: false),
                        CentroCustoId = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                        DataAtualizacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoteId);
            
            CreateTable(
                "dbo.MaoDeObra",
                c => new
                    {
                        MaoDeObraId = c.Int(nullable: false, identity: true),
                        Matricula = c.Int(nullable: false),
                        Nome = c.String(),
                        PercentualAlocacao = c.Int(nullable: false),
                        TotalGastoSemProvisoes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalProvisaoFerias13o = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProvisaoDemissao = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Data = c.DateTime(nullable: false),
                        LoteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaoDeObraId)
                .ForeignKey("dbo.Lote", t => t.LoteId, cascadeDelete: true)
                .Index(t => t.LoteId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.MaoDeObra", new[] { "LoteId" });
            DropForeignKey("dbo.MaoDeObra", "LoteId", "dbo.Lote");
            DropTable("dbo.MaoDeObra");
            DropTable("dbo.Lote");
        }
    }
}
