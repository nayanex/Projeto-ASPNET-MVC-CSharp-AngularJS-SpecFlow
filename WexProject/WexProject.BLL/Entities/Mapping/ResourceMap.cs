using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class ResourceMap : EntityTypeConfiguration<Resource>
    {
        public ResourceMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.Caption)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Resource");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Color).HasColumnName("Color");
            this.Property(t => t.Caption).HasColumnName("Caption");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
        }
    }
}
