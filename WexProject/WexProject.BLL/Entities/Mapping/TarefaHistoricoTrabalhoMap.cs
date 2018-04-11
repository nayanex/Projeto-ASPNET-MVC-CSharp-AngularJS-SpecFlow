using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Planejamento;

namespace WexProject.BLL.Entities.Mapping
{
    public class TarefaHistoricoTrabalhoMap : EntityTypeConfiguration<TarefaHistoricoTrabalho>
    {
        public TarefaHistoricoTrabalhoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxComentario)
                .HasMaxLength(200);

            this.Property(t => t.TxJustificativaReducao)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TarefaHistoricoTrabalho");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.NbHoraRealizado).HasColumnName("NbHoraRealizado");
            this.Property(t => t.NbHoraInicio).HasColumnName("NbHoraInicio");
            this.Property(t => t.NbHoraFinal).HasColumnName("NbHoraFinal");
            this.Property(t => t.OidColaborador).HasColumnName("Colaborador");
            this.Property(t => t.DtRealizado).HasColumnName("DtRealizado");
            this.Property(t => t.TxComentario).HasColumnName("TxComentario");
            this.Property(t => t.NbHoraRestante).HasColumnName("NbHoraRestante");
            this.Property(t => t.OidSituacaoPlanejamento).HasColumnName("SituacaoPlanejamento");
            this.Property(t => t.TxJustificativaReducao).HasColumnName("TxJustificativaReducao");
            this.Property(t => t.OidTarefa).HasColumnName("Tarefa");

            // Relationships
            this.HasOptional(t => t.Colaborador)
                .WithMany(t => t.TarefaHistoricoTrabalhoes)
                .HasForeignKey(d => d.OidColaborador);
            this.HasOptional(t => t.SituacaoPlanejamento)
                .WithMany(t => t.TarefaHistoricoTrabalhoes)
                .HasForeignKey(d => d.OidSituacaoPlanejamento);
            this.HasOptional(t => t.Tarefa)
                .WithMany(t => t.TarefaHistoricoTrabalhos)
                .HasForeignKey(d => d.OidTarefa);

        }
    }
}
