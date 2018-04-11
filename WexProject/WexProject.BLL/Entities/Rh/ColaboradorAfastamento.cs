using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.RH
{
    public partial class ColaboradorAfastamento
    {
        public ColaboradorAfastamento()
        {
            //this.FeriasPlanejamentoes = new List<FeriasPlanejamento>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> Colaborador { get; set; }
        public Nullable<System.DateTime> DtInicio { get; set; }
        public Nullable<System.DateTime> DtTermino { get; set; }
        public Nullable<System.Guid> TipoAfastamento { get; set; }
        public string TxObservacao { get; set; }
        public Nullable<bool> IsCriadoAutomaticamente { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public virtual Colaborador Colaborador1 { get; set; }
        public virtual TipoAfastamento TipoAfastamento1 { get; set; }
        public virtual ICollection<FeriasPlanejamento> FeriasPlanejamentoes { get; set; }
    }
}
