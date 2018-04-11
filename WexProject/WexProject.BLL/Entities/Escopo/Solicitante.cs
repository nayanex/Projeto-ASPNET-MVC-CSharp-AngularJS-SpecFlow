using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Escopo
{
    public partial class Solicitante
    {
        public System.Guid Oid { get; set; }
        public string TxNome { get; set; }
        public Nullable<System.Guid> EmpresaInstituicao { get; set; }
        public Nullable<System.Guid> Cargo { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual Cargo Cargo1 { get; set; }
        public virtual EmpresaInstituicao EmpresaInstituicao1 { get; set; }
    }
}
