namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClassesProjetos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClasseProjeto",
                c => new
                    {
                        ClasseProjetoId = c.Int(nullable: false, identity: true),
                        TxNome = c.String(),
                    })
                .PrimaryKey(t => t.ClasseProjetoId);
            
            AddColumn("dbo.TipoProjeto", "ClasseProjetoId", c => c.Int(nullable: false));
			//AddForeignKey("dbo.TipoProjeto", "ClasseProjetoId", "dbo.ClasseProjeto", "ClasseProjetoId", cascadeDelete: true);
			AddForeignKey("dbo.Projeto", "TipoProjetoId", "dbo.TipoProjeto", "TipoProjetoId", cascadeDelete: true);
            CreateIndex("dbo.TipoProjeto", "ClasseProjetoId");
			CreateIndex("dbo.Projeto", "TipoProjetoId");
        }
        
        public override void Down()
        {
			DropIndex("dbo.TipoProjeto", new[] { "ClasseProjetoId" });
			DropIndex("dbo.Projeto", new[] { "TipoProjetoId" });
			//DropForeignKey("dbo.TipoProjeto", "ClasseProjetoId", "dbo.ClasseProjeto");
			DropForeignKey("dbo.Projeto", "TipoProjetoId", "dbo.TipoProjeto");
            DropColumn("dbo.TipoProjeto", "ClasseProjetoId");
            DropTable("dbo.ClasseProjeto");
        }
    }
}
