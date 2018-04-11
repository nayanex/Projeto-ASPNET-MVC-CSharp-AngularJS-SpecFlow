using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Mapping
{
    public class ColaboradorPeriodoAquisitivoMap : EntityTypeConfiguration<ColaboradorPeriodoAquisitivo>
    {
        public ColaboradorPeriodoAquisitivoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("ColaboradorPeriodoAquisitivo");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.OidColaborador).HasColumnName("Colaborador");
            this.Property(t => t.DtInicio).HasColumnName("DtInicio");
            this.Property(t => t.DtTermino).HasColumnName("DtTermino");
            this.Property(t => t.NbFeriasPlanejadas).HasColumnName("NbFeriasPlanejadas");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.DtMaxima).HasColumnName("DtMaxima");

            // Relationships
            this.HasOptional(t => t.Colaborador)
                .WithMany(t => t.ColaboradorPeriodoAquisitivoes)
                .HasForeignKey(d => d.OidColaborador);

        }
    }
}
