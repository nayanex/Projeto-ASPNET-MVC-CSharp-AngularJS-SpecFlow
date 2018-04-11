using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Library.Libs.GerenciadorComandos
{
    /// <summary>
    /// Classe responsável por efetuar a atualização de tarefas impactadas das linhas no grid do cronograma view
    /// </summary>
    public class ComandoAtualizarTarefasImpactadasGrid : Comando
    {
        #region Atributos

        /// <summary>
        /// atributo responsável por armazenar as novas tarefas e suas respectivas novas posições
        /// </summary>
        private Dictionary<string, Int16> tarefasImpactadas;

        #endregion

        #region Métodos

        /// <summary>
        /// preencher um datasource com a nova tarefa criada 
        /// </summary>
        public override void Executar()
        {
            CronogramaTarefaGridItem tarefaAtual;

            gerenciador.Datasource.RaiseListChangedEvents = false;

            gerenciador.semaforoLeituraEscritaDataSource.EnterWriteLock();

            foreach(var tarefaImpactada in tarefasImpactadas)
            {
                tarefaAtual = gerenciador.Datasource.FirstOrDefault( o => o.OidCronogramaTarefa == Guid.Parse( tarefaImpactada.Key ) );
                if(tarefaAtual != null)
                {
                    tarefaAtual.NbID = tarefaImpactada.Value;

                    if(gerenciador.Datasource.Contains( tarefaAtual ))
                        gerenciador.Datasource.Remove( tarefaAtual );

                    int indice = int.Parse( tarefaAtual.NbID.ToString() ) - 1;

                    try
                    {
                        gerenciador.Datasource.Insert( indice, tarefaAtual );            
                    }
                    catch(ArgumentOutOfRangeException ex)
                    {
                        gerenciador.Datasource.Add( tarefaAtual );
                    }
                }
            }

            gerenciador.semaforoLeituraEscritaDataSource.ExitWriteLock();

            if(gerenciador.PodeExecutar)
                gerenciador.Datasource.RaiseListChangedEvents = true;
        }

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor responsável por armazenar o gerenciador e o dicionário contendo as tarefas impactadas.
        /// </summary>
        /// <param name="gerenciador">gerenciador de comandos</param>
        /// <param name="impactadas">lista de tarefas ordenadas</param>
        public ComandoAtualizarTarefasImpactadasGrid( GerenciadorComandos gerenciador, Dictionary<string, Int16> impactadas )
        {
            this.gerenciador = gerenciador;
            tarefasImpactadas = impactadas;
        }


        #endregion
    }
}
