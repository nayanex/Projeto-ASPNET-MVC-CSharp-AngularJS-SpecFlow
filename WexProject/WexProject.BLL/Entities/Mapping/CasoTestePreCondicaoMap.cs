using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Mapping
{
    public class CasoTestePreCondicaoMap : EntityTypeConfiguration<CasoTestePreCondicao>
    {
        public CasoTestePreCondicaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("CasoTestePreCondicao");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.CasoTeste).HasColumnName("CasoTeste");
            this.Property(t => t.NbSequencia).HasColumnName("NbSequencia");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.NbMaiorInformacaoAdicional).HasColumnName("NbMaiorInformacaoAdicional");

            // Relationships
            this.HasOptional(t => t.CasoTeste1)
                .WithMany(t => t.CasoTestePreCondicaos)
                .HasForeignKey(d => d.CasoTeste);
            this.HasRequired(t => t.Note)
                .WithOptional(t => t.CasoTestePreCondicao);

        }
    }
}
