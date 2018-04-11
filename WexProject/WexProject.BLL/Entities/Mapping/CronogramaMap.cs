using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Planejamento;

namespace WexProject.BLL.Entities.Mapping
{
    public class CronogramaMap : EntityTypeConfiguration<Cronograma>
    {
        public CronogramaMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxDescricao)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Cronograma");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.OidSituacaoPlanejamento).HasColumnName("SituacaoPlanejamento");
            this.Property(t => t.DtInicio).HasColumnName("DtInicio");
            this.Property(t => t.DtFinal).HasColumnName("DtFinal");

            // Relationships
            this.HasRequired(t => t.SituacaoPlanejamento)
                .WithMany(t => t.Cronogramas)
                .HasForeignKey(d => d.OidSituacaoPlanejamento);

        }
    }
}
