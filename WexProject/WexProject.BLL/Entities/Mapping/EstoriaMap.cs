using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Escopo;

namespace WexProject.BLL.Entities.Mapping
{
    public class EstoriaMap : EntityTypeConfiguration<Estoria>
    {
        public EstoriaMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxID)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Estoria");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxID).HasColumnName("TxID");
            this.Property(t => t.TxTitulo).HasColumnName("TxTitulo");
            this.Property(t => t.OidModulo).HasColumnName("Modulo");
            this.Property(t => t.CsTipo).HasColumnName("CsTipo");
            this.Property(t => t.TxGostariaDe).HasColumnName("TxGostariaDe");
            this.Property(t => t.TxEntaoPoderei).HasColumnName("TxEntaoPoderei");
            this.Property(t => t.OidProjetoParteInteressada).HasColumnName("ProjetoParteInteressada");
            this.Property(t => t.CsValorNegocio).HasColumnName("CsValorNegocio");
            this.Property(t => t.OidBeneficiado).HasColumnName("ComoUmBeneficiado");
            this.Property(t => t.TxReferencias).HasColumnName("TxReferencias");
            this.Property(t => t.TxDuvidas).HasColumnName("TxDuvidas");
            this.Property(t => t.OidEstoriaPai).HasColumnName("EstoriaPai");
            this.Property(t => t.NbPrioridade).HasColumnName("NbPrioridade");
            this.Property(t => t.NbTamanho).HasColumnName("NbTamanho");
            this.Property(t => t.TxPremissas).HasColumnName("TxPremissas");
            this.Property(t => t.CsSituacao).HasColumnName("CsSituacao");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.TxAnotacoes).HasColumnName("TxAnotacoes");
            this.Property(t => t.salvandoPrioridades).HasColumnName("salvandoPrioridades");
            this.Property(t => t.OidCiclo).HasColumnName("Ciclo");
            this.Property(t => t.CsEmAnalise).HasColumnName("CsEmAnalise");

            // Relationships
            this.HasOptional(t => t.Beneficiado)
                .WithMany(t => t.Estorias)
                .HasForeignKey(d => d.OidBeneficiado);
            this.HasOptional(t => t.CicloDesenv)
                .WithMany(t => t.Estorias)
                .HasForeignKey(d => d.OidCiclo);
            this.HasOptional(t => t.EstoriaPai)
                .WithMany(t => t.Estorias)
                .HasForeignKey(d => d.OidEstoriaPai);
            this.HasOptional(t => t.Modulo)
                .WithMany(t => t.Estorias)
                .HasForeignKey(d => d.OidModulo);
            this.HasOptional(t => t.ProjetoParteInteressada)
                .WithMany(t => t.Estorias)
                .HasForeignKey(d => d.OidProjetoParteInteressada);

        }
    }
}
