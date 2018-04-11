using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class CargoMap : EntityTypeConfiguration<Cargo>
    {
        public CargoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxDescricao)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Cargo");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
        }
    }
}
