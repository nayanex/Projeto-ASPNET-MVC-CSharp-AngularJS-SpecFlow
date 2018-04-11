using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.RH
{
    public partial class TipoAfastamento
    {
        public TipoAfastamento()
        {
            this.ColaboradorAfastamentoes = new List<ColaboradorAfastamento>();
        }

        public System.Guid Oid { get; set; }
        public string TxDescricao { get; set; }
        public Nullable<bool> IsParaFeriasRealizadas { get; set; }
        public Nullable<bool> IsRemunerado { get; set; }
        public Nullable<int> CsSituacao { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public virtual ICollection<ColaboradorAfastamento> ColaboradorAfastamentoes { get; set; }
    }
}
