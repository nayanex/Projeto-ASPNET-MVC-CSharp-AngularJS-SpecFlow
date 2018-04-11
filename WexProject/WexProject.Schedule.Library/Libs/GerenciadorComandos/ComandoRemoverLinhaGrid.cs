using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Library.Libs.GerenciadorComandos
{
    /// <summary>
    /// classe de comando re remoção de linha do grid
    /// </summary>
    public class ComandoRemoverLinhaGrid : Comando
    {
        /// <summary>
        /// atributo responsável por armazenar a  lista de novas tarefas a serem adicionadas ao datasource
        /// </summary>
        private List<Guid> tarefasExcluidas;

        /// <summary>
        /// gerenciador responsável por armazenar a lista de cronogramas tarefasdto datasource do cronogramaView
        /// </summary>
        /// <param name="gerenciador">gerenciador de comandos contendo o datasource</param>
        public ComandoRemoverLinhaGrid( GerenciadorComandos gerenciador, List<Guid> oidTarefasExcluidas)
        {
            this.gerenciador = gerenciador;
            tarefasExcluidas = oidTarefasExcluidas;
        }

        /// <summary>
        /// método responsável por efetuar a exclusão das tarefas com o oid contidos na lista de tarefas excluidas
        /// </summary>
        public override void Executar()
        {
            gerenciador.EsperarEscritaDataSource();
            gerenciador.Datasource.RaiseListChangedEvents = false;
            List<CronogramaTarefaGridItem> tarefas = new List<CronogramaTarefaGridItem>( gerenciador.Datasource.Where( o => tarefasExcluidas.Contains( o.OidCronogramaTarefa ) ) );

            foreach(CronogramaTarefaGridItem tarefa in tarefas)
            {
                gerenciador.Datasource.Remove( tarefa );
            }
            gerenciador.LiberarEscritaDataSource();
            gerenciador.Datasource.RaiseListChangedEvents = true;
        }
    }
}
