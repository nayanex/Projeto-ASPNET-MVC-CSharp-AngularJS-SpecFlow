namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RubricaMesValoresNulos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RubricaMes", "NbPlanejado", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.RubricaMes", "NbReplanejado", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.RubricaMes", "NbGasto", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
			//AlterColumn("dbo.RubricaMes", "NbGasto", c => c.Decimal(nullable: false, precision: 18, scale: 2));
			//AlterColumn("dbo.RubricaMes", "NbReplanejado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
			//AlterColumn("dbo.RubricaMes", "NbPlanejado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
