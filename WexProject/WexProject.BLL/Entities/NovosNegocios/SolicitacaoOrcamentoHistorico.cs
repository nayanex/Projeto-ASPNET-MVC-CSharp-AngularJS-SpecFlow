using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.NovosNegocios
{
    public partial class SolicitacaoOrcamentoHistorico
    {
        public System.Guid Oid { get; set; }
        public string Comentario { get; set; }
        public Nullable<System.DateTime> DataHora { get; set; }
        public Nullable<System.Guid> Situacoes { get; set; }
        public Nullable<System.Guid> ResponsavelHistorico { get; set; }
        public Nullable<System.Guid> SolicitacaoOrcamento { get; set; }
        public string AlteradoPor { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<System.Guid> AtualizadoPor { get; set; }
        public virtual Colaborador Colaborador { get; set; }
        public virtual Colaborador Colaborador1 { get; set; }
        public virtual ConfiguracaoDocumentoSituacao ConfiguracaoDocumentoSituacao { get; set; }
        public virtual SolicitacaoOrcamento SolicitacaoOrcamento1 { get; set; }
    }
}
