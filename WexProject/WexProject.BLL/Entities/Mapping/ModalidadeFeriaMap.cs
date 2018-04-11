using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Mapping
{
    public class ModalidadeFeriaMap : EntityTypeConfiguration<ModalidadeFeria>
    {
        public ModalidadeFeriaMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("ModalidadeFerias");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.NbDias).HasColumnName("NbDias");
            this.Property(t => t.PodeVender).HasColumnName("PodeVender");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.CsSituacao).HasColumnName("CsSituacao");
        }
    }
}
