using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Mapping
{
    public class CasoTestePreCondicaoInformacaoAdicionalMap : EntityTypeConfiguration<CasoTestePreCondicaoInformacaoAdicional>
    {
        public CasoTestePreCondicaoInformacaoAdicionalMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("CasoTestePreCondicaoInformacaoAdicional");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.CasoTestePreCondicao).HasColumnName("CasoTestePreCondicao");
            this.Property(t => t.NbSequencia).HasColumnName("NbSequencia");

            // Relationships
            this.HasOptional(t => t.CasoTestePreCondicao1)
                .WithMany(t => t.CasoTestePreCondicaoInformacaoAdicionals)
                .HasForeignKey(d => d.CasoTestePreCondicao);
            this.HasRequired(t => t.Note)
                .WithOptional(t => t.CasoTestePreCondicaoInformacaoAdicional);

        }
    }
}
