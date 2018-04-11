using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Escopo;

namespace WexProject.BLL.Entities.Mapping
{
    public class RequisitoMap : EntityTypeConfiguration<Requisito>
    {
        public RequisitoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxID)
                .HasMaxLength(20);

            this.Property(t => t.TxNome)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Requisito");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxID).HasColumnName("TxID");
            this.Property(t => t.TxNome).HasColumnName("TxNome");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.TxLinkPrototipo).HasColumnName("TxLinkPrototipo");
            this.Property(t => t.Modulo).HasColumnName("Modulo");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");

            // Relationships
            this.HasOptional(t => t.Modulo1)
                .WithMany(t => t.Requisitos)
                .HasForeignKey(d => d.Modulo);

        }
    }
}
