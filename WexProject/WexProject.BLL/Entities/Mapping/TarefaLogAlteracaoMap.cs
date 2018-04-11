using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Planejamento;

namespace WexProject.BLL.Entities.Mapping
{
    public class TarefaLogAlteracaoMap : EntityTypeConfiguration<TarefaLogAlteracao>
    {
        public TarefaLogAlteracaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("TarefaLogAlteracao");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.OidTarefa).HasColumnName("Tarefa");
            this.Property(t => t.DtDataHoraLog).HasColumnName("DtDataHoraLog");
            this.Property(t => t.OidColaborador).HasColumnName("Colaborador");
            this.Property(t => t.TxAlteracoes).HasColumnName("TxAlteracoes");

            // Relationships
            this.HasOptional(t => t.Colaborador)
                .WithMany(t => t.TarefaLogAlteracaos)
                .HasForeignKey(d => d.OidColaborador);
            this.HasOptional(t => t.Tarefa)
                .WithMany(t => t.LogsAlteracao)
                .HasForeignKey(d => d.OidTarefa);

        }
    }
}
