using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Geral
{
    public partial class Pai
    {
        public Pai()
        {
            this.EmpresaInstituicaos = new List<EmpresaInstituicao>();
        }

        public System.Guid Oid { get; set; }
        public string TxMascara { get; set; }
        public Nullable<int> CsSituacao { get; set; }
        public Nullable<bool> IsPadrao { get; set; }
        public Nullable<System.Guid> Country { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public virtual Country Country1 { get; set; }
        public virtual ICollection<EmpresaInstituicao> EmpresaInstituicaos { get; set; }
    }
}
