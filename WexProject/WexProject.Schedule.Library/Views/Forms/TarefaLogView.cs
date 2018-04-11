using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Library.Presenters;

namespace WexProject.Schedule.Library.Views.Forms
{
    /// <summary>
    /// Histórico de Atualizações da Tarefa
    /// </summary>
    public partial class TarefaLogView : XtraForm
    {
        /// <summary>
        /// Responsável por controlar as ações da TarefaLogView
        /// </summary>
        TarefaLogPresenter presenter;

        /// <summary>
        /// Construtor
        /// </summary>
        public TarefaLogView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="tarefa">Objeto de CronogramaTarefa</param>
        public TarefaLogView(Guid oidTarefa)       
        {
            InitializeComponent();
            presenter = new TarefaLogPresenter(this,oidTarefa);
        }

        /// <summary>
        /// Método responsável por apresentar no grid as alterações armazenadas para uma tarefa
        /// </summary>
        /// <param name="alteracoes">lista de alterações ocorridas na tarefa</param>
        public void ListarAlteracoes(List<TarefaLogAlteracaoDto> alteracoes) 
        {
            LogGridControl.DataSource = alteracoes;
        }
    }
}