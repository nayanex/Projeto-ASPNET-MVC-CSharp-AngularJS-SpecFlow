using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Qualidade
{
    public partial class CasoTestePassoResultadoEsperado
    {
        public CasoTestePassoResultadoEsperado()
        {
            this.CasoTestePassoResultadoEsperadoAnexoes = new List<CasoTestePassoResultadoEsperadoAnexo>();
            this.CasoTestePassoResultadoEsperadoInformacaoAdicionals = new List<CasoTestePassoResultadoEsperadoInformacaoAdicional>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> Passo { get; set; }
        public string TxResultadoEsperado { get; set; }
        public Nullable<int> TiposResultados { get; set; }
        public Nullable<decimal> NbSequencia { get; set; }
        public Nullable<int> CsTiposResultado { get; set; }
        public Nullable<decimal> NbMaiorInformacaoAdicional { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public virtual CasoTestePasso CasoTestePasso { get; set; }
        public virtual ICollection<CasoTestePassoResultadoEsperadoAnexo> CasoTestePassoResultadoEsperadoAnexoes { get; set; }
        public virtual ICollection<CasoTestePassoResultadoEsperadoInformacaoAdicional> CasoTestePassoResultadoEsperadoInformacaoAdicionals { get; set; }
    }
}
