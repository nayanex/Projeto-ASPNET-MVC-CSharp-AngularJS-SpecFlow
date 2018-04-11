using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Outros
{
    public partial class Ferias
    {
        public Ferias()
        {
            this.FeriasPlanejamentoes = new List<FeriasPlanejamento>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> Colaborador { get; set; }
        public Nullable<short> NbAnoBase { get; set; }
        public Nullable<System.DateTime> DtVencimento { get; set; }
        public Nullable<System.DateTime> DtDataMaxima { get; set; }
        public Nullable<int> CsSituacaoFerias { get; set; }
        public string TxObservacoes { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual Colaborador Colaborador1 { get; set; }
        public virtual ICollection<FeriasPlanejamento> FeriasPlanejamentoes { get; set; }
    }
}
