using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Entities.Geral
{
    public partial class Calendario
    {
        public System.Guid Oid { get; set; }
        public string TxDescricao { get; set; }
        public Nullable<System.DateTime> DtInicio { get; set; }
        public Nullable<System.DateTime> Periodo { get; set; }
        public Nullable<System.DateTime> DtTermino { get; set; }
        public Nullable<int> NbDia { get; set; }
        public Nullable<int> CsMes { get; set; }
        public Nullable<int> CsCalendario { get; set; }
        public Nullable<int> CsVigencia { get; set; }
        public Nullable<int> CsSituacao { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }

        [NotMapped]
        public CsCalendarioDomain CsCalendarioDomain
        {
            get 
            {
                return (CsCalendarioDomain)CsCalendario;
            }
        }

        [NotMapped]
        public CsVigenciaDomain CsVigenciaDomain
        {
            get
            {
                return (CsVigenciaDomain)CsVigencia;
            }
        }

        [NotMapped]
        public CsSituacaoDomain CsSituacaoDomain
        {
            get
            {
                return (CsSituacaoDomain)CsSituacao;
            }
        }

    }
}
