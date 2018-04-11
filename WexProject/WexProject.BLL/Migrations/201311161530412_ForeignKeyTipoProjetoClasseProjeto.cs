namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyTipoProjetoClasseProjeto : DbMigration
    {
        public override void Up()
		{
			AddForeignKey("dbo.TipoProjeto", "ClasseProjetoId", "dbo.ClasseProjeto", "ClasseProjetoId", cascadeDelete: true);
        }
        
        public override void Down()
		{
			DropForeignKey("dbo.TipoProjeto", "ClasseProjetoId", "dbo.ClasseProjeto");
        }
    }
}
