namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoteMaoDeObra : DbMigration
    {
        public override void Up()
		{
			RenameTable("Lote", "LoteMaoDeObra");
        }
        
        public override void Down()
		{
			RenameTable("LoteMaoDeObra", "Lote");
        }
    }
}
