using System.Data.Entity.Migrations;

namespace WexProject.BLL.Migrations
{
    public partial class RenomeandoGastosRelacionados : DbMigration
    {
        public override void Up()
        {
            Sql("sp_rename 'GastosRelacionados', 'NotaFiscal';");
            RenameColumn("dbo.NotaFiscal", "GastoId", "Id");
        }

        public override void Down()
        {
            Sql("sp_rename 'NotaFiscal', 'GastosRelacionados';");
            RenameColumn("dbo.GastosRelacionados", "Id", "GastoId");
        }
    }
}