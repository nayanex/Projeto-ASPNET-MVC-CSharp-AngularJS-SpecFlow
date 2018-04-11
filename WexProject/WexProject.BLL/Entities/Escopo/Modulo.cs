using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Escopo
{
    public partial class Modulo
    {
        public Modulo()
        {
            Oid = Guid.NewGuid();
        }

        public System.Guid Oid { get; set; }

        [Column( "Projeto" ), ForeignKey( "Projeto" )]
        public Nullable<System.Guid> OidProjeto { get; set; }
        public string TxID { get; set; }
        public string TxNome { get; set; }
        public string TxDescricao { get; set; }
        [Column( "ModuloPai" ), ForeignKey( "ModuloPai" )]
        public Nullable<System.Guid> OidModuloPai { get; set; }
        public Nullable<decimal> NbEsforcoPlanejado { get; set; }
        public Nullable<double> NbPontosTotal { get; set; }
        public Nullable<double> NbPontosNaoIniciado { get; set; }
        public Nullable<double> NbPontosEmAnalise { get; set; }
        public Nullable<double> NbPontosEmDesenv { get; set; }
        public Nullable<double> NbPontosPronto { get; set; }
        public Nullable<double> NbPontosDesvio { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual ICollection<Estoria> Estorias { get; set; }
        public virtual ICollection<Modulo> Modulos { get; set; }
        public virtual Modulo ModuloPai { get; set; }
        public virtual Projeto Projeto { get; set; }
        public virtual ICollection<Requisito> Requisitos { get; set; }
    }
}
