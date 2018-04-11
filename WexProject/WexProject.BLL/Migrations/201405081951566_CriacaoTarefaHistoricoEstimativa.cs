namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriacaoTarefaHistoricoEstimativa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TarefaHistoricoEstimativa",
                c => new
                    {
                        Oid = c.Guid(nullable: false),
                        DtPlanejado = c.DateTime(nullable: false),
                        NbHoraRestante = c.Double(nullable: false),
                        Tarefa = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Oid)
                .ForeignKey("dbo.Tarefa", t => t.Tarefa, cascadeDelete: true)
                .Index(t => t.Tarefa);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TarefaHistoricoEstimativa", new[] { "Tarefa" });
            DropForeignKey("dbo.TarefaHistoricoEstimativa", "Tarefa", "dbo.Tarefa");
            DropTable("dbo.TarefaHistoricoEstimativa");
        }
    }
}
