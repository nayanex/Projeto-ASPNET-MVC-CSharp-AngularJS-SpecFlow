using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Mapping
{
    public class CasoTestePassoResultadoEsperadoMap : EntityTypeConfiguration<CasoTestePassoResultadoEsperado>
    {
        public CasoTestePassoResultadoEsperadoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("CasoTestePassoResultadoEsperado");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Passo).HasColumnName("Passo");
            this.Property(t => t.TxResultadoEsperado).HasColumnName("TxResultadoEsperado");
            this.Property(t => t.TiposResultados).HasColumnName("TiposResultados");
            this.Property(t => t.NbSequencia).HasColumnName("NbSequencia");
            this.Property(t => t.CsTiposResultado).HasColumnName("CsTiposResultado");
            this.Property(t => t.NbMaiorInformacaoAdicional).HasColumnName("NbMaiorInformacaoAdicional");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");

            // Relationships
            this.HasOptional(t => t.CasoTestePasso)
                .WithMany(t => t.CasoTestePassoResultadoEsperadoes)
                .HasForeignKey(d => d.Passo);

        }
    }
}
