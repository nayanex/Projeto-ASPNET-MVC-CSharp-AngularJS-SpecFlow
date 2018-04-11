namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatrocinadorCentroCusto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AditivoPatrocinador",
                c => new
                    {
                        AditivoPatrocinadorId = c.Int(nullable: false, identity: true),
                        AditivoId = c.Int(nullable: false),
                        PatrocinadorOid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AditivoPatrocinadorId)
                .ForeignKey("dbo.Aditivo", t => t.AditivoId, cascadeDelete: true)
                .ForeignKey("dbo.EmpresaInstituicao", t => t.PatrocinadorOid, cascadeDelete: true)
                .Index(t => t.AditivoId)
                .Index(t => t.PatrocinadorOid);
            
            CreateTable(
                "dbo.AditivoCentroCusto",
                c => new
                    {
                        AditivoCentroCustoId = c.Int(nullable: false, identity: true),
                        AditivoId = c.Int(nullable: false),
                        CentroCustoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AditivoCentroCustoId)
                .ForeignKey("dbo.Aditivo", t => t.AditivoId, cascadeDelete: true)
                .ForeignKey("dbo.CentroCusto", t => t.CentroCustoId, cascadeDelete: true)
                .Index(t => t.AditivoId)
                .Index(t => t.CentroCustoId);

			CreateTable(
				"dbo.CentroCusto",
				c => new
					{
						CentroCustoId = c.Int(nullable: false, identity: true),
						Codigo = c.Int(nullable: false),
						Nome = c.String(),
					})
				.PrimaryKey(t => t.CentroCustoId)
				.Index(t => t.Codigo, true);
            
        }
        
        public override void Down()
        {
			DropIndex("dbo.CentroCusto", new[] { "Codigo" });
            DropIndex("dbo.AditivoCentroCusto", new[] { "CentroCustoId" });
            DropIndex("dbo.AditivoCentroCusto", new[] { "AditivoId" });
            DropIndex("dbo.AditivoPatrocinador", new[] { "PatrocinadorOid" });
            DropIndex("dbo.AditivoPatrocinador", new[] { "AditivoId" });
            DropForeignKey("dbo.AditivoCentroCusto", "CentroCustoId", "dbo.CentroCusto");
            DropForeignKey("dbo.AditivoCentroCusto", "AditivoId", "dbo.Aditivo");
            DropForeignKey("dbo.AditivoPatrocinador", "PatrocinadorOid", "dbo.EmpresaInstituicao");
            DropForeignKey("dbo.AditivoPatrocinador", "AditivoId", "dbo.Aditivo");
            DropTable("dbo.CentroCusto");
            DropTable("dbo.AditivoCentroCusto");
            DropTable("dbo.AditivoPatrocinador");
        }
    }
}
