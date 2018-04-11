using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.StoredPassword)
                .HasMaxLength(100);

            this.Property(t => t.UserName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("User");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.StoredPassword).HasColumnName("StoredPassword");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.ChangePasswordOnFirstLogon).HasColumnName("ChangePasswordOnFirstLogon");
            this.Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            this.HasRequired(t => t.Person)
                .WithOptional(t => t.User);

        }
    }
}
