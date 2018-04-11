using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class ConfiguracaoDocumentoMap : EntityTypeConfiguration<ConfiguracaoDocumento>
    {
        public ConfiguracaoDocumentoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("ConfiguracaoDocumento");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.CsTipoDocumento).HasColumnName("CsTipoDocumento");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
        }
    }
}
