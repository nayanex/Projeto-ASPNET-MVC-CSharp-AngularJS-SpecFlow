namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionarCentroCustoIdProjeto : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Projeto", name: "CentroCusto_CentroCustoId", newName: "CentroCustoId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Projeto", name: "CentroCustoId", newName: "CentroCusto_CentroCustoId");
        }
    }
}
