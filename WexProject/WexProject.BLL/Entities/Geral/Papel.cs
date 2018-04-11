using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Geral
{
    public partial class Papel
    {
        public Papel()
        {
            this.ProjetoParteInteressadas = new List<ProjetoParteInteressada>();
        }

        public System.Guid Oid { get; set; }
        public string TxNome { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual ICollection<ProjetoParteInteressada> ProjetoParteInteressadas { get; set; }
    }
}
