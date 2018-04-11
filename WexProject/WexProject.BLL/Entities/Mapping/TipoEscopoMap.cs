using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class TipoEscopoMap : EntityTypeConfiguration<TipoEscopo>
    {
        public TipoEscopoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxEscopo)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TipoEscopo");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxEscopo).HasColumnName("TxEscopo");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
        }
    }
}
