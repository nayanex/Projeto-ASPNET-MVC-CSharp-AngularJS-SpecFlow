namespace WexProject.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Aditivo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Custo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Aditivo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TxNome = c.String(nullable: false),
                        DtInicio = c.DateTime(nullable: false),
                        DtTermino = c.DateTime(nullable: false),
                        nbOrcamento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CustoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Custo", t => t.CustoId, cascadeDelete: true)
                .Index(t => t.CustoId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Aditivo", new[] { "CustoId" });
            DropForeignKey("dbo.Aditivo", "CustoId", "dbo.Custo");
            DropTable("dbo.Aditivo");
            DropTable("dbo.Custo");
        }
    }
}
