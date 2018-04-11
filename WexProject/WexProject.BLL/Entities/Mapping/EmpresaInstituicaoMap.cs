using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class EmpresaInstituicaoMap : EntityTypeConfiguration<EmpresaInstituicao>
    {
        public EmpresaInstituicaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxNome)
                .HasMaxLength(100);

            this.Property(t => t.TxSigla)
                .HasMaxLength(30);

            this.Property(t => t.TxEmail)
                .HasMaxLength(100);

            this.Property(t => t.TxFoneFax)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("EmpresaInstituicao");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxNome).HasColumnName("TxNome");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.TxSigla).HasColumnName("TxSigla");
            this.Property(t => t.TxEmail).HasColumnName("TxEmail");
            this.Property(t => t.TxFoneFax).HasColumnName("TxFoneFax");
            this.Property(t => t.Pais).HasColumnName("Pais");

            // Relationships
            this.HasOptional(t => t.Pai)
                .WithMany(t => t.EmpresaInstituicaos)
                .HasForeignKey(d => d.Pais);

        }
    }
}
