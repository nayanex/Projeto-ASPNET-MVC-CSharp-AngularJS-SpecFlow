using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.Street)
                .HasMaxLength(100);

            this.Property(t => t.City)
                .HasMaxLength(100);

            this.Property(t => t.StateProvince)
                .HasMaxLength(100);

            this.Property(t => t.ZipPostal)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Address");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Street).HasColumnName("Street");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.StateProvince).HasColumnName("StateProvince");
            this.Property(t => t.ZipPostal).HasColumnName("ZipPostal");
            this.Property(t => t.Country).HasColumnName("Country");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.Country1)
                .WithMany(t => t.Addresses)
                .HasForeignKey(d => d.Country);

        }
    }
}
