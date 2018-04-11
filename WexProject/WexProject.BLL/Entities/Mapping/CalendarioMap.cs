using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class CalendarioMap : EntityTypeConfiguration<Calendario>
    {
        public CalendarioMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxDescricao)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Calendario");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.DtInicio).HasColumnName("DtInicio");
            this.Property(t => t.Periodo).HasColumnName("Periodo");
            this.Property(t => t.DtTermino).HasColumnName("DtTermino");
            this.Property(t => t.NbDia).HasColumnName("NbDia");
            this.Property(t => t.CsMes).HasColumnName("CsMes");
            this.Property(t => t.CsCalendario).HasColumnName("CsCalendario");
            this.Property(t => t.CsVigencia).HasColumnName("CsVigencia");
            this.Property(t => t.CsSituacao).HasColumnName("CsSituacao");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
        }
    }
}
