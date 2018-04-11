using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Planejamento;

namespace WexProject.BLL.Entities.Mapping
{
    public class CronogramaTarefaMap : EntityTypeConfiguration<CronogramaTarefa>
    {
        public CronogramaTarefaMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("CronogramaTarefa");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.OidTarefa).HasColumnName("Tarefa");
            this.Property(t => t.NbID).HasColumnName("NbID");
            this.Property(t => t.OidCronograma).HasColumnName("Cronograma");
        }
    }
}
