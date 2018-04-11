using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Geral
{
    public partial class Cargo
    {
        public Cargo()
        {
            this.Colaboradors = new List<Colaborador>();
            this.ParteInteressadas = new List<ParteInteressada>();
            this.Solicitantes = new List<Solicitante>();
        }

        public System.Guid Oid { get; set; }
        public string TxDescricao { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual ICollection<Colaborador> Colaboradors { get; set; }
        public virtual ICollection<ParteInteressada> ParteInteressadas { get; set; }
        public virtual ICollection<Solicitante> Solicitantes { get; set; }
    }
}
