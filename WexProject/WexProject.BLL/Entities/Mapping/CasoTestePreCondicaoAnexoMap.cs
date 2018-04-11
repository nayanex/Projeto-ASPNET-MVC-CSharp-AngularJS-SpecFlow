using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Mapping
{
    public class CasoTestePreCondicaoAnexoMap : EntityTypeConfiguration<CasoTestePreCondicaoAnexo>
    {
        public CasoTestePreCondicaoAnexoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("CasoTestePreCondicaoAnexo");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.File).HasColumnName("File");
            this.Property(t => t.CasoTestePreCondicao).HasColumnName("CasoTestePreCondicao");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");

            // Relationships
            this.HasOptional(t => t.CasoTestePreCondicao1)
                .WithMany(t => t.CasoTestePreCondicaoAnexoes)
                .HasForeignKey(d => d.CasoTestePreCondicao);
            this.HasOptional(t => t.FileData)
                .WithMany(t => t.CasoTestePreCondicaoAnexoes)
                .HasForeignKey(d => d.File);

        }
    }
}
