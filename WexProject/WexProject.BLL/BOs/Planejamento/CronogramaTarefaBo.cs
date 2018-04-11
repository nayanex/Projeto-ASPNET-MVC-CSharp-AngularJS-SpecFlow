using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.Concorrencia;
using WexProject.Library.Libs.DataHora;
using WexProject.Library.Libs.SemaforoPorIntervalo;
using WexProject.BLL.Extensions.Entities;
using System.Diagnostics;

namespace WexProject.BLL.BOs.Planejamento
{
	public class CronogramaTarefaBo
	{
		#region Atributos

		/// <summary>
		/// Atributo responsável por controlar o acesso aos métodos de excluir CronogramaTarefas em um Cronograma.
		/// </summary>
		public static Dictionary<Guid, SemaforoControleAcesso> controleDeAcessosAoExcluir = new Dictionary<Guid, SemaforoControleAcesso>();

		/// <summary>
		/// Atributo responsável por controlar o acesso aos métodos de criar CronogramaTarefas em um Cronograma.
		/// </summary>
		public static Dictionary<Guid, SemaforoControleAcesso> ControleDeAcessosAoCriar = new Dictionary<Guid, SemaforoControleAcesso>();

		/// <summary>
		/// Atributo responsável por controlar o acesso a validação do cronograma no momento em que estiver verificando se existe no dicionário de controleAcessoAoExcluir.
		/// </summary>
		private static ReaderWriterLockSlim semaforoValidacaoCronogramaAoExcluir = new ReaderWriterLockSlim();

		/// <summary>
		/// Atributo responsável por controlar o acesso a validação do cronograma no momento em que estiver verificando se existe no dicionário de controleAcessoAoCriar.
		/// </summary>
		private static ReaderWriterLockSlim semaforoValidacaoCronogramaAoCriar = new ReaderWriterLockSlim();

		/// <summary>
		/// Dicionário responsável por armazenar objeto para contar as threads que estão em execução por cronograma.
		/// Key: OidCronograma
		/// Value: Objeto contador para threads
		/// </summary>
		private static Dictionary<Guid, ThreadsEmExecucao> threadsPorCronograma = new Dictionary<Guid, ThreadsEmExecucao>();

		/// <summary>
		/// Semáforo que controla a adição no dicionário de threads por cronograma.
		/// </summary>
		private static Semaphore semaforoControladorThreadsPorCronograma = new Semaphore( 1, 1 );

		/// <summary>
		/// Hash que guarda o último ID de um cronograma. Utilizado como cache para 
		/// não ter que realizar busca no banco desnecessária 
		/// quando criar tarefa na última posição.
		/// </summary>
		public static Hashtable maiorNbIDPorCronograma = new Hashtable();

		/// <summary>
		/// Semáforo responsável por controlar a chamada da exclusão dos semáforos não utilizados.
		/// </summary>
		private static Semaphore semaforoExclusaoSemaforosNaoUtilizados = new Semaphore( 1, 1 );

		#endregion

		#region Constantes

		/// <summary>
		/// Menor NbID possível em um cronograma.
		/// </summary>
		private const int MENOR_NBID_CRONOGRAMA = 1;

		/// <summary>
		/// Constante que serve como índice da Hashtable que retorna os semáforos novos que serão utilizados na reordenação.
		/// </summary>
		private const string SEMAFOROS_NOVOS = "semaforosNovos";

		/// <summary>
		/// Constante que serve como índice da Hashtable que retorna os semáforos para aguardar que serão utilizados na reordenação.
		/// </summary>
		private const string SEMAFOROS_PARA_AGUARDAR = "semaforosAguardar";

		#endregion

		#region Struct

		/// <summary>
		/// Struct que armazena o semáforo e o contador de acesso para cada ação
		/// </summary>
		public struct SemaforoControleAcesso
		{
			/// <summary>
			/// Semáforo responsável por controlar a concorrência ao excluir tarefas.
			/// </summary>
			public Semaphore semaforoAcesso;

			/// <summary>
			/// Semáforo responsável por controlar a concorrência ao incremento e decremento do contador de acesso.
			/// </summary>
			public Semaphore semaforoContador;

			/// <summary>
			/// responsável por controlar a quantidade de threads que estão acessando o método.
			/// </summary>
			public int contadorAcesso;
		}

		/// <summary>
		/// Struct que armazena o contador para contar quantas threads estão em execução no método 
		/// e o semáforo para não ocasionar erros no momento de incrementar e decrementar o contador.
		/// </summary>
		public struct ThreadsEmExecucao
		{
			/// <summary>
			/// contador de acesso das threads
			/// </summary>
			public int contador;

			/// <summary>
			/// Semáforo para controlar incrementação e decrementação
			/// </summary>
			public Semaphore semaforoDoContador;
		}


		#endregion

		#region Métodos

		/// <summary>
		/// Método responsável por excluir uma tarefa em cronogramaTarefa e em Tarefa
		/// </summary>
		/// <param name="session">Contexto do banco Corrente</param>
		/// <param name="oidTarefas">Lista de oids das tarefas que foram solicitadas para exclusão</param>
		/// <returns>Lista de oids das tarefas que foram excluidas</returns>
		private static List<CronogramaTarefa> ExcluirTarefas( List<Guid> oidTarefas, Guid oidCronograma, ref List<CronogramaTarefa> tarefasReordenadas, ref List<Guid> oidsTarefasNaoExcluidas, ref DateTime dataHoraAcao )
		{
			if(oidTarefas.Count == 0)
				throw new ArgumentException( "Os parâmetros OidTarefas não podem ser nulos." );

			List<CronogramaTarefa> tarefasParaExcluir = new List<CronogramaTarefa>();
			List<CronogramaTarefa> tarefasExcluidas = new List<CronogramaTarefa>();

			CronogramaTarefaBo.controleDeAcessosAoExcluir[oidCronograma].semaforoAcesso.WaitOne();

			tarefasParaExcluir = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOid( oidTarefas, o => o.Tarefa.TarefaHistoricoTrabalhos );

			CronogramaTarefaBo.VerificarTarefasJaExcluidas( oidTarefas, ref oidsTarefasNaoExcluidas, tarefasParaExcluir );

			if(tarefasParaExcluir.Count == 0)
				return tarefasExcluidas;

			for(int indiceExclusao = 0; indiceExclusao < tarefasParaExcluir.Count; indiceExclusao++)
			{
				if(tarefasParaExcluir[indiceExclusao] != null)
				{
					if(tarefasParaExcluir[indiceExclusao].Tarefa.TarefaHistoricoTrabalhos == null || tarefasParaExcluir[indiceExclusao].Tarefa.TarefaHistoricoTrabalhos.Count == 0)
					{
						CronogramaTarefaDao.Excluir( tarefasParaExcluir[indiceExclusao] );
					}
					CronogramaTarefaBo.VerificarTarefasExcluidasETarefasNaoExcluidas( ref oidsTarefasNaoExcluidas, tarefasParaExcluir, ref tarefasExcluidas, indiceExclusao );
				}
			}

			List<short> nbIdsExcluidos = tarefasExcluidas.Select( o => o.NbID ).ToList();

			if(nbIdsExcluidos.Count > 0)
			{
				short menorIdExcluido = nbIdsExcluidos.Min();

				List<CronogramaTarefa> tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( oidCronograma, menorIdExcluido );

				if(CronogramaTarefaBo.ValidarExistenciaTarefasParaReordenar( tarefasParaReordenar ))
				{
					CronogramaTarefa tarefaReferenciaParaReordenacao = CronogramaTarefaBo.VerificarTarefaReferenciaParaReordenacao( (Guid)tarefasParaReordenar.FirstOrDefault().OidCronograma, menorIdExcluido );

					tarefasParaReordenar = tarefasParaReordenar.Concat( tarefasExcluidas ).ToList();

					CronogramaTarefaBo.controleDeAcessosAoExcluir[oidCronograma].semaforoAcesso.Release();

					tarefasReordenadas = CronogramaTarefaBo.RecalcularPorBloco( tarefaReferenciaParaReordenacao, tarefasParaReordenar, ref dataHoraAcao, true );

					tarefasReordenadas = CronogramaTarefaBo.AtualizarNbIDTarefasReordenadas( tarefasReordenadas, ref dataHoraAcao );
				}
				else
				{
					CronogramaTarefaBo.controleDeAcessosAoExcluir[oidCronograma].semaforoAcesso.Release();
				}
			}
			else
			{
				CronogramaTarefaBo.controleDeAcessosAoExcluir[oidCronograma].semaforoAcesso.Release();
			}

			return tarefasExcluidas;
		}

		/// <summary>
		/// Método responsável por chamar o método criar tarefa (classe tarefa) e incluí-la a um cronograma.
		/// É usado pela classe Cronograma quando solicitada para criar uma nova tarefa
		/// </summary>
		/// <param name="login"> Login do usuário</param>
		/// <param name="session">Contexto do banco Corrente</param>
		/// <param name="oidCronograma">ID Cronograma</param>
		/// <param name="txDescricaoTarefa">Descrição da tarefa</param>
		/// <param name="txObservacaoTarefa">Observação da tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento da tarefa</param>
		/// <param name="responsaveis">responsáveis da tarefa</param>
		/// <param name="estimativaInicial">Estimativa inicial da tarefa</param>
		/// <param name="dtInicio">Data de início da tarefa</param>
		/// <param name="oidTarefaSelecionada">ID da tarefa que estava selecionada antes de uma nova tarefa ser criada</param>
		/// <returns>Objeto Tarefa Criada</returns>
		private static CronogramaTarefa IncluirTarefa( Guid oidCronograma,
														string txDescricaoTarefa,
														SituacaoPlanejamento situacaoPlanejamento,
														DateTime dtInicio,
														string responsaveis,
														string login,
														out List<CronogramaTarefa> tarefasReordenadas,
														ref DateTime dataHoraAcao,
														string txObservacaoTarefa = "",
														Int16 estimativaInicial = 0,
														short nbIdReferencia = 0 )
		{
			if(oidCronograma == null || String.IsNullOrEmpty( login ))
				throw new ArgumentException( "Os parâmetros OidCronograma são obrigatórios." );

			tarefasReordenadas = new List<CronogramaTarefa>();

			CronogramaTarefaBo.ControleDeAcessosAoCriar[oidCronograma].semaforoAcesso.WaitOne();

			CronogramaTarefa novaTarefa = new CronogramaTarefa()
			{
				OidCronograma = oidCronograma
			};

			CronogramaTarefaBo.AtribuirId( novaTarefa, nbIdReferencia );

			if(novaTarefa.Cronograma != null)
				novaTarefa.OidCronograma = novaTarefa.OidCronograma;

			novaTarefa.Tarefa = TarefaBo.CriarTarefa( txDescricaoTarefa, situacaoPlanejamento, DateTime.Now, login, txObservacaoTarefa, responsaveis, estimativaInicial );

			if(novaTarefa.Tarefa != null)
				novaTarefa.OidTarefa = novaTarefa.Tarefa.Oid;

			novaTarefa = CronogramaTarefaDao.Salvar( novaTarefa );

			List<CronogramaTarefa> tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( oidCronograma, novaTarefa.NbID );

			CronogramaTarefaBo.ControleDeAcessosAoCriar[oidCronograma].semaforoAcesso.Release();

			if(tarefasParaReordenar.Contains( novaTarefa ))
				tarefasParaReordenar.Remove( novaTarefa );

			if(novaTarefa.Oid != new Guid())
				if(CronogramaTarefaBo.ValidarExistenciaTarefasParaReordenar( tarefasParaReordenar ))
				{
					CronogramaTarefa tarefaReferencia = new CronogramaTarefa()
					{
						Oid = novaTarefa.Oid,
						OidCronograma = novaTarefa.OidCronograma,
						OidTarefa = novaTarefa.OidTarefa,
						NbID = novaTarefa.NbID
					};

					tarefasReordenadas = CronogramaTarefaBo.RecalcularPorBloco( tarefaReferencia, tarefasParaReordenar, ref dataHoraAcao, true, true, 0, novaTarefa );

					tarefasReordenadas = CronogramaTarefaBo.AtualizarNbIdTarefasReordenadas( tarefasReordenadas, ref novaTarefa, ref dataHoraAcao );
				}

			return novaTarefa;
		}

		/// <summary>
		/// Método responsável por atribuir um novo id para uma tarefa.
		/// Verifica se a tarefa é a 1ª em um cronograma, a última ou está no meio.
		/// </summary>
		/// <param name="session">Contexto do banco corrente</param>
		/// <param name="novaTarefa">Nova tarefa que está sendo criada</param>
		/// <param name="tarefaSelecionada">Tarefa que foi selecionada ao criar uma nova tarefa</param>
		public static void AtribuirId( CronogramaTarefa novaTarefa, short nbIDReferencia = 0 )
		{
			//Verifica se a hash está zerada. Isto ocorre quando não tiver nenhuma tarefa no cronograma ou 
			//quando o serviço cair e a hash perder seus dados. Então ela deverá resgatar quem é o último ID daquele cronograma.
			//Verifica se já tem aquele índice na hash
			if(!CronogramaTarefaBo.maiorNbIDPorCronograma.ContainsKey( novaTarefa.OidCronograma.ToString() ))
				CronogramaTarefaBo.maiorNbIDPorCronograma.Add( novaTarefa.OidCronograma.ToString(), CronogramaTarefaDao.ConsultarMaxNbIDPorCronograma( (Guid)novaTarefa.OidCronograma ) );

			short ultimoID = (Int16)CronogramaTarefaBo.maiorNbIDPorCronograma[novaTarefa.OidCronograma.ToString()];

			//Verifica se a tarefa é a primeira a ser criada no cronograma.
			if(nbIDReferencia == 0)
			{
				novaTarefa.NbID = Int16.Parse( ( CronogramaTarefaDao.ConsultarMaxNbIDPorCronograma( (Guid)novaTarefa.OidCronograma ) + 1 ).ToString() );

				if(CronogramaTarefaBo.maiorNbIDPorCronograma.ContainsKey( novaTarefa.OidCronograma.ToString() ))
					CronogramaTarefaBo.maiorNbIDPorCronograma[novaTarefa.OidCronograma.ToString()] = novaTarefa.NbID;
				else
					CronogramaTarefaBo.maiorNbIDPorCronograma.Add( novaTarefa.OidCronograma.ToString(), novaTarefa.NbID );
			}
			//Verifica se a tarefa é a última.
			else if(nbIDReferencia == ultimoID)
			{
				novaTarefa.NbID = Int16.Parse( ( CronogramaTarefaDao.ConsultarMaxNbIDPorCronograma( (Guid)novaTarefa.OidCronograma ) + 1 ).ToString() );

				//adiciona na hash como o último ID.
				CronogramaTarefaBo.maiorNbIDPorCronograma[novaTarefa.OidCronograma.ToString()] = novaTarefa.NbID;
			}
			//Cria tarefa no meio
			else
			{
				CronogramaTarefa cronogramaTarefaValida = CronogramaTarefaDao.ConsultarCronogramaTarefaPorNbId( (Guid)novaTarefa.OidCronograma, nbIDReferencia, null );

				VerificarExistenciaNbIDValidoParaNovaTarefa( novaTarefa, ref nbIDReferencia, cronogramaTarefaValida );

				novaTarefa.NbID = nbIDReferencia;

				//Se uma tarefa for criada no meio, o último ID recebe ele mesmo + 1;
				CronogramaTarefaBo.IncrementarMaiorNbIdCronograma( novaTarefa );
			}
		}

