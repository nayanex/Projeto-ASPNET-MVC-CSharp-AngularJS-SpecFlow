using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class ParteInteressadaMap : EntityTypeConfiguration<ParteInteressada>
    {
        public ParteInteressadaMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxParteInteressadaNome)
                .HasMaxLength(100);

            this.Property(t => t.TxTelefoneFixo)
                .HasMaxLength(100);

            this.Property(t => t.TxCelular)
                .HasMaxLength(100);

            this.Property(t => t.TxEmail)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ParteInteressada");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Projeto).HasColumnName("Projeto");
            this.Property(t => t.Cargo).HasColumnName("Cargo");
            this.Property(t => t.TxParteInteressadaNome).HasColumnName("TxParteInteressadaNome");
            this.Property(t => t.TxTelefoneFixo).HasColumnName("TxTelefoneFixo");
            this.Property(t => t.TxCelular).HasColumnName("TxCelular");
            this.Property(t => t.TxEmail).HasColumnName("TxEmail");
            this.Property(t => t.EmpresaInstituicao).HasColumnName("EmpresaInstituicao");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.IsColaborador).HasColumnName("IsColaborador");
            this.Property(t => t.Colaborador).HasColumnName("Colaborador");

            // Relationships
            this.HasOptional(t => t.Cargo1)
                .WithMany(t => t.ParteInteressadas)
                .HasForeignKey(d => d.Cargo);
            this.HasOptional(t => t.Colaborador1)
                .WithMany(t => t.ParteInteressadas)
                .HasForeignKey(d => d.Colaborador);
            this.HasOptional(t => t.EmpresaInstituicao1)
                .WithMany(t => t.ParteInteressadas)
                .HasForeignKey(d => d.EmpresaInstituicao);
            this.HasOptional(t => t.Projeto1)
                .WithMany(t => t.ParteInteressadas)
                .HasForeignKey(d => d.Projeto);

        }
    }
}
