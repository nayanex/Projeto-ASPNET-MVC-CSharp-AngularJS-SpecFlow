using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Planejamento
{
    public partial class TarefaHistoricoTrabalho
    {
        public TarefaHistoricoTrabalho()
        {
            Oid = Guid.NewGuid();
        }

        #region Propriedades

        public Guid Oid { get; set; }

        [DefaultValue( 0 )]
        public double NbHoraRealizado { get; set; }

        [DefaultValue( 0 )]
        public double NbHoraInicio { get; set; }

        [DefaultValue( 0 )]
        public double NbHoraFinal { get; set; }

        [DefaultValue( 0 )]
        public double NbHoraRestante { get; set; }

        [NotMapped]
        public TimeSpan HoraRealizado { get { return TimeSpan.FromTicks( (long)NbHoraRealizado ); } set { NbHoraRealizado = value.Ticks; } }

        [NotMapped]
        public TimeSpan HoraInicio { get { return TimeSpan.FromTicks( (long)NbHoraInicio ); } set { NbHoraInicio = value.Ticks; } }

        [NotMapped]
        public TimeSpan HoraRestante { get { return TimeSpan.FromTicks( (long)NbHoraRestante ); } set { NbHoraRestante = value.Ticks; } }

        [NotMapped]
        public TimeSpan HoraFinal { get { return TimeSpan.FromTicks( (long)NbHoraFinal ); } set { NbHoraFinal = value.Ticks; } }

        public string TxJustificativaReducao { get; set; }

        public DateTime DtRealizado { get; set; }

        public string TxComentario { get; set; }

        public bool CsExcluido { get; set; }

        #endregion

        #region Chaves Estrangeiras

        [Column( "Colaborador" ), ForeignKey( "Colaborador" )]
        public Nullable<Guid> OidColaborador { get; set; }

        [Column( "SituacaoPlanejamento" ), ForeignKey( "SituacaoPlanejamento" )]
        public Nullable<Guid> OidSituacaoPlanejamento { get; set; }

        [Column( "Tarefa" ), ForeignKey( "Tarefa" )]
        public Nullable<Guid> OidTarefa { get; set; } 

        #endregion

        #region Propriedades Navegacionais

        public virtual Colaborador Colaborador { get; set; }

        public SituacaoPlanejamento SituacaoPlanejamento { get; set; }

        public Tarefa Tarefa { get; set; }

        #endregion
    }
}
