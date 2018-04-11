using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class PaiMap : EntityTypeConfiguration<Pai>
    {
        public PaiMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxMascara)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("Pais");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxMascara).HasColumnName("TxMascara");
            this.Property(t => t.CsSituacao).HasColumnName("CsSituacao");
            this.Property(t => t.IsPadrao).HasColumnName("IsPadrao");
            this.Property(t => t.Country).HasColumnName("Country");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");

            // Relationships
            this.HasOptional(t => t.Country1)
                .WithMany(t => t.Pais)
                .HasForeignKey(d => d.Country);

        }
    }
}
