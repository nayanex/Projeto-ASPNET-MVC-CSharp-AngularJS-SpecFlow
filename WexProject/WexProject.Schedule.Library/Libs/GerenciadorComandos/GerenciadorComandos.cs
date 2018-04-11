using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Library.Libs.CrontroleMovimentacao;
using System.Threading;
using WexProject.Schedule.Library.Libs.Log;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Library.Libs.GerenciadorComandos
{
    /// <summary>
    /// Classe para gestão de comandos que devem ser executados, porém o grid se encontra em estado de edição
    /// </summary>
    public class GerenciadorComandos
    {
        #region Atributos

        /// <summary>
        /// Fila de comandos a serem executados quando o grid sair de edição
        /// </summary>
        protected static Queue<Comando> comandos;

        /// <summary>
        /// Responsável por bloquear momentaneamente as execucoes assincronas dos comandos do gerenciador
        /// </summary>
        protected readonly object _locker;

        /// <summary>
        /// Responsável por gerenciar as tarefas impactadas mais atuais e que podem ou não atualizar o grid
        /// </summary>
        protected GerenciadorTarefasImpactadas gerenciadorTarefasImpactadas;


        #endregion

        #region Propriedades


        /// <summary>
        /// Responsável por gerenciar as tarefas impactadas mais atuais e que podem ou não atualizar o grid
        /// </summary>
        public GerenciadorTarefasImpactadas GerenciadorTarefasImpactadas
        {
            get { return gerenciadorTarefasImpactadas; }
        }

        /// <summary>
        /// atributo responsável por armazenar quando poderá ser executado os comandos
        /// </summary>
        public bool PodeExecutar { get; set; }

        /// <summary>
        /// Propriedade responsável por controlar quantos pedidos de negação de atualização dos comandos foram realizados.
        /// </summary>
        public ReaderWriterLockSlim semaforoLeituraEscritaDataSource;

        /// <summary>
        /// lista de cronogramaTarefas referentes ao datasource do gridView
        /// </summary>
        public BindingList<CronogramaTarefaGridItem> Datasource { get; set; }

        /// <summary>
        /// Propriedade que indicar que comandos estão sendo executados
        /// </summary>
        public bool ExecutandoComandos { get; set; }
        
        #endregion

        #region Eventos


        /// <summary>
        /// evento necessário para configurar pre-condicoes de execucao de comandos (relativo a comportamentos da view)
        /// </summary>
        public event Action AntesDeIniciarExecucaoComandos;

        /// <summary>
        /// evento necessário para configurar pos-condicoes de execucao de comandos (relativo a comportamentos da view)
        /// </summary>
        public event Action AoTerminarExecucaoComandos;

        #endregion

        /// <summary>
        /// responsável por inicializar o gerenciador de comandos
        /// </summary>
        /// <param name="listaTarefas">lista de tarefas exibidas no grid (usada no datasource)</param>
        /// <param name="estadoEdicao">Propriedade da tela que representa se o grid encontra-se ou não em edição</param>
        public GerenciadorComandos( BindingList<CronogramaTarefaGridItem> listaTarefas )
        {
            // recebimento de referencia de fila de comandos
            PodeExecutar = true;
            comandos = new Queue<Comando>();
            Datasource = listaTarefas;
            _locker = new object();
            gerenciadorTarefasImpactadas = new GerenciadorTarefasImpactadas();
            semaforoLeituraEscritaDataSource = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Método responsável por solicitar a Execução da fila de comandos armazenados
        /// enquanto o grid se encontrava em edição
        /// </summary>
        public void ExecutarComandosPendentes()
        {
            if(!PodeExecutar)
                return;

            //Caso não possua comandos a serem executados sai do método
            if(comandos.Count < 1)
                return;
            Comando comando;

            ExecutandoComandos = true;

            ExecutarEventoAntesDosComandosPendentes();
            while(comandos.Count > 0 && PodeExecutar)
            {
                comando = comandos.Dequeue();
                comando.Executar();
            }
            ExecutarEventoAposOsComandosPendentes();

            ExecutandoComandos = false;
        }

        /// <summary>
        /// método responsável por criar um comando de Adicionar novas tarefas ao datasource
        /// </summary>
        /// <param name="tarefas">novos cronogramaTarefaDto a serem adicionadas</param>
        public void CriarComandoAdicionarNovasTarefas( CronogramaTarefaGridItem tarefa, Dictionary<string, Int16> tarefasImpactadas, DateTime dataAtualizacao )
        {
            Monitor.Enter( _locker );
            try
            {
                if(tarefasImpactadas != null)
                {
                    comandos.Enqueue( new ComandoCriarLinhaGrid( this, tarefa ) );
                    if(!tarefasImpactadas.ContainsKey( tarefa.OidCronogramaTarefa.ToString() ))
                        tarefasImpactadas.Add( tarefa.OidCronogramaTarefa.ToString(), (short)tarefa.NbID );
                    CriarComandoAtualizarTarefasImpactadas( tarefasImpactadas, dataAtualizacao ); 
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( _locker );
            }
        }

        /// <summary>
        /// método responsável por criar e armazenar um comando de exclusão de linhas
        /// </summary>
        /// <param name="oidTarefasExcluidas">lista de oidTarefas excluidas</param>
        public void CriarComandoRemoverTarefas( List<Guid> oidTarefasExcluidas, Dictionary<string, Int16> tarefasImpactadas, DateTime dataAtualizacao )
        {
            Monitor.Enter( _locker );
            try
            {
                comandos.Enqueue( new ComandoRemoverLinhaGrid( this, oidTarefasExcluidas ) );
                CriarComandoAtualizarTarefasImpactadas( tarefasImpactadas, dataAtualizacao );
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( _locker );
            }

        }

        /// <summary>
        /// método responsável por atualizar as tarefas impactadas.
        /// </summary>
        /// <param name="oidTarefasExcluidas">lista de oidTarefas excluidas</param>
        public void CriarComandoAtualizarTarefasImpactadas( Dictionary<string, Int16> tarefasImpactadas, DateTime dataAtualizacao )
        {
            Monitor.Enter( _locker );
            try
            {
                lock(this)
                {
                    tarefasImpactadas = gerenciadorTarefasImpactadas.ListarAtualizacoesValidas( tarefasImpactadas, dataAtualizacao );
                    TarefasImpactadasDebugUtil.ExibirLogAtualizacaoTarefasImpactadas( tarefasImpactadas, new List<CronogramaTarefaGridItem>( Datasource ) );
                    comandos.Enqueue( new ComandoAtualizarTarefasImpactadasGrid( this, tarefasImpactadas ) );
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            finally 
            {
                Monitor.Exit( _locker );
            }
        }

        /// <summary>
        /// método responsável por atualizar as tarefas impactadas.
        /// </summary>
        /// <param name="oidTarefasExcluidas">lista de oidTarefas excluidas</param>
        public void CriarComandoAtualizarTarefas( List<CronogramaTarefaGridItem> tarefas )
        {
            Monitor.Enter( _locker );
            try
            {
                if(tarefas != null && tarefas.Count > 0)
                {
                    comandos.Enqueue( new ComandoAtualizarTarefasCronograma( this, tarefas ) );
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( _locker );
            }
        }

        /// <summary>
        /// método responsável por criar um comando de movimentar tarefas
        /// </summary>
        /// <param name="oidCronogramaTarefaMovida">Oid de CronogramaTarefa que foi movida</param>
        /// <param name="posicaoInicial">posição inicial da tarefa</param>
        /// <param name="posicaoFinal">posição final da tarefa</param>
        /// <param name="tarefasImpactadas">dicionário com a nova ordem das tarefas que foram impactadas pela movimentação</param>
        /// <returns></returns>
        public TarefaMovida CriarComandoMovimentarTarefa( Guid oidCronogramaTarefaMovida, Int16 posicaoInicial, Int16 posicaoFinal, Dictionary<string, Int16> tarefasImpactadas, DateTime dataAtualizacao )
        {
            Monitor.Enter( _locker );
            TarefaMovida tarefaMovida = null;
            try
            {
                tarefaMovida = CriarTarefaMovida( oidCronogramaTarefaMovida, posicaoInicial, posicaoFinal, tarefasImpactadas );
                if(!tarefasImpactadas.ContainsKey( oidCronogramaTarefaMovida.ToString() ))
                    tarefaMovida.TarefasImpactadas.Add( oidCronogramaTarefaMovida.ToString(), posicaoFinal );
                CriarComandoAtualizarTarefasImpactadas( tarefaMovida.TarefasImpactadas, dataAtualizacao );
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( _locker );
            }
            return tarefaMovida;
        }

        /// <summary>
        /// Método responsável por 
        /// </summary>
        /// <param name="oidCronogramaTarefaMovida">oid da tarefa movida</param>
        /// <param name="posicaoInicial">posicao inicial da tarefa movida</param>
        /// <param name="posicaoFinal">posicao final da tarefa</param>
        /// <param name="tarefasImpactadas">tarefas impactadas na mudança</param>
        /// <returns></returns>
        public virtual TarefaMovida CriarTarefaMovida( Guid oidCronogramaTarefaMovida, Int16 posicaoInicial, Int16 posicaoFinal, Dictionary<string, Int16> tarefasImpactadas )
        {
            return new TarefaMovida( oidCronogramaTarefaMovida, posicaoInicial, posicaoFinal, tarefasImpactadas );
        }

        /// <summary>
        /// método responsável por acionar evento de desabilitar componente caso seja necessário
        /// </summary>
        public void ExecutarEventoAntesDosComandosPendentes()
        {
            if(AntesDeIniciarExecucaoComandos != null)
                AntesDeIniciarExecucaoComandos();
        }

        /// <summary>
        /// método responsável por acionar o evento de atualização do componente de tela
        /// </summary>
        public void ExecutarEventoAposOsComandosPendentes()
        {
            if(AoTerminarExecucaoComandos != null)
                AoTerminarExecucaoComandos();
        }

        /// <summary>
        /// Método responsável por Esperar o semáforo de leitura do datasource
        /// </summary>
        public void EsperarLeituraDataSource()
        {
            semaforoLeituraEscritaDataSource.EnterReadLock();
        }

        /// <summary>
        /// Método responsável por Liberar o semáforo de leitura do datasource
        /// </summary>
        public void LiberarLeituraDataSource()
        {
            semaforoLeituraEscritaDataSource.ExitReadLock();
        }

        /// <summary>
        /// Método responsável por Esperar o semáforo de escrita do datasource
        /// </summary>
        public void EsperarEscritaDataSource()
        {
            semaforoLeituraEscritaDataSource.EnterWriteLock();
        }

        /// <summary>
        /// Método responsável por liberar o semáforo de escrita do datasource
        /// </summary>
        public void LiberarEscritaDataSource()
        {
            semaforoLeituraEscritaDataSource.ExitWriteLock();
        }

    }
}
