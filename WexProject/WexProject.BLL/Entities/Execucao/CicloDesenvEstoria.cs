using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Shared.Domains.Execucao;

namespace WexProject.BLL.Entities.Execucao
{
    public partial class CicloDesenvEstoria
    {
        public CicloDesenvEstoria()
        {
            Oid = Guid.NewGuid();
        }

        public System.Guid Oid { get; set; }
        public Nullable<bool> Meta { get; set; }
        public Nullable<System.Guid> Estoria { get; set; }
        public Nullable<decimal> NbSequencia { get; set; }
        public Nullable<int> CsSituacao { get; set; }
        public Nullable<System.Guid> Ciclo { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual CicloDesenv CicloDesenv { get; set; }
        public virtual Estoria Estoria1 { get; set; }
    }
}
