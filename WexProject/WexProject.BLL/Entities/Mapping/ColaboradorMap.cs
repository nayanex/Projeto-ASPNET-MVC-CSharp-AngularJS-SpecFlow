using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Mapping
{
    public class ColaboradorMap : EntityTypeConfiguration<Colaborador>
    {
        public ColaboradorMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxMatricula)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Colaborador");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxMatricula).HasColumnName("TxMatricula");
            this.Property(t => t.DtAdmissao).HasColumnName("DtAdmissao");
            this.Property(t => t.OidCoordenador).HasColumnName("Coordenador");
            this.Property(t => t.OidUsuario).HasColumnName("Usuario");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.OidCargo).HasColumnName("Cargo");
            this.Property(t => t.OidColaboradorUltimoFiltro).HasColumnName("ColaboradorUltimoFiltro");

            // Relationships
            this.HasOptional(t => t.Cargo)
                .WithMany(t => t.Colaboradors)
                .HasForeignKey(d => d.OidCargo);
            this.HasOptional(t => t.ColaboradorUltimoFiltro)
                .WithMany(t => t.Colaboradors)
                .HasForeignKey(d => d.OidColaboradorUltimoFiltro);
            this.HasOptional(t => t.Coordenador)
                .WithMany(t => t.Colaborador1)
                .HasForeignKey(d => d.OidCoordenador);
            this.HasOptional( t => t.Usuario )
                .WithMany( t => t.Colaboradors )
                .HasForeignKey( d => d.OidUsuario );

        }
    }
}