		/// <summary>
		/// Método que será avisado quando o evento AoCriarLog for acionado e irá acrescentar um log referente a CronogramaTarefa.
		/// </summary>
		/// <param name="tarefaAtual"></param>
		/// <param name="tarefaAntiga"></param>
		/// <param name="alteracoes"></param>
		public static void CriarLogCronogramaTarefa( CronogramaTarefa cronogramaTarefaAtual, CronogramaTarefa cronogramaTarefaAntiga, StringBuilder alteracoes )
		{
			if(cronogramaTarefaAntiga != null && cronogramaTarefaAtual.NbID != cronogramaTarefaAntiga.NbID)
				alteracoes.Append( String.Format( "ID alterado de '{0}' para '{1}'\n", cronogramaTarefaAntiga.NbID, cronogramaTarefaAtual.NbID ) );
		}

		/// <summary>
		/// Método responsável por reordenar as tarefas de um cronograma quando uma tarefa for criada ou uma tarefa já existente trocar de posição.
		/// É usado pelo Serviço de PlanejamentoService
		/// </summary>
		/// <param name="tarefaSelecionada">Tarefa selecionada onde uma nova tarefa será criada, ou a tarefa que se deseja mover de posição.</param>
		/// <param name="novaTarefa">Objeto a nova tarefa criada</param>
		/// <param name="tarefaDestino">Representa a posição da tarefa para onde a tarefa selecionada será movida.</param>
		/// <returns>Lista contendo as tarefas que foram impactadas com a reordenação</returns>
		public static List<CronogramaTarefa> ReordenarTarefas( Guid oidCronogramaTarefaSelecionada, short nbIDDestino, ref short nbIDAtualizadoTarefaMovida, ref DateTime dataHoraAcao, ref Guid oidCronograma )
		{
			if(oidCronogramaTarefaSelecionada == new Guid() || oidCronogramaTarefaSelecionada == null || nbIDDestino == 0)
				throw new ArgumentException( "Os parâmetros oidCronogramaTarefaSelecionada e oidCronogramaTarefaDestino não podem ser nulos." );

			CronogramaTarefa cronogramaTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( oidCronogramaTarefaSelecionada, o => o.Cronograma );

			if(cronogramaTarefa == null)
			{
				oidCronograma = new Guid();
				return new List<CronogramaTarefa>();
			}

			if(cronogramaTarefa.NbID == nbIDDestino)
			{
				oidCronograma = new Guid();
				return new List<CronogramaTarefa>();
			}

			oidCronograma = (Guid)cronogramaTarefa.OidCronograma;

			List<CronogramaTarefa> tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( (Guid)cronogramaTarefa.OidCronograma, cronogramaTarefa.NbID, nbIDDestino );

			CronogramaTarefa tarefaReferencia = new CronogramaTarefa()
			{
				Oid = cronogramaTarefa.Oid,
				OidCronograma = cronogramaTarefa.OidCronograma,
				OidTarefa = cronogramaTarefa.OidTarefa,
				NbID = cronogramaTarefa.NbID
			};

			List<CronogramaTarefa> tarefasReordenadas = CronogramaTarefaBo.RecalcularPorBloco( tarefaReferencia, tarefasParaReordenar, ref dataHoraAcao, true, true, nbIDDestino );

			tarefasReordenadas = CronogramaTarefaBo.AtualizarNbIdTarefasReordenadas( tarefasReordenadas, ref cronogramaTarefa, ref dataHoraAcao );

			nbIDAtualizadoTarefaMovida = cronogramaTarefa.NbID;

			return tarefasReordenadas;
		}

		/// <summary>
		/// Método responsável por atribuir o id destino para a tarefa selecionada.
		/// </summary>
		/// <param name="tarefaSelecionada">CronogramaTarefa da tarefa selecionada</param>
		/// <param name="nbIdDestino">Id de destino para onde a tarefa selecionada irá</param>
		public static void MoverTarefaSelecionadaParaDestino( ref CronogramaTarefa tarefaParaMover, short nbIdDestino, ref short nbIdAntigoDaTarefaMovimentada )
		{
			nbIdAntigoDaTarefaMovimentada = tarefaParaMover.NbID;
			tarefaParaMover.NbIdAntigo = tarefaParaMover.NbID;
			tarefaParaMover.NbID = nbIdDestino;

			CronogramaTarefa cronogramaTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorNbId( (Guid)tarefaParaMover.OidCronograma, nbIdDestino );

			if(cronogramaTarefa == null)
			{
				CronogramaTarefa tarefaReferenciaParaMovimentacao = CronogramaTarefaBo.ValidarNbIDReferenciaAtualizado( (Guid)tarefaParaMover.OidCronograma, nbIdDestino );

				tarefaParaMover.NbID = tarefaReferenciaParaMovimentacao.NbID;
				nbIdDestino = tarefaReferenciaParaMovimentacao.NbID;
			}

			CronogramaTarefa copia = tarefaParaMover.Clone();
			copia.Cronograma = null;
			copia.Tarefa = null;

			try
			{
                using(WexDb contexto = ContextFactoryManager.CriarWexDb())
				{
					if(contexto.CronogramaTarefa.Existe( o => o.Oid == copia.Oid ))
					{
						contexto.CronogramaTarefa.Attach( copia );
						contexto.Entry( copia ).State = EntityState.Modified;
					}
					else
					{
						contexto.CronogramaTarefa.Add( copia );
					}
					contexto.SaveChanges();
				}
			}
			catch(Exception)
			{
                using(WexDb contexto = ContextFactoryManager.CriarWexDb())
				{
					try
					{

						if(contexto.CronogramaTarefa.Existe( o => o.Oid == copia.Oid ))
						{
							if(contexto.CronogramaTarefa.ExisteLocalmente( o => o.Oid == copia.Oid ))
							{
								contexto.Entry<CronogramaTarefa>( copia ).Reload();
								copia.NbID = tarefaParaMover.NbID;
								copia.NbIdAntigo = tarefaParaMover.NbIdAntigo;
								nbIdAntigoDaTarefaMovimentada = tarefaParaMover.NbIdAntigo;
								contexto.CronogramaTarefa.Attach( copia );
								contexto.Entry( copia ).State = EntityState.Modified;
							}
							else
							{
								contexto.CronogramaTarefa.Attach( copia );
								contexto.Entry<CronogramaTarefa>( copia ).Reload();
								copia.NbID = tarefaParaMover.NbID;
								copia.NbIdAntigo = tarefaParaMover.NbIdAntigo;
								nbIdAntigoDaTarefaMovimentada = tarefaParaMover.NbIdAntigo;
								contexto.CronogramaTarefa.Attach( copia );
								contexto.Entry( copia ).State = EntityState.Modified;
							}
						}
						else
						{
							contexto.CronogramaTarefa.Add( copia );
						}
						contexto.SaveChanges();
					}
					catch(Exception)
					{
						throw;
					}
				}
			}
		}

		/// <summary>
		/// Método responsável realizar as validações necessárias para reordenação e verificar se ainda pode reordenar.
		/// </summary>
		/// <param name="oidCronograma">Oid do cronograma</param>
		/// <param name="cronogramaTarefaCriada">Tarefa que foi criada recentemente</param>
		/// <param name="ultimoNbIdCriado">ultimo Id da tarefa que foi criada recentemente</param>
		/// <param name="semaforosParaReordenacao">Lista de semáforos para reordenação</param>
		/// <param name="tarefasParaReordenar">Lista tarefas que serão reordenadas</param>
		/// <param name="nbIdsAtuaisParaReordenar">Ois Ids atuais que serão reordenados no loop</param>
		/// <param name="nbIdsParaReordenar">todos os Ids que serão reordenados</param>
		/// <param name="indiceLoopReordenacao">índice do loop de reordenação</param>
		/// <returns>Autorização de pode reordenar ou não</returns>
		private static CsReordenarAutorizacao ValidarReordenacaoQuandoAcaoForCriacao( Guid oidCronograma, CronogramaTarefa cronogramaTarefaCriada, short ultimoNbIdCriado, ref List<SemaforoPorIntervalo> semaforosParaReordenacao, ref List<CronogramaTarefa> tarefasParaReordenar, ref List<short> nbIdsAtuaisParaReordenar, ref List<short> nbIdsParaReordenar, int indiceLoopReordenacao )
		{
			if(nbIdsAtuaisParaReordenar.Count > 0)
			{
				tarefasParaReordenar = CronogramaTarefaDao.ConsultarCronogramaTarefasPorIntervaloNbIDs( oidCronograma, nbIdsAtuaisParaReordenar.Min(), nbIdsAtuaisParaReordenar.Max() );

				if(!CronogramaTarefaBo.VerificarIntervaloPrecisaReordenar( semaforosParaReordenacao, tarefasParaReordenar ))
				{
					return CsReordenarAutorizacao.NaoAutorizado;
				}
			}

			if(semaforosParaReordenacao[indiceLoopReordenacao] == semaforosParaReordenacao.Last())
				//Se existir alguma tarefa que não esteja contemplada, ou seja, enquanto esteja reordenando outra reordenação ocorreu e por isso alguma tarefa está de fora da reordenação.
				//cria novos semáforos para reordenar a tarefa não contemplada posteriormente
				CronogramaTarefaBo.CriarSemaforosParaTarefasNaoContempladas( oidCronograma, ref nbIdsAtuaisParaReordenar, ref semaforosParaReordenacao, ref nbIdsParaReordenar );

			if(indiceLoopReordenacao == 0)
			{
				if(CronogramaTarefaBo.VerificarNbIdTarefaCriadaFoiAlterado( tarefasParaReordenar, cronogramaTarefaCriada.Oid, ultimoNbIdCriado ))
				{
					return CsReordenarAutorizacao.NaoAutorizado;
				}
			}

			return CsReordenarAutorizacao.Autorizado;
		}

		/// <summary>
		/// Método responsável realizar as validações necessárias para reordenação e verificar se ainda pode reordenar.
		/// </summary>
		/// <param name="oidCronograma">Oid do cronograma</param>
		/// <param name="tarefaReferenciaParaReordenacao">Tarefa referencia para reordenacao</param>
		/// <param name="semaforosParaReordenacao">Lista de semáforos para reordenação</param>
		/// <param name="tarefasParaReordenar">Lista tarefas que serão reordenadas</param>
		/// <param name="nbIdsAtuaisParaReordenar">Ois Ids atuais que serão reordenados no loop</param>
		/// <param name="nbIdsParaReordenar">todos os Ids que serão reordenados</param>
		/// <param name="indiceLoopReordenacao">índice do loop de reordenação</param>
		/// <returns>Autorização de pode reordenar ou não</returns>
		private static CsReordenarAutorizacao ValidarReordenacaoQuandoAcaoForExclusao( Guid oidCronograma, ref CronogramaTarefa tarefaReferenciaParaReordenacao, ref List<SemaforoPorIntervalo> semaforosParaReordenacao, ref List<CronogramaTarefa> tarefasParaReordenar, ref List<short> nbIdsAtuaisParaReordenar, ref List<short> nbIdsParaReordenar, int indiceLoopReordenacao )
		{
			if(nbIdsAtuaisParaReordenar.Count > 0)
			{
				tarefasParaReordenar = CronogramaTarefaDao.ConsultarCronogramaTarefaPorNbID( oidCronograma, nbIdsAtuaisParaReordenar, o => o.Tarefa ).OrderBy( o => o.NbID ).ToList();

				if(semaforosParaReordenacao[indiceLoopReordenacao] == semaforosParaReordenacao.Last())
					//Se existir alguma tarefa que não esteja contemplada, ou seja enquanto esteja reordenando outra reordenação ocorreu e por isso alguma tarefa está de fora da reordenação.
					//cria novos semáforos para reordenar a tarefa não contemplada posteriormente
					CronogramaTarefaBo.CriarSemaforosParaTarefasNaoContempladas( oidCronograma, ref nbIdsAtuaisParaReordenar, ref semaforosParaReordenacao, ref nbIdsParaReordenar );

				if(!( tarefasParaReordenar.Count > 0 ))
				{
					return CsReordenarAutorizacao.NaoAutorizadoMasPodeProsseguirOutraReordenacao;
				}

				tarefaReferenciaParaReordenacao = CronogramaTarefaBo.ValidarTarefaReferenciaParaReordenacao( tarefaReferenciaParaReordenacao );

				//verifica se já existe uma tarefa com o NbID que iria ser reordenado.
				short nbIdParaSerPesquisado = short.Parse( ( tarefaReferenciaParaReordenacao.NbID + 1 ).ToString() );
				CronogramaTarefa tarefaReordenada = CronogramaTarefaDao.ConsultarCronogramaTarefaPorNbId( oidCronograma, nbIdParaSerPesquisado );

				if(tarefaReordenada != null)
				{
					return CsReordenarAutorizacao.NaoAutorizado;
				}
			}

			return CsReordenarAutorizacao.Autorizado;
		}

