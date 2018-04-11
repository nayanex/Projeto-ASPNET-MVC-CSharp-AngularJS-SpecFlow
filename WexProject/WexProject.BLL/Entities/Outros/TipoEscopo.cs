using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class TipoEscopo
    {
        public System.Guid Oid { get; set; }
        public string TxEscopo { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
    }
}
