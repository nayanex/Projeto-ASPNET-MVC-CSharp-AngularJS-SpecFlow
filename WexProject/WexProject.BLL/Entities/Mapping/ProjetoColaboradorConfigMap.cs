using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class ProjetoColaboradorConfigMap : EntityTypeConfiguration<ProjetoColaboradorConfig>
    {
        public ProjetoColaboradorConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.Cor)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ProjetoColaboradorConfig");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Projeto).HasColumnName("Projeto");
            this.Property(t => t.Colaborador).HasColumnName("Colaborador");
            this.Property(t => t.Cor).HasColumnName("Cor");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.Colaborador1)
                .WithMany(t => t.ProjetoColaboradorConfigs)
                .HasForeignKey(d => d.Colaborador);
            this.HasOptional(t => t.Projeto1)
                .WithMany(t => t.ProjetoColaboradorConfigs)
                .HasForeignKey(d => d.Projeto);

        }
    }
}