		/// <summary>
		/// Método responsável realizar as validações necessárias para reordenação e verificar se ainda pode reordenar.
		/// </summary>
		/// <param name="oidCronograma">Oid do cronograma</param>
		/// <param name="indiceLoopReordenacao">índice do loop de reordenação</param>
		/// <param name="nbIdDestino">Id destino da movimentação</param>
		/// <param name="tarefaReferenciaParaReordenacao">Tarefa referencia para reordenacao</param>
		/// <param name="nbIdsAtuaisParaReordenar">Ois Ids atuais que serão reordenados no loop</param>
		/// <param name="tarefasParaReordenar">Lista tarefas que serão reordenadas</param>
		/// <param name="tarefasReordenadas">Lista de tarefas que ja foram reordenadas</param>
		/// <param name="semaforosParaReordenacao">Lista de semáforos para reordenação</param>
		/// <returns>Autorização de pode reordenar ou não</returns>
		private static CsReordenarAutorizacao ValidarReordenacaoQuandoAcaoForMovimentacao( Guid oidCronograma, int indiceLoopReordenacao, short nbIdDestino, ref CronogramaTarefa tarefaReferenciaParaReordenacao, List<short> nbIdsAtuaisParaReordenar, ref List<CronogramaTarefa> tarefasParaReordenar, ref List<CronogramaTarefa> tarefasReordenadas, ref List<SemaforoPorIntervalo> semaforosParaReordenacao, ref short nbIdAntigoDaTarefaMovimentada, ref Guid oidCronogramaTarefaMovimentada, List<short> nbIdsParaReordenar )
		{
			if(indiceLoopReordenacao == 0)
			{
				nbIdAntigoDaTarefaMovimentada = tarefaReferenciaParaReordenacao.NbID;
				oidCronogramaTarefaMovimentada = tarefaReferenciaParaReordenacao.Oid;

				if(semaforosParaReordenacao.Count > 1)
				{
					semaforosParaReordenacao.Last().semaforo.WaitOne();
				}
			}

			if(semaforosParaReordenacao.Count > 2)
			{
				int indiceSemaforoSubsequente = indiceLoopReordenacao + 1;

				if(VerificarExistenciaSemaforoSubsequente( semaforosParaReordenacao, indiceSemaforoSubsequente ))
					semaforosParaReordenacao[indiceSemaforoSubsequente].semaforo.WaitOne();
			}

			if(indiceLoopReordenacao == 0)
			{
				if(CronogramaTarefaBo.VerificarExistenciaTarefaReferencia( ref tarefaReferenciaParaReordenacao ))
				{
					if(CronogramaTarefaBo.VerificarTarefaFoiMovidaParaPosicaoDesejada( tarefaReferenciaParaReordenacao, nbIdDestino ))
					{
						return CsReordenarAutorizacao.NaoAutorizadoPorMovimentacaoInvalida;
					}
				}
				else
				{
					return CsReordenarAutorizacao.NaoAutorizadoPorMovimentacaoInvalida;
				}

				List<short> intervaloInvalido = CronogramaTarefaBo.PesquisarIntervalosInvalidos( semaforosParaReordenacao, tarefaReferenciaParaReordenacao.NbID, nbIdDestino );

				if(CronogramaTarefaBo.ValidarExistenciaIntervaloInvalido( intervaloInvalido ))
				{
					return CsReordenarAutorizacao.NaoAutorizadoPorSemaforosInvalidos;
				}
				else
				{
					intervaloInvalido = CronogramaTarefaBo.PesquisarIntervalosInvalidos( nbIdsParaReordenar, tarefaReferenciaParaReordenacao.NbID, nbIdDestino );

					if(CronogramaTarefaBo.ValidarExistenciaIntervaloInvalido( intervaloInvalido ))
						return CsReordenarAutorizacao.NaoAutorizadoPorSemaforosInvalidos;
				}

				CronogramaTarefaBo.MoverTarefaSelecionadaParaDestino( ref tarefaReferenciaParaReordenacao, nbIdDestino, ref nbIdAntigoDaTarefaMovimentada );

				if(!SemaforoPorIntervalo.VerificarSemaforosSaoIguais( semaforosParaReordenacao[indiceLoopReordenacao], semaforosParaReordenacao.Last() ))
				{
					//Liberar o último semáforo, pois já realizou a movimentação.
					SemaforoPorIntervalo.LiberarSemaforo( semaforosParaReordenacao.Last() );
				}
			}

			if(CronogramaTarefaBo.ValidarExistenciaNbIDsParaReordenar( nbIdsAtuaisParaReordenar ))
			{
				tarefasParaReordenar = CronogramaTarefaDao.ConsultarCronogramaTarefasPorIntervaloNbIDs( oidCronograma, nbIdsAtuaisParaReordenar.Min(), nbIdsAtuaisParaReordenar.Max(), o => o.Tarefa );

				Guid oidTarefaReferencia = tarefaReferenciaParaReordenacao.Oid;

				CronogramaTarefa tarefaSelecionadaNaLista = tarefasParaReordenar.FirstOrDefault( o => o.Oid == oidTarefaReferencia );

				if(tarefaSelecionadaNaLista != null)
				{
					tarefasParaReordenar.Remove( tarefaSelecionadaNaLista );
				}

				if(indiceLoopReordenacao > 0)
				{
					Guid oidCronogramaTarefaJaMovimentada = oidCronogramaTarefaMovimentada;

					CronogramaTarefa tarefaJaMovimentada = tarefasParaReordenar.FirstOrDefault( o => o.Oid == oidCronogramaTarefaJaMovimentada );

					if(tarefaJaMovimentada != null)
						tarefasParaReordenar.Remove( tarefaJaMovimentada );
				}

				if(CronogramaTarefaBo.ValidarExistenciaTarefasParaReordenar( tarefasParaReordenar ))
				{
					if(tarefasParaReordenar.Count > 0)
						for(int i = 0; i < tarefasParaReordenar.Count; i++)
						{
							Guid oidCronogramaTarefaParaPesquisar = tarefasParaReordenar[i].Oid;

							CronogramaTarefa cronogramaTarefaParaRemover = tarefasReordenadas.FirstOrDefault( o => o.Oid == oidCronogramaTarefaParaPesquisar );

							if(cronogramaTarefaParaRemover != null)
								if(tarefasReordenadas.Contains( cronogramaTarefaParaRemover ))
									tarefasParaReordenar.Remove( cronogramaTarefaParaRemover );
						}

					//neste momento a tarefa referencia ( a qual no momento em que iniciou o método era a tarefa que ia ser movida ) muda seus dados e agora é a primeira tarefa que está na lista que será reordenada
					tarefaReferenciaParaReordenacao = tarefasParaReordenar.First();

					if(indiceLoopReordenacao == 0)
					{
						//Verifica se já existe alguma outra tarefa com o NbID antigo da tarefa movida
						//caso exista não precisa mais reordenar, pois já existe outra thread reordenando range parecida.
						short nbIdAntigo = nbIdAntigoDaTarefaMovimentada;
						if(tarefasParaReordenar.FirstOrDefault( o => o.NbID == nbIdAntigo ) != null)
						{
							CronogramaTarefaBo.LiberarPrimeiroSubsequenteEUltimoSemaforoEDecrementarContadoresDosDemaisSemaforos( semaforosParaReordenacao, indiceLoopReordenacao );
							return CsReordenarAutorizacao.NaoAutorizadoPorMovimentacaoInvalida;
						}
					}
				}
			}
			else
			{
				return CsReordenarAutorizacao.NaoAutorizadoMasPodeProsseguirOutraReordenacao;
			}

			return CsReordenarAutorizacao.Autorizado;
		}

		/// <summary>
		/// Método responsável por Preparar a reordenação, ou seja, chamar validações se pode reordenar ou nao, atualizar Ids, e listas de tarefas para reordenar 
		/// </summary>
		/// <param name="oidCronograma">Oid do cronograma</param>
		/// <param name="tarefaReferenciaParaReordenacao">Tarefa referencia para reordenacao</param>
		/// <param name="semaforosParaReordenacao">Lista de semáforos para reordenação</param>
		/// <param name="tarefasParaReordenar">Lista tarefas que serão reordenadas</param>
		/// <param name="tarefasReordenadas">Lista de tarefas que ja foram reordenadas</param>
		/// <param name="nbIdsParaReordenar">todos os Ids que serão reordenados</param>
		/// <param name="indiceLoopReordenacao">índice do loop de reordenação</param>
		/// <param name="motivoReordenacaoEspecifico">Se a reordenação é por motivo de Criação, Exclusão, Movimentação pra baixo ou Movimentação pra cima</param>
		/// <param name="nbIdDestino">Id destino da movimentação</param>
		/// <param name="cronogramaTarefaCriada">Tarefa que foi criada recentemente</param>
		/// <returns>Autorização de pode reordenar ou não</returns>
		private static CsReordenarAutorizacao PrepararReordenacao( Guid oidCronograma, ref CronogramaTarefa tarefaReferenciaParaReordenacao, ref List<SemaforoPorIntervalo> semaforosParaReordenacao, ref List<CronogramaTarefa> tarefasParaReordenar, ref List<CronogramaTarefa> tarefasReordenadas, ref List<short> nbIdsParaReordenar, int indiceLoopReordenacao, out CsMotivoReordenacaoEspecifico motivoReordenacaoEspecifico, short nbIdDestino, CronogramaTarefa cronogramaTarefaCriada, ref short nbIdAntigoTarefaMovimentada, ref Guid oidCronogramaTarefaMovimentada )
		{
			CsMotivoReordenacaoGeral motivoReordenacaoGeral = CronogramaTarefaBo.VerificarMotivoReordenacaoGeral( nbIdDestino, cronogramaTarefaCriada );
			motivoReordenacaoEspecifico = CsMotivoReordenacaoEspecifico.NaoEspecificado;

			List<short> nbIdsAtuaisParaReordenar = CronogramaTarefaBo.ConsultarNbIdsAtuaisParaReordenar( semaforosParaReordenacao, nbIdsParaReordenar, indiceLoopReordenacao );

			if((int)CsMotivoReordenacaoGeral.Criacao == (int)motivoReordenacaoGeral)
			{
				short ultimoNbIdCriado = cronogramaTarefaCriada.NbID;

				CsReordenarAutorizacao reordenarAutorizacao = CronogramaTarefaBo.ValidarReordenacaoQuandoAcaoForCriacao( oidCronograma, cronogramaTarefaCriada, ultimoNbIdCriado, ref semaforosParaReordenacao, ref tarefasParaReordenar, ref nbIdsAtuaisParaReordenar, ref nbIdsParaReordenar, indiceLoopReordenacao );

				if((int)CsReordenarAutorizacao.Autorizado == (int)reordenarAutorizacao)
				{
					motivoReordenacaoEspecifico = CsMotivoReordenacaoEspecifico.CriacaoOuExclusao;

					if(indiceLoopReordenacao == 0)
					{
						CronogramaTarefaBo.RetirarTarefaJaReordenadaDasQueSeraoReordenadas( cronogramaTarefaCriada.Oid, ref tarefasParaReordenar );

						//quando for a primeira vez da reordenação, a tarefa criada servirá como referência para reordenação
						tarefaReferenciaParaReordenacao = cronogramaTarefaCriada;
					}
					else
					{
						//Quando não for a primeira reordenação dos semáforos então retira a tarefa selecionada, pois ela foi reordenada na reordenação anterior a que o método está querendo fazer agora.
						CronogramaTarefaBo.RetirarTarefaJaReordenadaDasQueSeraoReordenadas( tarefaReferenciaParaReordenacao, ref tarefasParaReordenar, indiceLoopReordenacao );
					}

					return CsReordenarAutorizacao.Autorizado;
				}
			}
			else if((int)CsMotivoReordenacaoGeral.Exclusao == (int)motivoReordenacaoGeral)
			{
				CsReordenarAutorizacao reordenarAutorizacao = CronogramaTarefaBo.ValidarReordenacaoQuandoAcaoForExclusao( oidCronograma, ref tarefaReferenciaParaReordenacao, ref semaforosParaReordenacao, ref tarefasParaReordenar, ref nbIdsAtuaisParaReordenar, ref nbIdsParaReordenar, indiceLoopReordenacao );

				if((int)CsReordenarAutorizacao.Autorizado == (int)reordenarAutorizacao)
				{
					motivoReordenacaoEspecifico = CronogramaTarefaBo.VerificarMotivoReordenacaoEspecifico( nbIdDestino );

					CronogramaTarefaBo.RetirarTarefaJaReordenadaDasQueSeraoReordenadas( tarefaReferenciaParaReordenacao.Oid, ref tarefasParaReordenar );

					return CsReordenarAutorizacao.Autorizado;
				}
				else if((int)CsReordenarAutorizacao.NaoAutorizadoMasPodeProsseguirOutraReordenacao == (int)reordenarAutorizacao)
				{
					return CsReordenarAutorizacao.NaoAutorizadoMasPodeProsseguirOutraReordenacao;
				}
			}
			else if((int)CsMotivoReordenacaoGeral.Movimentacao == (int)motivoReordenacaoGeral)
			{
				CsReordenarAutorizacao autorizacao = CronogramaTarefaBo.ValidarReordenacaoQuandoAcaoForMovimentacao( oidCronograma, indiceLoopReordenacao, nbIdDestino, ref tarefaReferenciaParaReordenacao, nbIdsAtuaisParaReordenar, ref tarefasParaReordenar, ref tarefasReordenadas, ref semaforosParaReordenacao, ref nbIdAntigoTarefaMovimentada, ref oidCronogramaTarefaMovimentada, nbIdsParaReordenar );

				if((int)CsReordenarAutorizacao.Autorizado == (int)autorizacao)
				{
					motivoReordenacaoEspecifico = CronogramaTarefaBo.VerificarMotivoReordenacaoEspecifico( nbIdDestino, tarefaReferenciaParaReordenacao, nbIdAntigoTarefaMovimentada );

					return autorizacao;
				}
				else
				{
					return autorizacao;
				}
			}

			return CsReordenarAutorizacao.NaoAutorizado;
		}

		/// <summary>
		/// Método responsável por reiniciar o ciclo de reordenação. (a reordenação irá começar do início)
		/// É utilizado quando um interalo inválido surgi, invalidando toda a reordenação.
		/// </summary>
		/// <param name="semafarosParaReordenacao">Lista de semáforos antigos para reordenar</param>
		/// <param name="tarefaReferenciaParaReordenacao">Tarefa Selecionada para ser movida</param>
		/// <param name="nbIdDestino">NbID para onde tarefa será movida</param>
		/// <param name="dataHoraDaAcao">objeto que guardará a data da atualização das tarefas</param>
		/// <param name="indiceLoopReordenacao">índice para liberar o semáforo</param>
		/// <returns>Lista das tarefas reordenadas corretamente</returns>
		private static List<CronogramaTarefa> ReiniciarReordenacao( CronogramaTarefa tarefaReferenciaParaReordenacao, short nbIdDestino, ref DateTime dataHoraDaAcao, int indiceLoopReordenacao, int controladorReinicioReordenacaoInvalida )
		{
			List<CronogramaTarefa> tarefasParaReordenarValidas = CronogramaTarefaDao.ConsultarTarefasImpactadas( (Guid)tarefaReferenciaParaReordenacao.OidCronograma, tarefaReferenciaParaReordenacao.NbID, nbIdDestino );

			if(controladorReinicioReordenacaoInvalida > 0)
				return new List<CronogramaTarefa>();
			else
				controladorReinicioReordenacaoInvalida += 1;

			return CronogramaTarefaBo.RecalcularPorBloco( tarefaReferenciaParaReordenacao, tarefasParaReordenarValidas, ref dataHoraDaAcao, true, true, nbIdDestino, null, controladorReinicioReordenacaoInvalida );
		}

