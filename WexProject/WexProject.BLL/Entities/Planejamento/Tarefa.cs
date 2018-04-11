using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.RH;
using System.Linq;

namespace WexProject.BLL.Entities.Planejamento
{
    public partial class Tarefa
    {
        #region Construtor

        public Tarefa()
        {
            this.Oid = Guid.NewGuid();
        }

        #endregion

        #region Propriedades

        public Guid Oid { get; set; }

        public string TxDescricao { get; set; }

        public double NbEstimativaRestante { get; set; }
        public Int16 NbEstimativaInicial { get; set; }
        public double NbRealizado { get; set; }

        [NotMapped]
        public TimeSpan EstimativaRestanteHora { get { return TimeSpan.FromTicks( (long)NbEstimativaRestante ); } set { NbEstimativaRestante = value.Ticks; } }

        [NotMapped]
        public TimeSpan EstimativaInicialHora { get { return TimeSpan.FromHours( (long)NbEstimativaInicial ); }}

        [NotMapped]
        public TimeSpan EstimativaRealizadoHora { get { return TimeSpan.FromTicks( (long)NbRealizado ); } set { NbRealizado = value.Ticks; } }

        public string TxObservacao { get; set; }
        public DateTime? DtInicio { get; set; }
        public string TxResponsaveis { get; set; }
        public bool CsLinhaBaseSalva { get; set; }
        public DateTime? DtAtualizadoEm { get; set; }

        public bool CsExcluido { get; set; }

        #endregion

        #region Chaves Estrangeiras

        [Column( "SituacaoPlanejamento" ), ForeignKey( "SituacaoPlanejamento" )]
        public Nullable<System.Guid> OidSituacaoPlanejamento { get; set; }

        [Column( "AtualizadoPor" ), ForeignKey( "AtualizadoPor" )]
        public Nullable<System.Guid> OidAtualizadoPor { get; set; }

        //[Column( "ItemDeTrabalho" ), ForeignKey( "ItemDeTrabalho" )]
        //public Nullable<System.Guid> OidItemDeTrabalho { get; set; }

        #endregion

        #region Não Mapeado

        public Tarefa clone;

        #endregion

        #region Propriedades Navegacionais

        public Colaborador AtualizadoPor { get; set; }
        //public virtual ItemDeTrabalho ItemDeTrabalho { get; set; }
        public SituacaoPlanejamento SituacaoPlanejamento { get; set; }
        public ICollection<TarefaHistoricoTrabalho> TarefaHistoricoTrabalhos { get; set; }
        public ICollection<TarefaLogAlteracao> LogsAlteracao { get; set; }
        public ICollection<TarefaResponsaveis> TarefaResponsaveis { get; set; }
        public ICollection<TarefaHistoricoEstimativa> HistoricoEstimativas { get; set; }

        #endregion

        /// <summary>
        /// Retorna um clone da instancia atual
        /// </summary>
        /// <returns></returns>
        public Tarefa Clone()
        {
            clone = MemberwiseClone() as Tarefa;
            return clone;
        }
    }
}
