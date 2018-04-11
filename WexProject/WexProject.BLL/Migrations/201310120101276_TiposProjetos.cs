namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TiposProjetos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipoProjeto",
                c => new
                    {
                        TipoProjetoId = c.Int(nullable: false, identity: true),
                        TxNome = c.String(),
                    })
                .PrimaryKey(t => t.TipoProjetoId);
            
            AddColumn("dbo.Projeto", "TipoProjetoId", c => c.Int());
            AddColumn("dbo.TipoRubrica", "TipoProjetoId", c => c.Int());
            AddForeignKey("dbo.TipoRubrica", "TipoProjetoId", "dbo.TipoProjeto", "TipoProjetoId");
            CreateIndex("dbo.TipoRubrica", "TipoProjetoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TipoRubrica", new[] { "TipoProjetoId" });
            DropForeignKey("dbo.TipoRubrica", "TipoProjetoId", "dbo.TipoProjeto");
            DropColumn("dbo.TipoRubrica", "TipoProjetoId");
            DropColumn("dbo.Projeto", "TipoProjetoId");
            DropTable("dbo.TipoProjeto");
        }
    }
}
