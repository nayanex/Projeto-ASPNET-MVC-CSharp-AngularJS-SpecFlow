using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class FeriasMap : EntityTypeConfiguration<Ferias>
    {
        public FeriasMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("Ferias");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Colaborador).HasColumnName("Colaborador");
            this.Property(t => t.NbAnoBase).HasColumnName("NbAnoBase");
            this.Property(t => t.DtVencimento).HasColumnName("DtVencimento");
            this.Property(t => t.DtDataMaxima).HasColumnName("DtDataMaxima");
            this.Property(t => t.CsSituacaoFerias).HasColumnName("CsSituacaoFerias");
            this.Property(t => t.TxObservacoes).HasColumnName("TxObservacoes");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.Colaborador1)
                .WithMany(t => t.Ferias)
                .HasForeignKey(d => d.Colaborador);

        }
    }
}
