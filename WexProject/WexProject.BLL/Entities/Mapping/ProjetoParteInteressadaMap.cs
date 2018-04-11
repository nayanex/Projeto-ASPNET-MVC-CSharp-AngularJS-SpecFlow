using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class ProjetoParteInteressadaMap : EntityTypeConfiguration<ProjetoParteInteressada>
    {
        public ProjetoParteInteressadaMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("ProjetoParteInteressada");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Projeto).HasColumnName("Projeto");
            this.Property(t => t.TxParteInteressada).HasColumnName("TxParteInteressada");
            this.Property(t => t.ParteInteressadaPapel).HasColumnName("ParteInteressadaPapel");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.csVerificarOrigemProjeto).HasColumnName("csVerificarOrigemProjeto");
            this.Property(t => t.csVerificarOrigemParteInteressada).HasColumnName("csVerificarOrigemParteInteressada");

            // Relationships
            this.HasOptional(t => t.Papel)
                .WithMany(t => t.ProjetoParteInteressadas)
                .HasForeignKey(d => d.ParteInteressadaPapel);
            this.HasOptional(t => t.ParteInteressada)
                .WithMany(t => t.ProjetoParteInteressadas)
                .HasForeignKey(d => d.TxParteInteressada);
            this.HasOptional(t => t.Projeto1)
                .WithMany(t => t.ProjetoParteInteressadas)
                .HasForeignKey(d => d.Projeto);

        }
    }
}
