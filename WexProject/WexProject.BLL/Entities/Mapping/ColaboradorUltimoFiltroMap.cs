using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Mapping
{
    public class ColaboradorUltimoFiltroMap : EntityTypeConfiguration<ColaboradorUltimoFiltro>
    {
        public ColaboradorUltimoFiltroMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("ColaboradorUltimoFiltro");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.OidLastSituacaoFilterSeot).HasColumnName("LastSituacaoFilterSeot");
            this.Property(t => t.OidLastUsuarioFilterSeot).HasColumnName("LastUsuarioFilterSeot");
            this.Property(t => t.LastPeriodoFilterPlanejamentoFerias).HasColumnName("LastPeriodoFilterPlanejamentoFerias");
            this.Property(t => t.LastSituacaoFilterPlanejamentoFerias).HasColumnName("LastSituacaoFilterPlanejamentoFerias");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.LastSuperiorImediatoFilterPlanejamentoFerias).HasColumnName("LastSuperiorImediatoFilterPlanejamentoFerias");
            this.Property(t => t.LastSituacaoFeriasFilterPlanejamentoFerias).HasColumnName("LastSituacaoFeriasFilterPlanejamentoFerias");
            this.Property(t => t.OidLastEmpresaInstituicaoSEOT).HasColumnName("LastEmpresaInstituicaoSEOT");
            this.Property(t => t.OidLastTipoSolicitacaoSEOT).HasColumnName("LastTipoSolicitacaoSEOT");
            this.Property(t => t.OidUltimoProjetoSelecionado).HasColumnName("OidUltimoProjetoSelecionado");

            // Relationships
            this.HasOptional(t => t.EmpresaInstituicao)
                .WithMany(t => t.ColaboradorUltimoFiltroes)
                .HasForeignKey(d => d.OidLastEmpresaInstituicaoSEOT);
            this.HasOptional(t => t.TipoSolicitacao)
                .WithMany(t => t.ColaboradorUltimoFiltroes)
                .HasForeignKey(d => d.OidLastTipoSolicitacaoSEOT);

        }
    }
}
