using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.NovosNegocios;

namespace WexProject.BLL.Entities.Mapping
{
    public class SolicitacaoOrcamentoHistoricoMap : EntityTypeConfiguration<SolicitacaoOrcamentoHistorico>
    {
        public SolicitacaoOrcamentoHistoricoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.AlteradoPor)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("SolicitacaoOrcamentoHistorico");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Comentario).HasColumnName("Comentario");
            this.Property(t => t.DataHora).HasColumnName("DataHora");
            this.Property(t => t.Situacoes).HasColumnName("Situacoes");
            this.Property(t => t.ResponsavelHistorico).HasColumnName("ResponsavelHistorico");
            this.Property(t => t.SolicitacaoOrcamento).HasColumnName("SolicitacaoOrcamento");
            this.Property(t => t.AlteradoPor).HasColumnName("AlteradoPor");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.AtualizadoPor).HasColumnName("AtualizadoPor");

            // Relationships
            this.HasOptional(t => t.Colaborador)
                .WithMany(t => t.SolicitacaoOrcamentoHistoricoes)
                .HasForeignKey(d => d.AtualizadoPor);
            this.HasOptional(t => t.Colaborador1)
                .WithMany(t => t.SolicitacaoOrcamentoHistoricoes1)
                .HasForeignKey(d => d.ResponsavelHistorico);
            this.HasOptional(t => t.ConfiguracaoDocumentoSituacao)
                .WithMany(t => t.SolicitacaoOrcamentoHistoricoes)
                .HasForeignKey(d => d.Situacoes);
            this.HasOptional(t => t.SolicitacaoOrcamento1)
                .WithMany(t => t.SolicitacaoOrcamentoHistoricoes)
                .HasForeignKey(d => d.SolicitacaoOrcamento);

        }
    }
}
