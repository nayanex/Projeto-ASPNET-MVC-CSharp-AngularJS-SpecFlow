using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.NovosNegocios;

namespace WexProject.BLL.Entities.Geral
{
    public partial class ConfiguracaoDocumentoSituacaoEmailCc
    {
        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> ConfiguracaoDocumentoSituacao { get; set; }
        public Nullable<System.Guid> SolicitacaoOrcamento { get; set; }
        public virtual ConfiguracaoDocumentoSituacao ConfiguracaoDocumentoSituacao1 { get; set; }
        public virtual ConfiguracaoDocumentoSituacaoEmail ConfiguracaoDocumentoSituacaoEmail { get; set; }
        public virtual SolicitacaoOrcamento SolicitacaoOrcamento1 { get; set; }
    }
}
