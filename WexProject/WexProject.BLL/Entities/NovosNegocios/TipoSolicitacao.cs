using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.NovosNegocios
{
    public partial class TipoSolicitacao
    {
        public TipoSolicitacao()
        {
            this.ColaboradorUltimoFiltroes = new List<ColaboradorUltimoFiltro>();
            this.SolicitacaoOrcamentoes = new List<SolicitacaoOrcamento>();
        }

        public System.Guid Oid { get; set; }
        public string TxDescricao { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual ICollection<ColaboradorUltimoFiltro> ColaboradorUltimoFiltroes { get; set; }
        public virtual ICollection<SolicitacaoOrcamento> SolicitacaoOrcamentoes { get; set; }
    }
}
