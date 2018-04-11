using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class ConfiguracaoDocumentoSituacaoEmailCcMap : EntityTypeConfiguration<ConfiguracaoDocumentoSituacaoEmailCc>
    {
        public ConfiguracaoDocumentoSituacaoEmailCcMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("ConfiguracaoDocumentoSituacaoEmailCc");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.ConfiguracaoDocumentoSituacao).HasColumnName("ConfiguracaoDocumentoSituacao");
            this.Property(t => t.SolicitacaoOrcamento).HasColumnName("SolicitacaoOrcamento");

            // Relationships
            this.HasOptional(t => t.ConfiguracaoDocumentoSituacao1)
                .WithMany(t => t.ConfiguracaoDocumentoSituacaoEmailCcs)
                .HasForeignKey(d => d.ConfiguracaoDocumentoSituacao);
            this.HasRequired(t => t.ConfiguracaoDocumentoSituacaoEmail)
                .WithOptional(t => t.ConfiguracaoDocumentoSituacaoEmailCc);
            this.HasOptional(t => t.SolicitacaoOrcamento1)
                .WithMany(t => t.ConfiguracaoDocumentoSituacaoEmailCcs)
                .HasForeignKey(d => d.SolicitacaoOrcamento);

        }
    }
}
