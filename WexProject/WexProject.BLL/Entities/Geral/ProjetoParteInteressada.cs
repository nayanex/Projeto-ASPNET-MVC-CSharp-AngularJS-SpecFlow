using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Escopo;

namespace WexProject.BLL.Entities.Geral
{
    public partial class ProjetoParteInteressada
    {
        public ProjetoParteInteressada()
        {
            this.Estorias = new List<Estoria>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> Projeto { get; set; }
        public Nullable<System.Guid> TxParteInteressada { get; set; }
        public Nullable<System.Guid> ParteInteressadaPapel { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<bool> csVerificarOrigemProjeto { get; set; }
        public Nullable<bool> csVerificarOrigemParteInteressada { get; set; }
        public virtual ICollection<Estoria> Estorias { get; set; }
        public virtual Papel Papel { get; set; }
        public virtual ParteInteressada ParteInteressada { get; set; }
        public virtual Projeto Projeto1 { get; set; }
    }
}
