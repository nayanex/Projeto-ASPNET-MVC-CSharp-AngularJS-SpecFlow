using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class PersistentPermissionMap : EntityTypeConfiguration<PersistentPermission>
    {
        public PersistentPermissionMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.SerializedPermission)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("PersistentPermission");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.SerializedPermission).HasColumnName("SerializedPermission");
            this.Property(t => t.Role).HasColumnName("Role");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.RoleBase)
                .WithMany(t => t.PersistentPermissions)
                .HasForeignKey(d => d.Role);

        }
    }
}
