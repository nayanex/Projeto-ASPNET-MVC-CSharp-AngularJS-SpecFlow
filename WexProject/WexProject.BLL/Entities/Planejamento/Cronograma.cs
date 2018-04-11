using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WexProject.BLL.Entities.Planejamento
{
    public partial class Cronograma
    {
        public Cronograma()
        {
            this.Oid = Guid.NewGuid();
        }

        #region Propriedades

        public Guid Oid { get; set; }

        public string TxDescricao { get; set; }

        public DateTime DtInicio { get; set; }

        public DateTime DtFinal { get; set; }

        public bool CsExcluido { get; set; } 
        #endregion

        #region Chaves estrangeiras

        public Guid OidSituacaoPlanejamento { get; set; } 

        #endregion

        #region Não mapeadas

        [NotMapped]
        public string TxDescricaoSituacaoPlanejamento
        {
            get { return SituacaoPlanejamento.TxDescricao; }
        }

        #endregion

        #region Propriedades Navegacionais

        public SituacaoPlanejamento SituacaoPlanejamento { get; set; }

        public virtual ICollection<CronogramaTarefa> CronogramaTarefas { get; set; }

        public virtual ICollection<CronogramaCapacidadePlan> CapacidadesPlanejamento { get; set; }

        #endregion
        
    }
}
