using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Mapping
{
    public class TipoAfastamentoMap : EntityTypeConfiguration<TipoAfastamento>
    {
        public TipoAfastamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxDescricao)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("TipoAfastamento");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.IsParaFeriasRealizadas).HasColumnName("IsParaFeriasRealizadas");
            this.Property(t => t.IsRemunerado).HasColumnName("IsRemunerado");
            this.Property(t => t.CsSituacao).HasColumnName("CsSituacao");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
        }
    }
}
