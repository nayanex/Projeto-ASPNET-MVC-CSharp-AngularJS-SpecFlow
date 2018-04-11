using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.NovosNegocios;

namespace WexProject.BLL.Entities.Mapping
{
    public class SolicitacaoOrcamentoMap : EntityTypeConfiguration<SolicitacaoOrcamento>
    {
        public SolicitacaoOrcamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.TxCodigo)
                .HasMaxLength(100);

            this.Property(t => t.TxTitulo)
                .HasMaxLength(255);

            this.Property(t => t.TxRepositorio)
                .HasMaxLength(255);

            this.Property(t => t.TxContatoCliente)
                .HasMaxLength(255);

            this.Property(t => t.TxEmailContatoCliente)
                .HasMaxLength(100);

            this.Property(t => t.TxFone)
                .HasMaxLength(30);

            this.Property(t => t.TxCc)
                .HasMaxLength(100);

            this.Property(t => t.TxCco)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("SolicitacaoOrcamento");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.Solicitante).HasColumnName("Solicitante");
            this.Property(t => t.Responsavel).HasColumnName("Responsavel");
            this.Property(t => t.Situacao).HasColumnName("Situacao");
            this.Property(t => t.TipoSolicitacao).HasColumnName("TipoSolicitacao");
            this.Property(t => t.CsPrioridade).HasColumnName("CsPrioridade");
            this.Property(t => t.TxCodigo).HasColumnName("TxCodigo");
            this.Property(t => t.TxTitulo).HasColumnName("TxTitulo");
            this.Property(t => t.DtPrazo).HasColumnName("DtPrazo");
            this.Property(t => t.TxRepositorio).HasColumnName("TxRepositorio");
            this.Property(t => t.Cliente).HasColumnName("Cliente");
            this.Property(t => t.TxContatoCliente).HasColumnName("TxContatoCliente");
            this.Property(t => t.TxEmailContatoCliente).HasColumnName("TxEmailContatoCliente");
            this.Property(t => t.TxFone).HasColumnName("TxFone");
            this.Property(t => t.TxDescricao).HasColumnName("TxDescricao");
            this.Property(t => t.TxObservacao).HasColumnName("TxObservacao");
            this.Property(t => t.DtEmissao).HasColumnName("DtEmissao");
            this.Property(t => t.DtConclusao).HasColumnName("DtConclusao");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.TxCc).HasColumnName("TxCc");
            this.Property(t => t.TxCco).HasColumnName("TxCco");
            this.Property(t => t.TxUltimoComentario).HasColumnName("TxUltimoComentario");
            this.Property(t => t.NbValor).HasColumnName("NbValor");
            this.Property(t => t.DtEntrega).HasColumnName("DtEntrega");

            // Relationships
            this.HasOptional(t => t.Colaborador)
                .WithMany(t => t.SolicitacaoOrcamentoes)
                .HasForeignKey(d => d.Responsavel);
            this.HasOptional(t => t.ConfiguracaoDocumentoSituacao)
                .WithMany(t => t.SolicitacaoOrcamentoes)
                .HasForeignKey(d => d.Situacao);
            this.HasOptional(t => t.EmpresaInstituicao)
                .WithMany(t => t.SolicitacaoOrcamentoes)
                .HasForeignKey(d => d.Cliente);
            this.HasOptional(t => t.User)
                .WithMany(t => t.SolicitacaoOrcamentoes)
                .HasForeignKey(d => d.Solicitante);
            this.HasOptional(t => t.TipoSolicitacao1)
                .WithMany(t => t.SolicitacaoOrcamentoes)
                .HasForeignKey(d => d.TipoSolicitacao);

        }
    }
}
