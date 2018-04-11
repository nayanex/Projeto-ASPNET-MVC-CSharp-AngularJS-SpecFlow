using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class PhoneTypeMap : EntityTypeConfiguration<PhoneType>
    {
        public PhoneTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TypeName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("PhoneType");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
        }
    }
}
