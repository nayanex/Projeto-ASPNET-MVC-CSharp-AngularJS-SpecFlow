using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Escopo
{
    public partial class EstoriaCasoTeste
    {
        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> Estoria { get; set; }
        public Nullable<System.Guid> CasoTeste { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual CasoTeste CasoTeste1 { get; set; }
        public virtual Estoria Estoria1 { get; set; }
    }
}
