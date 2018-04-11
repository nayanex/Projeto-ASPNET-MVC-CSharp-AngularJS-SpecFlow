namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RubricasAninhadas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rubrica", "PaiId", c => c.Int());
            AddColumn("dbo.TipoRubrica", "TipoPaiId", c => c.Int());
            AddForeignKey("dbo.Rubrica", "PaiId", "dbo.Rubrica", "RubricaId");
            AddForeignKey("dbo.TipoRubrica", "TipoPaiId", "dbo.TipoRubrica", "TipoRubricaId");
            CreateIndex("dbo.Rubrica", "PaiId");
            CreateIndex("dbo.TipoRubrica", "TipoPaiId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TipoRubrica", new[] { "TipoPaiId" });
            DropIndex("dbo.Rubrica", new[] { "PaiId" });
            DropForeignKey("dbo.TipoRubrica", "TipoPaiId", "dbo.TipoRubrica");
            DropForeignKey("dbo.Rubrica", "PaiId", "dbo.Rubrica");
            DropColumn("dbo.TipoRubrica", "TipoPaiId");
            DropColumn("dbo.Rubrica", "PaiId");
        }
    }
}
