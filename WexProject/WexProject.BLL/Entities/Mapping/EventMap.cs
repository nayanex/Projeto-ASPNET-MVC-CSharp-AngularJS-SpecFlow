using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class EventMap : EntityTypeConfiguration<Event>
    {
        public EventMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.Subject)
                .HasMaxLength(250);

            this.Property(t => t.Location)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Event");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.ResourceIds).HasColumnName("ResourceIds");
            this.Property(t => t.RecurrencePattern).HasColumnName("RecurrencePattern");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.StartOn).HasColumnName("StartOn");
            this.Property(t => t.EndOn).HasColumnName("EndOn");
            this.Property(t => t.AllDay).HasColumnName("AllDay");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.Label).HasColumnName("Label");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.RecurrenceInfoXml).HasColumnName("RecurrenceInfoXml");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.Event2)
                .WithMany(t => t.Event1)
                .HasForeignKey(d => d.RecurrencePattern);

        }
    }
}
