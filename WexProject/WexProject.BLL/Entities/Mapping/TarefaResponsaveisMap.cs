using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Planejamento;

namespace WexProject.BLL.Entities.Mapping
{
    public class TarefaResponsaveisMap : EntityTypeConfiguration<TarefaResponsaveis>
    {
        public TarefaResponsaveisMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("TarefaResponsaveis");
            this.Property(t => t.OidColaborador).HasColumnName("Responsaveis");
            this.Property(t => t.OidTarefa).HasColumnName("Tarefas");
            this.Property(t => t.Oid).HasColumnName("OID");

            // Relationships
            this.HasOptional(t => t.Colaborador)
                .WithMany(t => t.TarefaResponsaveis)
                .HasForeignKey(d => d.OidColaborador);
            this.HasOptional(t => t.Tarefa)
                .WithMany(t => t.TarefaResponsaveis)
                .HasForeignKey(d => d.OidTarefa);

        }
    }
}
