namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizadoAditivo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Aditivo", "CustoId", "dbo.Custo");
            DropIndex("dbo.Aditivo", new[] { "CustoId" });

			DropPrimaryKey("dbo.Aditivo", new[] { "Id" });
			DropColumn("dbo.Aditivo", "Id");
            AddColumn("dbo.Aditivo", "AditivoId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Aditivo", "ProjetoOid", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Aditivo", "AditivoId");
            AddForeignKey("dbo.Aditivo", "ProjetoOid", "dbo.Projeto", "Oid", cascadeDelete: true);
            CreateIndex("dbo.Aditivo", "ProjetoOid");
            DropColumn("dbo.Aditivo", "CustoId");
            DropTable("dbo.Custo");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Custo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.Aditivo", new[] { "ProjetoOid" });
            DropForeignKey("dbo.Aditivo", "ProjetoOid", "dbo.Projeto");
            DropPrimaryKey("dbo.Aditivo", new[] { "AditivoId" });
            DropColumn("dbo.Aditivo", "ProjetoOid");
			DropColumn("dbo.Aditivo", "AditivoId");
			AddColumn("dbo.Aditivo", "CustoId", c => c.Int(nullable: false));
			AddColumn("dbo.Aditivo", "Id", c => c.Int(nullable: false, identity: true));
			AddPrimaryKey("dbo.Aditivo", "Id");
            CreateIndex("dbo.Aditivo", "CustoId");
            AddForeignKey("dbo.Aditivo", "CustoId", "dbo.Custo", "Id", cascadeDelete: true);
        }
    }
}
