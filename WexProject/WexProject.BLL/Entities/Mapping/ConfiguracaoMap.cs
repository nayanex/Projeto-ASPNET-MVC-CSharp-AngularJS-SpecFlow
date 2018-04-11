using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Mapping
{
    public class ConfiguracaoMap : EntityTypeConfiguration<Configuracao>
    {
        public ConfiguracaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("Configuracao");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.NbQtdeMaxVenda).HasColumnName("NbQtdeMaxVenda");
            this.Property(t => t.NbQtdeMaxFerias).HasColumnName("NbQtdeMaxFerias");
            this.Property(t => t.NbDtMaxTirarFerias).HasColumnName("NbDtMaxTirarFerias");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
        }
    }
}
