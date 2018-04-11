namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjetoUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projeto", "NbValor", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Projeto", "GerenteOid", c => c.Guid(nullable: true));
            AddColumn("dbo.Projeto", "CsSituacaoProjeto", c => c.Int(nullable: false));
            AddForeignKey("dbo.Projeto", "GerenteOid", "dbo.Colaborador", "Oid", cascadeDelete: false);
            CreateIndex("dbo.Projeto", "GerenteOid");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Projeto", new[] { "GerenteOid" });
            DropForeignKey("dbo.Projeto", "GerenteOid", "dbo.Colaborador");
            DropColumn("dbo.Projeto", "CsSituacaoProjeto");
            DropColumn("dbo.Projeto", "GerenteOid");
            DropColumn("dbo.Projeto", "NbValor");
        }
    }
}
