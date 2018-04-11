using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("Role");
            this.Property(t => t.Oid).HasColumnName("Oid");

            // Relationships
            this.HasRequired(t => t.RoleBase)
                .WithOptional(t => t.Role);

        }
    }
}
