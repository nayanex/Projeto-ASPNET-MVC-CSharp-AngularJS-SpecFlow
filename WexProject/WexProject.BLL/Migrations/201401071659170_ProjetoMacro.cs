namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjetoMacro : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projeto", "ProjetoMacroOid", c => c.Guid());
            AddForeignKey("dbo.Projeto", "ProjetoMacroOid", "dbo.Projeto", "Oid");
            CreateIndex("dbo.Projeto", "ProjetoMacroOid");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Projeto", new[] { "ProjetoMacroOid" });
            DropForeignKey("dbo.Projeto", "ProjetoMacroOid", "dbo.Projeto");
            DropColumn("dbo.Projeto", "ProjetoMacroOid");
        }
    }
}
