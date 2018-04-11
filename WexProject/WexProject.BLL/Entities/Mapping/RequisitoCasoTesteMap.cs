using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class RequisitoCasoTesteMap : EntityTypeConfiguration<RequisitoCasoTeste>
    {
        public RequisitoCasoTesteMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("RequisitoCasoTeste");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Requisito).HasColumnName("Requisito");
            this.Property(t => t.CasoTeste).HasColumnName("CasoTeste");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.CasoTeste1)
                .WithMany(t => t.RequisitoCasoTestes)
                .HasForeignKey(d => d.CasoTeste);
            this.HasOptional(t => t.Requisito1)
                .WithMany(t => t.RequisitoCasoTestes)
                .HasForeignKey(d => d.Requisito);

        }
    }
}
