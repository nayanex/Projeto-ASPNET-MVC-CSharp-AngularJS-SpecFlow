namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustesMaoDeObras : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lote", "CentroCustoImportacao", c => c.Int(nullable: false));
            AddColumn("dbo.Lote", "RubricaMesId", c => c.Int(nullable: false));
            AddColumn("dbo.MaoDeObra", "ColaboradorOid", c => c.Guid(nullable: false));
            AddForeignKey("dbo.Lote", "CentroCustoImportacao", "dbo.CentroCusto", "CentroCustoId", cascadeDelete: true);
            AddForeignKey("dbo.Lote", "RubricaMesId", "dbo.RubricaMes", "RubricaMesId", cascadeDelete: true);
            AddForeignKey("dbo.MaoDeObra", "ColaboradorOid", "dbo.Colaborador", "Oid", cascadeDelete: true);
            CreateIndex("dbo.Lote", "CentroCustoImportacao");
            CreateIndex("dbo.Lote", "RubricaMesId");
            CreateIndex("dbo.MaoDeObra", "ColaboradorOid");
            DropColumn("dbo.Lote", "Numero");
            DropColumn("dbo.Lote", "CentroCustoId");
            DropColumn("dbo.Lote", "Data");
            DropColumn("dbo.MaoDeObra", "Matricula");
            DropColumn("dbo.MaoDeObra", "Nome");
            DropColumn("dbo.MaoDeObra", "Data");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MaoDeObra", "Data", c => c.DateTime(nullable: false));
            AddColumn("dbo.MaoDeObra", "Nome", c => c.String());
            AddColumn("dbo.MaoDeObra", "Matricula", c => c.Int(nullable: false));
            AddColumn("dbo.Lote", "Data", c => c.DateTime(nullable: false));
            AddColumn("dbo.Lote", "CentroCustoId", c => c.Int(nullable: false));
            AddColumn("dbo.Lote", "Numero", c => c.Int(nullable: false));
            DropIndex("dbo.MaoDeObra", new[] { "ColaboradorOid" });
            DropIndex("dbo.Lote", new[] { "RubricaMesId" });
            DropIndex("dbo.Lote", new[] { "CentroCustoImportacao" });
            DropForeignKey("dbo.MaoDeObra", "ColaboradorOid", "dbo.Colaborador");
            DropForeignKey("dbo.Lote", "RubricaMesId", "dbo.RubricaMes");
            DropForeignKey("dbo.Lote", "CentroCustoImportacao", "dbo.CentroCusto");
            DropColumn("dbo.MaoDeObra", "ColaboradorOid");
            DropColumn("dbo.Lote", "RubricaMesId");
            DropColumn("dbo.Lote", "CentroCustoImportacao");
        }
    }
}
