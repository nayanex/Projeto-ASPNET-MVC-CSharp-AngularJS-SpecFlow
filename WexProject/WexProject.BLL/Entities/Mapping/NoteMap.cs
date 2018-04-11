using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class NoteMap : EntityTypeConfiguration<Note>
    {
        public NoteMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.Author)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Note");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Author).HasColumnName("Author");
            this.Property(t => t.DateTime).HasColumnName("DateTime");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.ObjectType).HasColumnName("ObjectType");

            // Relationships
            this.HasOptional(t => t.XPObjectType)
                .WithMany(t => t.Notes)
                .HasForeignKey(d => d.ObjectType);

        }
    }
}
