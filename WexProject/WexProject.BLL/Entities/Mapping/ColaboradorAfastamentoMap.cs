using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Mapping
{
    public class ColaboradorAfastamentoMap : EntityTypeConfiguration<ColaboradorAfastamento>
    {
        public ColaboradorAfastamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxObservacao)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("ColaboradorAfastamento");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Colaborador).HasColumnName("Colaborador");
            this.Property(t => t.DtInicio).HasColumnName("DtInicio");
            this.Property(t => t.DtTermino).HasColumnName("DtTermino");
            this.Property(t => t.TipoAfastamento).HasColumnName("TipoAfastamento");
            this.Property(t => t.TxObservacao).HasColumnName("TxObservacao");
            this.Property(t => t.IsCriadoAutomaticamente).HasColumnName("IsCriadoAutomaticamente");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");

            // Relationships
            this.HasOptional(t => t.Colaborador1)
                .WithMany(t => t.ColaboradorAfastamentoes)
                .HasForeignKey(d => d.Colaborador);
            this.HasOptional(t => t.TipoAfastamento1)
                .WithMany(t => t.ColaboradorAfastamentoes)
                .HasForeignKey(d => d.TipoAfastamento);

        }
    }
}
