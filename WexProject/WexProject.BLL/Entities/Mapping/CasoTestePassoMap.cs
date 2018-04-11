using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Mapping
{
    public class CasoTestePassoMap : EntityTypeConfiguration<CasoTestePasso>
    {
        public CasoTestePassoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("CasoTestePasso");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.CasoTeste).HasColumnName("CasoTeste");
            this.Property(t => t.TxPasso).HasColumnName("TxPasso");
            this.Property(t => t.NbNumero).HasColumnName("NbNumero");
            this.Property(t => t.NbSequencia).HasColumnName("NbSequencia");
            this.Property(t => t.NbMaiorResultadoEsperado).HasColumnName("NbMaiorResultadoEsperado");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");

            // Relationships
            this.HasOptional(t => t.CasoTeste1)
                .WithMany(t => t.CasoTestePassoes)
                .HasForeignKey(d => d.CasoTeste);

        }
    }
}
