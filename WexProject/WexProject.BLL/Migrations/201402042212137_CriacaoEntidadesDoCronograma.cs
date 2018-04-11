namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriacaoEntidadesDoCronograma : DbMigration
    {
        public override void Up()
        {
            //Adição manual para não reverter o Migration BugFix que já estava na produção
            AddColumn( "dbo.Tarefa", "CsExcluido", o => o.Boolean() );
            AddColumn( "dbo.Cronograma", "CsExcluido", o => o.Boolean() );
            AddColumn( "dbo.CronogramaTarefa", "CsExcluido", o => o.Boolean() );
            AddColumn( "dbo.TarefaHistoricoTrabalho", "CsExcluido", o => o.Boolean() );
            AlterColumn( "dbo.Tarefa", "NbEstimativaInicial", o=>o.Short() );

            CreateTable(
                "dbo.CronogramaUltimaSelecao",
                c => new
                    {
                        Oid = c.Guid(nullable: false),
                        DtAcesso = c.DateTime(),
                        UltimoCronograma = c.Guid(nullable: false),
                        Usuario = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Oid)
                .ForeignKey("dbo.Cronograma", t => t.UltimoCronograma, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.Usuario, cascadeDelete: true)
                .Index(t => t.UltimoCronograma)
                .Index(t => t.Usuario);
            
            CreateTable(
                "dbo.CronogramaColaboradorConfig",
                c => new
                    {
                        Oid = c.Guid(nullable: false),
                        Cor = c.Int(),
                        Cronograma = c.Guid(nullable: false),
                        Colaborador = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Oid)
                .ForeignKey("dbo.Cronograma", t => t.Cronograma, cascadeDelete: true)
                .ForeignKey("dbo.Colaborador", t => t.Colaborador, cascadeDelete: true)
                .Index(t => t.Cronograma)
                .Index(t => t.Colaborador);
        }
        
        public override void Down()
        {
            DropIndex("dbo.CronogramaColaboradorConfig", new[] { "Colaborador" });
            DropIndex("dbo.CronogramaColaboradorConfig", new[] { "Cronograma" });
            DropIndex("dbo.CronogramaUltimaSelecao", new[] { "Usuario" });
            DropIndex("dbo.CronogramaUltimaSelecao", new[] { "UltimoCronograma" });
            DropForeignKey("dbo.CronogramaColaboradorConfig", "Colaborador", "dbo.Colaborador");
            DropForeignKey("dbo.CronogramaColaboradorConfig", "Cronograma", "dbo.Cronograma");
            DropForeignKey("dbo.CronogramaUltimaSelecao", "Usuario", "dbo.User");
            DropForeignKey("dbo.CronogramaUltimaSelecao", "UltimoCronograma", "dbo.Cronograma");
            DropTable("dbo.CronogramaColaboradorConfig");
            DropTable("dbo.CronogramaUltimaSelecao");

            AlterColumn( "dbo.Tarefa", "NbEstimativaInicial", o => o.Double() );

            //Adição manual para não reverter o Migration BugFix que já estava na produção
            DropColumn( "dbo.TarefaHistoricoTrabalho", "CsExcluido" );
            DropColumn( "dbo.Tarefa", "CsExcluido" );
            DropColumn( "dbo.Cronograma", "CsExcluido" );
            DropColumn( "dbo.CronogramaTarefa", "CsExcluido" );
        }
    }
}
