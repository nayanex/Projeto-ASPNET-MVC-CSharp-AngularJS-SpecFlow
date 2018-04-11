namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlagGastoRelacionadoMesRubrica : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RubricaMes", "PossuiGastosRelacionados", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RubricaMes", "PossuiGastosRelacionados");
        }
    }
}
