using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Mapping
{
    public class CasoTestePassoResultadoEsperadoInformacaoAdicionalMap : EntityTypeConfiguration<CasoTestePassoResultadoEsperadoInformacaoAdicional>
    {
        public CasoTestePassoResultadoEsperadoInformacaoAdicionalMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("CasoTestePassoResultadoEsperadoInformacaoAdicional");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.CasoTestePassoResultadoEsperado).HasColumnName("CasoTestePassoResultadoEsperado");
            this.Property(t => t.NbSequencia).HasColumnName("NbSequencia");

            // Relationships
            this.HasOptional(t => t.CasoTestePassoResultadoEsperado1)
                .WithMany(t => t.CasoTestePassoResultadoEsperadoInformacaoAdicionals)
                .HasForeignKey(d => d.CasoTestePassoResultadoEsperado);
            this.HasRequired(t => t.Note)
                .WithOptional(t => t.CasoTestePassoResultadoEsperadoInformacaoAdicional);

        }
    }
}
