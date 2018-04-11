using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Geral
{
    public partial class ProjetoColaboradorConfig
    {
        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> Projeto { get; set; }
        public Nullable<System.Guid> Colaborador { get; set; }
        public string Cor { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual Colaborador Colaborador1 { get; set; }
        public virtual Projeto Projeto1 { get; set; }
    }
}
