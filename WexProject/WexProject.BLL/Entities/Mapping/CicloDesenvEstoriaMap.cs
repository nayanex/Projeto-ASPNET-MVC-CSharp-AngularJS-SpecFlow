using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Execucao;

namespace WexProject.BLL.Entities.Mapping
{
    public class CicloDesenvEstoriaMap : EntityTypeConfiguration<CicloDesenvEstoria>
    {
        public CicloDesenvEstoriaMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("CicloDesenvEstoria");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Meta).HasColumnName("Meta");
            this.Property(t => t.Estoria).HasColumnName("Estoria");
            this.Property(t => t.NbSequencia).HasColumnName("NbSequencia");
            this.Property(t => t.CsSituacao).HasColumnName("CsSituacao");
            this.Property(t => t.Ciclo).HasColumnName("Ciclo");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.CicloDesenv)
                .WithMany(t => t.CicloDesenvEstorias)
                .HasForeignKey(d => d.Ciclo);
            this.HasOptional(t => t.Estoria1)
                .WithMany(t => t.CicloDesenvEstorias)
                .HasForeignKey(d => d.Estoria);

        }
    }
}
