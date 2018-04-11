namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GastosRelacionadosChaveImportacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GastosRelacionados", "ChaveImportacao", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GastosRelacionados", "ChaveImportacao");
        }
    }
}
