using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class ProjetoMap : EntityTypeConfiguration<Projeto>
    {
        public ProjetoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxNome)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Projeto");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.TxNome).HasColumnName("TxNome");
            this.Property(t => t.EmpresaInstituicao).HasColumnName("EmpresaInstituicao");
            this.Property(t => t.NbTamanhoTotal).HasColumnName("NbTamanhoTotal");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.DtInicioPlan).HasColumnName("DtInicioPlan");
            this.Property(t => t.DtInicioReal).HasColumnName("DtInicioReal");
            this.Property(t => t.DtTerminoPlan).HasColumnName("DtTerminoPlan");
            this.Property(t => t.NbCicloTotalPlan).HasColumnName("NbCicloTotalPlan");
            this.Property(t => t.NbCicloDuracaoDiasPlan).HasColumnName("NbCicloDuracaoDiasPlan");
            this.Property(t => t.NbCicloDiasIntervalo).HasColumnName("NbCicloDiasIntervalo");
            this.Property(t => t.oldDate).HasColumnName("oldDate");
            this.Property(t => t.DtTerminoReal).HasColumnName("DtTerminoReal");
            this.Property(t => t.UltimoFiltro).HasColumnName("UltimoFiltro");
            this.Property(t => t.NbRitmoTime).HasColumnName("NbRitmoTime");
            this.Property(t => t.NbValor).HasColumnName("NbValor");
			this.Property(t => t.TipoProjetoId).HasColumnName("TipoProjetoId");
            this.Property(t => t.CsSituacaoProjeto).HasColumnName("CsSituacaoProjeto");
            this.Property(t => t.GerenteOid).HasColumnName("GerenteOid");
			this.Property(t => t.ProjetoMacroOid).HasColumnName("ProjetoMacroOid");
            this.Property(t => t.CentroCustoId).HasColumnName("CentroCustoId");

            // Relationships
            this.HasOptional(t => t.Colaborador)
                .WithMany(t => t.Projetoes)
                .HasForeignKey(d => d.GerenteOid);
            this.HasOptional(t => t.EmpresaInstituicao1)
                .WithMany(t => t.Projetoes)
                .HasForeignKey(d => d.EmpresaInstituicao);
            this.HasOptional(t => t.ProjetoUltimoFiltro)
                .WithMany(t => t.Projetoes)
                .HasForeignKey(d => d.UltimoFiltro);
			this.HasOptional(t => t.TipoProjeto)
				.WithMany(t => t.Projetos)
				.HasForeignKey(d => d.TipoProjetoId);

        }
    }
}
