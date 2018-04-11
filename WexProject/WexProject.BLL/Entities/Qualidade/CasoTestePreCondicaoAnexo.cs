using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Qualidade
{
    public partial class CasoTestePreCondicaoAnexo
    {
        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> File { get; set; }
        public Nullable<System.Guid> CasoTestePreCondicao { get; set; }
        public string TxDescricao { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public virtual CasoTestePreCondicao CasoTestePreCondicao1 { get; set; }
        public virtual FileData FileData { get; set; }
    }
}
