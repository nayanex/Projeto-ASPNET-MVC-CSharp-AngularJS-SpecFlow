using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Execucao
{
    public partial class MotivoCancelamento
    {
        public MotivoCancelamento()
        {
            this.CicloDesenvs = new List<CicloDesenv>();
            this.ProjetoUltimoFiltroes = new List<ProjetoUltimoFiltro>();
        }

        public System.Guid Oid { get; set; }
        public string TxDescricao { get; set; }
        public Nullable<int> CsSituacao { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual ICollection<CicloDesenv> CicloDesenvs { get; set; }
        public virtual ICollection<ProjetoUltimoFiltro> ProjetoUltimoFiltroes { get; set; }
    }
}
