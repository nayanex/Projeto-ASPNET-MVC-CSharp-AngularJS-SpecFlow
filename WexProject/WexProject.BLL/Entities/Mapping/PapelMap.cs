using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class PapelMap : EntityTypeConfiguration<Papel>
    {
        public PapelMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxNome)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Papel");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxNome).HasColumnName("TxNome");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
        }
    }
}
