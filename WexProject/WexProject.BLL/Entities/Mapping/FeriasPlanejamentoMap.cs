using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Mapping
{
    public class FeriasPlanejamentoMap : EntityTypeConfiguration<FeriasPlanejamento>
    {
        public FeriasPlanejamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxPlanejado)
                .HasMaxLength(100);

            this.Property(t => t.TxAtualizado)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("FeriasPlanejamento");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Ferias).HasColumnName("Ferias");
            this.Property(t => t.Modalidade).HasColumnName("Modalidade");
            this.Property(t => t.DtInicio).HasColumnName("DtInicio");
            this.Property(t => t.Vender).HasColumnName("Vender");
            this.Property(t => t.Realizadas).HasColumnName("Realizadas");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.Periodo).HasColumnName("Periodo");
            this.Property(t => t.CsSituacaoFerias).HasColumnName("CsSituacaoFerias");
            this.Property(t => t.TxPlanejado).HasColumnName("TxPlanejado");
            this.Property(t => t.TxAtualizado).HasColumnName("TxAtualizado");
            this.Property(t => t.Afastamento).HasColumnName("Afastamento");
            this.Property(t => t.DtRetorno).HasColumnName("DtRetorno");

            // Relationships
            this.HasOptional(t => t.ColaboradorAfastamento)
                .WithMany(t => t.FeriasPlanejamentoes)
                .HasForeignKey(d => d.Afastamento);
            this.HasOptional(t => t.ColaboradorPeriodoAquisitivo)
                .WithMany(t => t.FeriasPlanejamentoes)
                .HasForeignKey(d => d.Periodo);
            this.HasOptional(t => t.Feria)
                .WithMany(t => t.FeriasPlanejamentoes)
                .HasForeignKey(d => d.Ferias);
            this.HasOptional(t => t.ModalidadeFeria)
                .WithMany(t => t.FeriasPlanejamentoes)
                .HasForeignKey(d => d.Modalidade);

        }
    }
}
