using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Qualidade
{
    public partial class CasoTestePreCondicao
    {
        public CasoTestePreCondicao()
        {
            this.CasoTestePreCondicaoAnexoes = new List<CasoTestePreCondicaoAnexo>();
            this.CasoTestePreCondicaoInformacaoAdicionals = new List<CasoTestePreCondicaoInformacaoAdicional>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> CasoTeste { get; set; }
        public Nullable<int> NbSequencia { get; set; }
        public string TxDescricao { get; set; }
        public Nullable<decimal> NbMaiorInformacaoAdicional { get; set; }
        public virtual CasoTeste CasoTeste1 { get; set; }
        public virtual Note Note { get; set; }
        public virtual ICollection<CasoTestePreCondicaoAnexo> CasoTestePreCondicaoAnexoes { get; set; }
        public virtual ICollection<CasoTestePreCondicaoInformacaoAdicional> CasoTestePreCondicaoInformacaoAdicionals { get; set; }
    }
}
