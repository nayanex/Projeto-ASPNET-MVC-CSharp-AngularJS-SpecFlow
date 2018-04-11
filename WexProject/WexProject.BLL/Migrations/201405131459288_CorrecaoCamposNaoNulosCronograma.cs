namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CorrecaoCamposNaoNulosCronograma : DbMigration
    {

        public override void Up()
        {

            // DEVEXPRESS =======================
            Sql("IF EXISTS ( SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_Cronograma_SituacaoPlanejamento') AND parent_object_id = OBJECT_ID(N'dbo.Cronograma') ) ALTER TABLE [dbo].[Cronograma] DROP CONSTRAINT [FK_Cronograma_SituacaoPlanejamento]");
            Sql("IF EXISTS ( SELECT * FROM sys.indexes WHERE object_id = object_id('dbo.Cronograma') AND NAME = 'iSituacaoPlanejamento_Cronograma' ) DROP INDEX iSituacaoPlanejamento_Cronograma ON dbo.Cronograma");
            // ==================================

            AlterColumn("dbo.Cronograma", "DtInicio", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Cronograma", "DtFinal", c => c.DateTime(nullable: false));

            AlterColumn("dbo.Cronograma", "SituacaoPlanejamento", c => c.Guid(nullable: false));
            AddForeignKey("dbo.Cronograma", "SituacaoPlanejamento", "dbo.SituacaoPlanejamento", "Oid");
            CreateIndex("dbo.Cronograma", "SituacaoPlanejamento");

        }

        public override void Down()
        {

            AlterColumn("dbo.Cronograma", "DtFinal", c => c.DateTime());
            AlterColumn("dbo.Cronograma", "DtInicio", c => c.DateTime());

            AlterColumn("dbo.Cronograma", "SituacaoPlanejamento", c => c.Guid());
            DropForeignKey("dbo.Cronograma", "SituacaoPlanejamento", "dbo.SituacaoPlanejamento");
            DropIndex("dbo.Cronograma", new[] { "SituacaoPlanejamento" });

            // DEVEXPRESS =======================
            Sql("CREATE INDEX iSituacaoPlanejamento_Cronograma ON dbo.Cronograma (SituacaoPlanejamento)");
            Sql("ALTER TABLE dbo.Cronograma ADD CONSTRAINT FK_Cronograma_SituacaoPlanejamento FOREIGN KEY (SituacaoPlanejamento) REFERENCES dbo.SituacaoPlanejamento(Oid)");
            // ==================================

        }

    }

}
