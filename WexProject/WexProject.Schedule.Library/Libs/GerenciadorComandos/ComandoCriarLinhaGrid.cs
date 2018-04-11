using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Library.Libs.GerenciadorComandos
{
    /// <summary>
    /// Classe responsável por efetuar a criação de linhas no grid do cronograma view
    /// </summary>
    public class ComandoCriarLinhaGrid : Comando
    {
        /// <summary>
        /// atributo responsável por armazenar a  lista de novas tarefas a serem adicionadas ao datasource
        /// </summary>
        private CronogramaTarefaGridItem novaTarefa;

        /// <summary>
        /// gerenciador responsável por armazenar a lista de cronogramas tarefasdto datasource do cronogramaView
        /// </summary>
        /// <param name="gerenciador">gerenciador de comandos</param>
        /// <param name="NovaTarefaCriada">nova tarefa criada</param>
        /// <param name="impactadas">lista de tarefas ordenadas</param>
        public ComandoCriarLinhaGrid( GerenciadorComandos gerenciador, CronogramaTarefaGridItem NovaTarefaCriada )
        {
            this.gerenciador = gerenciador;
            novaTarefa = NovaTarefaCriada;
        }

        /// <summary>
        /// preencher um datasource com a nova tarefa criada 
        /// </summary>
        public override void Executar()
        {
            RnAdicionarNovaTarefa( novaTarefa );
        }

        /// <summary>
        /// Método responsável por adicionar uma nova tarefa criada
        /// </summary>
        /// <param name="novaTarefaCriada"></param>
        private void RnAdicionarNovaTarefa( CronogramaTarefaGridItem novaTarefaCriada )
        {
            gerenciador.Datasource.RaiseListChangedEvents = false;

            gerenciador.EsperarEscritaDataSource();

            int posicaoRelativa = (int)( novaTarefaCriada.NbID - 1 );
            int posicaoDefinitiva;
            int posicaoMaxima = gerenciador.Datasource.Count - 1;

            if(posicaoMaxima < 0)
                posicaoDefinitiva = 0;
            else
                posicaoDefinitiva = posicaoRelativa <= posicaoMaxima ? posicaoRelativa : posicaoMaxima;

            try
            {
                gerenciador.Datasource.Insert( posicaoDefinitiva, novaTarefaCriada );
            }
            catch(Exception)
            {
                gerenciador.Datasource.Add( novaTarefaCriada );
            }

            gerenciador.LiberarEscritaDataSource();

            gerenciador.Datasource.RaiseListChangedEvents = true;
        }
    }
}
