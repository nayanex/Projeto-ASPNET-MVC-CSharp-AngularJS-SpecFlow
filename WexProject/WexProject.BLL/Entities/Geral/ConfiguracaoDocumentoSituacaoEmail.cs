using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Geral
{
    public partial class ConfiguracaoDocumentoSituacaoEmail
    {
        public System.Guid Oid { get; set; }
        public string TxEmail { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<int> ObjectType { get; set; }
        public virtual XPObjectType XPObjectType { get; set; }
        public virtual ConfiguracaoDocumentoSituacaoEmailCc ConfiguracaoDocumentoSituacaoEmailCc { get; set; }
        public virtual ConfiguracaoDocumentoSituacaoEmailCco ConfiguracaoDocumentoSituacaoEmailCco { get; set; }
    }
}
