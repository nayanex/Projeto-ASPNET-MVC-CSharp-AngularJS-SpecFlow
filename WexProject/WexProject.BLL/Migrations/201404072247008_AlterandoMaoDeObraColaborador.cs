namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterandoMaoDeObraColaborador : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MaoDeObra", "ColaboradorOid", "dbo.Colaborador");
            DropIndex("dbo.MaoDeObra", new[] { "ColaboradorOid" });
            AddColumn("dbo.MaoDeObra", "Matricula", c => c.Int(nullable: false));
            AddColumn("dbo.MaoDeObra", "Nome", c => c.String());
            AddColumn("dbo.MaoDeObra", "Cargo", c => c.String());
            AddColumn("dbo.MaoDeObra", "ValorTotalSemProvisoes", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.MaoDeObra", "TotalGastoSemProvisoes");
            DropColumn("dbo.MaoDeObra", "TotalProvisaoFerias13o");
            DropColumn("dbo.MaoDeObra", "ProvisaoDemissao");
            DropColumn("dbo.MaoDeObra", "ColaboradorOid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MaoDeObra", "ColaboradorOid", c => c.Guid(nullable: false));
            AddColumn("dbo.MaoDeObra", "ProvisaoDemissao", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MaoDeObra", "TotalProvisaoFerias13o", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MaoDeObra", "TotalGastoSemProvisoes", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.MaoDeObra", "ValorTotalSemProvisoes");
            DropColumn("dbo.MaoDeObra", "Cargo");
            DropColumn("dbo.MaoDeObra", "Nome");
            DropColumn("dbo.MaoDeObra", "Matricula");
            CreateIndex("dbo.MaoDeObra", "ColaboradorOid");
            AddForeignKey("dbo.MaoDeObra", "ColaboradorOid", "dbo.Colaborador", "Oid", cascadeDelete: true);
        }
    }
}
