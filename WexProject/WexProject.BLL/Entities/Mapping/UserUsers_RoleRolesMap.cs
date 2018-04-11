using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class UserUsers_RoleRolesMap : EntityTypeConfiguration<UserUsers_RoleRoles>
    {
        public UserUsers_RoleRolesMap()
        {
            // Primary Key
            this.HasKey(t => t.OID);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserUsers_RoleRoles");
            this.Property(t => t.OidRole).HasColumnName("Roles");
            this.Property(t => t.OidUser).HasColumnName("Users");
            this.Property(t => t.OID).HasColumnName("OID");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");

            // Relationships
            this.HasOptional(t => t.Role)
                .WithMany(t => t.UserUsers_RoleRoles)
                .HasForeignKey(d => d.OidRole);
            this.HasOptional(t => t.User)
                .WithMany(t => t.UserUsers_RoleRoles)
                .HasForeignKey(d => d.OidUser);

        }
    }
}
