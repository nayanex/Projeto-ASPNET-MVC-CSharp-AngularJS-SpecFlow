namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RetirandoCampoTxObservacaoDeEstoria : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tarefa", "CsLinhaBaseSalva", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tarefa", "CsLinhaBaseSalva", c => c.Boolean());
        }
    }
}
