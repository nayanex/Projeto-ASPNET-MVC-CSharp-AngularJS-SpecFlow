using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class RoleBaseMap : EntityTypeConfiguration<RoleBase>
    {
        public RoleBaseMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("RoleBase");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.ObjectType).HasColumnName("ObjectType");

            // Relationships
            this.HasOptional(t => t.XPObjectType)
                .WithMany(t => t.RoleBases)
                .HasForeignKey(d => d.ObjectType);

        }
    }
}
