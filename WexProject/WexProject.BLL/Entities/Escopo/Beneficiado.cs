using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Generico;

namespace WexProject.BLL.Entities.Escopo
{
    public partial class Beneficiado : Entidade
    {
        public Beneficiado()
            :base()
        {
            this.Estorias = new List<Estoria>();
        }

        public string TxDescricao { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual ICollection<Estoria> Estorias { get; set; }
    }
}
