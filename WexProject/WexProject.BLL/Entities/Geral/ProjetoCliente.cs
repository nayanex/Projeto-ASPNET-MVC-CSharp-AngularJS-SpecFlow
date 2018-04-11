using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WexProject.BLL.Entities.Geral
{
    public partial class ProjetoCliente
    {

        public System.Guid Oid { get; set; }

        [Column("Projeto"), ForeignKey("Projeto")]
        public Nullable<System.Guid> IdProjeto { get; set; }

        [Column("Cliente"), ForeignKey("EmpresaInstituicao")]
        public Nullable<System.Guid> IdCliente { get; set; }

        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }

        public virtual EmpresaInstituicao EmpresaInstituicao { get; set; }
        public virtual Projeto Projeto { get; set; }

    }
}
