using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class ConfiguracaoDocumentoSituacaoEmailMap : EntityTypeConfiguration<ConfiguracaoDocumentoSituacaoEmail>
    {
        public ConfiguracaoDocumentoSituacaoEmailMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxEmail)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ConfiguracaoDocumentoSituacaoEmail");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxEmail).HasColumnName("TxEmail");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.ObjectType).HasColumnName("ObjectType");

            // Relationships
            this.HasOptional(t => t.XPObjectType)
                .WithMany(t => t.ConfiguracaoDocumentoSituacaoEmails)
                .HasForeignKey(d => d.ObjectType);

        }
    }
}
