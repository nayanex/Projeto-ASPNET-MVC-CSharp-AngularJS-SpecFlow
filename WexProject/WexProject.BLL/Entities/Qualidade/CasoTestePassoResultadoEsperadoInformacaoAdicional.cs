using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Qualidade
{
    public partial class CasoTestePassoResultadoEsperadoInformacaoAdicional
    {
        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> CasoTestePassoResultadoEsperado { get; set; }
        public Nullable<decimal> NbSequencia { get; set; }
        public virtual CasoTestePassoResultadoEsperado CasoTestePassoResultadoEsperado1 { get; set; }
        public virtual Note Note { get; set; }
    }
}
