using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Qualidade
{
    public partial class CasoTestePasso
    {
        public CasoTestePasso()
        {
            this.CasoTestePassoResultadoEsperadoes = new List<CasoTestePassoResultadoEsperado>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> CasoTeste { get; set; }
        public string TxPasso { get; set; }
        public Nullable<decimal> NbNumero { get; set; }
        public Nullable<decimal> NbSequencia { get; set; }
        public Nullable<decimal> NbMaiorResultadoEsperado { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public virtual CasoTeste CasoTeste1 { get; set; }
        public virtual ICollection<CasoTestePassoResultadoEsperado> CasoTestePassoResultadoEsperadoes { get; set; }
    }
}
