namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AditivoNbOrcamento : DbMigration
    {
        public override void Up()
        {
			RenameColumn("dbo.Aditivo", "nbOrcamento", "NbOrcamento");
        }
        
        public override void Down()
		{
			RenameColumn("dbo.Aditivo", "NbOrcamento", "nbOrcamento");
        }
    }
}
