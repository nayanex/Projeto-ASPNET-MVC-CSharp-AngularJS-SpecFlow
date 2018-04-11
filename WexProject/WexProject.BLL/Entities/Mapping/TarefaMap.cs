using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Planejamento;

namespace WexProject.BLL.Entities.Mapping
{
    public class TarefaMap : EntityTypeConfiguration<Tarefa>
    {
        public TarefaMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxResponsaveis)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Tarefa");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.OidSituacaoPlanejamento).HasColumnName("SituacaoPlanejamento");
            this.Property(t => t.NbEstimativaRestante).HasColumnName("NbEstimativaRestante");
            this.Property(t => t.NbEstimativaInicial).HasColumnName("NbEstimativaInicial");
            this.Property(t => t.NbRealizado).HasColumnName("NbRealizado");
            this.Property(t => t.TxObservacao).HasColumnName("TxObservacao");
            this.Property(t => t.DtInicio).HasColumnName("DtInicio");
            this.Property(t => t.TxResponsaveis).HasColumnName("TxResponsaveis");
            this.Property(t => t.CsLinhaBaseSalva).HasColumnName("CsLinhaBaseSalva");
            this.Property(t => t.DtAtualizadoEm).HasColumnName("DtAtualizadoEm");
            this.Property(t => t.OidAtualizadoPor).HasColumnName("AtualizadoPor");
        }
    }
}
