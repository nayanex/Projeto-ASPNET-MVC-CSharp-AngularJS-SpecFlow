using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class FileDataMap : EntityTypeConfiguration<FileData>
    {
        public FileDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.FileName)
                .HasMaxLength(260);

            // Table & Column Mappings
            this.ToTable("FileData");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.size).HasColumnName("size");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
        }
    }
}
