using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Qualidade
{
    public partial class CasoTeste
    {
        public CasoTeste()
        {
            this.CasoTestePassoes = new List<CasoTestePasso>();
            this.CasoTestePreCondicaos = new List<CasoTestePreCondicao>();
            this.EstoriaCasoTestes = new List<EstoriaCasoTeste>();
            this.RequisitoCasoTestes = new List<RequisitoCasoTeste>();
        }

        public System.Guid Oid { get; set; }
        public string TxID { get; set; }
        public Nullable<System.Guid> Requisito { get; set; }
        public Nullable<System.Guid> EstoriaCriacao { get; set; }
        public string TxSumario { get; set; }
        public Nullable<int> CsCasoTeste { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<decimal> NbMaiorPrecondicao { get; set; }
        public Nullable<decimal> NbMaiorPasso { get; set; }
        public Nullable<System.Guid> Usuario { get; set; }
        public Nullable<System.DateTime> DtHoraEData { get; set; }
        public Nullable<bool> CsVerificandoNestedObject { get; set; }
        public virtual Estoria Estoria { get; set; }
        public virtual Requisito Requisito1 { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CasoTestePasso> CasoTestePassoes { get; set; }
        public virtual ICollection<CasoTestePreCondicao> CasoTestePreCondicaos { get; set; }
        public virtual ICollection<EstoriaCasoTeste> EstoriaCasoTestes { get; set; }
        public virtual ICollection<RequisitoCasoTeste> RequisitoCasoTestes { get; set; }
    }
}
