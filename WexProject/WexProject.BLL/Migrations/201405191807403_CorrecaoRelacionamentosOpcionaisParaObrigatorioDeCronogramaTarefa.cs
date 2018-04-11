namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrecaoRelacionamentosOpcionaisParaObrigatorioDeCronogramaTarefa : DbMigration
    {

        private void DropForeignKeyIfExists(string tableName, string foreignKey)
        {
            string query = "IF EXISTS ( SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'" + foreignKey + "') AND parent_object_id = OBJECT_ID(N'" + tableName + "') ) " +
                " ALTER TABLE " + tableName + " DROP CONSTRAINT " + foreignKey;
            Sql(query);
        }

        private void AddForeignKeySql(string tableName, string fkName, string columnName, string referencesTableName, string referencesTableColumn)
        {
            string query = "ALTER TABLE " + tableName + " ADD CONSTRAINT " + fkName + " FOREIGN KEY (" + columnName + ") REFERENCES " + referencesTableName + "(" + referencesTableColumn + ")";
            Sql(query);
        }

        public override void Up()
        {

            // DEVEXPRESS =======================
            DropForeignKeyIfExists("[dbo].[CronogramaTarefa]", "FK_CronogramaTarefa_Cronograma");
            DropForeignKeyIfExists("[dbo].[CronogramaTarefa]", "FK_CronogramaTarefa_Tarefa");
            // ==================================

            AlterColumn("dbo.CronogramaTarefa", "Tarefa", c => c.Guid(nullable: false));
            AlterColumn("dbo.CronogramaTarefa", "Cronograma", c => c.Guid(nullable: false));
            
            AddForeignKey("dbo.CronogramaTarefa", "Cronograma", "dbo.Cronograma", "Oid", cascadeDelete: false);
            AddForeignKey("dbo.CronogramaTarefa", "Tarefa", "dbo.Tarefa", "Oid", cascadeDelete: false);

            CreateIndex("dbo.CronogramaTarefa", "Cronograma");
            CreateIndex("dbo.CronogramaTarefa", "Tarefa");

        }
        
        public override void Down()
        {

            AlterColumn("dbo.CronogramaTarefa", "Cronograma", c => c.Guid());
            AlterColumn("dbo.CronogramaTarefa", "Tarefa", c => c.Guid());

            DropForeignKey("dbo.CronogramaTarefa", "Tarefa", "dbo.Tarefa");
            DropForeignKey("dbo.CronogramaTarefa", "Cronograma", "dbo.Cronograma");

            DropIndex("dbo.CronogramaTarefa", new[] { "Tarefa" });
            DropIndex("dbo.CronogramaTarefa", new[] { "Cronograma" });

            // DEVEXPRESS =======================
            AddForeignKeySql("[dbo].[CronogramaTarefa]", "FK_CronogramaTarefa_Cronograma", "Cronograma", "[dbo].[Cronograma]", "Oid");
            AddForeignKeySql("[dbo].[CronogramaTarefa]", "FK_CronogramaTarefa_Tarefa", "Tarefa", "[dbo].[Tarefa]", "Oid");
            // ==================================

        }

    }

}
