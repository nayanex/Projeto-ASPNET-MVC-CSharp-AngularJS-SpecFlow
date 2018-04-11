using System;
using System.Collections.Generic;
using WexProject.BLL.Shared.Domains.Planejamento;

namespace WexProject.BLL.Entities.Planejamento
{
    public partial class SituacaoPlanejamento
    {
        #region Construtor

        public SituacaoPlanejamento()
        {
            this.Oid = Guid.NewGuid();
        }

        #endregion

        #region Propriedades

        public System.Guid Oid { get; set; }
        public string TxDescricao { get; set; }
        public CsTipoSituacaoPlanejamento CsSituacao { get; set; }
        public CsTipoPlanejamento CsTipo { get; set; }
        public byte[] BlImagem { get; set; }
        public CsPadraoSistema CsPadrao { get; set; }
        public Nullable<int> KeyPress { get; set; }
        public string TxKeys { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }

        #endregion

        #region Propriedades Navegacionais

        public virtual ICollection<Cronograma> Cronogramas { get; set; }
        public virtual ICollection<Tarefa> Tarefas { get; set; }
        public virtual ICollection<TarefaHistoricoTrabalho> TarefaHistoricoTrabalhoes { get; set; }

        #endregion
    }
}
