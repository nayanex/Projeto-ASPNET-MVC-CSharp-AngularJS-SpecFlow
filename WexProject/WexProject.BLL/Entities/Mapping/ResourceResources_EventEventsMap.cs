using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class ResourceResources_EventEventsMap : EntityTypeConfiguration<ResourceResources_EventEvents>
    {
        public ResourceResources_EventEventsMap()
        {
            // Primary Key
            this.HasKey(t => t.OID);

            // Properties
            // Table & Column Mappings
            this.ToTable("ResourceResources_EventEvents");
            this.Property(t => t.Events).HasColumnName("Events");
            this.Property(t => t.Resources).HasColumnName("Resources");
            this.Property(t => t.OID).HasColumnName("OID");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");

            // Relationships
            this.HasOptional(t => t.Event)
                .WithMany(t => t.ResourceResources_EventEvents)
                .HasForeignKey(d => d.Events);
            this.HasOptional(t => t.Resource)
                .WithMany(t => t.ResourceResources_EventEvents)
                .HasForeignKey(d => d.Resources);

        }
    }
}
