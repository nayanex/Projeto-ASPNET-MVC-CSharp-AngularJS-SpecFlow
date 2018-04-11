using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Mapping
{
    public class CasoTestePassoResultadoEsperadoAnexoMap : EntityTypeConfiguration<CasoTestePassoResultadoEsperadoAnexo>
    {
        public CasoTestePassoResultadoEsperadoAnexoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("CasoTestePassoResultadoEsperadoAnexo");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.File).HasColumnName("File");
            this.Property(t => t.CasoTestePassoResultadoEsperado).HasColumnName("CasoTestePassoResultadoEsperado");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");

            // Relationships
            this.HasOptional(t => t.CasoTestePassoResultadoEsperado1)
                .WithMany(t => t.CasoTestePassoResultadoEsperadoAnexoes)
                .HasForeignKey(d => d.CasoTestePassoResultadoEsperado);
            this.HasOptional(t => t.FileData)
                .WithMany(t => t.CasoTestePassoResultadoEsperadoAnexoes)
                .HasForeignKey(d => d.File);

        }
    }
}
