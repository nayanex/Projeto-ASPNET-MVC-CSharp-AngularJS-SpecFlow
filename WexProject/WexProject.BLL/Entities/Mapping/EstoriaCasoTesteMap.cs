using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Escopo;

namespace WexProject.BLL.Entities.Mapping
{
    public class EstoriaCasoTesteMap : EntityTypeConfiguration<EstoriaCasoTeste>
    {
        public EstoriaCasoTesteMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("EstoriaCasoTeste");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Estoria).HasColumnName("Estoria");
            this.Property(t => t.CasoTeste).HasColumnName("CasoTeste");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.CasoTeste1)
                .WithMany(t => t.EstoriaCasoTestes)
                .HasForeignKey(d => d.CasoTeste);
            this.HasOptional(t => t.Estoria1)
                .WithMany(t => t.EstoriaCasoTestes)
                .HasForeignKey(d => d.Estoria);

        }
    }
}
