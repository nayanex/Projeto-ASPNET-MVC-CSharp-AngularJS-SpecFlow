using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Execucao;

namespace WexProject.BLL.Entities.Mapping
{
    public class CicloDesenvMap : EntityTypeConfiguration<CicloDesenv>
    {
        public CicloDesenvMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxMeta)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("CicloDesenv");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.NbCiclo).HasColumnName("NbCiclo");
            this.Property(t => t.DtInicio).HasColumnName("DtInicio");
            this.Property(t => t.DtTermino).HasColumnName("DtTermino");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.Projeto).HasColumnName("Projeto");
            this.Property(t => t.TxMeta).HasColumnName("TxMeta");
            this.Property(t => t.NbPontosPlanejados).HasColumnName("NbPontosPlanejados");
            this.Property(t => t.NbPontosRealizados).HasColumnName("NbPontosRealizados");
            this.Property(t => t.CsSituacaoCiclo).HasColumnName("CsSituacaoCiclo");
            this.Property(t => t.NbAlcanceMeta).HasColumnName("NbAlcanceMeta");
            this.Property(t => t.NbMaiorCicloDesenvEstoria).HasColumnName("NbMaiorCicloDesenvEstoria");
            this.Property(t => t.MotivoCancelamento).HasColumnName("MotivoCancelamento");
            this.Property(t => t.IsCancelado).HasColumnName("IsCancelado");

            // Relationships
            this.HasOptional(t => t.MotivoCancelamento1)
                .WithMany(t => t.CicloDesenvs)
                .HasForeignKey(d => d.MotivoCancelamento);
            this.HasOptional(t => t.Projeto1)
                .WithMany(t => t.CicloDesenvs)
                .HasForeignKey(d => d.Projeto);

        }
    }
}
