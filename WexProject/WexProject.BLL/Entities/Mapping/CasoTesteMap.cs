using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Mapping
{
    public class CasoTesteMap : EntityTypeConfiguration<CasoTeste>
    {
        public CasoTesteMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxID)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("CasoTeste");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxID).HasColumnName("TxID");
            this.Property(t => t.Requisito).HasColumnName("Requisito");
            this.Property(t => t.EstoriaCriacao).HasColumnName("EstoriaCriacao");
            this.Property(t => t.TxSumario).HasColumnName("TxSumario");
            this.Property(t => t.CsCasoTeste).HasColumnName("CsCasoTeste");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.NbMaiorPrecondicao).HasColumnName("NbMaiorPrecondicao");
            this.Property(t => t.NbMaiorPasso).HasColumnName("NbMaiorPasso");
            this.Property(t => t.Usuario).HasColumnName("Usuario");
            this.Property(t => t.DtHoraEData).HasColumnName("DtHoraEData");
            this.Property(t => t.CsVerificandoNestedObject).HasColumnName("CsVerificandoNestedObject");

            // Relationships
            this.HasOptional(t => t.Estoria)
                .WithMany(t => t.CasoTestes)
                .HasForeignKey(d => d.EstoriaCriacao);
            this.HasOptional(t => t.Requisito1)
                .WithMany(t => t.CasoTestes)
                .HasForeignKey(d => d.Requisito);
            this.HasOptional(t => t.User)
                .WithMany(t => t.CasoTestes)
                .HasForeignKey(d => d.Usuario);

        }
    }
}
