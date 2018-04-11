using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WexProject.BLL.Entities.RH
{
    public partial class ColaboradorPeriodoAquisitivo
    {
        public ColaboradorPeriodoAquisitivo()
        {
            Oid = Guid.NewGuid();
        }

        public System.Guid Oid { get; set; }
        [ForeignKey( "Colaborador" )]
        public Nullable<System.Guid> OidColaborador { get; set; }
        public Nullable<System.DateTime> DtInicio { get; set; }
        public Nullable<System.DateTime> DtTermino { get; set; }
        public Nullable<decimal> NbFeriasPlanejadas { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<System.DateTime> DtMaxima { get; set; }
        public virtual Colaborador Colaborador { get; set; }
        public virtual ICollection<FeriasPlanejamento> FeriasPlanejamentoes { get; set; }
    }
}
