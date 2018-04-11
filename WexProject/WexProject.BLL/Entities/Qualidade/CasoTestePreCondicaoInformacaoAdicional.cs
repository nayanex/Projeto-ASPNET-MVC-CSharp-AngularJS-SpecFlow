using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Qualidade
{
    public partial class CasoTestePreCondicaoInformacaoAdicional
    {
        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> CasoTestePreCondicao { get; set; }
        public Nullable<decimal> NbSequencia { get; set; }
        public virtual CasoTestePreCondicao CasoTestePreCondicao1 { get; set; }
        public virtual Note Note { get; set; }
    }
}
