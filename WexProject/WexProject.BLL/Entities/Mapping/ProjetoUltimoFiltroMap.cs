using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class ProjetoUltimoFiltroMap : EntityTypeConfiguration<ProjetoUltimoFiltro>
    {
        public ProjetoUltimoFiltroMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("ProjetoUltimoFiltro");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.MotivoCancelamentoCiclo).HasColumnName("MotivoCancelamentoCiclo");
            this.Property(t => t.Projeto).HasColumnName("Projeto");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.MotivoCancelamento)
                .WithMany(t => t.ProjetoUltimoFiltroes)
                .HasForeignKey(d => d.MotivoCancelamentoCiclo);
            this.HasOptional(t => t.Projeto1)
                .WithMany(t => t.ProjetoUltimoFiltroes)
                .HasForeignKey(d => d.Projeto);

        }
    }
}
