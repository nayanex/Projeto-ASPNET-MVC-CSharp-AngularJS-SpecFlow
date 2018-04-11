using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Execucao;

namespace WexProject.BLL.Entities.Geral
{
    public partial class ProjetoUltimoFiltro
    {
        public ProjetoUltimoFiltro()
        {
            Oid = Guid.NewGuid();
        }

        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> MotivoCancelamentoCiclo { get; set; }
        public Nullable<System.Guid> Projeto { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual MotivoCancelamento MotivoCancelamento { get; set; }
        public virtual ICollection<Projeto> Projetoes { get; set; }
        public virtual Projeto Projeto1 { get; set; }
    }
}