		/// <summary>
		/// Método responsável por criar novos semáforos e adicioná-los na lista de Semáforos Ordenados caso tenha alguma tarefa que não esteja contemplada pela listaAtualReordenacao.
		/// </summary>
		/// <param name="nbIDsAtuaisParaReordenar">Lista atual dos nbIds que serão reordenados.</param>
		/// <param name="semaforosOrdenados">Lista de semáforos por intevalo que está sendo usado para controlar a reordenação (Utilizando a referência)</param>
		private static void CriarSemaforosParaTarefasNaoContempladas( Guid oidCronograma,
																	  ref List<short> nbIDsAtuaisParaReordenar,
																	  ref List<SemaforoPorIntervalo> semaforosOrdenados,
																	  ref List<short> nbIdsParaReordenar )
		{
			List<SemaforoPorIntervalo> semaforosNovos = new List<SemaforoPorIntervalo>();
			List<SemaforoPorIntervalo> semaforosParaAguardar = new List<SemaforoPorIntervalo>();

			if(nbIDsAtuaisParaReordenar.Count > 0)
			{
				List<CronogramaTarefa> cronogramaTarefasNaoContempladas = CronogramaTarefaDao.ConsultarCronogramaTarefasMaiorQueNbID( oidCronograma, nbIDsAtuaisParaReordenar.Max(), o => o.Cronograma ).OrderBy( o => o.NbID ).ToList();

				if(cronogramaTarefasNaoContempladas.Count > 0)
				{
					Hashtable novosSemaforos = SemaforoSingleton.GetInstancia().ControlarSemaforos( (Guid)cronogramaTarefasNaoContempladas.FirstOrDefault().OidCronograma,
																									cronogramaTarefasNaoContempladas.Select( o => o.NbID ).Min(),
																									cronogramaTarefasNaoContempladas.Select( o => o.NbID ).Max() );

					semaforosNovos = (List<SemaforoPorIntervalo>)novosSemaforos["semaforosNovos"];
					semaforosParaAguardar = (List<SemaforoPorIntervalo>)novosSemaforos["semaforosAguardar"];

					List<SemaforoPorIntervalo> novosSemaforosOrdenados = SemaforoPorIntervalo.OrdenarSemaforos( semaforosNovos, semaforosParaAguardar );

					for(int indice = 0; indice < novosSemaforosOrdenados.Count; indice++)
					{
						semaforosOrdenados.Add( novosSemaforosOrdenados[indice] );
						nbIdsParaReordenar = nbIdsParaReordenar.Union( novosSemaforosOrdenados[indice].intervalo ).ToList();
					}
				}
			}
		}

		/// <summary>
		/// Método responsável por controlar a exclusão das tarefas, utilizando a validação dos cronogramas que estão sendo acessados no momento para determinada ação.
		/// </summary>
		/// <param name="oidTarefas">Lista de oids das tarefas que devem ser excluidas</param>
		/// <param name="oidCronograma">Oid do cronograma</param>
		/// <param name="tarefasImpactadas">Lista que será preenchida com as tarefas reordenadas</param>
		/// <param name="oidsTarefasNaoExcluidas">Lista de oids das tarefas que nao foram excluidas</param>
		/// <param name="dataHoraAcao">Objeto que será prrenchido com a data e hora da atualização</param>
		/// <returns></returns>
		public static List<CronogramaTarefa> ExcluirCronogramaTarefas( List<Guid> oidTarefas, Guid oidCronograma, ref List<CronogramaTarefa> tarefasImpactadas, ref List<Guid> oidsTarefasNaoExcluidas, ref DateTime dataHoraAcao )
		{
			ControlarCriacaoSemaforoParaAcoesDasTarefas( oidCronograma, controleDeAcessosAoExcluir, semaforoValidacaoCronogramaAoExcluir );

			return CronogramaTarefaBo.ExcluirTarefas( oidTarefas, oidCronograma, ref tarefasImpactadas, ref oidsTarefasNaoExcluidas, ref dataHoraAcao );
		}

		/// <summary>
		/// Método responsável por controlar a criação das tarefas, utilizando a validação dos cronogramas que estão sendo acessados no momento para determinada ação.
		/// </summary>
		/// <param name="login"> Login do usuário</param>
		/// <param name="oidCronograma">ID Cronograma</param>
		/// <param name="txDescricaoTarefa">Descrição da tarefa</param>
		/// <param name="txObservacaoTarefa">Observação da tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento da tarefa</param>
		/// <param name="responsaveis">responsáveis da tarefa</param>
		/// <param name="estimativaInicial">Estimativa inicial da tarefa</param>
		/// <param name="dtInicio">Data de início da tarefa</param>
		/// <param name="oidTarefaSelecionada">ID da tarefa que estava selecionada antes de uma nova tarefa ser criada</param>
		/// <returns>Objeto Tarefa Criada</returns>
		public static CronogramaTarefa CriarCronogramaTarefa( Guid oidCronograma, string txDescricaoTarefa, SituacaoPlanejamento situacaoPlanejamento, DateTime dtInicio, string responsaveis, string login, out List<CronogramaTarefa> tarefasImpactadas, ref DateTime dataHoraAcao, string txObservacaoTarefa = "", Int16 estimativaInicial = 0, short nbIDReferencia = 0 )
		{
			ControlarCriacaoSemaforoParaAcoesDasTarefas( oidCronograma, ControleDeAcessosAoCriar, semaforoValidacaoCronogramaAoCriar );
			return CronogramaTarefaBo.IncluirTarefa( oidCronograma, txDescricaoTarefa, situacaoPlanejamento, dtInicio, responsaveis, login, out tarefasImpactadas, ref dataHoraAcao, txObservacaoTarefa, estimativaInicial, nbIDReferencia );
		}


		/// <summary>
		/// Método responsável por controlar a adição, do cronograma e seu semáforo de controle para uma determinada ação, ao dicionário de controleAcesso específico.
		/// </summary>
		/// <param name="oidCronograma">Oid do cronograma</param>
		/// <param name="dicControleAcessos">Dicionário de controle de acesso dos cronogramas para cada ação</param>
		/// <param name="semaforoValidacaoDicionario">semaforo de validação para o dicionário específico</param>
		private static void ControlarCriacaoSemaforoParaAcoesDasTarefas( Guid oidCronograma,
																		 Dictionary<Guid, SemaforoControleAcesso> dicControleAcessos,
																		 ReaderWriterLockSlim semaforoValidacaoDicionario )
		{
			if(ValidarExistenciaCronogramaControleAcesso( oidCronograma, semaforoValidacaoDicionario, dicControleAcessos ))
			{
				SemaforoControleAcesso semaforoControleAcesso = dicControleAcessos[oidCronograma];

				CronogramaTarefaBo.IncrementarContadorControleAcesso( semaforoControleAcesso );
			}
			else
			{
				EsperarEscritaCronogramas( semaforoValidacaoDicionario );

				if(!dicControleAcessos.ContainsKey( oidCronograma ))
					dicControleAcessos.Add( oidCronograma, new CronogramaTarefaBo.SemaforoControleAcesso()
					{
						semaforoAcesso = new Semaphore( 1, 1 ),
						semaforoContador = new Semaphore( 1, 1 ),
						contadorAcesso = 1
					} );

				LiberarEscritaCronogramas( semaforoValidacaoDicionario );
			}
		}

		/// <summary>
		/// Método responsável por mover as tarefas impactadas para baixo quando houver uma movimentação de alguma tarefa.
		/// Este método é acionado dentro do RnReordenarTarefas
		/// </summary>
		/// <param name="tarefaReferenciaParaReordenacao">Tarefa que serve de referência para reordenar as próximas</param>
		/// <param name="tarefasParaReordenar">Lista onde serão armazenadas as tarefas impactadas</param>
		/// <returns>Lista contendo as tarefas impactadas</returns>
		private static List<CronogramaTarefa> ReordenarIdsTarefas( CronogramaTarefa tarefaReferenciaParaReordenacao, ref List<CronogramaTarefa> tarefasParaReordenar, CsMotivoReordenacaoEspecifico motivoReordenacaoEspecifico )
		{
			List<CronogramaTarefa> tarefasImpactadas = new List<CronogramaTarefa>();
			Guid oidCronograma = (Guid)tarefaReferenciaParaReordenacao.OidCronograma;

			short novoNbId = tarefaReferenciaParaReordenacao.NbID;

			if((int)CsMotivoReordenacaoEspecifico.MovimentacaoParaBaixo == (int)motivoReordenacaoEspecifico)
			{
				novoNbId = tarefaReferenciaParaReordenacao.NbID;
				novoNbId -= 1;
			}
			else
			{
				novoNbId += 1;
			}

			CronogramaTarefaBo.ValidarNbId( ref novoNbId );

			for(int i = 0; i < tarefasParaReordenar.Count; i++)
			{
				tarefasParaReordenar[i].NbID = novoNbId;

				CronogramaTarefaBo.AtualizarNbId( tarefasParaReordenar[i].Oid, ref novoNbId );

				if(CronogramaTarefaBo.VerificarSeTarefaExiste( CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( tarefasParaReordenar[i].Oid ) ))
					tarefasImpactadas.Add( tarefasParaReordenar[i] );

				novoNbId += 1;
			}

			CronogramaTarefaBo.AtualizarAtributoMaiorNbIdPorCronograma( oidCronograma );

			return tarefasImpactadas;
		}

		/// <summary>
		/// Método responsável por Atualizar e Salvar o novo NbId de uma tarefa.
		/// </summary>
		/// <param name="cronogramaTarefa">Tarefa que será atualizada o nbId</param>
		/// <param name="novoNbId">Novo NbId</param>
		private static void AtualizarNbId( Guid oidCronogramaTarefa, ref short novoNbId )
		{
			try
			{
				CronogramaTarefa cronogramaTarefaParaAtualizar = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( oidCronogramaTarefa, o => o.Tarefa );
				cronogramaTarefaParaAtualizar.NbID = novoNbId;

                using(WexDb contexto = ContextFactoryManager.CriarWexDb())
				{
					if(contexto.CronogramaTarefa.Existe( o => o.Oid == cronogramaTarefaParaAtualizar.Oid ))
					{
						contexto.CronogramaTarefa.Attach( cronogramaTarefaParaAtualizar );
						contexto.Entry( cronogramaTarefaParaAtualizar ).State = EntityState.Modified;
					}

					contexto.SaveChanges();
				}
			}
			catch(Exception)
			{
				CronogramaTarefa cronogramaTarefaParaAtualizar = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( oidCronogramaTarefa, o => o.Tarefa );

				if(cronogramaTarefaParaAtualizar == null)
				{
					novoNbId -= 1;
					CronogramaTarefaBo.ValidarNbId( ref novoNbId );
				}
				else
				{
					cronogramaTarefaParaAtualizar.NbID = novoNbId;

					try
					{
                        using(WexDb contexto = ContextFactoryManager.CriarWexDb())
						{
							if(contexto.CronogramaTarefa.Existe( o => o.Oid == cronogramaTarefaParaAtualizar.Oid ))
							{
								contexto.CronogramaTarefa.Attach( cronogramaTarefaParaAtualizar );
								contexto.Entry( cronogramaTarefaParaAtualizar ).State = EntityState.Modified;
							}

							contexto.SaveChanges();
						}
					}
					catch(Exception)
					{
						try
						{
							using(WexDb contexto = ContextFactoryManager.CriarWexDb())
							{
								if(contexto.CronogramaTarefa.Existe( o => o.Oid == cronogramaTarefaParaAtualizar.Oid ))
								{

									contexto.CronogramaTarefa.Attach( cronogramaTarefaParaAtualizar );
									contexto.Entry<CronogramaTarefa>( cronogramaTarefaParaAtualizar ).Reload();
									cronogramaTarefaParaAtualizar.NbID = novoNbId;
									contexto.CronogramaTarefa.Attach( cronogramaTarefaParaAtualizar );
									contexto.Entry( cronogramaTarefaParaAtualizar ).State = EntityState.Modified;
								}

								contexto.SaveChanges();
							}
						}
						catch(Exception)
						{

							throw;
						}
					}
				}
			}
		}

