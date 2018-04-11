using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class ModuleInfoMap : EntityTypeConfiguration<ModuleInfo>
    {
        public ModuleInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Version)
                .HasMaxLength(100);

            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.AssemblyFileName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ModuleInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.AssemblyFileName).HasColumnName("AssemblyFileName");
            this.Property(t => t.IsMain).HasColumnName("IsMain");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
        }
    }
}
