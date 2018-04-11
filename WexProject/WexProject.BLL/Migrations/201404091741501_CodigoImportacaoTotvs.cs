namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodigoImportacaoTotvs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoteMaoDeObra", "CodigoImportacao", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoteMaoDeObra", "CodigoImportacao");
        }
    }
}
