using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Escopo;

namespace WexProject.BLL.Entities.Mapping
{
    public class ModuloMap : EntityTypeConfiguration<Modulo>
    {
        public ModuloMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxID)
                .HasMaxLength(100);

            this.Property(t => t.TxNome)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Modulo");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.OidProjeto).HasColumnName("Projeto");
            this.Property(t => t.TxID).HasColumnName("TxID");
            this.Property(t => t.TxNome).HasColumnName("TxNome");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.OidModuloPai).HasColumnName("ModuloPai");
            this.Property(t => t.NbEsforcoPlanejado).HasColumnName("NbEsforcoPlanejado");
            this.Property(t => t.NbPontosTotal).HasColumnName("NbPontosTotal");
            this.Property(t => t.NbPontosNaoIniciado).HasColumnName("NbPontosNaoIniciado");
            this.Property(t => t.NbPontosEmAnalise).HasColumnName("NbPontosEmAnalise");
            this.Property(t => t.NbPontosEmDesenv).HasColumnName("NbPontosEmDesenv");
            this.Property(t => t.NbPontosPronto).HasColumnName("NbPontosPronto");
            this.Property(t => t.NbPontosDesvio).HasColumnName("NbPontosDesvio");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.ModuloPai)
                .WithMany(t => t.Modulos)
                .HasForeignKey(d => d.OidModuloPai);
            this.HasOptional(t => t.Projeto)
                .WithMany(t => t.Moduloes)
                .HasForeignKey(d => d.OidProjeto);

        }
    }
}
