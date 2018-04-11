using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Geral
{
    public partial class ConfiguracaoDocumento
    {
        public ConfiguracaoDocumento()
        {
            this.ConfiguracaoDocumentoSituacaos = new List<ConfiguracaoDocumentoSituacao>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<int> CsTipoDocumento { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual ICollection<ConfiguracaoDocumentoSituacao> ConfiguracaoDocumentoSituacaos { get; set; }
    }
}
