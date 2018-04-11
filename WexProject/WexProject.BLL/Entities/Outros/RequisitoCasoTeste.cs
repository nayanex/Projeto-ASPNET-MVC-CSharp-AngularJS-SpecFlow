using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Outros
{
    public partial class RequisitoCasoTeste
    {
        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> Requisito { get; set; }
        public Nullable<System.Guid> CasoTeste { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual CasoTeste CasoTeste1 { get; set; }
        public virtual Requisito Requisito1 { get; set; }
    }
}
