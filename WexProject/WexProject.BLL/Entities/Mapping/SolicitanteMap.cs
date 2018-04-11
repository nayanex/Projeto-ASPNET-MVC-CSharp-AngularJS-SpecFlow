using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Escopo;

namespace WexProject.BLL.Entities.Mapping
{
    public class SolicitanteMap : EntityTypeConfiguration<Solicitante>
    {
        public SolicitanteMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxNome)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Solicitante");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxNome).HasColumnName("TxNome");
            this.Property(t => t.EmpresaInstituicao).HasColumnName("EmpresaInstituicao");
            this.Property(t => t.Cargo).HasColumnName("Cargo");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.Cargo1)
                .WithMany(t => t.Solicitantes)
                .HasForeignKey(d => d.Cargo);
            this.HasOptional(t => t.EmpresaInstituicao1)
                .WithMany(t => t.Solicitantes)
                .HasForeignKey(d => d.EmpresaInstituicao);

        }
    }
}
