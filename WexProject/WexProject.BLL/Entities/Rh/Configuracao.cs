using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.RH
{
    public partial class Configuracao
    {
        public System.Guid Oid { get; set; }
        public Nullable<decimal> NbQtdeMaxVenda { get; set; }
        public Nullable<decimal> NbQtdeMaxFerias { get; set; }
        public Nullable<decimal> NbDtMaxTirarFerias { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
    }
}
