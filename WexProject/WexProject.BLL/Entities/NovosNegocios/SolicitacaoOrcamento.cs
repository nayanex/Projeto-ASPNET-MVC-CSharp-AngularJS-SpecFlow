using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.NovosNegocios
{
    public partial class SolicitacaoOrcamento
    {
        public SolicitacaoOrcamento()
        {
            this.ConfiguracaoDocumentoSituacaoEmailCcs = new List<ConfiguracaoDocumentoSituacaoEmailCc>();
            this.SolicitacaoOrcamentoHistoricoes = new List<SolicitacaoOrcamentoHistorico>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> Solicitante { get; set; }
        public Nullable<System.Guid> Responsavel { get; set; }
        public Nullable<System.Guid> Situacao { get; set; }
        public Nullable<System.Guid> TipoSolicitacao { get; set; }
        public Nullable<int> CsPrioridade { get; set; }
        public string TxCodigo { get; set; }
        public string TxTitulo { get; set; }
        public Nullable<System.DateTime> DtPrazo { get; set; }
        public string TxRepositorio { get; set; }
        public Nullable<System.Guid> Cliente { get; set; }
        public string TxContatoCliente { get; set; }
        public string TxEmailContatoCliente { get; set; }
        public string TxFone { get; set; }
        public string TxDescricao { get; set; }
        public string TxObservacao { get; set; }
        public Nullable<System.DateTime> DtEmissao { get; set; }
        public Nullable<System.DateTime> DtConclusao { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public string TxCc { get; set; }
        public string TxCco { get; set; }
        public string TxUltimoComentario { get; set; }
        public Nullable<double> NbValor { get; set; }
        public Nullable<System.DateTime> DtEntrega { get; set; }
        public virtual Colaborador Colaborador { get; set; }
        public virtual ConfiguracaoDocumentoSituacao ConfiguracaoDocumentoSituacao { get; set; }
        public virtual ICollection<ConfiguracaoDocumentoSituacaoEmailCc> ConfiguracaoDocumentoSituacaoEmailCcs { get; set; }
        public virtual EmpresaInstituicao EmpresaInstituicao { get; set; }
        public virtual User User { get; set; }
        public virtual TipoSolicitacao TipoSolicitacao1 { get; set; }
        public virtual ICollection<SolicitacaoOrcamentoHistorico> SolicitacaoOrcamentoHistoricoes { get; set; }
    }
}
