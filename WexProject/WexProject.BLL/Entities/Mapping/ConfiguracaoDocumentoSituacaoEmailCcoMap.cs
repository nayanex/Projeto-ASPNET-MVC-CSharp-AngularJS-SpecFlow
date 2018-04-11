using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class ConfiguracaoDocumentoSituacaoEmailCcoMap : EntityTypeConfiguration<ConfiguracaoDocumentoSituacaoEmailCco>
    {
        public ConfiguracaoDocumentoSituacaoEmailCcoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("ConfiguracaoDocumentoSituacaoEmailCco");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.ConfiguracaoDocumentoSituacao).HasColumnName("ConfiguracaoDocumentoSituacao");

            // Relationships
            this.HasOptional(t => t.ConfiguracaoDocumentoSituacao1)
                .WithMany(t => t.ConfiguracaoDocumentoSituacaoEmailCcoes)
                .HasForeignKey(d => d.ConfiguracaoDocumentoSituacao);
            this.HasRequired(t => t.ConfiguracaoDocumentoSituacaoEmail)
                .WithOptional(t => t.ConfiguracaoDocumentoSituacaoEmailCco);

        }
    }
}
