using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.RH
{
    public partial class FeriasPlanejamento
    {
        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> Ferias { get; set; }
        public Nullable<System.Guid> Modalidade { get; set; }
        public Nullable<System.DateTime> DtInicio { get; set; }
        public Nullable<bool> Vender { get; set; }
        public Nullable<bool> Realizadas { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<System.Guid> Periodo { get; set; }
        public Nullable<int> CsSituacaoFerias { get; set; }
        public string TxPlanejado { get; set; }
        public string TxAtualizado { get; set; }
        public Nullable<System.Guid> Afastamento { get; set; }
        public Nullable<System.DateTime> DtRetorno { get; set; }
        public virtual ColaboradorAfastamento ColaboradorAfastamento { get; set; }
        public virtual ColaboradorPeriodoAquisitivo ColaboradorPeriodoAquisitivo { get; set; }
        public virtual Ferias Feria { get; set; }
        public virtual ModalidadeFeria ModalidadeFeria { get; set; }
    }
}
