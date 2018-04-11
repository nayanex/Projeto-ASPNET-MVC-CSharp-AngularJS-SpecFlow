using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Planejamento;

namespace WexProject.BLL.Entities.Mapping
{
    public class SituacaoPlanejamentoMap : EntityTypeConfiguration<SituacaoPlanejamento>
    {
        public SituacaoPlanejamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxDescricao)
                .HasMaxLength(30);

            this.Property(t => t.TxKeys)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("SituacaoPlanejamento");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.CsSituacao).HasColumnName("CsSituacao");
            this.Property(t => t.CsTipo).HasColumnName("CsTipo");
            this.Property(t => t.BlImagem).HasColumnName("BlImagem");
            this.Property(t => t.CsPadrao).HasColumnName("CsPadrao");
            this.Property(t => t.KeyPress).HasColumnName("KeyPress");
            this.Property(t => t.TxKeys).HasColumnName("TxKeys");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
        }
    }
}
