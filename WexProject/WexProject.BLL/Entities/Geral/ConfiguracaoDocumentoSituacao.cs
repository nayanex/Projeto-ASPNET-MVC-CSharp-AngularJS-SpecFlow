using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.NovosNegocios;

namespace WexProject.BLL.Entities.Geral
{
    public partial class ConfiguracaoDocumentoSituacao
    {
        public ConfiguracaoDocumentoSituacao()
        {
            this.ConfiguracaoDocumentoSituacaoEmailCcs = new List<ConfiguracaoDocumentoSituacaoEmailCc>();
            this.ConfiguracaoDocumentoSituacaoEmailCcoes = new List<ConfiguracaoDocumentoSituacaoEmailCco>();
            this.SolicitacaoOrcamentoes = new List<SolicitacaoOrcamento>();
            this.SolicitacaoOrcamentoHistoricoes = new List<SolicitacaoOrcamentoHistorico>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> ConfiguracaoDocumento { get; set; }
        public string TxDescricao { get; set; }
        public string TxNomeCor { get; set; }
        public Nullable<int> TypeColor { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<bool> IsSituacaoInicial { get; set; }
        public string TxCc { get; set; }
        public string TxCco { get; set; }
        public virtual ConfiguracaoDocumento ConfiguracaoDocumento1 { get; set; }
        public virtual ICollection<ConfiguracaoDocumentoSituacaoEmailCc> ConfiguracaoDocumentoSituacaoEmailCcs { get; set; }
        public virtual ICollection<ConfiguracaoDocumentoSituacaoEmailCco> ConfiguracaoDocumentoSituacaoEmailCcoes { get; set; }
        public virtual ICollection<SolicitacaoOrcamento> SolicitacaoOrcamentoes { get; set; }
        public virtual ICollection<SolicitacaoOrcamentoHistorico> SolicitacaoOrcamentoHistoricoes { get; set; }
    }
}
