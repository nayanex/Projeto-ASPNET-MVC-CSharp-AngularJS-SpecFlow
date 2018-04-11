using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Escopo
{
    public partial class Requisito
    {
        public Requisito()
        {
            this.CasoTestes = new List<CasoTeste>();
            this.RequisitoCasoTestes = new List<RequisitoCasoTeste>();
        }

        public System.Guid Oid { get; set; }
        public string TxID { get; set; }
        public string TxNome { get; set; }
        public string TxDescricao { get; set; }
        public string TxLinkPrototipo { get; set; }
        public Nullable<System.Guid> Modulo { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual ICollection<CasoTeste> CasoTestes { get; set; }
        public virtual Modulo Modulo1 { get; set; }
        public virtual ICollection<RequisitoCasoTeste> RequisitoCasoTestes { get; set; }
    }
}
