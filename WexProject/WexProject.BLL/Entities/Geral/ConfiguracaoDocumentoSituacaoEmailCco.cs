using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Geral
{
    public partial class ConfiguracaoDocumentoSituacaoEmailCco
    {
        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> ConfiguracaoDocumentoSituacao { get; set; }
        public virtual ConfiguracaoDocumentoSituacao ConfiguracaoDocumentoSituacao1 { get; set; }
        public virtual ConfiguracaoDocumentoSituacaoEmail ConfiguracaoDocumentoSituacaoEmail { get; set; }
    }
}
