using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class PhoneNumberMap : EntityTypeConfiguration<PhoneNumber>
    {
        public PhoneNumberMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.Number)
                .HasMaxLength(100);

            this.Property(t => t.PhoneType)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("PhoneNumber");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Number).HasColumnName("Number");
            this.Property(t => t.Party).HasColumnName("Party");
            this.Property(t => t.PhoneType).HasColumnName("PhoneType");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.Party1)
                .WithMany(t => t.PhoneNumbers)
                .HasForeignKey(d => d.Party);

        }
    }
}