		/// <summary>
		/// Método responsável por controlar a reordenação e/ou recalculo de datas quando uma tarefa é criada, excluida ou movimentada e outras tarefas são impactadas por ela
		/// </summary>
		/// <param name="tarefaReferenciaParaReordenacao">Tarefa que serve como referência para reordenação e recalculo</param>
		/// <param name="tarefasParaReordenar">Lista de Tarefas que serão usadas na reordenação</param>
		/// <param name="dataHoraAcao">atributo que guarda a hora em que as tarefas foram reordenadas</param>
		/// <param name="reordenarERecalcularDatas">atributo que indica se é pra reordenar e recalcular as datas das tarefas</param>
		/// <param name="recalcularDatas">Atributo que indica se é pra somente recalcular as datas das tarefas</param>
		/// <param name="nbIdDestino">NbId destino, caso seja uma movimentação, ou seja, para qual Id irá a tarefa</param>
		/// <param name="cronogramaTarefaCriado">Caso seja uma criação, deve ter a tarefa criada</param>
		/// <param name="controladorReinicioReordenacaoInvalida">atributo que controla se o reínicio da reordenação (só pode tentar reordenar 2 vezes)</param>
		/// <returns>Lista das tarefas que foram reordenadas</returns>
		public static List<CronogramaTarefa> RecalcularPorBloco( CronogramaTarefa tarefaReferenciaParaReordenacao,
																List<CronogramaTarefa> tarefasParaReordenar,
																ref DateTime dataHoraAcao,
																bool reordenarERecalcularDatas = false,
																bool recalcularDatas = false,
																short nbIdDestino = 0,
																CronogramaTarefa cronogramaTarefaCriado = null,
																int controladorReinicioReordenacaoInvalida = 0 )
		{

			List<CronogramaTarefa> tarefasReordenadas = new List<CronogramaTarefa>();
			List<short> nbIdsParaReordenar = new List<short>();
			Guid oidCronograma = (Guid)tarefaReferenciaParaReordenacao.OidCronograma;
			short menorNbId = 0;
			short maiorNbId = 0;
			int qtdDecrementosContadorAcessoPorCronograma = 0;
			short nbIdAntigoTarefaMovimentada = 0;
			Guid oidCronogramaTarefaMovimentada = new Guid();

			semaforoExclusaoSemaforosNaoUtilizados.WaitOne();

			CronogramaTarefaBo.ControlarIncrementacaoContadorAcessoPorCronograma( oidCronograma );

			if(CronogramaTarefaBo.VerificarContadorThreadsEmExecucao( oidCronograma ))
				SemaforoSingleton.ExcluirSemaforosInativosPorCronograma( oidCronograma, true );

			semaforoExclusaoSemaforosNaoUtilizados.Release();

			CronogramaTarefaBo.ConsultarMenorEMaiorNbIdParaReordenar( tarefasParaReordenar, out menorNbId, out maiorNbId );

			nbIdsParaReordenar = SemaforoPorIntervalo.CriarIntervalo( menorNbId, maiorNbId );

			List<SemaforoPorIntervalo> semaforosParaReordenacao = CronogramaTarefaBo.ConsultarSemaforosParaReordenacao( oidCronograma, menorNbId, maiorNbId );

			for(int indiceLoopReordenacao = 0; indiceLoopReordenacao < semaforosParaReordenacao.Count; indiceLoopReordenacao++)
			{
				semaforosParaReordenacao[indiceLoopReordenacao].semaforo.WaitOne();

				if(reordenarERecalcularDatas)
				{
					CsMotivoReordenacaoEspecifico motivoReordenacaoEspecifico = CsMotivoReordenacaoEspecifico.NaoEspecificado;

					CsReordenarAutorizacao autorizacao = CronogramaTarefaBo.PrepararReordenacao( oidCronograma, ref tarefaReferenciaParaReordenacao, ref semaforosParaReordenacao, ref tarefasParaReordenar, ref tarefasReordenadas, ref nbIdsParaReordenar, indiceLoopReordenacao, out motivoReordenacaoEspecifico, nbIdDestino, cronogramaTarefaCriado, ref nbIdAntigoTarefaMovimentada, ref oidCronogramaTarefaMovimentada );

					if((int)CsReordenarAutorizacao.Autorizado == (int)autorizacao)
					{
						List<CronogramaTarefa> tarefasImpactadas = ReordenarIdsTarefas( tarefaReferenciaParaReordenacao, ref tarefasParaReordenar, motivoReordenacaoEspecifico );
						tarefasReordenadas = CronogramaTarefaBo.ValidarTarefasReordenadas( tarefasImpactadas, tarefasReordenadas );

						CsReordenarAutorizacao autorizaoAposUmaReordenacao = CronogramaTarefaBo.ConsultarNovaTarefaReferenciaParaReordenacao( ref tarefasReordenadas, ref tarefaReferenciaParaReordenacao );

						if((int)CsMotivoReordenacaoEspecifico.MovimentacaoParaBaixo == (int)motivoReordenacaoEspecifico || (int)CsMotivoReordenacaoEspecifico.MovimentacaoParaCima == (int)motivoReordenacaoEspecifico)
						{
							if(semaforosParaReordenacao.Count > 2)
							{
								int indiceSemaforoSubsequente = indiceLoopReordenacao + 1;

								if(VerificarExistenciaSemaforoSubsequente( semaforosParaReordenacao, indiceSemaforoSubsequente ))
								{
									SemaforoPorIntervalo.LiberarSemaforoEDecrementarContadorSemaforoEmEspera( semaforosParaReordenacao[indiceSemaforoSubsequente] );
								}
							}
						}

						if((int)CsReordenarAutorizacao.NaoAutorizado == (int)autorizaoAposUmaReordenacao)
						{
							SemaforoPorIntervalo.LiberarSemaforoEDecrementarContadorSemaforoEmEspera( semaforosParaReordenacao[indiceLoopReordenacao] );
							break;
						}
					}
					else if((int)CsReordenarAutorizacao.NaoAutorizadoPorMovimentacaoInvalida == (int)autorizacao)
					{
						//CronogramaTarefaBo.LiberarPrimeiroSubsequenteEUltimoSemaforoEDecrementarContadoresDosDemaisSemaforos( semaforosParaReordenacao, indiceLoopReordenacao );
						break;
					}
					else if((int)CsReordenarAutorizacao.NaoAutorizadoPorSemaforosInvalidos == (int)autorizacao)
					{
						CronogramaTarefaBo.LiberarPrimeiroSubsequenteEUltimoSemaforoEDecrementarContadoresDosDemaisSemaforos( semaforosParaReordenacao, indiceLoopReordenacao );

						CronogramaTarefaBo.DecrementarContadorAcessoPorCronograma( oidCronograma );

						qtdDecrementosContadorAcessoPorCronograma += 1;

						tarefasReordenadas = CronogramaTarefaBo.ReiniciarReordenacao( tarefaReferenciaParaReordenacao, nbIdDestino, ref dataHoraAcao, indiceLoopReordenacao, controladorReinicioReordenacaoInvalida );

						break;
					}
					else if((int)CsReordenarAutorizacao.NaoAutorizado == (int)autorizacao)
					{
						SemaforoPorIntervalo.LiberarSemaforoEDecrementarContadoresEmEsperaDosDemaisSemaforos( semaforosParaReordenacao[indiceLoopReordenacao], semaforosParaReordenacao, indiceLoopReordenacao );
						break;
					}
				}
				else if(recalcularDatas)
				{
					//RecalcularDatas.
				}

				SemaforoPorIntervalo.LiberarSemaforoEDecrementarContadorSemaforoEmEspera( semaforosParaReordenacao[indiceLoopReordenacao] );
			}

			//Isso influenciara quando tentar excluir os semaforos que nao sao mais válidos
			if(qtdDecrementosContadorAcessoPorCronograma == 0)
				CronogramaTarefaBo.DecrementarContadorAcessoPorCronograma( oidCronograma );

			return tarefasReordenadas;
		}

		#endregion

		#region Utilitários

		/// <summary>
		/// Método responsável por controlar a incrementação do contador
		/// </summary>
		/// <param name="oidCronograma">Oid Cronograma</param>
		public static void ControlarIncrementacaoContadorAcessoPorCronograma( Guid oidCronograma )
		{
			semaforoControladorThreadsPorCronograma.WaitOne();

			if(!threadsPorCronograma.ContainsKey( oidCronograma ))
				threadsPorCronograma.Add( oidCronograma, new ThreadsEmExecucao() { contador = 0, semaforoDoContador = new Semaphore( 1, 1 ) } );

			semaforoControladorThreadsPorCronograma.Release();

			IncrementarContadorAcessoPorCronograma( oidCronograma );
		}

		/// <summary>
		/// Método responsável por incrementar a variável que controla o acesso ao dicionário de cronogramas
		/// </summary>
		public static void IncrementarContadorAcessoPorCronograma( Guid oidCronograma )
		{
			threadsPorCronograma[oidCronograma].semaforoDoContador.WaitOne();

			ThreadsEmExecucao objetoContadorAcesso = threadsPorCronograma[oidCronograma];

			objetoContadorAcesso.contador += 1;

			threadsPorCronograma[oidCronograma] = objetoContadorAcesso;

			threadsPorCronograma[oidCronograma].semaforoDoContador.Release();
		}

		/// <summary>
		/// Método responsável por decrementar a variável que controla o acesso ao dicionário de cronogramas
		/// </summary>
		public static void DecrementarContadorAcessoPorCronograma( Guid oidCronograma )
		{
			threadsPorCronograma[oidCronograma].semaforoDoContador.WaitOne();

			ThreadsEmExecucao objetoContadorAcesso = threadsPorCronograma[oidCronograma];

			objetoContadorAcesso.contador -= 1;

			threadsPorCronograma[oidCronograma] = objetoContadorAcesso;

			threadsPorCronograma[oidCronograma].semaforoDoContador.Release();
		}

		/// <summary>
		/// Método responsável por verificar se o NbID da tarefa criada foi alterado
		/// (caso tenha sido alterado não deve mais realizar a reordenação)
		/// </summary>
		/// <param name="semaforosOrdenados">Lista de semáforos utilizados</param>
		/// <param name="tarefasParaReordenar">Lista de tarefas para reordenar</param>
		/// <param name="cronogramaTarefaCriada">Objeto da tarefa criada</param>
		/// <param name="indice">indice da reordenação</param>
		/// <param name="ultimoNbIDTarefaCriada">último nbID criado para a nova tarefa</param>
		/// <returns>confirmação se foi alterado ou não (caso tenha sido alterado não deve mais realizar a reordenação)</returns>
		private static bool VerificarNbIdTarefaCriadaFoiAlterado( List<CronogramaTarefa> tarefasParaReordenar, Guid oidCronogramaTarefaCriada, short ultimoNbIDTarefaCriada )
		{
			CronogramaTarefa objetoNovo = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( oidCronogramaTarefaCriada );

			if(objetoNovo != null)
				if(ultimoNbIDTarefaCriada != objetoNovo.NbID)
					return true;

			return false;
		}

		/// <summary>
		/// Método responsável por procurar se existe intervalos inválidos na lista de intervalos que serão reordenados.
		/// Obs: Esse cenário ocorre quando antes de uma thread ser executada uma thread anterior já reordenou a tarefas que será reordenada, alterando assim os intervalos que deveriam ser criados.
		/// </summary>
		/// <param name="semaforosOrdenados">Lista de semáforos que serão usados na reordenação</param>
		/// <param name="tarefaSelecionada">CronogramaTarefa que será movida</param>
		/// <param name="nbIDDestino">NbID do destino da tarefa movida</param>
		/// <returns>Lista contendo o intervalo inválido</returns>
		public static List<short> PesquisarIntervalosInvalidos( List<SemaforoPorIntervalo> semaforosOrdenados, short nbIDTarefaSelecionada, short nbIDDestino )
		{
			List<short> intervaloInvalido = new List<short>();
			List<short> intervaloValido = new List<short>();
			List<short> concatenacaoIntervaloSemaforos = new List<short>();

			List<SemaforoPorIntervalo> copiaSemaforosOrdenados = new List<SemaforoPorIntervalo>( semaforosOrdenados );

			for(int indice = 0; indice < copiaSemaforosOrdenados.Count; indice++)
				concatenacaoIntervaloSemaforos = concatenacaoIntervaloSemaforos.Concat( copiaSemaforosOrdenados[indice].intervalo ).ToList();

			intervaloValido = SemaforoPorIntervalo.CriarIntervalo( nbIDTarefaSelecionada, nbIDDestino );

			//verifica se na lista de intervalos atuais (semaforosOrdenados) existe alguma exceção (que não contenha) na lista de intervalos válidos (intervaloValido)
			//se existe alguma exceção, significa que existe intervalos inválidos.
			intervaloInvalido = concatenacaoIntervaloSemaforos.Except( intervaloValido ).ToList();

			return intervaloInvalido;
		}

		/// <summary>
		/// Método responsável por verificar se existem intervalo inválido a partir da lista de NbIdsParaReordenar
		/// </summary>
		/// <param name="nbIdsParaReordenar"></param>
		/// <param name="nbIDTarefaSelecionada"></param>
		/// <param name="nbIDDestino"></param>
		/// <returns></returns>
		public static List<short> PesquisarIntervalosInvalidos( List<short> nbIdsParaReordenar, short nbIDTarefaSelecionada, short nbIDDestino )
		{
			List<short> intervaloInvalido = new List<short>();
			List<short> intervaloValido = new List<short>();

			intervaloValido = SemaforoPorIntervalo.CriarIntervalo( nbIDTarefaSelecionada, nbIDDestino );

			//verifica se na lista de intervalos atuais (semaforosOrdenados) existe alguma exceção (que não contenha) na lista de intervalos válidos (intervaloValido)
			//se existe alguma exceção, significa que existe intervalos inválidos.
			intervaloInvalido = intervaloValido.Except( nbIdsParaReordenar ).ToList();

			return intervaloInvalido;
		}

		/// <summary>
		/// Método responsável por verificar se o último semáforo da lista de semáforos para reordenar pode ser liberado. (Verifica se existe e se é a primeira vez que se tenta liberar o semáforo)
		/// Obs: Utilizado quando ocorrer ação de MOVER tarefas.
		/// </summary>
		/// <param name="semaforosOrdenados">Lista de semáforos que estão na lista de semáforos para reordenar</param>
		/// <param name="indiceAtualLoop">índice do loop atual</param>
		/// <returns>Se pode ou não liberar o semáforo</returns>
		public static bool VerificarLiberacaoUltimoSemaforo( List<SemaforoPorIntervalo> semaforosOrdenados, int indiceAtualLoop )
		{
			return semaforosOrdenados.Count > 1 && indiceAtualLoop == 0;
		}

