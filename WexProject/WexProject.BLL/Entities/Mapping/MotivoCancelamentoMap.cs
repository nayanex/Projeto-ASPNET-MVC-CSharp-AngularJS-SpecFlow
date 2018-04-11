using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Execucao;

namespace WexProject.BLL.Entities.Mapping
{
    public class MotivoCancelamentoMap : EntityTypeConfiguration<MotivoCancelamento>
    {
        public MotivoCancelamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxDescricao)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("MotivoCancelamento");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.CsSituacao).HasColumnName("CsSituacao");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
        }
    }
}
