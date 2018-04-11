using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class PartyMap : EntityTypeConfiguration<Party>
    {
        public PartyMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("Party");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Photo).HasColumnName("Photo");
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.ObjectType).HasColumnName("ObjectType");

            // Relationships
            this.HasOptional(t => t.Address)
                .WithMany(t => t.Parties)
                .HasForeignKey(d => d.Address1);
            this.HasOptional(t => t.Address3)
                .WithMany(t => t.Parties1)
                .HasForeignKey(d => d.Address2);
            this.HasOptional(t => t.XPObjectType)
                .WithMany(t => t.Parties)
                .HasForeignKey(d => d.ObjectType);

        }
    }
}
