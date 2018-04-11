namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DuracaoAditivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Aditivo", "NbDuracao", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Aditivo", "NbDuracao");
        }
    }
}
