using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class ConfiguracaoDocumentoSituacaoMap : EntityTypeConfiguration<ConfiguracaoDocumentoSituacao>
    {
        public ConfiguracaoDocumentoSituacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxDescricao)
                .HasMaxLength(100);

            this.Property(t => t.TxNomeCor)
                .HasMaxLength(100);

            this.Property(t => t.TxCc)
                .HasMaxLength(100);

            this.Property(t => t.TxCco)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ConfiguracaoDocumentoSituacao");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.ConfiguracaoDocumento).HasColumnName("ConfiguracaoDocumento");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.TxNomeCor).HasColumnName("TxNomeCor");
            this.Property(t => t.TypeColor).HasColumnName("TypeColor");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.IsSituacaoInicial).HasColumnName("IsSituacaoInicial");
            this.Property(t => t.TxCc).HasColumnName("TxCc");
            this.Property(t => t.TxCco).HasColumnName("TxCco");

            // Relationships
            this.HasOptional(t => t.ConfiguracaoDocumento1)
                .WithMany(t => t.ConfiguracaoDocumentoSituacaos)
                .HasForeignKey(d => d.ConfiguracaoDocumento);

        }
    }
}