		/// <summary>
		/// Método responsável por verificar se o semáforo subsequente existe
		/// </summary>
		/// <param name="semaforosOrdenados">Lista de semáforos que estão na lista de semáforos para reordenar</param>
		/// <param name="indiceSemaforoSubsequente">índice do loop</param>
		/// <returns>True (se existe semáforo), False (se não existe)</returns>
		public static bool VerificarExistenciaSemaforoSubsequente( List<SemaforoPorIntervalo> semaforosOrdenados, int indiceSemaforoSubsequente )
		{
			try
			{
				SemaforoPorIntervalo semaforoSubsequente = semaforosOrdenados[indiceSemaforoSubsequente];
				SemaforoPorIntervalo semaforoUltimo = semaforosOrdenados.Last();

				if(semaforoSubsequente != null)
					if(semaforoSubsequente.Oid != semaforoUltimo.Oid)
						return true;
					else
						return false;
				else
					return false;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Método responsável por verificar se tarefa selecionada para mover já foi movida para a posição desejada.
		/// </summary>
		/// <param name="tarefaSelecionada">CronogramaTarefa que se deseja mover</param>
		/// <param name="nbIDDestino">NbID para onde se desejar mover a tarefa</param>
		/// <returns>TRUE se já foi movida para a posição desejada e FALSE se ainda não foi movida para a posição desejada</returns>
		public static bool VerificarTarefaFoiMovidaParaPosicaoDesejada( CronogramaTarefa tarefaSelecionada, short nbIDDestino )
		{
			return tarefaSelecionada.NbID == nbIDDestino;
		}

		/// <summary>
		/// Método responsável por liberar o semáforo indicado
		/// </summary>
		/// <param name="semaforosOrdenados">Lista de semáforos que estão alocados para a reordenação</param>
		/// <param name="indice">indice do loop em que o semáforo se encontra na lista de semáforos</param>
		/// <param name="liberarUltimoSemaforoDaLista">Parâmetro que indica se quer liberar o último semáforo da lista (Utilizado na reordenação quando ocorrer ação de MOVER tarefas)</param>
		public static void LiberarSemaforo( List<SemaforoPorIntervalo> semaforosOrdenados, int indice, bool liberarUltimoSemaforoDaLista = false )
		{
			if(liberarUltimoSemaforoDaLista)
			{
				//release no último semáforo (destino da movimentação).
				if(semaforosOrdenados.Last().emEspera > MENOR_NBID_CRONOGRAMA)
				{
					semaforosOrdenados.Last().semaforo.Release();
					SemaforoPorIntervalo.DecrementarContadorSemaforoEmEspera( semaforosOrdenados.Last() );
				}
			}
			else
			{
				semaforosOrdenados[indice].semaforo.Release();
				SemaforoPorIntervalo.DecrementarContadorSemaforoEmEspera( semaforosOrdenados[indice] );
			}
		}

		/// <summary>
		/// Método responsável por Liberar todos os semáforo que estão em Lock ( Semáforo corrente(atual), semáforo subsequente e último semáforo da lista)
		/// </summary>
		/// <param name="semaforosOrdenados">Lista de semáforos que estão alocados para a reordenação</param>
		/// <param name="indiceSemaforoAtual">índice atual do loop, indice corrente</param>
		/// <param name="indiceSemaforoSubsequente">índice do semáforo subsequente</param>
		/// <param name="liberarUltimoSemaforoDaLista">confirmação para liberar o último semáforo da lista</param>
		public static void LiberarTodosSemaforos( List<SemaforoPorIntervalo> semaforosOrdenados, int indiceSemaforoAtual, int indiceSemaforoSubsequente, bool liberarUltimoSemaforoDaLista = true )
		{
			if(CronogramaTarefaBo.VerificarLiberacaoUltimoSemaforo( semaforosOrdenados, indiceSemaforoAtual ))
				CronogramaTarefaBo.LiberarSemaforo( semaforosOrdenados, 0, liberarUltimoSemaforoDaLista );

			//verifica se o índice existe
			if(indiceSemaforoSubsequente < semaforosOrdenados.Count && indiceSemaforoSubsequente >= 0)
			{
				//Verifica se o semáforo subsequente não é o último semáforo, pois se for não deverá dar waitone pois ele já deu waitone
				//deve ser != 0 pois se for igual a 0 o semáforo atual será liberado no final do método e nao precisará liberar o semáforo subsequente
				if(( semaforosOrdenados[indiceSemaforoSubsequente] != semaforosOrdenados.Last() && indiceSemaforoSubsequente != 0 ))
				{
					if(CronogramaTarefaBo.VerificarExistenciaSemaforoSubsequente( semaforosOrdenados, indiceSemaforoSubsequente ))
						CronogramaTarefaBo.LiberarSemaforo( semaforosOrdenados, indiceSemaforoSubsequente );
				}
			}

			//Libera o semáforo que está lockado atualmente 
			CronogramaTarefaBo.LiberarSemaforo( semaforosOrdenados, indiceSemaforoAtual );
		}

		/// <summary>
		/// Método responsável por incrementar o contador de acesso do objeto SemaforoControleAcesso.
		/// </summary>
		/// <param name="semaforoControleAcesso">Objeto semaforoControleAcesso</param>
		public static void IncrementarContadorControleAcesso( SemaforoControleAcesso semaforoControleAcesso )
		{
			semaforoControleAcesso.semaforoContador.WaitOne();
			semaforoControleAcesso.contadorAcesso += 1;
			semaforoControleAcesso.semaforoContador.Release();
		}

		/// <summary>
		/// Método responsável por decrementar o contador de acesso do objeto SemaforoControleAcesso.
		/// </summary>
		/// <param name="semaforoControleAcesso">Objeto semaforoControleAcesso</param>
		public static void DecrementarContadorControleAcesso( SemaforoControleAcesso semaforoControleAcesso )
		{
			semaforoControleAcesso.semaforoContador.WaitOne();
			semaforoControleAcesso.contadorAcesso -= 1;
			semaforoControleAcesso.semaforoContador.Release();
		}

		/// <summary>
		/// Verificar se já existe aquele cronograma na lista
		/// </summary>
		/// <param name="oidCronograma">Oid do cronograma</param>
		/// <param name="semaforoAcao">Semáforo por acao</param>
		/// <param name="controleAcesso">dicionário que controla os semáforos de cada cronograma</param>
		/// <returns></returns>
		public static bool ValidarExistenciaCronogramaControleAcesso( Guid oidCronograma, ReaderWriterLockSlim semaforoAcao, Dictionary<Guid, SemaforoControleAcesso> controleAcesso )
		{
			bool cronogramaExistente;

			EsperarLeituraCronogramas( semaforoAcao );

			if(controleAcesso.ContainsKey( oidCronograma ))
				cronogramaExistente = true;
			else
				cronogramaExistente = false;

			LiberarLeituraCronogramas( semaforoAcao );

			return cronogramaExistente;
		}

		/// <summary>
		/// Método responsável por solicitar a leitura do dicionário de cronogramas.
		/// </summary>
		public static void EsperarLeituraCronogramas( ReaderWriterLockSlim semaforoAcao )
		{
			semaforoAcao.EnterReadLock();
		}

		/// <summary>
		/// Método responsável por solicitar a escrita do dicionário de cronogramas.
		/// </summary>
		public static void EsperarEscritaCronogramas( ReaderWriterLockSlim semaforoAcao )
		{
			semaforoAcao.EnterWriteLock();
		}

		/// <summary>
		/// Método responsável por liberar a leitura ao dicionário de cronogramas.
		/// </summary>
		public static void LiberarLeituraCronogramas( ReaderWriterLockSlim semaforoAcao )
		{
			semaforoAcao.ExitReadLock();
		}

		/// <summary>
		/// Método responsável por liberar a escrita ao dicionário de cronogramas.
		/// </summary>
		public static void LiberarEscritaCronogramas( ReaderWriterLockSlim semaforoAcao )
		{
			semaforoAcao.ExitWriteLock();
		}

		/// <summary>
		/// Método responsável por verificar os oids que não foram encontrados para excluir para adicionar na lista de oids das tarefas que não foram excluidas.
		/// </summary>
		/// <param name="oidTarefasParaExcluir">Lista de Oids requisitados para excluir</param>
		/// <param name="oidsTarefasNaoExcluidas">Lista de Oids das tarefas que não foram excluidas</param>
		/// <param name="tarefasEncontradasParaExcluir">Lista de objetos cronogramaTarefa encontrados para serem excluídos</param>
		public static void VerificarTarefasJaExcluidas( List<Guid> oidTarefasParaExcluir, ref List<Guid> oidsTarefasNaoExcluidas, List<CronogramaTarefa> tarefasEncontradasParaExcluir )
		{
			List<Guid> oidsEncontradosParaExcluir = new List<Guid>( tarefasEncontradasParaExcluir.Select( o => o.Oid ) );

			List<Guid> oidsJaExcluidos = oidTarefasParaExcluir.Except( oidsEncontradosParaExcluir ).ToList();

			for(int indice = 0; indice < oidsJaExcluidos.Count; indice++)
				oidsTarefasNaoExcluidas.Add( oidsJaExcluidos[indice] );
		}

		/// <summary>
		/// Método responsável por verificar se a tarefa foi excluída ou não.
		/// Caso tenha sido, adiciona na sua respectiva lista.
		/// </summary>
		/// <param name="oidsTarefasNaoExcluidas">Lista de oids das tarefas nao excluidas.</param>
		/// <param name="tarefasParaExcluir">Lista das tarefas que foram para exclusão.</param>
		/// <param name="tarefasExcluidas">Lista das tarefas que foram excluídas</param>
		/// <param name="indiceExclusao">indíce da localização da tarefa na lista de tarefas que foram para exclusão</param>
		public static void VerificarTarefasExcluidasETarefasNaoExcluidas( ref List<Guid> oidsTarefasNaoExcluidas, List<CronogramaTarefa> tarefasParaExcluir, ref List<CronogramaTarefa> tarefasExcluidas, int indiceExclusao )
		{
			using(WexDb contexo = ContextFactoryManager.CriarWexDb())
			{
				CronogramaTarefa cronogramaTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( tarefasParaExcluir[indiceExclusao].Oid, o => o.Tarefa );

				if(cronogramaTarefa == null)
					tarefasExcluidas.Add( tarefasParaExcluir[indiceExclusao] );
				else
					oidsTarefasNaoExcluidas.Add( tarefasParaExcluir[indiceExclusao].Oid );
			}
		}

		/// <summary>
		/// Método responsável por validar e avaliar qual será a tarefa que servirá como referência para a reordenação.
		/// </summary>
		/// <param name="cronograma">Objeto Cronograma</param>
		/// <param name="menorIdExcluido">Menor id excluido das tarefas</param>
		/// <returns>Objeto CronogramaTarefa tarefaReferencia para a reordenação</returns>
		public static CronogramaTarefa VerificarTarefaReferenciaParaReordenacao( Guid oidCronograma, short menorIdExcluido )
		{
			CronogramaTarefa tarefaReferenciaParaReordenacao = null;

			//caso a tarefa 1 seja excluida
			if(menorIdExcluido == 1)
			{
				tarefaReferenciaParaReordenacao = new CronogramaTarefa();
				tarefaReferenciaParaReordenacao.OidCronograma = oidCronograma;
				tarefaReferenciaParaReordenacao.NbID = 0;
			}
			else
			{
				while(tarefaReferenciaParaReordenacao == null)
				{
					short IDValido = short.Parse( ( (int)menorIdExcluido - 1 ).ToString() );

					CronogramaTarefa cronogramaTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorNbId( oidCronograma, IDValido, o => o.Cronograma, o => o.Tarefa );

					if(cronogramaTarefa != null)
						tarefaReferenciaParaReordenacao = cronogramaTarefa;
				}
			}

			return tarefaReferenciaParaReordenacao;
		}

		/// <summary>
		/// Método responsável por atualizar o NbID das tarefas.
		/// </summary>
		/// <param name="tarefasParaAtualizar">Lista contendo as tarefas para atualizar</param>
		/// <param name="dataHoraAcao"> data da atualização das tarefas</param>      
		/// <returns>Lista de tarefas reordenadas com os nbIDs atualizados.</returns>
		public static List<CronogramaTarefa> AtualizarNbIDTarefasReordenadas( List<CronogramaTarefa> tarefasParaAtualizar, ref DateTime dataHoraAcao )
		{
			using(WexDb contexto = ContextFactoryManager.CriarWexDb())
			{
				List<CronogramaTarefa> cronogramaTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOid( tarefasParaAtualizar.Select( o => Guid.Parse( o.Oid.ToString() ) ).ToList() );

				dataHoraAcao = DateTime.Now;

				for(int i = 0; i < cronogramaTarefas.Count; i++)
					if(tarefasParaAtualizar.Contains( cronogramaTarefas[i] ))
						tarefasParaAtualizar.ElementAt( tarefasParaAtualizar.IndexOf( cronogramaTarefas[i] ) ).NbID = cronogramaTarefas[i].NbID;

				return tarefasParaAtualizar;
			}
		}

		/// <summary>
		/// Método responsável por atualizar o NbID das tarefas e de uma tarefa específica.
		/// </summary>
		/// <param name="tarefasReordenadas">Dicionário contendo as tarefas reordenadas</param>
		/// <param name="cronogramaTarefaCriadaOuMovida">Objeto da nova tarefa criada recentemente</param>
		/// <param name="dataHoraAcao">objeto da data e hora em que foi realizada a atualização</param>
		/// <returns>Lista de tarefas reordenadas com os nbIDs atualizados.</returns>
		public static List<CronogramaTarefa> AtualizarNbIdTarefasReordenadas( List<CronogramaTarefa> tarefasReordenadas, ref CronogramaTarefa cronogramaTarefaCriadaOuMovida, ref DateTime dataHoraAcao )
		{
			Guid oidCronogramaTarefa = cronogramaTarefaCriadaOuMovida.Oid;
			List<CronogramaTarefa> cronogramaTarefas = new List<CronogramaTarefa>();

			//adiciona na lista de tarefas reordenadas para realizar apenas 1 consulta ao banco.
			tarefasReordenadas.Add( cronogramaTarefaCriadaOuMovida );

			cronogramaTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOid( tarefasReordenadas.Select( o => Guid.Parse( o.Oid.ToString() ) ).ToList() );

			dataHoraAcao = DateTime.Now;

			CronogramaTarefa tarefaCriadaOuMovida = cronogramaTarefas.FirstOrDefault( o => o.Oid == oidCronogramaTarefa );

			if(tarefaCriadaOuMovida != null)
				cronogramaTarefaCriadaOuMovida.NbID = tarefaCriadaOuMovida.NbID;

			List<CronogramaTarefa> resultadoTarefaCriadaOuMovida = cronogramaTarefas.Where( o => o.Oid == oidCronogramaTarefa ).ToList();

			if(resultadoTarefaCriadaOuMovida.Count > 0)
				for(int i = 0; i < resultadoTarefaCriadaOuMovida.Count; i++)
					cronogramaTarefas.Remove( resultadoTarefaCriadaOuMovida[i] );

			resultadoTarefaCriadaOuMovida.Clear();
			resultadoTarefaCriadaOuMovida = tarefasReordenadas.Where( o => o.Oid == oidCronogramaTarefa ).ToList();

			if(resultadoTarefaCriadaOuMovida.Count > 0)
				for(int i = 0; i < resultadoTarefaCriadaOuMovida.Count; i++)
					tarefasReordenadas.Remove( resultadoTarefaCriadaOuMovida[i] );

			for(int i = 0; i < cronogramaTarefas.Count; i++)
				if(tarefasReordenadas.Contains( cronogramaTarefas[i] ))
					tarefasReordenadas.ElementAt( tarefasReordenadas.IndexOf( cronogramaTarefas[i] ) ).NbID = cronogramaTarefas[i].NbID;

			return tarefasReordenadas;
		}

		/// <summary>
		/// Método responsável por verificar se um intervalo ainda precisa de reordenação, caso não precise deve liberar todos os outros
		/// É utilizado para verificar se há nbIDs duplicados em determinado intervalo, se não houver significa que não precisa ser reordenado.
		/// </summary>
		/// <param name="semaforosOrdenados">Lista de semáforos utilizados</param>
		/// <param name="tarefasParaReordenar">Lista de tarefas para reordenar</param>
		/// <param name="indicePrincipalReordenacao">indíce do loop principal reordenação</param>
		/// <param name="oidCronograma">Oid do cronograma</param>
		/// <param name="nbIDsAtuaisParaReordenar">Lista atual dos nbids que se deve reordenar no momento</param>
		/// <returns>Confirmação se deve para  a reordenação ou não</returns>
		private static bool VerificarIntervaloPrecisaReordenar( List<SemaforoPorIntervalo> semaforosOrdenados, List<CronogramaTarefa> tarefasParaReordenar )
		{
			List<short> listaOriginalnbIDs = new List<short>( tarefasParaReordenar.Select( o => o.NbID ) );
			List<short> listaSemDuplicacoes = new List<short>( listaOriginalnbIDs.Distinct() );

			/// É utilizado para verificar se há nbIDs duplicados em determinado intervalo, se não houver significa que não precisa ser reordenado.
			if(listaOriginalnbIDs.Count == listaSemDuplicacoes.Count)
				return false;

			return true;
		}

		/// <summary>
		/// Método responsável por validar a tarefa referencia antes de ir para reordenação
		/// </summary>
		/// <param name="tarefaReferenciaParaReordenacao">objeto tarefa referencia para reordenação</param>
		/// <returns>Retorna objeto tarefa referencia</returns>
		private static CronogramaTarefa ValidarTarefaReferenciaParaReordenacao( CronogramaTarefa tarefaReferenciaParaReordenacao )
		{
			if(tarefaReferenciaParaReordenacao.NbID != 0)
			{
				CronogramaTarefa cronogramaTarefaPesquisado = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( tarefaReferenciaParaReordenacao.Oid );

				if(cronogramaTarefaPesquisado == null)
				{
					int contador = 1;
					tarefaReferenciaParaReordenacao.NbID--;

					while(contador > 0)
					{
						if(tarefaReferenciaParaReordenacao.NbID == 0)
							break;

						cronogramaTarefaPesquisado = CronogramaTarefaDao.ConsultarCronogramaTarefaPorNbId( (Guid)tarefaReferenciaParaReordenacao.OidCronograma, tarefaReferenciaParaReordenacao.NbID );

						if(cronogramaTarefaPesquisado != null)
							break;

						tarefaReferenciaParaReordenacao.NbID--;
					}
				}

				if(cronogramaTarefaPesquisado != null)
					tarefaReferenciaParaReordenacao = cronogramaTarefaPesquisado;
			}

			return tarefaReferenciaParaReordenacao;
		}

		/// <summary>
		/// Método responsável por consultar os semáforos que serão utilizados na reordenãção.
		/// </summary>
		/// <param name="Guid">Oid do Cronograma</param>
		/// <param name="minNbID">Menor nbID afetado na reordenação</param>
		/// <param name="maxNbID">Maior nbID afetado na reordenação</param>
		/// <returns>Lista de semáforos que serão usados na reordenação</returns>
		private static List<SemaforoPorIntervalo> ConsultarSemaforosParaReordenacao( Guid oidCronograma, short minNbID, short maxNbID )
		{
			List<SemaforoPorIntervalo> semaforosOrdenados = new List<SemaforoPorIntervalo>();

			Hashtable semaforos = SemaforoSingleton.GetInstancia().ControlarSemaforos( oidCronograma, minNbID, maxNbID );

			semaforosOrdenados = SemaforoPorIntervalo.OrdenarSemaforos( (List<SemaforoPorIntervalo>)semaforos[SEMAFOROS_NOVOS], (List<SemaforoPorIntervalo>)semaforos[SEMAFOROS_PARA_AGUARDAR] );

			return semaforosOrdenados;
		}

		/// <summary>
		/// Método responsável por consultar os nbIds que serão utilizados naquele momento para a reordenação, isso ocorre de acordo com o intervalo do semáforo que está em uso.
		/// </summary>
		/// <param name="semaforosParaReordenacao">Lista de semáforos que serão usados para reordenação</param>
		/// <param name="nbIds">Todos os nbIds que serão usados na reordenação</param>
		/// <param name="indiceLoopReordenacao">índice do loop da reordenação</param>
		/// <returns>Lista de nbIds que serão usados naquele loop da reordenação</returns>
		private static List<short> ConsultarNbIdsAtuaisParaReordenar( List<SemaforoPorIntervalo> semaforosParaReordenacao, List<short> nbIds, int indiceLoopReordenacao )
		{
			List<short> nbIDsAtuaisParaReordenar = new List<short>();
			nbIDsAtuaisParaReordenar = semaforosParaReordenacao[indiceLoopReordenacao].intervalo.Intersect( nbIds ).ToList();
			return nbIDsAtuaisParaReordenar;
		}

		/// <summary>
		/// Método responsável por atualizar o maior nbID atualmente em um cronograma
		/// </summary>
		/// <param name="novaTarefa">CronogramaTarefa</param>
		private static void IncrementarMaiorNbIdCronograma( CronogramaTarefa novaTarefa )
		{
			using(WexDb contexto = ContextFactoryManager.CriarWexDb())
			{
				CronogramaTarefaBo.maiorNbIDPorCronograma[novaTarefa.OidCronograma.ToString()] = Int16.Parse( ( CronogramaTarefaDao.ConsultarMaxNbIDPorCronograma( (Guid)novaTarefa.OidCronograma ) + 1 ).ToString() );
			}
		}

		/// <summary>
		/// Método responsável por liberar o primeiro e último semáforo.
		/// Utilizado quando a reordenação não é mais preciso, pois tarefa não existe ou já está na posição desejada.
		/// </summary>
		/// <param name="semaforosParaReordenacao">Lista de semáforos</param>
		/// <param name="indiceLoopReordenacao">indíce do primeiro semáforo</param>
		private static void LiberarPrimeiroSubsequenteEUltimoSemaforoEDecrementarContadoresDosDemaisSemaforos( List<SemaforoPorIntervalo> semaforosParaReordenacao, int indiceLoopReordenacao )
		{
			//Libera primeiro semáforo
			SemaforoPorIntervalo.LiberarSemaforo( semaforosParaReordenacao[indiceLoopReordenacao] );

			if(semaforosParaReordenacao.Count > 1)
			{
				if(semaforosParaReordenacao.Count > 2)
				{
					int indiceSemaforoSubsequente = indiceLoopReordenacao + 1;

					//verifica se o índice do semáforo subsequente existe
					if(VerificarExistenciaSemaforoSubsequente( semaforosParaReordenacao, indiceSemaforoSubsequente ))
						//Libera semáforo subsequente
						SemaforoPorIntervalo.LiberarSemaforo( semaforosParaReordenacao[indiceSemaforoSubsequente] );
				}

				if(!SemaforoPorIntervalo.VerificarSemaforosSaoIguais( semaforosParaReordenacao[indiceLoopReordenacao], semaforosParaReordenacao.Last() ))
					if(semaforosParaReordenacao.Last().emEspera != 0)
						//Libera último semáforo
						SemaforoPorIntervalo.LiberarSemaforo( semaforosParaReordenacao.Last() );
			}

			SemaforoPorIntervalo.DecrementarContadorSemaforoEmEspera( semaforosParaReordenacao, indiceLoopReordenacao );
		}

		/// <summary>
		/// Método responsável por consultar qual é o maior NbId do Cronograma e atualizar o atributo estático.
		/// </summary>
		/// <param name="oidCronograma">Oid do Cronograma</param>
		private static void AtualizarAtributoMaiorNbIdPorCronograma( Guid oidCronograma )
		{
			CronogramaTarefaBo.maiorNbIDPorCronograma[oidCronograma.ToString()] = CronogramaTarefaDao.ConsultarMaxNbIDPorCronograma( oidCronograma );
		}

		/// <summary>
		/// Método responsável por Retirar tarefaReferencia da lista que será reordenada, pois ela não necessita de reordenação, ela ja foi reordenada ou é fruto de criação ou exclusão.
		/// </summary>
		/// <param name="tarefaReferenciaParaReordenacao">Tarefa referência</param>
		/// <param name="tarefasParaReordenar">lista de tarefas para reordenar</param>
		/// <param name="indicePrincipalReordenacao">índice do loop de reordenacao</param>
		private static void RetirarTarefaJaReordenadaDasQueSeraoReordenadas( CronogramaTarefa tarefaReferenciaParaReordenacao, ref List<CronogramaTarefa> tarefasParaReordenar, int indicePrincipalReordenacao )
		{
			if(indicePrincipalReordenacao > 0)
			{
				CronogramaTarefa tarefaParaRetirar = tarefasParaReordenar.FirstOrDefault( o => o.Oid == tarefaReferenciaParaReordenacao.Oid );

				if(tarefasParaReordenar.Contains( tarefaParaRetirar ))
					tarefasParaReordenar.Remove( tarefaParaRetirar );
			}
		}

		/// <summary>
		/// Método responsável por Retirar tarefaReferencia da lista que será reordenada, pois ela não necessita de reordenação, ela ja foi reordenada ou é fruto de criação ou exclusão.
		/// </summary>
		/// <param name="tarefaReferenciaParaReordenacao">Tarefa referência</param>
		/// <param name="tarefasParaReordenar">lista de tarefas para reordenar</param>
		/// <param name="indicePrincipalReordenacao">índice do loop de reordenacao</param>
		private static void RetirarTarefaJaReordenadaDasQueSeraoReordenadas( Guid oidCronogramaTarefa, ref List<CronogramaTarefa> tarefasParaReordenar )
		{
			CronogramaTarefa tarefaPesquisada = tarefasParaReordenar.FirstOrDefault( o => o.Oid == oidCronogramaTarefa );

			if(tarefaPesquisada != null)
				tarefasParaReordenar.Remove( tarefaPesquisada );
		}

		/// <summary>
		/// Método responsável por verificar qual será a próxima tarefa referência para reordenação
		/// </summary>
		/// <param name="tarefasReordenadas">Lista de tarefas que foram reordenadas</param>
		/// <param name="novaTarefaReferencia">qual será a nova tarefa referência depois de validado</param>
		/// <returns>Autorizacao se pode continuar com a reordenacao ou nao</returns>
		private static CsReordenarAutorizacao ConsultarNovaTarefaReferenciaParaReordenacao( ref List<CronogramaTarefa> tarefasReordenadas, ref CronogramaTarefa novaTarefaReferencia )
		{
			if(tarefasReordenadas.Count > 0)
			{
				novaTarefaReferencia = tarefasReordenadas.OrderBy( o => o.NbID ).ToList().Last();
				CronogramaTarefa pesquisaNovaTarefaReferencia = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefaReferencia.Oid );

				if(pesquisaNovaTarefaReferencia == null)
				{
					//retira da lista de reordenadas, pois ela não existe mais. (Foi excluída)
					tarefasReordenadas.Remove( novaTarefaReferencia );

					int contador = tarefasReordenadas.Count;

					CronogramaTarefaBo.VerificarExistenciaTarefaReferencia( ref novaTarefaReferencia, ref tarefasReordenadas, ref novaTarefaReferencia, ref contador );

					if(novaTarefaReferencia == null)
					{
						return CsReordenarAutorizacao.NaoAutorizado;
					}

					if(tarefasReordenadas.Count == 0)
					{
						return CsReordenarAutorizacao.NaoAutorizado;
					}
				}

				novaTarefaReferencia = pesquisaNovaTarefaReferencia;

				return CsReordenarAutorizacao.Autorizado;
			}
			else
			{
				novaTarefaReferencia = null;
				return CsReordenarAutorizacao.NaoAutorizado;
			}
		}

		#endregion

		#region Validações

		/// <summary>
		/// Método responsável por validar a necessidade de reordenação.
		/// </summary>
		/// <param name="tarefasParaReordenar">Lista contendo as tarefas que serão impactadas</param>
		/// <returns>Confirmação de que é necessário ou não a reordenação</returns>
		public static bool ValidarExistenciaTarefasParaReordenar( List<CronogramaTarefa> tarefasParaReordenar )
		{
			bool reordenar = false;

			if(tarefasParaReordenar != null)
				if(tarefasParaReordenar.Count > 0)
					reordenar = true;

			return reordenar;
		}

		/// <summary>
		/// Método responsável por validar os ID início e final
		/// Caso o final seja menor que o início, o método realiza a troca.
		/// </summary>
		/// <param name="nbIDInicio">nbID do ínicio da range da pesquisa</param>
		/// <param name="nbIDFinal">nbID do final da range da pesquisa</param>
		public static void ValidarValoresNbID( ref short nbIDInicio, ref short nbIDFinal )
		{
			short auxiliar;

			if(nbIDFinal < nbIDInicio)
			{
				auxiliar = nbIDFinal;
				nbIDFinal = nbIDInicio;
				nbIDInicio = auxiliar;
			}
		}

		/// <summary>
		/// Validação para intervalos inválidos, verifica se existe intervalos inválidos
		/// </summary>
		/// <param name="intervaloInvalido">Lista que contém intervalos inválidos</param>
		/// <returns>Confirmação se existe ou não intervalos inválidos</returns>
		private static bool ValidarExistenciaIntervaloInvalido( List<short> intervaloInvalido )
		{
			return intervaloInvalido.Count > 0;
		}

		/// <summary>
		/// Validação para verificar a existência de NbIDs para reordenar.
		/// </summary>
		/// <param name="intervaloInvalido"></param>
		/// <returns></returns>
		private static bool ValidarExistenciaNbIDsParaReordenar( List<short> nbIDsAtuaisParaReordenar )
		{
			return nbIDsAtuaisParaReordenar.Count > 0;
		}

		/// <summary>
		/// Método responsável por validar qual é o motivo específico que ocasionou a reordenação
		/// Motivos: Criação ou Exclusão; Movimentação Para Cima ou Movimentação Para Baixo.
		/// </summary>
		/// <param name="nbIdDestino">nbIdDestino para onde a tarefa movimentada irá</param>
		/// <param name="tarefaReferenciaMovimentacao">Objeto referência para verificar se foi para cima ou para baixo</param>
		/// <returns>Motivo Específico da reordenação</returns>
		private static CsMotivoReordenacaoEspecifico VerificarMotivoReordenacaoEspecifico( short nbIdDestino, CronogramaTarefa tarefaReferenciaMovimentacao = null, short nbIdAntigoDaTarefaMovimentada = 0 )
		{
			if(nbIdDestino == 0 || tarefaReferenciaMovimentacao == null)
				return CsMotivoReordenacaoEspecifico.CriacaoOuExclusao;
			else
				if(nbIdAntigoDaTarefaMovimentada < nbIdDestino)
					return CsMotivoReordenacaoEspecifico.MovimentacaoParaBaixo;
				else
					return CsMotivoReordenacaoEspecifico.MovimentacaoParaCima;
		}

		/// <summary>
		/// Método responsável por validar qual é o motivo geral que ocasionou a reordenação
		///  Motivos: Criação; Exclusão; Movimentação.
		/// </summary>
		/// <param name="nbIdDestino">nbIdDestino para onde a tarefa movimentada irá</param>
		/// <param name="cronogramaTarefaCriado">Objeto CronogramaTarefa criado no banco recentemente</param>
		/// <returns>Motivo Geral da reordenação</returns>
		private static CsMotivoReordenacaoGeral VerificarMotivoReordenacaoGeral( short nbIdDestino, CronogramaTarefa cronogramaTarefaCriado )
		{
			if(nbIdDestino == 0)
				if(cronogramaTarefaCriado == null)
					return CsMotivoReordenacaoGeral.Exclusao;
				else
					return CsMotivoReordenacaoGeral.Criacao;
			else
				return CsMotivoReordenacaoGeral.Movimentacao;
		}

		/// <summary>
		/// Método responsável que valida se tarefa ainda existe.
		/// </summary>
		/// <param name="contexto">Contexto do banco</param>
		/// <param name="cronogramaTarefa">Tarefa a ser verificado</param>
		/// <returns>Confirmação se existe ou não a tarefa</returns>
		private static bool VerificarSeTarefaExiste( CronogramaTarefa cronogramaTarefa )
		{
			if(cronogramaTarefa != null)
				return true;

			return false;
		}

		/// <summary>
		/// Método responsável por verificar se Id não é zero.
		/// </summary>
		/// <param name="novoNbId">Atributo que será atualizado</param>
		private static void ValidarNbId( ref short novoNbId )
		{
			if(novoNbId <= 0)
				novoNbId = 1;
		}

		/// <summary>
		/// Método responsável por validar o nbID que servirá como referência para a nova tarefa.
		/// </summary>
		/// 
		/// <param name="novaTarefa">Objeto da nova tarefa</param>
		/// <param name="nbIDReferencia">NbID referência para a nova tarefa</param>
		/// <param name="cronogramaTarefaValida">Objeto da tarefa que servirá como referência para a nova tarefa</param>
		public static void VerificarExistenciaNbIDValidoParaNovaTarefa( CronogramaTarefa novaTarefa, ref short nbIDReferencia, CronogramaTarefa cronogramaTarefaValida )
		{
			if(cronogramaTarefaValida == null)
			{
				CronogramaTarefa tarefaReferenciaParaMovimentacao = CronogramaTarefaBo.ValidarNbIDReferenciaAtualizado( (Guid)novaTarefa.OidCronograma, nbIDReferencia );
				nbIDReferencia = tarefaReferenciaParaMovimentacao.NbID;
			}
		}

		/// <summary>
		/// Método responsável por buscar o NbID que servirá como referência para movimentação atualizado (pode ser que tenham ocorrido exclusões).
		/// </summary>
		/// <param name="cronograma">Objeto Cronograma</param>
		/// <param name="nbIDReferencia">NbIDReferencia</param>
		/// <returns>Retorna nbIDReferencia correto</returns>
		public static CronogramaTarefa ValidarNbIDReferenciaAtualizado( Guid oidCronograma, short nbIDReferencia )
		{
			int valorPesquisa = nbIDReferencia - 1;

			CronogramaTarefa tarefaReferenciaParaMovimentacao = null;

			while(tarefaReferenciaParaMovimentacao == null)
			{
				tarefaReferenciaParaMovimentacao = CronogramaTarefaDao.ConsultarCronogramaTarefaPorNbId( oidCronograma, short.Parse( valorPesquisa.ToString() ) );

				valorPesquisa--;

				if(valorPesquisa <= 0)
				{
					tarefaReferenciaParaMovimentacao = new CronogramaTarefa() { NbID = 1, OidCronograma = oidCronograma };
					break;
				}
			}
			return tarefaReferenciaParaMovimentacao;
		}

		/// <summary>
		/// Método responsável por pesquisar a tarefa e verificar se ela ainda existe.
		/// </summary>
		/// <param name="oidCronogramaTarefa">Oid do objeto que se deseja verificar a existência</param>
		/// <returns>Confirmação se existe ou não.</returns>
		private static bool VerificarExistenciaTarefaReferencia( ref CronogramaTarefa cronogramaTarefa )
		{
			//atualiza dados da tarefa.
			cronogramaTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( cronogramaTarefa.Oid );

			//Verificar se tarefa existe (pode já ter sido excluída).
			if(cronogramaTarefa == null)
			{
				//atualiza dados da tarefa.
				cronogramaTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( cronogramaTarefa.Oid );

				//Verificar se tarefa existe (pode já ter sido excluída).
				if(cronogramaTarefa == null)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Método responsável por validar a existencia da tarefa referencia para reordenação
		/// </summary>
		/// <param name="tarefaReferenciaParaReordenacao">Tarefa a ser consultada</param>
		/// <param name="tarefasReordenadas">Lista de tarefas reordenadas</param>
		/// <param name="novaTarefaSelecionada">Objeto a ser setado para novaTarefaReferencia</param>
		/// <param name="contador">contador do indice do while</param>
		private static void VerificarExistenciaTarefaReferencia( ref CronogramaTarefa tarefaReferenciaParaReordenacao, ref List<CronogramaTarefa> tarefasReordenadas, ref CronogramaTarefa novaTarefaSelecionada, ref int contador )
		{
			while(contador > 0)
			{
				novaTarefaSelecionada = tarefasReordenadas.OrderBy( o => o.NbID ).ToList().Last();

				tarefaReferenciaParaReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefaSelecionada.Oid );

				if(tarefaReferenciaParaReordenacao != null)
					break;
				else
					tarefasReordenadas.Remove( novaTarefaSelecionada );

				contador--;
			}
		}

		/// <summary>
		/// Método responsável por consultar qual é o menor e o maior nbId que deverá ser reordenado.
		/// </summary>
		/// <param name="tarefasParaReordenar">Lista de tarefas para reordenar</param>
		/// <param name="menorNbId">Menor nbId</param>
		/// <param name="maiorNbId">Maior nbId</param>
		private static void ConsultarMenorEMaiorNbIdParaReordenar( List<CronogramaTarefa> tarefasParaReordenar, out short menorNbId, out short maiorNbId )
		{
			menorNbId = 0;
			maiorNbId = 0;

			if(tarefasParaReordenar.Count > 0)
			{
				menorNbId = tarefasParaReordenar.Select( o => o.NbID ).Min();
				maiorNbId = tarefasParaReordenar.Select( o => o.NbID ).Max();
			}
		}

		/// <summary>
		/// Método responsável por validar se existem mais que 1 thread em execução.
		/// </summary>
		/// <param name="oidCronograma">Oid do cronograma</param>
		private static bool VerificarContadorThreadsEmExecucao( Guid oidCronograma )
		{
			return threadsPorCronograma[oidCronograma].contador <= 1;
		}

		/// <summary>
		/// Método responsável por validar se existem tarefas impactadas, se existir adiciona na lista de tarefas reordenadas
		/// </summary>
		/// <param name="tarefasImpactadas">Lista de tarefas que foram impactadas na reordenacao</param>
		/// <param name="tarefasReordenadas">Lista de tarefas que ja foram reordenadas</param>
		/// <returns>Lista de tarefas que ja foram reordenadas</returns>
		private static List<CronogramaTarefa> ValidarTarefasReordenadas( List<CronogramaTarefa> tarefasImpactadas, List<CronogramaTarefa> tarefasReordenadas )
		{
			if(CronogramaTarefaBo.ValidarSeExisteTarefasImpactadas( tarefasImpactadas ))
			{
				for(int i = 0; i < tarefasImpactadas.Count; i++)
				{
					if(tarefasReordenadas.FirstOrDefault( o => o.Oid == tarefasImpactadas[i].Oid ) == null)
						if(!tarefasImpactadas[i].CsExcluido)
							tarefasReordenadas.Add( tarefasImpactadas[i] );
				}
			}

			return tarefasReordenadas;
		}

		/// <summary>
		/// Método responsável por verificar se existe alguma tarefa que foi impactada na reordenação.
		/// </summary>
		/// <param name="tarefasImpactadas">Lista de tarefas impactadas</param>
		/// <returns>Confirmação se existe ou não tarefas que foram impactadas na reordenação</returns>
		private static bool ValidarSeExisteTarefasImpactadas( List<CronogramaTarefa> tarefasImpactadas )
		{
			return tarefasImpactadas.Count > 0;
		}

        public static List<CronogramaTarefaDto> ConsultarCronogramaTarefasPorOidCronogramaDto(Guid oidCronograma)
        {
            List<CronogramaTarefaDto> CronogramasTarefaDto;
            CronogramasTarefaDto = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronogramaDto(oidCronograma);
            
            return CronogramasTarefaDto;
        }

        public static CronogramaTarefaDto ConsultarCronogramaTarefaPorOidDto(Guid oidCronogramaTarefa)
        {
            var cronogramaTarefaDto = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOidDto(oidCronogramaTarefa);

            return cronogramaTarefaDto;
        }

        public static List<CronogramaTarefaDto> ConsultarCronogramasTarefaPorOidDto(List<Guid> oidCronogramasTarefas)
        {

            var cronogramasTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOidDto(oidCronogramasTarefas);

            return cronogramasTarefas;
        }

        public static TarefasMovidasDto TarefasMovidasDtoFactory(List<CronogramaTarefa> impactadas, DateTime dataHoraAcao, Guid oidCronogramaTarefaMovida, short nbIDAtualizadoTarefaMovida, Guid oidCronograma)
        {
            var TarefaMovidaDto = CronogramaTarefaDao.TarefasMovidasDtoFactory(impactadas, dataHoraAcao,
                                                    oidCronogramaTarefaMovida, nbIDAtualizadoTarefaMovida, oidCronograma);
            return TarefaMovidaDto;
        }

        public static TarefasExcluidasDto TarefasExcluidasDtoFactory(List<CronogramaTarefa> impactadas, List<Guid> oidsNaoExcluidos, List<CronogramaTarefa> excluidas, DateTime dataHoraAcao, Guid oidCronograma)
        {
            var tarefasExcluidasDto = CronogramaTarefaDao.TarefasExcluidasDtoFactory(impactadas, oidsNaoExcluidos, excluidas, dataHoraAcao, oidCronograma);
            return tarefasExcluidasDto;
        }

		#endregion

		#region Factories

		/// <summary>
		/// Responsável por converter um objeto CronogramaTarefa para CronogramaTarefaDto
		/// </summary>
		/// <param name="tarefa">CronogramaTarefa a ser convertido</param>
		/// <returns>CronogramaTarefaDto a ser enviado por Json</returns>
		public static CronogramaTarefaDto DtoFactory( CronogramaTarefa cronogramaTarefa )
		{
			if(cronogramaTarefa == null || cronogramaTarefa.Tarefa == null)
				return null;

			try
			{
				CronogramaTarefaDto cronogramaTarefaDto = new CronogramaTarefaDto()
		   {
			   OidCronogramaTarefa = cronogramaTarefa.Oid,
			   OidCronograma = (Guid)cronogramaTarefa.OidCronograma,
			   NbID = cronogramaTarefa.NbID,
			   OidSituacaoPlanejamentoTarefa = cronogramaTarefa.Tarefa.SituacaoPlanejamento.Oid,
			   OidTarefa = cronogramaTarefa.Tarefa.Oid,
			   CsLinhaBaseSalva = cronogramaTarefa.Tarefa.CsLinhaBaseSalva,
			   DtInicio = RetornarData( cronogramaTarefa.Tarefa.DtInicio ),
			   TxDescricaoColaborador = cronogramaTarefa.Tarefa.TxResponsaveis,
			   TxDescricaoSituacaoPlanejamentoTarefa = cronogramaTarefa.Tarefa.SituacaoPlanejamento.TxDescricao,
			   TxDescricaoTarefa = cronogramaTarefa.Tarefa.TxDescricao,
			   TxObservacaoTarefa = cronogramaTarefa.Tarefa.TxObservacao,
			   DtAtualizadoEm = RetornarData( cronogramaTarefa.Tarefa.DtAtualizadoEm ),
			   TxAtualizadoPor = RetornarNomeColaborador( cronogramaTarefa.Tarefa.AtualizadoPor ),
			   NbEstimativaInicial = cronogramaTarefa.Tarefa.NbEstimativaInicial,
			   NbEstimativaRestante = cronogramaTarefa.Tarefa.NbEstimativaRestante,
			   NbRealizado = cronogramaTarefa.Tarefa.NbRealizado
		   };
				return cronogramaTarefaDto;
			}
			catch(Exception e)
			{
				throw;
			}

		}

		/// <summary>
		/// Método responsável por retornar um dto com a hora que foi consultada a tarefa
		/// </summary>
		/// <param name="dataHoraDaAcao">data e hora da atualização</param>
		/// <returns></returns>
		public static CronogramaTarefaDto DtoFactory( CronogramaTarefa cronogramaTarefa, DateTime dataHoraDaAcao )
		{
			if(cronogramaTarefa == null)
				return null;

			CronogramaTarefaDto tarefaDto = CronogramaTarefaBo.DtoFactory( cronogramaTarefa );
			tarefaDto.DtHoraConsulta = dataHoraDaAcao;
			return tarefaDto;
		}

		#endregion

		/// <summary>
		/// Método responsável por retornar a menor data válida.
		/// </summary>
		/// <param name="data">DataTime data</param>
		/// <returns>Menor data</returns>
		private static DateTime RetornarData( DateTime? data )
		{
			if(data == null || data.Value == null)
				return DateTime.MinValue;
			return (DateTime)data;
		}

		/// <summary>
		/// Método responsável por retornar o nome do colaborador.
		/// </summary>
		/// <param name="colaborador">Objeto Colaborador</param>
		/// <returns></returns>
		private static string RetornarNomeColaborador( Colaborador colaborador )
		{
			if(colaborador == null || colaborador.NomeCompleto == null)
				return string.Empty;
			return colaborador.NomeCompleto;
		}
	}
}
