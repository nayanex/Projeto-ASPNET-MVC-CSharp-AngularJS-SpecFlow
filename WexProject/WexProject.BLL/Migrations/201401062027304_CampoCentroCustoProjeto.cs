namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoCentroCustoProjeto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projeto", "CentroCusto_CentroCustoId", c => c.Int());
            AddForeignKey("dbo.Projeto", "CentroCusto_CentroCustoId", "dbo.CentroCusto", "CentroCustoId");
            CreateIndex("dbo.Projeto", "CentroCusto_CentroCustoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Projeto", new[] { "CentroCusto_CentroCustoId" });
            DropForeignKey("dbo.Projeto", "CentroCusto_CentroCustoId", "dbo.CentroCusto");
            DropColumn("dbo.Projeto", "CentroCusto_CentroCustoId");
        }
    }
}
