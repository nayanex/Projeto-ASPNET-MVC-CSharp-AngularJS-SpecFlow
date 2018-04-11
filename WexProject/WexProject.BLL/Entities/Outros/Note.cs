using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Outros
{
    public partial class Note
    {
        public System.Guid Oid { get; set; }
        public string Author { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public string Text { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<int> ObjectType { get; set; }
        public virtual CasoTestePassoResultadoEsperadoInformacaoAdicional CasoTestePassoResultadoEsperadoInformacaoAdicional { get; set; }
        public virtual CasoTestePreCondicao CasoTestePreCondicao { get; set; }
        public virtual CasoTestePreCondicaoInformacaoAdicional CasoTestePreCondicaoInformacaoAdicional { get; set; }
        public virtual XPObjectType XPObjectType { get; set; }
    }
}
