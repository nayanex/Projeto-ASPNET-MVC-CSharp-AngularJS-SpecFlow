using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Outros
{
    public partial class FileData
    {
        public FileData()
        {
            this.CasoTestePassoResultadoEsperadoAnexoes = new List<CasoTestePassoResultadoEsperadoAnexo>();
            this.CasoTestePreCondicaoAnexoes = new List<CasoTestePreCondicaoAnexo>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<int> size { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual ICollection<CasoTestePassoResultadoEsperadoAnexo> CasoTestePassoResultadoEsperadoAnexoes { get; set; }
        public virtual ICollection<CasoTestePreCondicaoAnexo> CasoTestePreCondicaoAnexoes { get; set; }
    }
}
