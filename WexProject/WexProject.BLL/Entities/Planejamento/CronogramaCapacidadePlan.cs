using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Planejamento;

namespace WexProject.BLL.Entities.Planejamento
{
    [Table( "CronogramaCapacidadePlan" )]
    public class CronogramaCapacidadePlan
    {
        public CronogramaCapacidadePlan()
        {
            Oid = new Guid();
        }

        #region Propriedades

        [Key]
        public Guid Oid { get; set; }

        public DateTime DtDia { get; set; }

        public TimeSpan HorasCapacidade { get; set; }

        public TimeSpan HorasPlanejadas { get; set; }

        public TimeSpan HorasDiaAnterior { get; set; }

        public CsSituacaoCapacidadePlan CsSituacao { get; set; }

        #endregion

        #region Chaves estrangeiras

        [Column("Cronograma"),ForeignKey( "Cronograma" )]
        public Guid OidCronograma { get; set; }

        #endregion

        #region Propriedades Navegacionais

        public virtual Cronograma Cronograma { get; set; }

        #endregion

        public override bool Equals( object obj )
        {
            CronogramaCapacidadePlan objComparacao = obj as CronogramaCapacidadePlan;
            return DtDia.Date.Equals( objComparacao.DtDia.Date ) && Oid.Equals( objComparacao.Oid )
                && HorasCapacidade.Equals( objComparacao.HorasCapacidade )
                && HorasDiaAnterior.Equals( objComparacao.HorasDiaAnterior )
                && HorasPlanejadas.Equals( objComparacao.HorasPlanejadas )
                && CsSituacao.Equals(objComparacao.CsSituacao)
                && OidCronograma.Equals(objComparacao.OidCronograma)
                ;
        }
    }
}
