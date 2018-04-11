using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Geral
{
    public partial class ParteInteressada
    {
        public ParteInteressada()
        {
            this.ProjetoParteInteressadas = new List<ProjetoParteInteressada>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> Projeto { get; set; }
        public Nullable<System.Guid> Cargo { get; set; }
        public string TxParteInteressadaNome { get; set; }
        public string TxTelefoneFixo { get; set; }
        public string TxCelular { get; set; }
        public string TxEmail { get; set; }
        public Nullable<System.Guid> EmpresaInstituicao { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<int> IsColaborador { get; set; }
        public Nullable<System.Guid> Colaborador { get; set; }
        public virtual Cargo Cargo1 { get; set; }
        public virtual Colaborador Colaborador1 { get; set; }
        public virtual EmpresaInstituicao EmpresaInstituicao1 { get; set; }
        public virtual Projeto Projeto1 { get; set; }
        public virtual ICollection<ProjetoParteInteressada> ProjetoParteInteressadas { get; set; }
    }
}
