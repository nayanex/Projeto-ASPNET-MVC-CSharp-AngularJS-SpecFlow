using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Execucao
{
    public partial class CicloDesenv
    {
        public CicloDesenv()
        {
            Oid = Guid.NewGuid();
        }

        public System.Guid Oid { get; set; }
        public Nullable<int> NbCiclo { get; set; }
        public Nullable<System.DateTime> DtInicio { get; set; }
        public Nullable<System.DateTime> DtTermino { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<System.Guid> Projeto { get; set; }
        public string TxMeta { get; set; }
        public Nullable<double> NbPontosPlanejados { get; set; }
        public Nullable<double> NbPontosRealizados { get; set; }
        public Nullable<int> CsSituacaoCiclo { get; set; }
        public Nullable<double> NbAlcanceMeta { get; set; }
        public Nullable<decimal> NbMaiorCicloDesenvEstoria { get; set; }
        public Nullable<System.Guid> MotivoCancelamento { get; set; }
        public Nullable<bool> IsCancelado { get; set; }
        public virtual MotivoCancelamento MotivoCancelamento1 { get; set; }
        public virtual Projeto Projeto1 { get; set; }
        public virtual ICollection<CicloDesenvEstoria> CicloDesenvEstorias { get; set; }
        public virtual ICollection<Estoria> Estorias { get; set; }
    }
}
