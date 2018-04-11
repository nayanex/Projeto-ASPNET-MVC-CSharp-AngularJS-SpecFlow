using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.Library.Libs.ActiveDirectory;
using WexProject.Library.Libs.Logger;
using WexProject.Library.Libs.Rede;
using WexProject.Library.Libs.SemaforoPorIntervalo;
using WexProject.MultiAccess.Library.Components;
using WexProject.MultiAccess.Library.Dtos;
using WexProject.MultiAccess.Library.Libs;
using WexProject.Schedule.Library.Domains;
using WexProject.Schedule.Library.Helpers;
using WexProject.Schedule.Library.Libs.Configuracoes;
using WexProject.Schedule.Library.Libs.ControleEdicao;
using WexProject.Schedule.Library.Libs.CrontroleMovimentacao;
using WexProject.Schedule.Library.Libs.GerenciadorComandos;
using WexProject.Schedule.Library.Properties;
using WexProject.Schedule.Library.ServiceUtils;
using WexProject.Schedule.Library.ServiceUtils.Interfaces;
using WexProject.Schedule.Library.Views.Interfaces;
using WexProject.Schedule.Library.Libs.Log;
using WexProject.Library.Libs.Extensions.Log;
using WexProject.Library.Libs.Extensions.Clone;
using WexProject.Library.Libs.Comparacao;
using WexProject.Schedule.Library.Libs.CronogramaConfig;
using WexProject.Library.Libs.DataHora.Extension;

namespace WexProject.Schedule.Library.Presenters
{
	public class CronogramaPresenter :ISituacaoPlanejamentoComboController , IEditorDadosCronograma
	{
		#region Atributos

		public int ContadorEdicoes { get; set; }

		/// <summary>
		/// Constante que representa o campo do nome da coluna.
		/// </summary>
		const string NomeColuna = "OidSituacaoPlanejamentoTarefa";

		/// <summary>
		/// atributo responsável por armazenar a view utilizada
		/// </summary>
		private ICronogramaView cronogramaView;

		/// <summary>
		/// atributo responsável por armazenar a view utilizada
		/// </summary>
		public ICronogramaView CronogramaView
		{
			get { return cronogramaView; }
		}

		/// <summary>
		/// atributo responsável por armazenar o client de conexão com o servidor
		/// </summary>
		protected IWexMultiAccessClient accessClient;

		/// <summary>
		/// atributo responsável por armazenar as tarefas que estão aguardando a autorização
		/// </summary>
		protected Dictionary<Guid, bool> tarefaRespostaAutorizacaoPendente;

		/// <summary>
		/// atributo responsável por armazenar caso a tarefa tenha sido autorizada ou recusada
		/// </summary>
		protected Dictionary<Guid, bool> autorizacaoTarefas;

		/// <summary>
		/// atributo responsável por armazenar a instancia do cliente do servico
		/// </summary>
		private static IPlanejamentoServiceUtil servicoPlanejamento;

		/// <summary>
		/// atributo responsável por armazenar a instancia do utilitário para controle do GeralService
		/// </summary>
		private static IGeralServiceUtil servicoGeral;

		/// <summary>
		/// Responsável por efetuar a leitura das configurações do cronograma
		/// </summary>
		private static CronogramaConfigBase _cronogramaConfig;

		/// <summary>
		/// Responsável por retornar 
		/// </summary>
		public static CronogramaConfigBase CronogramaConfig
		{
			get
			{
				_cronogramaConfig = _cronogramaConfig ?? new CronogramaConfigImpl();
				return _cronogramaConfig;
			}
			set { _cronogramaConfig = value; }
		}



		/// <summary>
		/// atributo responsável por armazenar as configurações dos usuarios conectados
		///   - Nome Completo
		///   - Cor Selecionada 
		///   - Login do Colaborador
		///   - Oid do Colaborador
		///   - Foto do colaborador
		/// </summary>
		private List<CronogramaColaboradorConfigDto> configUsuariosConectados;

		/// <summary>
		/// Atributo responsável por armazenar o estado de conexão do cronograma
		/// </summary>
		protected bool conectado;

		/// <summary>
		/// Atributo responsável por armazenar as situações planejamento
		/// </summary>
		protected List<SituacaoPlanejamentoDTO> situacoesPlanejamento;

		/// <summary>
		/// Atributo responsável por armazenar as situações planejamento em estado inativo
		/// </summary>
		protected List<SituacaoPlanejamentoDTO> situacoesPlanejamentoInativas;

		/// <summary>
		/// Guarda o colaborador logado na sessão.
		/// </summary>
		protected ColaboradorDto colaboradorLogado;

		/// <summary>
		/// Responsável por armazenar o ultimo cronograma selecionado pelo colaborador atual
		/// </summary>
		protected CronogramaDto cronogramaSelecionado;

		/// <summary>
		/// Responsável por armazenar todos os cronogramas atuais
		/// </summary>
		protected List<CronogramaDto> cronogramas;

		/// <summary>
		/// Responsável por armazenar todos o colaboradores responsaveis
		/// </summary>
		protected List<ColaboradorDto> colaboradores;

		/// <summary>
		/// Responsável por armzenar a situação planejamento padrão
		/// </summary>
		protected SituacaoPlanejamentoDTO situacaoPlanejamentoPadrao;

		/// <summary>
		/// responsável por armazenar se a tela está aguardando a tarefa ser criada no banco;
		/// </summary>
		protected bool aguardandoCriacaoTarefa;

		/// <summary>
		/// Responsável por gerenciar os comandos de atualização da tela
		/// </summary>
		protected GerenciadorComandos gerenciadorComandos;

		/// <summary>
		/// Responsável por armazenar a ultima tarefa que servirá de referencia para criação da nova tarefa criada
		/// </summary>
		protected CronogramaTarefaGridItem tarefaReferencia;

		/// <summary>
		/// Atributo responsável por armazenar o objeto anteriormente selecionado antes do ser criada uma nova tarefa.
		/// </summary>
		private CronogramaTarefaGridItem ultimaTarefaSelecionada;

		/// <summary>
		/// responsável por armazenar as tarefas que sofreram movimentação
		/// </summary>
		private List<TarefaMovida> tarefasMovidas;

		/// <summary>
		/// responsável por armazenar se o grid está atualmente editando uma tarefa
		/// </summary>
		public bool viewEditandoTarefa;

		/// <summary>
		/// Dicionário que armazena a lista de semáforos que determina ação de movimentação utilizou.
		/// </summary>
		private Dictionary<Guid, List<SemaforoPorIntervalo>> semaforosPorMovimentacao = new Dictionary<Guid , List<SemaforoPorIntervalo>>();


		/// <summary>
		/// Semáforo controlador das validações de movimentações
		/// </summary>
		private static Semaphore semaforo = new Semaphore( 1 , 1 );

		/// <summary>
		/// Atributo responsável por indicar se alguma ação está pendente, ou seja, aguardando retorno do serviço.
		/// </summary>
		private static int contadorAcoesPendentes;

		/// <summary>
		/// Armazena a tarefa que se encontra em edição
		/// </summary>
		private TarefaEditada tarefaEmEdicao;

		/// <summary>
		/// Armazena o locker para o fim da edição
		/// </summary>
		private Semaphore edicaoLocker = new Semaphore( 1 , 1 );

		/// <summary>
		/// Armazenar o filtro situação customizado
		/// </summary>
		private static string filtroSituacaoCustom;

		/// <summary>
		/// Responsável por gerenciar o controle sobre a edição de dados do cronograma
		/// </summary>
		private GerenciadorEdicaoCronograma gerenciadorEdicaoCronograma;

		/// <summary>
		/// Armazenar o controlador das regras de ação sobre o combo de situação planejamento
		/// </summary>
		public SituacaoPlanejamentoRegrasComboUtil situacaoPlanejamentoRegrasCombo;
		#endregion

		#region Propriedades

		/// <summary>
		/// Armazena a tarefa que se encontra em edição
		/// </summary>
		public TarefaEditada TarefaEmEdicao
		{
			get { return tarefaEmEdicao; }
		}

		/// <summary>
		/// Guarda o valor do cronograma selecionado
		/// </summary>
		public CronogramaDto CronogramaSelecionado { get { return cronogramaSelecionado; } }

		/// <summary>
		/// Guarda o colaborador logado na sessão.
		/// </summary>
		public ColaboradorDto ColaboradorLogado { get { return colaboradorLogado; } }

		/// <summary>
		/// Guarda o Login (UserName) do usuário logado na sessão.
		/// </summary>
		private string Login { get; set; }

		/// <summary>
		/// responsável por armazenar o estado da conexão com o manager
		/// </summary>
		public bool Conectado { get { return conectado; } }

		/// <summary>
		/// Propriedade Utilizado no CronogramaView para obtenção da lista de situações planejamento
		/// </summary>
		public List<SituacaoPlanejamentoDTO> SituacoesPlanejamento { get { return situacoesPlanejamento; } }

		/// <summary>
		/// Propriedade Utilizado no CronogramaView para obtenção da lista de situações planejamento em estado inativo
		/// </summary>
		public List<SituacaoPlanejamentoDTO> SituacoesPlanejamentoInativas { get { return situacoesPlanejamentoInativas; } }

		/// <summary>
		/// Responsável por disponibilizar para a view a lista de cronogramas existentes
		/// </summary>
		public virtual List<CronogramaDto> Cronogramas
		{
			get { return cronogramas; }
		}

		/// <summary>
		/// Responsável por disponibilizar para a view a lista de colaboradores responsáveis
		/// </summary>
		public List<ColaboradorDto> Colaboradores
		{
			get { return colaboradores; }
		}

		/// <summary>
		/// responsável por armazenar se a tela está aguardando a tarefa ser criada no banco;
		/// </summary>
		public bool AguardandoCriacaoTarefa
		{
			get { return aguardandoCriacaoTarefa; }
		}

		/// <summary>
		/// Responsável por armazenar a ultima tarefa que servirá de referencia para criação da nova tarefa criada
		/// </summary>
		public CronogramaTarefaGridItem TarefaReferencia
		{
			get { return tarefaReferencia; }
		}

		/// <summary>
		/// Lista de configurações por cronograma dos colaboradores
		/// </summary>
		public List<CronogramaColaboradorConfigDto> ConfigUsuariosConectados
		{
			get { return configUsuariosConectados; }
		}

		/// <summary>
		/// responsável por disponibilizar a view a lista de tarefas que sofreram movimentação
		/// </summary>
		public List<TarefaMovida> TarefasMovidas
		{
			get { return tarefasMovidas; }
		}

		/// <summary>
		/// Disponibilizar o mesmo serviço para outras views
		/// </summary>
		public static IPlanejamentoServiceUtil ServicoPlanejamento
		{
			get { return servicoPlanejamento; }
			set { servicoPlanejamento = value; }
		}

		/// <summary>
		/// Disponibilizar o mesmo serviço para as demais views
		/// </summary>
		public static IGeralServiceUtil ServicoGeral
		{
			get { return servicoGeral; }
			set { servicoGeral = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public GerenciadorEdicaoCronograma GerenciadorEdicaoCronograma
		{
			get { return gerenciadorEdicaoCronograma; }
		}

		public static string FiltroSituacaoPersonalizado
		{
			get { return CronogramaPresenter.filtroSituacaoCustom; }
			set
			{
				CronogramaPresenter.filtroSituacaoCustom = value;
			}
		}

		/// <summary>
		/// Dicionário de teclas de atalho em relação ao situação planejamento
		/// </summary>
		public Dictionary<string , SituacaoPlanejamentoDTO> TeclasAtalhoSituacoesPlanejamento { get; set; }

		#endregion

		/// <summary>
		/// Inicializar presenter e seus atributos
		/// </summary>
		/// <param name="view">CronogramaView a ser gerenciado pelo presenter</param>
		/// /// <param name="client">WexMultiAccessClient que encontra-se conectado ao servidor</param>
		public CronogramaPresenter( ICronogramaView view , IWexMultiAccessClient client )
		{
			cronogramaView = view;
			accessClient = client;
			Inicializar();
		}

		/// <summary>
		/// Responsável por realizar a inicialização dos atributos e propriedades do presenter
		/// </summary>
		public void Inicializar()
		{
			servicoPlanejamento = CarregarPlanejamentoService();
			servicoGeral = CarregarGeralService();
			CarregarDados();
			ConfigurarEventosAcessClient();
			ConfigurarEventosServiceClient();
			InicializarVariaveis();
			InicializarCronograma( false );
		}

		/// <summary>
		/// Responsável por criar uma instancia de acesso ao servico
		/// </summary>
		/// <returns>retornar instancia do serviço</returns>
		protected virtual IPlanejamentoServiceUtil CarregarPlanejamentoService()
		{
			if( ServicoPlanejamento == null )
				return new PlanejamentoServiceUtil();
			else
				return servicoPlanejamento;
		}

		/// <summary>
		/// Responsável por criar uma instancia de acesso ao GeralService
		/// </summary>
		/// <returns>retornar instancia do serviço</returns>
		protected virtual IGeralServiceUtil CarregarGeralService()
		{
			if( ServicoGeral == null )
				return new GeralServiceUtil();
			else
				return servicoGeral;
		}

		/// <summary>
		/// Método para inicializar variaveis do presenter
		/// </summary>
		public void InicializarVariaveis()
		{
			configUsuariosConectados = new List<CronogramaColaboradorConfigDto>();
			cronogramaView.LinhasParaExcluir = new List<Guid>();
			tarefasMovidas = new List<TarefaMovida>();
			tarefaRespostaAutorizacaoPendente = new Dictionary<Guid , bool>();
			autorizacaoTarefas = new Dictionary<Guid , bool>();
			aguardandoCriacaoTarefa = false;
			tarefaReferencia = null;
			aguardandoCriacaoTarefa = false;
			contadorAcoesPendentes = 0;
		}

		private void InicializarPosConexao()
		{
			InicializarVariaveis();
			gerenciadorComandos = new GerenciadorComandos( cronogramaView.TarefasCronograma );
			gerenciadorEdicaoCronograma = new GerenciadorEdicaoCronograma( this );
			situacaoPlanejamentoRegrasCombo = new SituacaoPlanejamentoRegrasComboUtil( situacoesPlanejamento , this , Login );
		}
		/// <summary>
		/// Efetuar Inicializacao do cronograma
		/// </summary>
		public void InicializarCronograma( bool conectar = true )
		{
			cronogramaView.RemoverFocoTarefas();
			CarregarDadosCronograma();
			EscolherCorColaborador();
			CarregarValoresBarraSuperiorCronograma();
			if( conectar )
				ConectarCronograma();
		}

		/// <summary>
		/// Métodos responsável por carregar dados independentes do Cronograma
		/// </summary>
		protected virtual void CarregarDados()
		{
			if( !CarregarColaborador() || !CarregarSituacoesPlanejamento() )
				return;

			colaboradores = ConsultarColaboradoresResponsaveis();
		}

		/// <summary>
		/// Método responsável por efetuar a inicialização dos dados do cronograma
		/// </summary>
		public void CarregarDadosCronograma()
		{
			cronogramaSelecionado = ConsultarUltimoCronogramaSelecionado( Login );
			cronogramas = ConsultarCronogramas();

			if( cronogramaSelecionado != null )
			{
				cronogramaView.ListarCronogramas( cronogramas , cronogramaSelecionado.TxDescricao );
				cronogramaView.AtualizarCronogramaSelecionado( cronogramaSelecionado );
			}
			else
				cronogramaView.ListarCronogramas( cronogramas , String.Empty );
		}

		/// <summary>
		/// Método utilizado para carregar as tarefas
		/// </summary>
		public void CarregarTarefas()
		{
			if( cronogramaSelecionado != null )
			{
				cronogramaView.TarefasCronograma = new BindingList<CronogramaTarefaGridItem>( ListarTarefasCronogramaAtual() );
				gerenciadorComandos = new GerenciadorComandos( cronogramaView.TarefasCronograma );

				if( cronogramaView.TarefasCronograma.Count > 0 )
				{
					DateTime dataHoraAtualizacao = cronogramaView.TarefasCronograma[0].DtHoraConsulta;
					Dictionary<string, short> tarefasAtualizadas = cronogramaView.TarefasCronograma.ToDictionary( o => o.OidCronogramaTarefa.ToString() , o => (short)o.NbID );
					gerenciadorComandos.CriarComandoAtualizarTarefasImpactadas( tarefasAtualizadas , dataHoraAtualizacao );
				}
				ConfigurarEventosGerenciadorComandos();
				cronogramaView.ExibirTarefas();
				cronogramaView.ExibirColaboradoresResponsaveis( colaboradores );
				cronogramaView.ListarSituacoesPlanejamento( situacoesPlanejamento , situacaoPlanejamentoPadrao );
			}
		}

		/// <summary>
		/// Método responsável por configurar os eventos empilhados sobre o gerenciador de comandos
		/// </summary>
		public void ConfigurarEventosGerenciadorComandos()
		{
			gerenciadorComandos.AntesDeIniciarExecucaoComandos += gerenciadorComandos_AntesDeIniciarExecucaoComandos;
			gerenciadorComandos.AoTerminarExecucaoComandos += gerenciadorComandos_AoTerminarExecucaoComandos;
		}

		/// <summary>
		/// Método responsável por executar o comportamento da view antes de ser iniciada a execução dos comandos pendentes de tela
		/// </summary>
		public void gerenciadorComandos_AntesDeIniciarExecucaoComandos()
		{
			cronogramaView.DesabilitarViewTarefas();
		}

		/// <summary>
		/// Método responsável por executar o comportamento da view após ser executados todos os comandos de atualizações pendentes da view
		/// </summary>
		public void gerenciadorComandos_AoTerminarExecucaoComandos()
		{
			if( conectado && accessClient.Conectado )
			{
				RemoverIconesExistentesGridItens();
				cronogramaView.HabilitarViewTarefas();
				cronogramaView.AtualizarView();
			}
		}

		/// <summary>
		/// Método responsável por remover os icones de exclusão das tarefas
		/// </summary>
		public void RemoverIconesExistentesGridItens()
		{
			if( cronogramaView.TarefasCronograma == null )
				return;

			cronogramaView.TarefasCronograma.Where( o => o.Icone != null && !o.EmExclusao ).ToList().ForEach( item => item.RemoverIcone() );
		}

		/// <summary>
		/// Método responsável por efetuar o carregamento do combo de cronograma na view
		/// </summary>
		public void CarregarValoresBarraSuperiorCronograma()
		{
			string descricaoCronograma = null;
			if( cronogramaSelecionado != null )
			{
				descricaoCronograma = cronogramaSelecionado.TxDescricao;
				cronogramaView.NomeCronograma = descricaoCronograma;
				if( situacoesPlanejamento != null && situacoesPlanejamento.Count > 0 )
					cronogramaView.SituacaoCronograma = cronogramaSelecionado.TxDescricaoSituacaoPlanejamento;
			}
			else
			{
				cronogramaView.NomeCronograma = "";
				cronogramaView.SituacaoCronograma = "";
			}
			cronogramaView.ListarCronogramas( cronogramas , descricaoCronograma );
		}

		/// <summary>
		/// Método utilizado para escolher a cor do colaborador atual para o atual cronograma
		/// </summary>
		private void EscolherCorColaborador()
		{
			if( cronogramaSelecionado != null )
				SelecionarCorColaborador( Login , CronogramaSelecionado.Oid.ToString() );
		}

		#region Validações de Inicialização do cronograma

		/// <summary>
		/// Responsável por efetuar o carregamento dos dados do colaborador
		/// </summary>
		public bool CarregarColaborador()
		{
			Login = GetLoginUsuarioWindows();
			colaboradorLogado = ConsultarColaborador( Login );
			if( colaboradorLogado == null || colaboradorLogado.OidColaborador == new Guid() )
			{
				cronogramaView.NotificarAlerta( Resources.Caption_Atencao , Resources.Alerta_ColaboradorInvalido );
				cronogramaView.ExibeConfirmacaoAoFechar = false;
				cronogramaView.Fechar();
				return false;
			}
			return true;
		}

		/// <summary>
		/// Resposável por validar se existem situações planejamento validas para criação de tarefas no cronograma
		/// </summary>
		public bool CarregarSituacoesPlanejamento()
		{
			situacoesPlanejamento = ListarSituacoesPlanejamento();
			situacoesPlanejamentoInativas = ListarSituacoesPlanejamentoInativas();
			situacaoPlanejamentoPadrao = ConsultarSituacaoPlanejamentoPadrao();

			if( ContemSituacoesPlanejamento() )
			{
				cronogramaView.NotificarAlerta( Resources.Caption_Atencao , Resources.Alerta_NaoExisteSituacaoPlanejamento );
				cronogramaView.ExibeConfirmacaoAoFechar = false;
				cronogramaView.Fechar();
				return false;
			}
			else
			{
				if( situacaoPlanejamentoPadrao == null )
				{
					cronogramaView.NotificarAlerta( Resources.Caption_Atencao , Resources.Alerta_NaoExisteSituacaoPlanejamentoAtiva );
					cronogramaView.ExibeConfirmacaoAoFechar = false;
					cronogramaView.Fechar();
					return false;
				}
			}

			GerarFiltrosSituacaoPlanejamento();
			TeclasAtalhoSituacoesPlanejamento = situacoesPlanejamento.ToDictionary( o => o.TxKeys , o => o );
			return true;
		}

		/// <summary>
		/// Método para validação se há situações planejamento
		/// </summary>
		/// <returns></returns>
		private bool ContemSituacoesPlanejamento()
		{
			return situacoesPlanejamento == null || situacoesPlanejamento.Count == 0;
		}
		#endregion

		#region Métodos e Métodos de eventos do Serviço

		/// <summary>
		/// Método responsável por linkar os métodos da tela aos eventos do serviço assincrono
		/// </summary>
		protected virtual void ConfigurarEventosServiceClient()
		{
			servicoPlanejamento.AoCompletarSolicitacaoCriarNovaTarefa += ExecutarAoCriarNovaTarefa;
			servicoPlanejamento.AoCompletarMovimentacaoTarefa += ExecutarAoCompletarMovimentacaoTarefa;
			servicoPlanejamento.AoCompletarSolicitacaoExclusaoTarefas += ExecutarAoExcluirTarefas;
		}

		/// <summary>
		/// Método responsável por efetuar o comportamento da tela encerrar a criação da tarefa assíncrona no serviço
		/// </summary>
		/// <param name="objeto">Nova tarefa que foi criada pelo servico</param>
		private void ExecutarAoCriarNovaTarefa( object objeto )
		{
			TarefaCriadaDto retornoDadosNovaTarefa = objeto as TarefaCriadaDto;

			if( cronogramaSelecionado != null )
				if( retornoDadosNovaTarefa == null || !retornoDadosNovaTarefa.OidCronograma.Equals( cronogramaSelecionado.Oid ) )
					return;

			DateTime dataHoraAcao = retornoDadosNovaTarefa.dataHoraAcao;

			EsperarLeituraDataSource();

			CronogramaTarefaGridItem tarefa = ConsultarTarefaRecentementeCriadaNoGrid( retornoDadosNovaTarefa );

			LiberarLeituraDataSource();

			if( tarefa == null )
			{
				tarefa = InstanciarTarefaComRetornoServico( retornoDadosNovaTarefa , tarefa );
				InserirTarefaAleatoriaGrid( tarefa );
			}
			else
			{
				tarefa.OidCronogramaTarefa = retornoDadosNovaTarefa.OidCronogramaTarefa;
				tarefa.OidTarefa = retornoDadosNovaTarefa.OidTarefa;
				tarefa.NbEstimativaRestante = retornoDadosNovaTarefa.NbEstimativaRestante;
				tarefa.DtAtualizadoEm = retornoDadosNovaTarefa.DtAtualizadoEm;
				tarefa.TxAtualizadoPor = retornoDadosNovaTarefa.TxAtualizadoPor;
			}
			WexLogger.Debug( string.Format( "A tarefa {0} foi salva e recebeu o oid {1} no retorno do serviço" , tarefa.TxDescricaoTarefa , retornoDadosNovaTarefa.OidCronogramaTarefa ) );

			retornoDadosNovaTarefa.TarefasImpactadas.Add( retornoDadosNovaTarefa.OidCronogramaTarefa.ToString() , retornoDadosNovaTarefa.NbIdTarefa );

			gerenciadorComandos.CriarComandoAtualizarTarefasImpactadas( retornoDadosNovaTarefa.TarefasImpactadas , dataHoraAcao );

			cronogramaView.AtualizarTarefaEmSelecao( tarefa.OidCronogramaTarefa );
			cronogramaView.RetirarOrdenacao();
			cronogramaView.Ordenar();
			cronogramaView.AtualizarView();

			cronogramaView.AtribuirFocoTarefa( tarefa );

			cronogramaView.AtualizarUltimaAcao( string.Format( "Nova tarefa salva com sucesso na posição {0}." , retornoDadosNovaTarefa.NbIdTarefa ) );

			aguardandoCriacaoTarefa = false;
			tarefa.RemoverIcone();
			gerenciadorComandos.ExecutarComandosPendentes();
			accessClient.RnComunicarNovaTarefaCriada( retornoDadosNovaTarefa.OidCronogramaTarefa.ToString() , cronogramaSelecionado.Oid.ToString() , retornoDadosNovaTarefa.TarefasImpactadas , retornoDadosNovaTarefa.dataHoraAcao );
			contadorAcoesPendentes -= 1;

			AtualizarGraficoBurndown();
		}

		private CronogramaTarefaGridItem ConsultarTarefaRecentementeCriadaNoGrid( TarefaCriadaDto retornoDadosNovaTarefa )
		{
			return  cronogramaView.TarefasCronograma.FirstOrDefault( o => o.OidCronogramaTarefa == new Guid() || o.OidCronogramaTarefa == retornoDadosNovaTarefa.OidCronogramaTarefa );
		}

		/// <summary>
		/// Método para atualizar o gráfico de burndown apenas quando ele estiver visivel
		/// </summary>
		public void AtualizarGraficoBurndown()
		{
			if( !cronogramaView.BurndownVisivel )
				return;

			var graficoDto = ServicoPlanejamento.ConsultarDadosGraficoBurndown( cronogramaSelecionado.Oid );
			cronogramaView.AtualizarGraficoBurndown( graficoDto );
		}

		/// <summary>
		/// Método responsável por efetuar o comportamento da tela quando for encerrada a movimentação da tarefa assíncrona no serviço
		/// </summary>
		/// <param name="objeto">Tarefas impactadas pela movimentação</param>
		private void ExecutarAoCompletarMovimentacaoTarefa( object objeto )
		{
			TarefasMovidasDto tarefasMovidasDto = objeto as TarefasMovidasDto;

			if( tarefasMovidas == null || !tarefasMovidasDto.OidCronograma.Equals( cronogramaSelecionado.Oid ) )
			{
				LiberarSemaforosMovimentacao( tarefasMovidasDto.OidCronogramaTarefaMovida );
				return;
			}


			CronogramaTarefaDto tarefaSelecionada = ConsultarCronogramaTarefaPorOidNoDataSourceDaView( tarefasMovidasDto.OidCronogramaTarefaMovida );
			if( tarefaSelecionada == null )
			{
				LiberarSemaforosMovimentacao( tarefasMovidasDto.OidCronogramaTarefaMovida );
				return;
			}

			LiberarSemaforosMovimentacao( tarefaSelecionada.OidCronogramaTarefa );

			Int16 nbIDAntigo = (Int16)tarefaSelecionada.NbID;
			TarefasImpactadasDebugUtil.ExibirLogTarefaMovida( tarefasMovidasDto , new List<CronogramaTarefaGridItem>( cronogramaView.TarefasCronograma ) , "Mover (Retorno Serviço)" );

			TarefaMovida tarefa = gerenciadorComandos.CriarComandoMovimentarTarefa( tarefasMovidasDto.OidCronogramaTarefaMovida , 0 , tarefasMovidasDto.NbIDTarefaMovida , tarefasMovidasDto.TarefasImpactadas , tarefasMovidasDto.DataHoraAcao );
			tarefasMovidas.Add( tarefa );
			gerenciadorComandos.ExecutarComandosPendentes();
			cronogramaView.AtualizarView();

			accessClient.RnComunicarMovimentacaoTarefa( nbIDAntigo , (Int16)tarefasMovidasDto.NbIDTarefaMovida , tarefasMovidasDto.OidCronogramaTarefaMovida.ToString() , tarefasMovidasDto.TarefasImpactadas , tarefasMovidasDto.DataHoraAcao );
			contadorAcoesPendentes -= 1;
		}



		/// <summary>
		/// Método responsável por solicitar as exclusões das tarefas na view e solicitar comunicação do fim da exclusão das tarefas.
		/// </summary>
		/// <param name="objeto">Mensagem recebida do client, vinda do manager</param>
		private void ExecutarAoExcluirTarefas( object objeto )
		{
			TarefasExcluidasDto tarefasExcluidasDto = objeto as TarefasExcluidasDto;
			if( tarefasExcluidasDto == null || !tarefasExcluidasDto.OidCronograma.Equals( cronogramaSelecionado.Oid ) )
				return;

			if( CronogramaSelecionado.Oid != tarefasExcluidasDto.OidCronograma )
				return;

			string[] tarefasExcluidas = null;
			string[] tarefasNaoExcluidas = null;

			if( tarefasExcluidasDto.TarefasNaoExcluidas.Count > 0 )
			{
				RemoverMarcacaoLinhasEmExclusao( tarefasExcluidasDto.TarefasNaoExcluidas );
				tarefasNaoExcluidas = tarefasExcluidasDto.TarefasNaoExcluidas.Select( o => o.ToString() ).ToArray();
			}

			if( tarefasExcluidasDto.TarefasExcluidas.Count > 0 )
			{
				RemoverMarcacaoLinhasEmExclusao( tarefasExcluidasDto.TarefasExcluidas );
				tarefasExcluidas = tarefasExcluidasDto.TarefasExcluidas.Select( o => o.ToString() ).ToArray();
				gerenciadorComandos.CriarComandoRemoverTarefas( tarefasExcluidasDto.TarefasExcluidas , tarefasExcluidasDto.TarefasImpactadas , tarefasExcluidasDto.DataHoraAcao );
				gerenciadorComandos.PodeExecutar = true;
				gerenciadorComandos.ExecutarComandosPendentes();
			}

			if( tarefasNaoExcluidas == null )
				tarefasNaoExcluidas = new string[] { };

			if( tarefasExcluidas == null )
				tarefasExcluidas = new string[] { };

			accessClient.RnComunicarFimExclusaoTarefaConcluida( tarefasExcluidas , tarefasExcluidasDto.TarefasImpactadas , tarefasNaoExcluidas , (DateTime)tarefasExcluidasDto.DataHoraAcao );
			contadorAcoesPendentes -= 1;
			AtualizarGraficoBurndown();
		}

		#endregion

		#region Métodos e Métodos de eventos do MultiAcessClient

		/// <summary>
		/// Método chamado para realizar conexão com o cronograma selecionado
		/// </summary>
		public virtual void ConectarCronograma()
		{
			if( cronogramaSelecionado == null )
				return;

			if( VerificarSeEstaConectadoNoCronogramaAtual() )
				return;
			else if( VerificarSeEstaConectadoEmOutroCronograma())
				accessClient.RnDesconectar();

			accessClient.Login = Login;

			string nomeServidor;
			int portaServidor;

			CronogramaConfig.ConfigurarNomeServidor( out nomeServidor );
			CronogramaConfig.ConfigurarPortaServidor( out portaServidor );

			if( string.IsNullOrEmpty( nomeServidor ) )
				throw new Exception( "No arquivo de configurações não foi encontrada a chave com o nome do servidor" );
			try
			{
				accessClient.EnderecoIp = RedeUtil.GetEnderecoIp( nomeServidor ).ToString();
				accessClient.Porta = portaServidor;
				if( string.IsNullOrEmpty( accessClient.EnderecoIp ) )
					throw new Exception();
			}
			catch( Exception )
			{
				throw new Exception( string.Format( "O servidor {0} não foi encontrado na rede!" , nomeServidor ) );
			}

			accessClient.OidCronograma = CronogramaSelecionado.Oid.ToString();
			InicializarPosConexao();
			accessClient.Conectar();
			cronogramaView.AtualizarVisibilidadeBotoesBarra( true );
		}

		/// <summary>
		/// Método responsável por verificar se o usuário está conectado em outro cronograma que não seja o atual.
		/// </summary>
		/// <returns>Retorna a confirmação se está conectado ou não.</returns>
		private bool VerificarSeEstaConectadoEmOutroCronograma()
		{
			return accessClient.Conectado && accessClient.OidCronograma != null && accessClient.OidCronograma != cronogramaSelecionado.Oid.ToString();
		}

		/// <summary>
		/// Método responsável por verificar se o usuário está conectado ao cronograma atual.
		/// </summary>
		/// <returns>Retorna a confirmação se está conectado ou não.</returns>
		private bool VerificarSeEstaConectadoNoCronogramaAtual()
		{
			return accessClient.Conectado && accessClient.OidCronograma != null && accessClient.OidCronograma == cronogramaSelecionado.Oid.ToString();
		}

		/// <summary>
		/// Método Responsável por associar métodos em eventos do MultiAcessClient
		/// </summary>
		private void ConfigurarEventosAcessClient()
		{
			//Eventos conexão e desconexão de usuários
			accessClient.AoConectarNovoUsuario += accessClient_AoConectarNovoUsuario;
			accessClient.AoSerDesconectado += accessClient_AoSerDesconectado;
			accessClient.AoUsuarioDesconectar += accessClient_AoUsuarioDesconectar;
			accessClient.AoSerAutenticadoComSucesso += accessClient_AoSerAutenticadoComSucesso;
			accessClient.AoServidorDesconectar += accessClient_AoServidorDesconectar;
			accessClient.AoFalharConexaoNoServidor += accessClient_AoFalharConexaoNoServidor;

			//Evento de criação de tarefas
			accessClient.AoSerCriadaNovaTarefa += accessClient_AoSerCriadaNovaTarefa;

			//Eventos de edição de tarefas
			accessClient.AoIniciarEdicaoTarefa += accessClient_AoIniciarEdicaoTarefa;
			accessClient.AoSerAutorizadaEdicaoTarefa += accessClient_AoSerAutorizadaEdicaoTarefa;
			accessClient.AoSerRecusadaEdicaoTarefa += accessClient_AoSerRecusadaEdicaoTarefa;
			accessClient.AoSerFinalizadaEdicaoTarefaPorOutroUsuario += accessClient_AoSerFinalizadaEdicaoTarefaPorOutroUsuario;

			//Eventos de exclusão de tarefas
			accessClient.ExecutarExclusaoTarefa += accessClient_ExecutarExclusaoTarefa;
			accessClient.AoSerExcluidaTarefaPorOutroUsuario += accessClient_AoSerExcluidaTarefaPorOutroUsuario;

			//Eventos de movimentação de tarefas
			accessClient.AoOcorrerMovimentacaoPosicaoTarefa += accessClient_AoOcorrerMovimentacaoPosicaoTarefa;

			//Eventos de edição do nome do cronograma
			accessClient.AoIniciarEdicaoDadosCronograma += accessClient_AoIniciarEdicaoDadosCronograma;
			accessClient.AoSerNotificadoAlteracaoDadosCronograma += accessClient_AoSerNotificadoAlteracaoDadosCronograma;
			accessClient.AoSerRecusadaEdicaoDadosCronograma += accessClient_AoSerRecusadaEdicaoDadosCronograma;
			accessClient.AoSerPermitidaEdicaoDadosCronograma += accessClient_AoSerPermitidaEdicaoDadosCronograma;

			//eventos de log
			accessClient.LogarAoOcorrerException += accessClient_LogarAoOcorrerException;
		}

		/// <summary>
		/// Método responsável por logar.
		/// </summary>
		/// <param name="mensagem"></param>
		public void accessClient_LogarAoOcorrerException( object sender , EventArgs e )
		{
			Exception excessao = sender as Exception;
			WexLogger.Error( "" , excessao );
		}

		/// <summary>
		/// Método responsável pelo comportamento da funcionalidade quando não for permitida a edição do nome do cronograma
		/// </summary>
		/// <param name="mensagem"></param>
		public virtual void accessClient_AoSerRecusadaEdicaoDadosCronograma( MensagemDto mensagem )
		{
			cronogramaView.ForcarFimEdicaoDadosCronograma();
			cronogramaView.NotificarAlerta( "Ação não permitida" , "Edição de dados do cronograma foi recusada!" );
		}
		/// <summary>
		/// Método responsável pelo comportamento da funcionalidade quando for permitida a edição do nome do cronograma
		/// </summary>
		/// <param name="mensagem"></param>
		public virtual void accessClient_AoSerPermitidaEdicaoDadosCronograma( MensagemDto mensagem )
		{
			gerenciadorEdicaoCronograma.PermitirSalvarEdicao();
		}

		/// <summary>
		/// Método responsável por associar o comportamento da view a falha de conexão do cronograma com o manager
		/// </summary>
		/// <param name="oidCronograma">oid do cronograma atual</param>
		/// <param name="login">login do colaborador</param>
		private void accessClient_AoFalharConexaoNoServidor( string oidCronograma , string login )
		{
			cronogramaView.HabilitarBotoes( false );
			cronogramaView.DesabilitarViewTarefas( true );
			cronogramaView.ExecutarAoDesconectar();
			cronogramaView.NotificarErro( Resources.Caption_Erro , Resources.Erro_FalhaConexao );
			cronogramaView.ExibeConfirmacaoAoFechar = false;
			cronogramaView.Fechar();
			conectado = false;
		}

		/// <summary>
		/// Método responsável pelo comportamento da view quando um usuário iniciar a edição do nome do cronograma
		/// </summary>
		/// <param name="mensagem"></param>
		private void accessClient_AoIniciarEdicaoDadosCronograma( MensagemDto mensagem )
		{
			string autorEdicao = mensagem.Propriedades[Constantes.AUTOR_ACAO] as string;
			NotificarEdicaoNomeCronograma( autorEdicao );
		}

		/// <summary>
		/// responsável pela ação ocorrida quando um usuário se conectar
		/// </summary>
		/// <param name="mensagem">MensagemDto do Tipo UsuárioConectado</param>
		/// <param name="login">login</param>
		private void accessClient_AoConectarNovoUsuario( MensagemDto mensagem , string login )
		{
			StringBuilder msg = new StringBuilder();
			string[] usuarios = mensagem.Propriedades[Constantes.USUARIOS] as string[];
			string tituloMensagem;
			if( usuarios.Length < 2 )
				tituloMensagem = "Novo Usuário Conectado:";
			else
				tituloMensagem = "Novos Usuários Conectados:";

			List<CronogramaColaboradorConfigDto> colaboradorConfigs = ListarConfigColaboradores( usuarios.Distinct().ToList() );
			if( colaboradorConfigs != null && colaboradorConfigs.Count > 0 )
			{
				foreach( CronogramaColaboradorConfigDto config in colaboradorConfigs )
				{
					configUsuariosConectados.Add( config );
					msg.Append( config.NomeCompletoColaborador );
					msg.Append( "\n" );
				}
				cronogramaView.AdicionarNovosUsuariosConectados( colaboradorConfigs );
				if( colaboradorConfigs.Count == 1 )
				{
					cronogramaView.NotificarMensagemComFoto( tituloMensagem , msg.ToString() , colaboradorConfigs.First().Foto );
				}
				else
					cronogramaView.NotificarMensagem( tituloMensagem , msg.ToString() );
			}
		}

		/// <summary>
		/// responsável pela ação ocorrida quando um usuário se desconectar
		/// </summary>
		/// <param name="mensagem">MensagemDto do Tipo UsuárioDesconectado</param>
		private void accessClient_AoUsuarioDesconectar( MensagemDto mensagem )
		{
			StringBuilder msg = new StringBuilder();

			string[] usuarios = (string[])mensagem.Propriedades[Constantes.USUARIOS];
			bool forcarAtualizacao = (bool)mensagem.Propriedades[Constantes.FORCAR_ATUALIZACAO];
			
			string tituloMensagem = AtribuirMensagemUsuarioDesconectado( usuarios );

			List<CronogramaColaboradorConfigDto> configs = configUsuariosConectados.Where( o => usuarios.Contains( o.Login ) ).ToList();

			if( configs == null || configs.Count < 1 )
				return;

			foreach( CronogramaColaboradorConfigDto config in configs )
			{
				msg.Append( config.NomeCompletoColaborador );
				msg.Append( "\n" );
				cronogramaView.RemoverCorEdicao( Convert.ToInt32( config.Cor ) );
			}

			configUsuariosConectados = configUsuariosConectados.Except( configs ).ToList();

			cronogramaView.RemoverUsuariosDesconectados( configs.Select( o => o.NomeCompletoColaborador ).ToList() );

			if( configs != null )
				ExibirMensagemUsuarioDesconectado( msg , tituloMensagem , configs );
			
			cronogramaView.AtualizarView();

			if( forcarAtualizacao )
				SolicitarAtualizacaoTarefasNoCronograma( mensagem );
		}

		/// <summary>
		/// Método responsável por solicitar que as tarefas do cronograma sejam atualizadas, deve buscar no serviço as tarefas novamente.
		/// </summary>
		/// <param name="mensagem">Objeto contendo oid do cronograma</param>
		private void SolicitarAtualizacaoTarefasNoCronograma( MensagemDto mensagem )
		{
			if( mensagem.Propriedades.ContainsKey( Constantes.OIDCRONOGRAMA ) && mensagem.Propriedades[Constantes.OIDCRONOGRAMA] != null )
			{
				Guid oidCronograma = Guid.Parse( mensagem.Propriedades[Constantes.OIDCRONOGRAMA] as string );
				ForcarAtualizacaoTarefas( oidCronograma );
			}
		}

		/// <summary>
		/// Método responsável por selecionar qual mensagem será exibida para o usuário no caso de desconexão de outro usuário.
		/// </summary>
		/// <param name="nomesUsuarios">nomes dos usuários que se desconectaram</param>
		/// <param name="tituloMensagem">Mensagem para ser exibida</param>
		/// <param name="configs">lista contendo a config dos usuários que se desconectaram</param>
		private void ExibirMensagemUsuarioDesconectado( StringBuilder nomesUsuarios , string tituloMensagem , List<CronogramaColaboradorConfigDto> configs )
		{
			if( configs.Count == 1 )
				cronogramaView.NotificarMensagemComFoto( tituloMensagem , nomesUsuarios.ToString() , configs.First().Foto );
			else
				cronogramaView.NotificarMensagem( tituloMensagem , nomesUsuarios.ToString() );
		}

		/// <summary>
		/// Método responsável por atribuir qual será a mensagem a ser exibida para usuários desconectados.
		/// </summary>
		/// <param name="usuarios">Array de usuários conectados</param>
		/// <returns></returns>
		private static string AtribuirMensagemUsuarioDesconectado( string[] usuarios )
		{
			if( usuarios.Length < 2 )
				return "Usuário Desconectou-se:";
			else
				return "Usuários Desconectaram-se:";
		}

		/// <summary>
		/// Método responsável pelo comportamento da aplicação quando o cronograma for conectado ao
		/// </summary>
		/// <param name="mensagem"></param>
		private void accessClient_AoSerAutenticadoComSucesso( MensagemDto mensagem )
		{
			conectado = true;

			cronogramaView.ExecutarAoConectar();
			cronogramaView.HabilitarBotoes( true );

			Dictionary<string, string> edicoes = mensagem.Propriedades[Constantes.EDICOES_CRONOGRAMA] as Dictionary<string , string>;

			if( mensagem.Propriedades[Constantes.USUARIOS] == null )
				return;

			List<string> usuariosOnline = ( (string[])mensagem.Propriedades[Constantes.USUARIOS] ).ToList<string>();

			InserirUsuariosOnline( ref usuariosOnline );

			CarregarTarefas();
			SinalizarEdicao( edicoes.ToDictionary( o => o.Key , o => o.Value ) );
			cronogramaView.HabilitarAoConectar();

			if( mensagem.Propriedades.ContainsKey( Constantes.LOGIN_AUTOR_EDICAO_NOME_CRONOGRAMA ) )
			{
				string login = mensagem.Propriedades[Constantes.LOGIN_AUTOR_EDICAO_NOME_CRONOGRAMA] as string;
				NotificarEdicaoNomeCronograma( login );
			}

			AtualizarGraficoBurndown();
		}

		/// <summary>
		/// Método responsável pelo comportamento da aplicação quando o cronograma perder a conexão
		/// </summary>
		/// <param name="mensagem"></param>
		private void accessClient_AoServidorDesconectar( MensagemDto mensagem )
		{
			cronogramaView.NotificarAlerta( Resources.Caption_Atencao , Resources.Alerta_Desconectado );
			cronogramaView.ExecutarAoDesconectar();
			cronogramaView.HabilitarBotoes( false );
			cronogramaView.DesabilitarViewTarefas( true );
			conectado = false;
		}

		/// <summary>
		/// Método responsável pelo comportamento da aplicação quando for alterado o nome do cronograma por outro usuário
		/// </summary>
		public virtual void accessClient_AoSerNotificadoAlteracaoDadosCronograma( MensagemDto mensagem )
		{
			string oidCronograma = mensagem.Propriedades[Constantes.OIDCRONOGRAMA] as string;
			string autorAcao = mensagem.Propriedades[Constantes.AUTOR_ACAO] as string;

			if( !string.IsNullOrEmpty( oidCronograma ) && !string.IsNullOrEmpty( oidCronograma ) )
			{
				CronogramaDto cronograma = servicoPlanejamento.ConsultarUltimoCronogramaSelecionado( Login );

				if( VerificarSeHouveAlteracaoDatasCronograma( cronograma.DtInicio, cronograma.DtFinal ) )
					AtualizarGraficoBurndown();

				CronogramaView.AtualizarCronogramaSelecionado( cronograma );

				CronogramaColaboradorConfigDto usuario = configUsuariosConectados.FirstOrDefault( o => o.Login == autorAcao );

				byte[] fotoUsuario;

				if( usuario == null )
					fotoUsuario = null;
				else
				{
					autorAcao = usuario.NomeCompletoColaborador;
					fotoUsuario = usuario.Foto;
				}

				cronogramaView.NotificarMensagemComFoto( "Modificação" , string.Format( "{0} Alterou dados do cronograma" , autorAcao ) , fotoUsuario );
			}

			cronogramaView.NotificarFimEdicaoDadosCronograma();
		}

		/// <summary>
		/// Método responsável pelo comportarmento da aplicação quando for permitida a a edição de uma tarefa
		/// </summary>
		/// <param name="mensagem">mensagem de autorização de edição da tarefa</param>
		private void accessClient_AoSerAutorizadaEdicaoTarefa( MensagemDto mensagem )
		{
			WexLogger.Info( String.Format( "Recebendo Requisicao: {0} Tarefa Autorizada - OidTarefa: {1} - Autor: {2}" , mensagem.Propriedades[Constantes.ID_REQUISICAO] , mensagem.Propriedades[Constantes.OIDTAREFA] , mensagem.Propriedades[Constantes.AUTOR_ACAO] ) );
			try
			{
				edicaoLocker.WaitOne();
				WexLogger.Info( " Evento autorização edição" );
				string oidCrongramaTarefa = mensagem.Propriedades[Constantes.OIDTAREFA] as string;
				string autor = mensagem.Propriedades[Constantes.AUTOR_ACAO] as string;

				if( autor != Login )
				{
					WexLogger.Error( string.Format( "A mensagem de autorização recebida não pertence a este colaborador.\nColaborador Atual:{0}\nColaborador Autorizado:{1}" , Login , autor ) );
				}
				mensagem.Dump();
				Guid oidTarefa = Guid.Parse( oidCrongramaTarefa );
				CronogramaTarefaGridItem tarefa = ConsultarTarefaPodOidCronogramaTarefaNoDataSource( oidTarefa );
				if( tarefaEmEdicao.Oid.Equals( oidTarefa ) )
				{
					tarefaEmEdicao.RecebeuRespostaSolicitacaoEdicao();
					tarefaEmEdicao.PodeSalvar = true;
				}

				if( tarefa != null )
					WexLogger.Info( string.Format( "{0} teve a edição da tarefa {2} ID:{1} OID:{3}  Autorizada " , Login , tarefa.TxDescricaoTarefa , tarefa.NbID , tarefa.OidCronogramaTarefa ) );

				if( !tarefaEmEdicao.Editando && tarefaEmEdicao.PodeSalvar )
				{
					SalvarEdicaoTarefa( tarefa );
					accessClient.RnComunicarFimEdicaoTarefa( oidTarefa.ToString() );
					RemoverTarefaDeEdicao();
				}
			}
			catch( Exception e )
			{
				WexLogger.Error( String.Format( "Gerou exception autorizacao edicao" ) );
			}
			finally
			{
				edicaoLocker.Release();
			}
		}

		/// <summary>
		/// Método responsável pelo comportamento da aplicação quando for recusada a edição de uma tarefa
		/// </summary>
		/// <param name="mensagem"></param>
		private void accessClient_AoSerRecusadaEdicaoTarefa( MensagemDto mensagem )
		{
			WexLogger.Info( string.Format( "Recebendo Recusa Requisicao: {0} Tarefa Recusada - OidTarefa: {1} - Autor: {2}" , mensagem.Propriedades[Constantes.ID_REQUISICAO] , mensagem.Propriedades[Constantes.OIDTAREFA] , mensagem.Propriedades[Constantes.AUTOR_ACAO] ) );

			edicaoLocker.WaitOne();
			string oidCrongramaTarefa = mensagem.Propriedades[Constantes.OIDTAREFA] as string;
			string autorEditando = mensagem.Propriedades[Constantes.AUTOR_ACAO] as string;
			Guid oidTarefa;

			if( !string.IsNullOrEmpty( oidCrongramaTarefa ) )
			{
				oidTarefa = Guid.Parse( oidCrongramaTarefa );
				cronogramaView.ForcarFimEdicao();
				RetornarValoresAnterioresTarefa( oidTarefa );
				CronogramaTarefaGridItem tarefa = ConsultarTarefaPodOidCronogramaTarefaNoDataSource( oidTarefa );
				if( tarefa != null )
					WexLogger.Info( string.Format( "{0} teve a edição da tarefa {1} ID:{2} OID:{3}  Recusada " , Login , tarefa.TxDescricaoTarefa , tarefa.NbID , tarefa.OidCronogramaTarefa ) );
			}
			string msg;
			if( string.IsNullOrEmpty( autorEditando ) )
				msg = "A edição de tarefa foi recusada pelo servidor!";
			else
				msg = string.Format( "A tarefa já se encontra em edição pelo colaborador {0}" , autorEditando );
			cronogramaView.NotificarMensagem( Resources.Caption_Atencao , msg );
			oidTarefa = Guid.Parse( oidCrongramaTarefa );
			RemoverTarefaDeEdicao( oidTarefa );
			edicaoLocker.Release();
		}

		/// <summary>
		/// Método responsável pelo comportamento da aplicação quando for editada uma tarefa por outro usuário
		/// </summary>
		/// <param name="mensagem"></param>
		private void accessClient_AoSerFinalizadaEdicaoTarefaPorOutroUsuario( MensagemDto mensagem )
		{
			try
			{
				string autorAcao = mensagem.Propriedades[Constantes.AUTOR_ACAO] as string;
				string oidCronogramaTarefa = mensagem.Propriedades[Constantes.OIDTAREFA] as string;

				CronogramaColaboradorConfigDto configTemp;

				if( autorAcao != null )
				{
					CronogramaTarefaGridItem tarefaAtualizada = servicoPlanejamento.ConsultarCronogramaTarefaPorOid( Guid.Parse( oidCronogramaTarefa ) );
					CronogramaTarefaGridItem tarefaAtual = ConsultarTarefaPodOidCronogramaTarefaNoDataSource( tarefaAtualizada.OidCronogramaTarefa );

					if( tarefaAtualizada != null && tarefaAtual != null )
					{
						VerificarEdicoesRelevantes( tarefaAtual , tarefaAtualizada , autorAcao );
						tarefaAtual = TarefaEditada.CopiarValores( tarefaAtualizada , tarefaAtual );
						tarefaAtual.Cor = null;
						CronogramaView.AtualizarTarefaEmSelecao( tarefaAtual.OidCronogramaTarefa );
						AtualizarGraficoBurndown();
					}
				}
			}
			catch( Exception excecao )
			{
				WexLogger.Error( "" , excecao );
			}
		}

		/// <summary>
		/// Método responsável pelo comportamento da aplicação quando algum usuário iniciar a edição de tarefa
		/// </summary>
		/// <param name="mensagem"></param>
		private void accessClient_AoIniciarEdicaoTarefa( MensagemDto mensagem )
		{
			try
			{
				string autorAcao = mensagem.Propriedades[Constantes.AUTOR_ACAO] as string;
				string oidCronogramaTarefa = mensagem.Propriedades[Constantes.OIDTAREFA] as string;
				SinalizarEdicao( autorAcao , oidCronogramaTarefa );
			}
			catch( Exception excecao )
			{
				WexLogger.Error( "" , excecao );
			}
		}

		/// <summary>
		/// Método executando quando o cronograma é desconectado
		/// </summary>
		private void accessClient_AoSerDesconectado()
		{
			cronogramaView.DesabilitarViewTarefas( true );
			cronogramaView.HabilitarBotoes( false );
			cronogramaView.ExecutarAoDesconectar();
			conectado = false;
		}

		/// <summary>
		/// Método responsável por
		/// </summary>
		/// <param name="mensagem"></param>
		private void accessClient_AoSerCriadaNovaTarefa( MensagemDto mensagem )
		{
			Dictionary<string, Int16> tarefasImpactadas = (Dictionary<string , Int16>)mensagem.Propriedades[Constantes.TAREFAS_IMPACTADAS];
			string login = (string)mensagem.Propriedades[Constantes.AUTOR_ACAO];
			string oidTarefa = (string)mensagem.Propriedades[Constantes.OIDTAREFA];
			DateTime dataHoraAcao = (DateTime)mensagem.Propriedades[Constantes.DATAHORA_ACAO];

			CronogramaTarefaGridItem tarefa = servicoPlanejamento.ConsultarCronogramaTarefaPorOid( Guid.Parse( oidTarefa ) );

			if( tarefa == null )
				return;

			CronogramaColaboradorConfigDto usuario = configUsuariosConectados.FirstOrDefault( o => o.Login == login );

			if( usuario != null )
			{
				login = usuario.NomeCompletoColaborador;
				cronogramaView.AtualizarHintColaborador( login , "Criou uma nova tarefa na posição " + tarefa.NbID , tarefa.DtAtualizadoEm );
				cronogramaView.AtualizarUltimaAlteracaoCronograma( string.Format( "{0} criou uma nova tarefa na posição {1}" , login , tarefa.NbID ) );
			}

			gerenciadorComandos.CriarComandoAdicionarNovasTarefas( tarefa , tarefasImpactadas , dataHoraAcao );

			if( tarefa.NbEstimativaInicial > 0 )
				AtualizarGraficoBurndown();
			gerenciadorComandos.ExecutarComandosPendentes();
		}

		/// <summary>
		/// Método responsável pela execução do comportamento quando uma tarefa for permitida sua exclusão.
		/// </summary>
		/// <param name="mensagem"></param>
		private void accessClient_ExecutarExclusaoTarefa( MensagemDto mensagem )
		{
			string[] tarefas = mensagem.Propriedades[Constantes.TAREFAS] as string[];
			string oidCronograma = mensagem.Propriedades[Constantes.OIDCRONOGRAMA] as string;

			if( tarefas != null && tarefas.Length > 0 )
			{
				List<Guid> oidTarefas = new List<Guid>( tarefas.Select( o => Guid.Parse( o ) ) );

				servicoPlanejamento.ExcluirTarefas( oidTarefas , Guid.Parse( oidCronograma ) );
			}

			string[] tarefasNaoPermitidasExclusao = mensagem.Propriedades[Constantes.TAREFAS_NAO_EXCLUIDAS] as string[];
			if( tarefasNaoPermitidasExclusao == null || tarefasNaoPermitidasExclusao.Length < 1 )
				return;
			List<Guid> oidTarefasNaoExcluidas = new List<Guid>( tarefasNaoPermitidasExclusao.Select( o => Guid.Parse( o ) ) );

			string[] nbIdTarefasNaoExcluidas = cronogramaView.TarefasCronograma.Where( o => oidTarefasNaoExcluidas.Contains( o.OidCronogramaTarefa ) ).Select( o => o.NbID.ToString() ).ToArray();

			if( nbIdTarefasNaoExcluidas == null || nbIdTarefasNaoExcluidas.Length == 0 )
				return;

			string msg = null;
			if( nbIdTarefasNaoExcluidas.Length > 1 )
				msg = string.Format( "{0} tarefas não puderam ser excluídas, pois encontram-se em edição ou sendo excluídas por outros colaboradores." , nbIdTarefasNaoExcluidas.Length );
			else
				msg = "A tarefa não pode ser excluída, pois encontra-se em edição ou sendo excluída por outro colaborador.";
			cronogramaView.NotificarMensagem( Resources.Caption_Erro , msg );
			RemoverMarcacaoLinhasEmExclusao( oidTarefasNaoExcluidas );
		}

		/// <summary>
		/// Método responsável pela execução do comportamento quando outro usuário excluir uma ou mais tarefas.
		/// </summary>
		/// <param name="mensagem"></param>
		private void accessClient_AoSerExcluidaTarefaPorOutroUsuario( MensagemDto mensagem )
		{
			string[] tarefas = (string[])mensagem.Propriedades[Constantes.TAREFAS];
			DateTime dataHoraAcao = (DateTime)mensagem.Propriedades[Constantes.DATAHORA_ACAO];

			List<Guid> oidTarefasExcluidas = tarefas.Select( o => Guid.Parse( o ) ).ToList();
			Dictionary<string, Int16> tarefasImpactadas = (Dictionary<string , Int16>)mensagem.Propriedades[Constantes.TAREFAS_IMPACTADAS];
			gerenciadorComandos.CriarComandoRemoverTarefas( oidTarefasExcluidas , tarefasImpactadas , dataHoraAcao );

			AdicionarMarcacaoLinhasEmExclusao( oidTarefasExcluidas );
			if( oidTarefasExcluidas != null && oidTarefasExcluidas.Count > 0 )
			{
				string loginAutor = mensagem.Propriedades[Constantes.AUTOR_ACAO] as string;
				CronogramaColaboradorConfigDto usuario = configUsuariosConectados.FirstOrDefault( o => o.Login == loginAutor );
				if( usuario != null )
				{
					string msg = AtribuirMensagemExclusaoTarefas( oidTarefasExcluidas );

					cronogramaView.AtualizarHintColaborador( usuario.NomeCompletoColaborador , msg , dataHoraAcao );
					cronogramaView.AtualizarUltimaAlteracaoCronograma( string.Format( "{0} {1}" , usuario.NomeCompletoColaborador , msg ) );
				}
				AtualizarGraficoBurndown();
			}
			if( !viewEditandoTarefa )
				gerenciadorComandos.ExecutarComandosPendentes();
		}

		/// <summary>
		/// Método responsável por atribuir qual mensagem deve ser exibida
		/// </summary>
		/// <param name="oidTarefasExcluidas">Lista contendo o oid das tarefas excluidas</param>
		/// <returns>mensagem que deve ser exibida</returns>
		private static string AtribuirMensagemExclusaoTarefas( List<Guid> oidTarefasExcluidas )
		{
			if( oidTarefasExcluidas.Count > 1 )
				return string.Format( "excluiu {0} tarefa(s)" , oidTarefasExcluidas.Count );
			else
				return "excluiu 1 tarefa";
		}

		/// <summary>
		/// Método responsável pela execução do comportamento quando outro usuário mover uma tarefa
		/// </summary>
		/// <param name="mensagem"></param>
		private void accessClient_AoOcorrerMovimentacaoPosicaoTarefa( MensagemDto mensagem )
		{
			Int16 posicaoInicial = Int16.Parse( mensagem.Propriedades["posicaoInicial"].ToString() );
			Int16 posicaoFinal = Int16.Parse( mensagem.Propriedades["posicaoFinal"].ToString() );
			string oidCronogramaTarefa = mensagem.Propriedades[Constantes.OIDTAREFA] as string;
			Dictionary<string, Int16> tarefasImpactadas = mensagem.Propriedades[Constantes.TAREFAS_IMPACTADAS] as Dictionary<string , Int16>;
			string autor = mensagem.Propriedades[Constantes.AUTOR_ACAO] as string;
			DateTime dataHoraAcao = (DateTime)mensagem.Propriedades[Constantes.DATAHORA_ACAO];
			CronogramaTarefaGridItem tarefa = ConsultarTarefaPodOidCronogramaTarefaNoDataSource( Guid.Parse( oidCronogramaTarefa ) );
			gerenciadorComandos.CriarComandoMovimentarTarefa( tarefa.OidCronogramaTarefa , posicaoInicial , posicaoFinal , tarefasImpactadas , dataHoraAcao );

			CronogramaColaboradorConfigDto usuario = configUsuariosConectados.FirstOrDefault( o => o.Login == autor );
			string acao = string.Format( "mover a tarefa {0} para posicao {1}" , tarefa.TxDescricaoTarefa , posicaoFinal );
			TarefasImpactadasDebugUtil.ExibirLogReordenacoesComunicadas( autor , new List<CronogramaTarefaGridItem>( cronogramaView.TarefasCronograma ) , tarefasImpactadas , acao , dataHoraAcao );
			if( usuario != null && !string.IsNullOrEmpty( usuario.NomeCompletoColaborador ) )
			{
				autor = usuario.NomeCompletoColaborador;
				string mensagemNotificacaoTela = string.Format( "movimentou a tarefa {1} para posição {2}" , autor , posicaoInicial , posicaoFinal );
				cronogramaView.AtualizarHintColaborador( autor , mensagemNotificacaoTela , dataHoraAcao );
				cronogramaView.AtualizarUltimaAlteracaoCronograma( string.Format( "{0} {1}" , autor , mensagemNotificacaoTela ) );
			}

			if( viewEditandoTarefa )
			{
				tarefa.AdicionarIconeMovimentacao( posicaoInicial , posicaoFinal );
				cronogramaView.AtualizarView();
			}

			gerenciadorComandos.ExecutarComandosPendentes();
		}

		#endregion

		#region Métodos Utilitários


		/// <summary>
		/// Método responsável por buscar o login do usuário logado no Windows.
		/// </summary>
		/// <returns></returns>
		private static string GetLoginUsuarioWindows()
		{
			return ADUtil.GetUsuarioLogadoPeloWindows();
		}


		#endregion

		#region Consultas ao Serviço


		/// <summary>
		/// Carregar a lista de situações planejamento atuais
		/// </summary>
		/// <returns>Lista de situações planejamento</returns>
		protected virtual List<SituacaoPlanejamentoDTO> ListarSituacoesPlanejamento()
		{
			return servicoPlanejamento.ConsultartSituacoesPlanejamento();
		}

		/// <summary>
		/// Método responsável por selecionar a cor para um determinado colaborador no cronograma atual
		/// </summary>
		/// <param name="login">login do colaborador</param>
		/// <param name="oidCronograma">oid de identificação do cronograma atual</param>
		protected virtual void SelecionarCorColaborador( string login , string oidCronograma )
		{
			if( string.IsNullOrEmpty( login ) || string.IsNullOrEmpty( oidCronograma ) )
				return;
			servicoPlanejamento.EscolherCorColaborador( login , oidCronograma );
		}

		/// <summary>
		/// Responsável por buscar no serviço o colaborador associado ao login do usuário conectado
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
		protected virtual ColaboradorDto ConsultarColaborador( string login )
		{
			return servicoGeral.ConsultarColaboradorLogado( Login );
		}

		/// <summary>
		/// responsável por buscar no serviço o Ultimo cronograma selecionado pelo colaborador do login atual
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
		protected virtual CronogramaDto ConsultarUltimoCronogramaSelecionado( string login )
		{
			return servicoPlanejamento.ConsultarUltimoCronogramaSelecionado( Login );
		}

		/// <summary>
		/// Listar CronogramColaboradorConfig dos usuários contidos na lista
		/// </summary>
		/// <param name="loginColaboradores"> lista de login dos colaboradores</param>
		/// <returns>Lista de configurações do colaboradores</returns>
		protected virtual List<CronogramaColaboradorConfigDto> ListarConfigColaboradores( List<string> loginColaboradores )
		{
			return servicoPlanejamento.ConsultarConfigUsuariosConectados( loginColaboradores.ToArray() , CronogramaSelecionado.Oid.ToString() );
		}

		/// <summary>
		/// Responsável por listar as situações de planejamento inativas
		/// </summary>
		/// <returns>lista de situações de planejamento inativas</returns>
		protected virtual List<SituacaoPlanejamentoDTO> ListarSituacoesPlanejamentoInativas()
		{
			return servicoPlanejamento.ConsultarSituacoesInativas();
		}

		/// <summary>
		/// Responsável por retornar a situação planejamento padrão ao criar uma nova tarefa
		/// </summary>
		/// <returns></returns>
		protected virtual SituacaoPlanejamentoDTO ConsultarSituacaoPlanejamentoPadrao()
		{
			return servicoPlanejamento.ConsultarSituacaoPlanejamentoPadrao();
		}

		/// <summary>
		/// Responsável por retornar uma lista com os cronogramas existentes
		/// </summary>
		/// <returns></returns>
		protected virtual List<CronogramaDto> ConsultarCronogramas()
		{
			return servicoPlanejamento.ListarCronogramas();
		}

		/// <summary>
		/// Responsável por retornar uma lista com os cronogramas existentes
		/// </summary>
		/// <returns></returns>
		protected virtual List<ColaboradorDto> ConsultarColaboradoresResponsaveis()
		{
			return servicoPlanejamento.ListarColaboradores();
		}

		/// <summary>
		/// Método responsável por buscar no serviço as tarefas cadastradas para o cronograma atual
		/// </summary>
		/// <returns></returns>
		protected virtual List<CronogramaTarefaGridItem> ListarTarefasCronogramaAtual()
		{
			if( cronogramaSelecionado != null && cronogramaSelecionado.Oid != new Guid() )
				return servicoPlanejamento.ConsultarCronogramaTarefasPorOidCronograma( cronogramaSelecionado.Oid );
			else
				return null;
		}

		/// <summary>
		/// Método responsável por salvar o ultimo cronograma selecionado para o colaborador atual
		/// </summary>
		private void SalvarUltimoCronogramaSelecionado()
		{
			if( string.IsNullOrEmpty( Login ) || cronogramaSelecionado == null )
				return;
			servicoPlanejamento.SalvarUltimoCronogramaSelecionado( Login , cronogramaSelecionado.Oid.ToString() );
		}

		/// <summary>
		/// Método responsável por criar novo cronograma na base
		/// </summary>
		/// <returns>novo cronograma criado</returns>
		private CronogramaDto CriarNovoCronograma()
		{
			return servicoPlanejamento.CriarCronogramaPadrao();
		}


		#endregion

		#region Regras de Negócio

		/// <summary>
		/// Método que retorna se uma tarefa encontra-se ou não em edição
		/// </summary>
		/// <returns> true existe tarefa em edição e false caso crontrário</returns>
		public bool ExisteTarefaEmEdicao()
		{
			return tarefaEmEdicao != null;
		}

		/// <summary>
		/// Método responsável por selecionar um cronograma por nome
		/// </summary>
		/// <param name="descricaoCronograma"></param>
		public void SelecionarCronograma( string descricaoCronograma )
		{
			CronogramaDto cronograma = cronogramas.FirstOrDefault( o => o.TxDescricao == descricaoCronograma );
			if( cronograma != null )
			{
				if( contadorAcoesPendentes > 0 )
					accessClient.RnFinalizarConexao( true );

				cronogramaSelecionado = cronograma;
				SalvarUltimoCronogramaSelecionado();
				InicializarCronograma();
			}
		}

		/// <summary>
		/// Método responsável por incluir um novo cronograma e atualizar a tela
		/// </summary>
		public void IncluirNovoCronograma()
		{
			cronogramaSelecionado = CriarNovoCronograma();
			SalvarUltimoCronogramaSelecionado();
			InicializarCronograma();
		}

		/// <summary>
		/// Método responsável por excluir o cronograma atual
		/// </summary>
		public void ExcluirCronograma()
		{
			if( cronogramaSelecionado == null )
			{
				cronogramaView.NotificarAlerta( Resources.Caption_Atencao , Resources.Alerta_NaoExisteCronogramaParaExclusao );
				return;
			}

			if( configUsuariosConectados.Count > 0 )
			{
				cronogramaView.NotificarErro( Resources.Caption_ErroExclusaoCronograma , Resources.Erro_ExcluirCronogramaComColaboradoresConectados );
				return;
			}

			if( cronogramaView.ConfirmarExclusaoCronograma() )
			{
				bool respostaExclusao = servicoPlanejamento.ExcluirCronograma( cronogramaSelecionado.Oid.ToString() );

				if( respostaExclusao )
				{
					cronogramaView.AtualizarVisibilidadeBotoesBarra( false );
					CronogramaDto cronograma = cronogramas.FirstOrDefault( o => o.Oid == cronogramaSelecionado.Oid );
					cronogramas.Remove( cronograma );
					cronogramaSelecionado = null;
					cronogramaView.TarefasCronograma = new BindingList<CronogramaTarefaGridItem>();
					InicializarCronograma();
				}
				else
					cronogramaView.NotificarErro( Resources.Caption_ErroExclusaoCronograma , Resources.Erro_ExcluirCronogramaQuePossuaTarefas );
			}
		}

		/// <summary>
		/// Método responsável por criar uma tarefa padrão
		/// </summary>
		/// <param name="tarefaSelecionada">tarefa selacionada quando solicitada a criação de uma nova tarefa</param>
		/// <returns>Uma nova tarefa com os dados padrões preenchidos</returns>
		private CronogramaTarefaGridItem CriarTarefaPadrao( CronogramaTarefaGridItem tarefaSelecionada )
		{
			CronogramaTarefaGridItem tarefa = new CronogramaTarefaGridItem()
			{
				DtAtualizadoEm = DateTime.Now ,
				TxDescricaoTarefa = "" ,
				TxAtualizadoPor = colaboradorLogado.TxNomeCompletoColaborador ,
				OidCronograma = cronogramaSelecionado.Oid ,
				NbEstimativaInicial = 0 ,
				NbEstimativaRestante = 0 ,
				NbRealizado = 0 ,
			};

			if( tarefaSelecionada != null )
				tarefa.DtInicio = tarefaSelecionada.DtInicio;
			else
				tarefa.DtInicio = DateTime.Now;

			if( situacaoPlanejamentoPadrao != null )
				tarefa.OidSituacaoPlanejamentoTarefa = situacaoPlanejamentoPadrao.Oid;

			return tarefa;
		}

		/// <summary>
		/// Tratamento de uma tarefa que acabou de ser criada
		/// </summary>
		/// <param name="tarefaReferencia">tarefa selacionada quando solicitada a criação de uma nova tarefa</param>
		public CronogramaTarefaDto NovaTarefaCriada( CronogramaTarefaGridItem tarefaReferencia )
		{
			if( cronogramaSelecionado == null )
				return null;

			if( cronogramaView.TarefasCronograma.Any( o => o.OidCronogramaTarefa == new Guid() ) )
			{
				cronogramaView.NotificarMensagem( Resources.Caption_Atencao , "Já existe uma tarefa em criação, para descartá-la use a tecla ESC" );
				return null;
			}

			CronogramaTarefaGridItem tarefa = CriarTarefaPadrao( tarefaReferencia );
			int posicao;
			CronogramaTarefaGridItem ultimaTarefa = cronogramaView.TarefasCronograma.LastOrDefault();
			if( ultimaTarefa != null )
			{
				if( ultimaTarefa == tarefaReferencia )
				{
					posicao = -1;
					this.tarefaReferencia = null;
				}
				else
				{
					this.tarefaReferencia = tarefaReferencia;
					posicao = cronogramaView.TarefasCronograma.IndexOf( tarefaReferencia );
				}
			}
			else
			{
				posicao = -1;
			}
			tarefa.AdicionarIconeCriacao();
			cronogramaView.InserirTarefaPadrao( tarefa , posicao );
			cronogramaView.AtualizarUltimaAcao( "Criando nova tarefa ..." );

			return tarefa;
		}

		/// <summary>
		/// responsável por efetuar a criação da nova tarefa
		/// </summary>
		/// <param name="posicao">Posicao da nova tarefa</param>
		public void CriarTarefa( CronogramaTarefaGridItem tarefaAtual )
		{
			if( !conectado )
				return;

			if( !string.IsNullOrEmpty( tarefaAtual.TxDescricaoTarefa ) )
			{
				contadorAcoesPendentes += 1;
				aguardandoCriacaoTarefa = true;
				SalvarNovaTarefa( tarefaAtual );
			}
		}

		/// <summary>
		/// Método responsável pelo comportamento quando uma tarefa entrar em edição
		/// </summary>
		/// <param name="autoSalvarEdicao">define se deve ou não auto-salvar edição quando receber autorização de edição, default false</param>
		public void SolicitarInicioEdicaoTarefa()
		{
			edicaoLocker.WaitOne();
			viewEditandoTarefa = true;
			CronogramaTarefaGridItem tarefa = cronogramaView.ConsultarTarefaSelecionada();
			if( tarefa == null )
				return;

			ultimaTarefaSelecionada = tarefa;
			if( !TarefaSelecionadaForUmaNovaTarefa( tarefa ) )
			{
				if( tarefa.EmExclusao )
				{
					cronogramaView.NotificarAlerta( Resources.Caption_Atencao , Resources.Alerta_EdicaoRecusadaTarefaEmExclusao );
					return;
				}

				WexLogger.Info( string.Format( "Entrou em edição a tarefa de ID:{0} - {1} OID: {2}" , tarefa.NbID , tarefa.TxDescricaoTarefa , tarefa.OidCronogramaTarefa ) );
				cronogramaView.AtualizarUltimaAcao( string.Format( "Editando tarefa {0} ..." , tarefa.NbID ) );

				if( !ExisteTarefaEmEdicao() )
				{
					ContadorEdicoes += 1;
					string idRequisicao = CriarIdentificadorEdicao( Login , ContadorEdicoes );
					accessClient.RnComunicarInicioEdicaoTarefa( tarefa.OidCronogramaTarefa.ToString() , idRequisicao );
					WexLogger.Info( string.Format( "Cronograma comunicou o inicio da edição da tarefa de ID:{0} - {1} OID:{2}" , tarefa.NbID , tarefa.TxDescricaoTarefa , tarefa.OidCronogramaTarefa ) );
					tarefaEmEdicao = new TarefaEditada( tarefa );
				}
				else
				{
					if( !IgualATarefaEmEdicao( tarefa.OidCronogramaTarefa ) )
					{
						cronogramaView.NotificarAlerta( Resources.Caption_Atencao , Resources.Alerta_JaPossuiTarefaEmEdicao );
					}
				}
			}
			edicaoLocker.Release();
			gerenciadorComandos.PodeExecutar = false;
		}

		public string CriarIdentificadorEdicao( string login , int numEdicao )
		{
			return string.Format( "{0}_edit_{1}" , login , numEdicao );
		}

		/// Método responsável pelo comportamento quando uma tarefa sair de edição
		/// </summary>
		public void TarefaSaiuDeEdicao()
		{
			edicaoLocker.WaitOne();

			viewEditandoTarefa = false;

			LimparMensagemBarraStatus();

			if( ultimaTarefaSelecionada != null )
				WexLogger.Info( string.Format( "A tarefa selecionada {0} - {1} saiu de edição. OID:{2}" , ultimaTarefaSelecionada.NbID , ultimaTarefaSelecionada.TxDescricaoTarefa , ultimaTarefaSelecionada.OidCronogramaTarefa ) );

			if( TarefaSelecionadaForUmaNovaTarefa( ultimaTarefaSelecionada ) )
			{
				if( !string.IsNullOrWhiteSpace( ultimaTarefaSelecionada.TxDescricaoTarefa ) )
				{
					CriarTarefa( ultimaTarefaSelecionada );
					WexLogger.Info( string.Format( "Criação no serviço da tarefa {0} - {1}" , ultimaTarefaSelecionada.NbID , ultimaTarefaSelecionada.TxDescricaoTarefa ) );
				}
			}
			else
			{
				if( ultimaTarefaSelecionada != null && tarefaEmEdicao != null )
				{
					lock( tarefaEmEdicao )
					{
						Guid oidTarefa = ultimaTarefaSelecionada.OidCronogramaTarefa;
						if( tarefaEmEdicao.Oid.Equals( oidTarefa ) )
						{
							tarefaEmEdicao.Editando = false;
							if( !tarefaEmEdicao.PermissaoPendente && tarefaEmEdicao.PodeSalvar )
							{
								EsperarLeituraDataSource();
								CronogramaTarefaGridItem tarefa = ConsultarTarefaPodOidCronogramaTarefaNoDataSource( oidTarefa );
								LiberarLeituraDataSource();
								SalvarEdicaoTarefa( tarefa );
								accessClient.RnComunicarFimEdicaoTarefa( oidTarefa.ToString() );
								RemoverTarefaDeEdicao();
								ultimaTarefaSelecionada = null;
							}
						}
					}
				}
			}
			gerenciadorComandos.PodeExecutar = true;
			gerenciadorComandos.ExecutarComandosPendentes();
			edicaoLocker.Release();
		}

		/// <summary>
		/// Método que verifica se a tarefa atual é uma nova tarefa
		/// </summary>
		/// <param name="tarefa">Tarefa Selecionada</param>
		/// <returns></returns>
		private static bool TarefaSelecionadaForUmaNovaTarefa( CronogramaTarefaGridItem tarefa )
		{
			if( tarefa == null )
				return false;

			return tarefa.OidCronogramaTarefa == new Guid();
		}

		/// <summary>
		/// Método responsável por efetuar a criação da nova tarefa criada;
		/// </summary>
		/// <param name="tarefa">Nova Tarefa Criada</param>
		private void SalvarNovaTarefa( CronogramaTarefaGridItem tarefa )
		{
			short nbIDReferencia;

			if( ( tarefaReferencia != null ) && ( tarefaReferencia.NbID != null ) )
				nbIDReferencia = (short)tarefaReferencia.NbID;
			else
				nbIDReferencia = 0;
			servicoPlanejamento.CriarNovaTarefa( tarefa.OidCronograma , tarefa.TxDescricaoTarefa , tarefa.OidSituacaoPlanejamentoTarefa.ToString() ,
																							 tarefa.DtInicio , tarefa.TxDescricaoColaborador , ColaboradorLogado.Login , tarefa.TxObservacaoTarefa , tarefa.NbEstimativaInicial , nbIDReferencia );
			WexLogger.Info( string.Format( "A nova tarefa {0} foi enviada para o serviço!" , tarefa.TxDescricaoTarefa ) );
		}

		/// <summary>
		/// Método responsável por salvar a edição de uma tarefa
		/// </summary>
		/// <param name="tarefa"></param>
		public virtual void SalvarEdicaoTarefa( CronogramaTarefaGridItem tarefa )
		{
			if( tarefa == null || string.IsNullOrEmpty( tarefa.TxDescricaoTarefa ) )
				return;

			if( String.IsNullOrEmpty( tarefa.TxObservacaoTarefa ) )
				tarefa.TxObservacaoTarefa = String.Empty;

			bool consumiuHoras = false;
			TarefaEditada tarefaEditada = tarefaEmEdicao;

			if( tarefaEmEdicao.Oid.Equals( tarefa.OidCronogramaTarefa ) )
			{
				if( tarefaEditada.HouveMudancaEm( tarefa , o => o.OidSituacaoPlanejamentoTarefa ) )
					if( !ValidarSituacaoPlanejamento( tarefa , tarefaEditada , ref consumiuHoras ) )
						return;

				if( tarefaEmEdicao.HouveMudancas( tarefa ) )
				{
					if( !consumiuHoras )
					{
						if( !tarefaEditada.TarefaAntiga.NbEstimativaInicial.Equals( tarefa.NbEstimativaInicial ) )
							tarefa.NbEstimativaRestante = TimeSpan.FromHours( (int)tarefa.NbEstimativaInicial ).Ticks;

						contadorAcoesPendentes += 1;

						tarefa.TxAtualizadoPor = ColaboradorLogado.Login;

						Hashtable dadosEdicao = servicoPlanejamento.EditarTarefa( tarefa );

						if( VerificarSeEdicaoFoiConcluidaComSucesso( dadosEdicao ) )
						{
							ForcarAtualizarDadosTarefa( tarefa );

							if( tarefaEditada.HouveMudancaEm( tarefa , o => o.NbEstimativaRestante ) )
								AtualizarGraficoBurndown();
						}
						else
						{
							cronogramaView.NotificarAlerta( Resources.Caption_Atencao , Resources.Alerta_EdicaoNaoSalva );
							RetornarValoresAnterioresTarefa( tarefa.OidCronogramaTarefa );
						}

						contadorAcoesPendentes -= 1;
					}
				}

				gerenciadorComandos.PodeExecutar = true;
			}
		}

		/// <summary>
		/// Método responsável por verificar se a situação planejamento é válida e executar os procedimentos adequados.
		/// </summary>
		/// <param name="tarefa">Objeto alterado a ser validado</param>
		/// <returns>Se é válido ou não</returns>
		private bool ValidarSituacaoPlanejamento( CronogramaTarefaGridItem tarefa, TarefaEditada tarefaEditada, ref bool consumiuHoras )
		{
			Guid oidSituacaoAlterada = tarefa.OidSituacaoPlanejamentoTarefa;

			tarefa.OidSituacaoPlanejamentoTarefa = tarefaEditada.TarefaAntiga.OidSituacaoPlanejamentoTarefa;

			switch( GerenciarSituacaoPlanejamento( oidSituacaoAlterada ) )
			{
				case CsSituacaoPlanejamentoTipoRetorno.ConsumiuHoras:
					consumiuHoras = true;
					return true;
				case CsSituacaoPlanejamentoTipoRetorno.NaoConsumiuHoras:
					return true;
				case CsSituacaoPlanejamentoTipoRetorno.SituacaoPlanejamentoRecusada:
					RetornarValoresAnterioresTarefa( tarefa.OidCronogramaTarefa );
					return true;
				default:
					return false;
			}
		}

		private static bool VerificarSeEdicaoFoiConcluidaComSucesso( Hashtable dadosEdicao )
		{
			return (bool)dadosEdicao["EdicaoStatus"];
		}

		private void ForcarAtualizarDadosTarefa( CronogramaTarefaGridItem tarefa )
		{
			CronogramaTarefaGridItem tarefaAtualizada = servicoPlanejamento.ConsultarCronogramaTarefaPorOid( tarefa.OidCronogramaTarefa );

			TarefaEditada.CopiarValores( tarefaAtualizada , tarefa );

			gerenciadorComandos.GerenciadorTarefasImpactadas.AplicarDataAtualizacao( tarefa.OidCronogramaTarefa , tarefa.DtHoraConsulta );

			tarefa.DtAtualizadoEm = tarefaAtualizada.DtAtualizadoEm;
			tarefa.TxAtualizadoPor = tarefaAtualizada.TxAtualizadoPor;

			cronogramaView.AtualizarTarefaEmSelecao( tarefa.OidCronogramaTarefa );
			cronogramaView.AtualizarUltimaAcao( string.Format( "A edição da tarefa {0} foi salva com sucesso!" , tarefa.NbID ) );
			WexLogger.Info( string.Format( "Edição da tarefa de ID:{0} OID:{1} Salva." , tarefaAtualizada.NbID , tarefaAtualizada.OidCronogramaTarefa ) );
		}

		/// <summary>
		/// Método responsável por alterar o nome do cronograma no serviço
		/// </summary>
		/// <param name="NomeCronograma"></param>
		/// <returns></returns>
		protected virtual bool SalvarNovoNomeCronograma( string NomeCronograma )
		{
			return servicoPlanejamento.EditarCronograma( cronogramaSelecionado );
		}

		/// <summary>
		/// Método resposável por validar os campos editados e Acionar MensagemPopup caso haja Alterações Relevantes
		/// Campos Relevantes :
		///   - Situação Planejamento
		///   - Estimativa Inicial
		///   - Estimativa Restante
		/// </summary>
		/// <param name="tarefasAtuais">Tarefas Atuais no grid</param>
		/// <param name="tarefasAtualizadas">Tarefas Atualizadas</param>
		public void VerificarEdicoesRelevantes( CronogramaTarefaGridItem tarefaAtual , CronogramaTarefaGridItem tarefaAtualizada , string loginAutorAcao )
		{
			if( TarefaEditada.HouveMudancas( tarefaAtualizada , tarefaAtual ) )
			{
				List<int> camposAlterados;
				StringBuilder mensagem = new StringBuilder();

				camposAlterados = TarefaEditada.VerificarAlteracoesRelevantes( tarefaAtual , tarefaAtualizada );

				AtribuirMensagemAlteracaoImportantesTarefa( tarefaAtualizada , camposAlterados , mensagem );

				if( camposAlterados.Count > 0 )
					cronogramaView.NotificarMensagemComFoto( "Tarefa Editada" , mensagem.ToString() , configUsuariosConectados.First( o => o.Login == loginAutorAcao ).Foto );

				cronogramaView.AtualizarUltimaAlteracaoCronograma( mensagem.ToString() );
				cronogramaView.AtualizarHintColaborador( tarefaAtualizada.TxAtualizadoPor , mensagem.ToString() , tarefaAtualizada.DtAtualizadoEm );
			}
		}

		/// <summary>
		/// Método responsável por verificar a mensagem para ser exibida ao usuário
		/// </summary>
		/// <param name="tarefaAtualizada">Objeto que foi atualizado</param>
		/// <param name="camposAlterados">Campos que foram alterados</param>
		/// <param name="mensagem">Objeto a ser construido de acordo com as alterações</param>
		private static void AtribuirMensagemAlteracaoImportantesTarefa( CronogramaTarefaGridItem tarefaAtualizada , List<int> camposAlterados , StringBuilder mensagem )
		{
			mensagem.AppendFormat( "{0} editou a tarefa {1}" , tarefaAtualizada.TxAtualizadoPor , tarefaAtualizada.NbID );

			foreach( int campo in camposAlterados )
			{
				switch( (CsTipoCampoEditado)campo )
				{
					case CsTipoCampoEditado.SituacaoPlanejamento:
						mensagem.AppendFormat( ", nova situação planejamento: {0}" , tarefaAtualizada.TxDescricaoSituacaoPlanejamentoTarefa );
						break;

					case CsTipoCampoEditado.EstimativaInicial:
						mensagem.AppendFormat( ",a nova estimativa inicial é de {0}hs" , tarefaAtualizada.NbEstimativaInicial.ToHoursTimeSpan() );
						break;

					case CsTipoCampoEditado.EstimativaRestante:
						mensagem.AppendFormat( ",a nova estimativa restante é de {0}hs" , tarefaAtualizada.NbEstimativaRestante.ToTimeSpan() );
						break;
				}
			}
		}

		/// <summary>
		/// Método responsável por realizar a solicitação de exclusão das tarefas selecionadas.
		/// </summary>
		/// <param name="indicesTarefasParaExclusao">indice das tarefas selecionadas para exclusão</param>
		public void SolicitarExclusaoTarefas( List<CronogramaTarefaGridItem> tarefasParaExclusao )
		{
			if( !Conectado )
				return;

			gerenciadorComandos.PodeExecutar = false;

			if( cronogramaView.ConfirmarExclusaoTarefas() )
			{
				contadorAcoesPendentes += 1;
				gerenciadorComandos.PodeExecutar = true;

				List<string> oidTarefasParaExcluir = new List<string>();

				for( int indice = 0; indice < tarefasParaExclusao.Count; indice++ )
				{
					if( !cronogramaView.LinhasParaExcluir.Contains( tarefasParaExclusao[indice].OidCronogramaTarefa ) )
					{
						cronogramaView.LinhasParaExcluir.Add( tarefasParaExclusao[indice].OidCronogramaTarefa );

						oidTarefasParaExcluir.Add( tarefasParaExclusao[indice].OidCronogramaTarefa.ToString() );
					}
				}

				accessClient.RnComunicarInicioExclusaoTarefa( oidTarefasParaExcluir.ToArray() );
			}
		}

		#endregion

		/// <summary>
		/// Método responsável por efetuar a desconexão do cronograma
		/// </summary>
		public void DesconectarCronograma()
		{
			if( accessClient.Conectado )
			{
				accessClient.RnDesconectar();
			}
		}

		/// Método utilizado par verificar se uma tarefa está com a resposta do servico pendente
		/// </summary>
		/// <returns></returns>
		public bool EdicaoComPermissaoPendente( Guid oidCronogramaTarefa )
		{
			return tarefaEmEdicao != null && tarefaEmEdicao.PermissaoPendente;
		}

		/// <summary>
		/// Verificar se a edição foi permitida
		/// </summary>
		/// <param name="oidCronogramaTarefa">oid de identificação da tarefa atual</param>
		/// <returns></returns>
		public bool EdicaoPermitida( Guid oidCronogramaTarefa )
		{
			return tarefaEmEdicao != null && tarefaEmEdicao.PodeSalvar;
		}

		/// <summary>
		/// Verificar se o oid pertence a tarefa atual
		/// </summary>
		/// <param name="oidCronogramaTarefa">oid de identificação da tarefa</param>
		/// <returns></returns>
		public bool IgualATarefaEmEdicao( Guid oidCronogramaTarefa )
		{
			return tarefaEmEdicao != null && tarefaEmEdicao.Oid.Equals( oidCronogramaTarefa );
		}

		/// <summary>
		/// Método responsável por informar o presenter de que houve movimentação na view
		/// </summary>
		public void SolicitarMovimentacaoTarefa( CronogramaTarefaGridItem tarefaParaMover , CronogramaTarefaGridItem tarefaDestino )
		{
			EsperarLeituraDataSource();

			if( cronogramaView.TarefasCronograma == null || cronogramaView.TarefasCronograma.Count == 0 )
			{
				LiberarLeituraDataSource();
				return;
			}

			if( !ValidarTarefaParaMovimentacao( tarefaParaMover ) || !ValidarTarefaParaMovimentacao( tarefaDestino ) || tarefaParaMover == tarefaDestino )
			{
				LiberarLeituraDataSource();
				return;
			}

			LiberarLeituraDataSource();

			contadorAcoesPendentes += 1;

			servicoPlanejamento.MoverTarefa( tarefaParaMover.OidCronogramaTarefa , (short)tarefaDestino.NbID );
		}

		/// <summary>
		/// Verificar se pode ser efetuada a movimentação de uma tarefa
		/// </summary>
		/// <param name="tarefa"></param>
		/// <returns></returns>
		public bool ValidarTarefaParaMovimentacao( CronogramaTarefaGridItem tarefa )
		{
			return tarefa != null && !TarefaSelecionadaForUmaNovaTarefa( tarefa ) && cronogramaView.TarefasCronograma.Contains( tarefa );
		}

		/// <summary>
		/// Método para adicionar marcação visual ao exclusão de linha no presenter
		/// </summary>
		/// <param name="oidTarefas"></param>
		public void AdicionarMarcacaoLinhasEmExclusao( Guid oidTarefa )
		{
			if( oidTarefa == null && oidTarefa != new Guid() )
				return;

			if( !cronogramaView.LinhasParaExcluir.Contains( oidTarefa ) )
				cronogramaView.LinhasParaExcluir.Add( oidTarefa );
			CronogramaTarefaGridItem item = ConsultarTarefaPodOidCronogramaTarefaNoDataSource( oidTarefa );
			if( item != null )
			{
				item.AdicionarIconeExcluir();
			}
		}

		/// <summary>
		/// Método para adicionar marcação visual ao exclusão de linha no presenter
		/// </summary>
		/// <param name="oidTarefas">lista de identificadores de tarefas</param>
		public void AdicionarMarcacaoLinhasEmExclusao( List<Guid> oidTarefas )
		{
			if( oidTarefas == null )
				return;

			foreach( Guid oid in oidTarefas )
			{
				AdicionarMarcacaoLinhasEmExclusao( oid );
			}
		}

		/// <summary>
		/// Método utilizado para remover a as mensagens do dicionario de tarefas marcadas visualmente como em exclusão
		/// </summary>
		public void RemoverMarcacaoLinhasEmExclusao( List<Guid> oidTarefas )
		{
			if( oidTarefas == null )
				return;

			foreach( Guid oid in oidTarefas )
			{
				var tarefa = ConsultarTarefaPodOidCronogramaTarefaNoDataSource( oid );

				if( tarefa != null && tarefa.Icone != null )
					tarefa.RemoverIcone();

				if( cronogramaView.LinhasParaExcluir.Contains( oid ) )
					cronogramaView.LinhasParaExcluir.Remove( oid );
			}
		}

		/// <summary>
		/// Método responsável por atualizar os dados do grid para uma tarefa modificada
		/// </summary>
		/// <param name="oidCronogramaTarefa"></param>
		public void AtualizarDadosTarefa( Guid oidTarefa )
		{
			CronogramaTarefaGridItem tarefaAtualizada = servicoPlanejamento.ConsultarCronogramaTarefaPorOid( oidTarefa );
			CronogramaTarefaGridItem tarefaAtual = ConsultarCronogramaTarefaPorOidNoDataSourceDaView( oidTarefa );

			if( tarefaAtualizada.OidCronograma != CronogramaSelecionado.Oid )
				return;

			if( tarefaAtualizada == null || tarefaAtual == null )
				return;

			tarefaAtual = TarefaEditada.CopiarValores( tarefaAtualizada , tarefaAtual );
			cronogramaView.AtualizarTarefaEmSelecao( tarefaAtual.OidCronogramaTarefa );
			AtualizarGraficoBurndown();
		}

		/// <summary>
		/// Método responsável por buscar objeto CronogramaTarefaDto no datasource da View
		/// </summary>
		/// <param name="oidCronogramaTarefa">Oid do objeto</param>
		/// <returns>Objeto CronogramaTarefaDto</returns>
		private CronogramaTarefaGridItem ConsultarCronogramaTarefaPorOidNoDataSourceDaView( Guid oidTarefa )
		{
			CronogramaTarefaGridItem cronogramaTarefa;

			EsperarLeituraDataSource();

			cronogramaTarefa = ConsultarTarefaPodOidCronogramaTarefaNoDataSource( oidTarefa );

			LiberarLeituraDataSource();

			return cronogramaTarefa;
		}

		public CronogramaTarefaGridItem ConsultarTarefaPodOidCronogramaTarefaNoDataSource( Guid oidTarefa )
		{
			return cronogramaView.TarefasCronograma.FirstOrDefault( o => o.OidCronogramaTarefa == oidTarefa );
		}

		/// <summary>
		/// Método responsável por sinalizar o fim da edição do nome do cronograma
		/// </summary>
		/// <param name="cronogramaAlterado"></param>
		public void FimEdicaoDadosCronograma()
		{
			if( ValidarDadosCronograma() )
				AtribuirNovosDadosCronograma();
			else
				RetornarValoresAntigosDadosCronograma();
			
			gerenciadorEdicaoCronograma.FimEdicaoDadosCronograma();
		}

		/// <summary>
		/// Método responsável por validar os dados dos campos Nome Cronograma, Data Inicio e Data Final
		/// </summary>
		/// <returns>Retorna se está válido ou não</returns>
		private bool ValidarDadosCronograma()
		{
			if( !string.IsNullOrEmpty( cronogramaView.NomeCronograma ) && cronogramaView.DataInicio.Date < cronogramaView.DataTermino.Date)
				return true;

			return false;
		}

		/// <summary>
		/// Método responsável por atribuir os novos valores ao objeto CronogramaSelecionado para salvar posteriormente.
		/// </summary>
		private void AtribuirNovosDadosCronograma()
		{
			cronogramaSelecionado.TxDescricao = cronogramaView.NomeCronograma;
			cronogramaSelecionado.DtInicio = cronogramaView.DataInicio;
			cronogramaSelecionado.DtFinal = cronogramaView.DataTermino;
		}

		/// <summary>
		/// Método responsável por retornar os valores antigos para os campos da view.
		/// </summary>
		private void RetornarValoresAntigosDadosCronograma()
		{
			cronogramaView.NomeCronograma = cronogramaSelecionado.TxDescricao;
			cronogramaView.DataInicio = cronogramaSelecionado.DtInicio;
			cronogramaView.DataTermino = cronogramaSelecionado.DtFinal;
		}

		/// <summary>
		/// Método responsável por recarregas as tarefas do cronograma
		/// </summary>
		public virtual void ForcarAtualizacaoTarefas( Guid oidCronograma )
		{
			if( cronogramaSelecionado == null || oidCronograma == new Guid() || !oidCronograma.Equals( cronogramaSelecionado.Oid ) )
				return;

			List<CronogramaTarefaGridItem> tarefas = servicoPlanejamento.ConsultarCronogramaTarefasPorOidCronograma( cronogramaSelecionado.Oid );
			gerenciadorComandos.CriarComandoAtualizarTarefas( tarefas );
			gerenciadorComandos.ExecutarComandosPendentes();
		}

		/// <summary>
		/// Método responsável por gerenciar os semáforos de cada movimentação.
		/// </summary>
		/// <param name="posicaoInicial">NbID tarefa para ser movida</param>
		/// <param name="posicaoFinal">NbID destino da tarefa</param>
		/// <param name="tarefaParaMover">Objeto da tarefa movida</param>
		private void EsperarSemaforosMovimentacao( short posicaoInicial , short posicaoFinal , Guid oidCronogramaTarefaParaMover )
		{
			Hashtable semaforos = SemaforoSingleton.GetInstancia().ControlarSemaforos( CronogramaSelecionado.Oid , posicaoInicial , posicaoFinal );
			List<SemaforoPorIntervalo> semaforosNovos = (List<SemaforoPorIntervalo>)semaforos["semaforosNovos"];
			List<SemaforoPorIntervalo> semaforosParaAguardar = (List<SemaforoPorIntervalo>)semaforos["semaforosAguardar"];

			List<SemaforoPorIntervalo> semaforosOrdenados = SemaforoPorIntervalo.OrdenarSemaforos( semaforosNovos , semaforosParaAguardar );

			if( semaforosOrdenados.Count > 0 )
			{
				for( int i = 0; i < semaforosOrdenados.Count; i++ )
				{
					semaforosOrdenados[i].semaforo.WaitOne();

					if( semaforosOrdenados[i] != semaforosOrdenados.Last() )
						continue;

					semaforo.WaitOne();

					if( !semaforosPorMovimentacao.ContainsKey( oidCronogramaTarefaParaMover ) )
						semaforosPorMovimentacao.Add( oidCronogramaTarefaParaMover , new List<SemaforoPorIntervalo>( semaforosOrdenados ) );
					else
						for( int indice = 0; indice < semaforosOrdenados.Count; indice++ )
							semaforosPorMovimentacao[oidCronogramaTarefaParaMover].AddRange( semaforosOrdenados );

					semaforo.Release();
				}
			}
		}

		/// <summary>
		/// Método responsável por liberar os semáforos atrelados a ação de movimentação, após ela ser executada.
		/// </summary>
		/// <param name="oidTarefaSelecionada">Guid que serve como Key no dicionário</param>
		private void LiberarSemaforosMovimentacao( Guid oidTarefaSelecionada )
		{
			try
			{
				if( semaforosPorMovimentacao.ContainsKey( oidTarefaSelecionada ) )
				{
					List<SemaforoPorIntervalo> semaforosEmEspera = semaforosPorMovimentacao[oidTarefaSelecionada];

					semaforo.WaitOne();

					for( int i = 0; i < semaforosEmEspera.Count; i++ )
					{
						if( semaforosEmEspera[i].emEspera <= 0 )
							continue;

						semaforosEmEspera[i].semaforo.Release();
						SemaforoPorIntervalo.DecrementarContadorSemaforoEmEspera( semaforosEmEspera[i] );
					}

					semaforosPorMovimentacao.Remove( oidTarefaSelecionada );
					semaforo.Release();
				}
			}
			catch( Exception e )
			{
				string mensagemCostumizada = String.Format( "Messagem: {0} /n \n - StackTrace: {1} /n \n -" , e.Message , e.StackTrace );
				Exception novaException = new Exception( mensagemCostumizada );
				WexLogger.Error( mensagemCostumizada , e );
				throw novaException;
			}
		}

		/// <summary>
		/// Método que encerra conexão enviando uma confirmação
		/// </summary>
		public void EncerrarConexao( bool forcarAtualizacao = false )
		{
			if( contadorAcoesPendentes > 0 )
				forcarAtualizacao = true;

			accessClient.RnFinalizarConexao( forcarAtualizacao );
		}

		/// <summary>
		/// Método responsável pelo comportamento da aplicação quando for alterado via o cronograma a situação da tarefa
		/// </summary>
		/// <param name="oidSituacaoPlanejamento">oid da nova situação</param>
		public CsSituacaoPlanejamentoTipoRetorno GerenciarSituacaoPlanejamento( Guid oidSituacaoPlanejamento )
		{
			CronogramaTarefaGridItem tarefa = cronogramaView.ConsultarTarefaSelecionada();
			SituacaoPlanejamentoDTO situacaoAlterada = SituacoesPlanejamento.FirstOrDefault( o => o.Oid.Equals( oidSituacaoPlanejamento ) );

			return situacaoPlanejamentoRegrasCombo.Executar( situacaoAlterada.Oid , tarefa );
		}

		/// <summary>
		/// Limpa a ultima atualização da barra de status
		/// </summary>
		private void LimparMensagemBarraStatus()
		{
			cronogramaView.AtualizarUltimaAcao( null );
		}

		/// <summary>
		/// Método responsável por Esperar o semáforo de leitura do datasource
		/// </summary>
		public void EsperarLeituraDataSource()
		{
			gerenciadorComandos.EsperarLeituraDataSource();
		}

		/// <summary>
		/// Método responsável por Liberar o semáforo de leitura do datasource
		/// </summary>
		public void LiberarLeituraDataSource()
		{
			gerenciadorComandos.LiberarLeituraDataSource();
		}

		/// <summary>
		/// Método responsável por Esperar o semáforo de escrita do datasource
		/// </summary>
		public void EsperarEscritaDataSource()
		{
			gerenciadorComandos.EsperarEscritaDataSource();
		}

		/// <summary>
		/// Método responsável por liberar o semáforo de escrita do datasource
		/// </summary>
		public void LiberarEscritaDataSource()
		{
			gerenciadorComandos.LiberarEscritaDataSource();
		}

		/// <summary>
		/// Validar se uma tecla de atalho digitada é uma situação planejamento;
		/// </summary>
		/// <param name="atalho">tecla de atalho</param>
		/// <returns>True é uma tecla de atalho de situação planejamento, caso contrário retonará false</returns>
		public bool ValidarAtalhoSituacaoPlanejamento( string atalho )
		{
			return !string.IsNullOrEmpty( atalho ) && TeclasAtalhoSituacoesPlanejamento != null && TeclasAtalhoSituacoesPlanejamento.ContainsKey( atalho );
		}

		/// <summary>
		/// Método para processar uma tecla de atalho caso ela seja referente a uma situação planejamento
		/// </summary>
		/// <param name="atalho">tecla de atalho digitada</param>
		public void ProcessarTeclaDeAtalhoSituacaoPlanejamento( string atalho )
		{
			CronogramaTarefaGridItem tarefa = cronogramaView.ConsultarTarefaSelecionada();
			if( tarefa == null || ExisteTarefaEmEdicao() )
				return;

			if( !ValidarAtalhoSituacaoPlanejamento( atalho ) )
				return;
			var situacaoSelecionada = TeclasAtalhoSituacoesPlanejamento[atalho];
			if( situacaoSelecionada == null )
				return;

			cronogramaView.ForcarEdicaoSituacaoPlanejamentoTarefa();
			tarefa.OidSituacaoPlanejamentoTarefa = situacaoSelecionada.Oid;
		}

		/// <summary>
		/// Método que o gerenciador de comandos atualize a view
		/// </summary>
		/// <param name="podeExecutar"></param>
		public void PermitirGerenciadorComandosAtualizarView( bool podeExecutar )
		{
			gerenciadorComandos.PodeExecutar = podeExecutar;
		}

		/// <summary>
		/// Método responsável por remover uma tarefa do datasource.
		/// </summary>
		/// <param name="tarefa"></param>
		public void RemoverTarefaDataSource( CronogramaTarefaGridItem tarefa )
		{
			cronogramaView.TarefasCronograma.Remove( tarefa );
		}

		/// <summary>
		/// Método responsável por pegar o indíce da tarefana lista.
		/// </summary>
		/// <param name="tarefa">Objeto Tarefa</param>
		/// <returns>indice da tarefa na lista</returns>
		public int BuscarIndiceDaTarefaNoDataSource( CronogramaTarefaGridItem tarefa )
		{
			return cronogramaView.TarefasCronograma.IndexOf( tarefa );
		}

		/// <summary>
		/// Método que instancia um novo objeto CronogramaTarefaGridItem quando retorna os dados do serviço de criação.
		/// </summary>
		/// <param name="retornoDadosNovaTarefa">informações do serviço</param>
		/// <param name="tarefa">objeto CronogramaTarefaGridItem</param>
		/// <returns>Objeto CronogramaTarefaGridItem</returns>
		private static CronogramaTarefaGridItem InstanciarTarefaComRetornoServico( TarefaCriadaDto retornoDadosNovaTarefa , CronogramaTarefaGridItem tarefa )
		{
			tarefa = new CronogramaTarefaGridItem();
			tarefa.TxDescricaoTarefa = retornoDadosNovaTarefa.TxDescricaoTarefa;
			tarefa.TxObservacaoTarefa = retornoDadosNovaTarefa.TxObservacaoTarefa;
			tarefa.OidSituacaoPlanejamentoTarefa = retornoDadosNovaTarefa.OidSituacaoPlanejamentoTarefa;
			tarefa.TxDescricaoSituacaoPlanejamentoTarefa = retornoDadosNovaTarefa.TxDescricaoSituacaoPlanejamentoTarefa;
			tarefa.TxDescricaoColaborador = retornoDadosNovaTarefa.TxDescricaoColaborador;
			tarefa.NbEstimativaRestante = retornoDadosNovaTarefa.NbEstimativaRestante;
			tarefa.NbEstimativaInicial = retornoDadosNovaTarefa.NbEstimativaInicial;
			tarefa.NbRealizado = retornoDadosNovaTarefa.NbRealizado;
			tarefa.CsLinhaBaseSalva = retornoDadosNovaTarefa.CsLinhaBaseSalva;
			tarefa.DtInicio = retornoDadosNovaTarefa.DtInicio;
			tarefa.OidCronogramaTarefa = retornoDadosNovaTarefa.OidCronogramaTarefa;
			tarefa.OidTarefa = retornoDadosNovaTarefa.OidTarefa;
			tarefa.DtAtualizadoEm = retornoDadosNovaTarefa.DtAtualizadoEm;
			tarefa.TxAtualizadoPor = retornoDadosNovaTarefa.TxAtualizadoPor;
			return tarefa;
		}

		/// <summary>
		/// Método responsável por inserir uma tarefa no grid
		/// </summary>
		/// <param name="tarefa">objeto CronogramaTarefaGridItem</param>
		private void InserirTarefaAleatoriaGrid( CronogramaTarefaGridItem tarefa )
		{
			gerenciadorComandos.EsperarEscritaDataSource();
			cronogramaView.TarefasCronograma.Add( tarefa );
			gerenciadorComandos.LiberarEscritaDataSource();
		}

		/// <summary>
		/// Gerar Filtros Situacao Planejamento
		/// </summary>
		private void GerarFiltrosSituacaoPlanejamento()
		{
			if( cronogramaView.FiltrosSituacao == null )
				cronogramaView.FiltrosSituacao = new Dictionary<string , string>();

			cronogramaView.FiltrosSituacao.Add( "pendentes" , GerarFiltroTarefasPendentes() );
			cronogramaView.FiltrosSituacao.Add( "encerradas" , GerarFiltroTarefasEncerradas() );
			CarregarFiltroSitaucaoPlanejamento();

		}

		/// <summary>
		/// Método para carregar o filtro de situação planejamento
		/// </summary>
		public void CarregarFiltroSitaucaoPlanejamento()
		{
			var filtro = GerenciadorConfiguracao.CarregarFiltroSituacaoPlanejamento( out filtroSituacaoCustom );
			cronogramaView.AplicarFiltroSituacao( filtro , filtroSituacaoCustom );
		}

		/// <summary>
		/// Criar string filtro para tarefas pendentes
		/// </summary>
		/// <returns></returns>
		private string GerarFiltroTarefasPendentes()
		{
			var situacaoExecucao = situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Execução );
			var situacaoImpedimento = situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Impedimento );
			var situacaoPlanejamento = situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento );
			return string.Format( "[{3}] = '{0}' Or [{3}] = '{1}' Or [{3}] = '{2}'" ,
				situacaoExecucao.Oid , situacaoImpedimento.Oid , situacaoPlanejamento.Oid , NomeColuna );
		}

		/// <summary>
		/// Criar string filtro para tarefas encerradas
		/// </summary>
		/// <returns></returns>
		private string GerarFiltroTarefasEncerradas()
		{
			var situacaoCancelado = situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Cancelamento );
			var situacaoPronto = situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Encerramento );
			return string.Format( "[{2}] = '{0}' Or [{2}] = '{1}'" ,
				situacaoCancelado.Oid , situacaoPronto.Oid , NomeColuna );
		}

		/// <summary>
		/// Método responsável por retornar os valores antigos da tarefa quando uma situação planejamento é recusada
		/// </summary>
		/// <param name="decorator"></param>
		/// <param name="oidSituacaoPlanejamento"></param>
		public void RecusarSituacaoPlanejamento( CronogramaTarefaDecorator decorator , Guid oidSituacaoPlanejamento )
		{
			RetornarValoresAnterioresTarefa( decorator.OidCronogramaTarefa );
		}

		/// <summary>
		/// Responsável por chamar o método da view que notifica uma mensagem
		/// </summary>
		/// <param name="titulo"></param>
		/// <param name="mensagem"></param>
		/// <param name="alerta"></param>
		public void NotificarMensagem( string titulo , string mensagem , bool alerta = true )
		{
			if( alerta )
			{
				cronogramaView.NotificarAlerta( titulo , mensagem );
				return;
			}

			cronogramaView.NotificarMensagem( titulo , mensagem );
		}

		/// <summary>
		/// Limpa barra de status da tela do cronograma quando solicitado
		/// </summary>
		public void LimparBarraStatus()
		{
			LimparMensagemBarraStatus();
		}

		/// <summary>
		/// Responsável por chamar o método de ForcarFim Edicao de cronograma view
		/// </summary>
		public void ForcarFimEdicao()
		{
			cronogramaView.ForcarFimEdicao();
		}

		/// <summary>
		/// Método responsável por chamar o método da view que inicializa um formulario para criar historico
		/// </summary>
		/// <param name="oidSituacaoPlanejamento"></param>
		public void InicializarFormularioHistoricoView( Guid oidSituacaoPlanejamento )
		{
			cronogramaView.InicializarFormularioTarefaHistoricoView( oidSituacaoPlanejamento );
		}

		/// <summary>
		/// Método responsável por iniciar a edição dos dados do cronograma
		/// </summary>
		public void InicioEdicaoDadosCronograma()
		{
			gerenciadorEdicaoCronograma.InicioEdicaoDadosCronograma( cronogramaSelecionado );
		}

		/// <summary>
		/// Responsável por desfazer as alterações dos dados do cronograma
		/// </summary>
		/// <param name="dadosAtuais"></param>
		/// <param name="dadosAntigos"></param>
		public void DesfazerEdicaoDadosCronograma( CronogramaDto dadosAtuais , CronogramaDto dadosAntigos )
		{
			cronogramaSelecionado = dadosAntigos;
			PreencherDadosViewDoCronograma( dadosAntigos );
		}

		/// <summary>
		/// Método utilizado para salvar a edição de dados do cronograma
		/// </summary>
		/// <param name="dadosAtuais">dados atuais do cronograma</param>
		/// <param name="dadosAntigos">dados do cronograma antes da modificação</param>
		public void SalvarEdicaoDadosCronograma( CronogramaDto dadosAtuais , CronogramaDto dadosAntigos )
		{
			if( ComparadorGenerico.HouveMudancaEm( dadosAtuais , dadosAntigos ) )
			{
				if( servicoPlanejamento.EditarCronograma( dadosAtuais ) )
					PreencherDadosViewDoCronograma( dadosAtuais );

				if( ComparadorGenerico.HouveMudancaEm( dadosAtuais , dadosAntigos , o => o.DtInicio , o => o.DtFinal ) )
					AtualizarGraficoBurndown();
			}
			accessClient.RnComunicarAlteracaoDadosCronograma();

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dadosAtuais"></param>
		private void PreencherDadosViewDoCronograma( CronogramaDto dadosAtuais )
		{
			cronogramaView.DataInicio = dadosAtuais.DtInicio;
			cronogramaView.DataTermino = dadosAtuais.DtFinal;
			cronogramaView.ListarCronogramas( cronogramas , dadosAtuais.TxDescricao );
		}

		/// <summary>
		/// Método responsável por comunicar o manager que os dados do cronograma estão em edição.
		/// </summary>
		public void ComunicarInicioEdicaoDadosCronograma()
		{
			accessClient.RnComunicarInicioEdicaoDadosCronograma();
		}

		/// <summary>
		/// Método utilizado para marcar tarefas que se encontrarem em edição
		/// </summary>
		/// <param name="autoresEdicoes"></param>
		private void SinalizarEdicao( Dictionary<string , string> autoresEdicoes )
		{
			Dictionary<CronogramaTarefaGridItem, int> coresTarefas = new Dictionary<CronogramaTarefaGridItem , int>();
			CronogramaColaboradorConfigDto config;
			CronogramaTarefaGridItem tarefa;

			if( autoresEdicoes != null )
			{
				List<Guid> oidTarefasAtualizarGrid = new List<Guid>();
				foreach( var edicao in autoresEdicoes )
				{
					config = configUsuariosConectados.FirstOrDefault( o => o.Login == edicao.Value );
					tarefa = ConsultarTarefaPodOidCronogramaTarefaNoDataSource( Guid.Parse( edicao.Key ) );
					oidTarefasAtualizarGrid.Add( Guid.Parse( edicao.Key ) );
					if( config != null && tarefa != null )
					{
						tarefa.Cor = Convert.ToInt32( config.Cor );
						coresTarefas.Add( tarefa , Convert.ToInt32( config.Cor ) );
					}
				}
				cronogramaView.NotificarInicioEdicaoTarefaExterna( coresTarefas );
				cronogramaView.AtualizarTarefaEmSelecao( oidTarefasAtualizarGrid );
			}
		}

		/// <summary>
		/// Método utilizado para marcar taredas que se encontrarem em edição
		/// </summary>
		/// <param name="autoresEdicoes"></param>
		private void SinalizarEdicao( string autorEdicao , string oidCronogramaTarefa )
		{
			Dictionary<CronogramaTarefaGridItem, int> coresTarefas = new Dictionary<CronogramaTarefaGridItem , int>();
			CronogramaColaboradorConfigDto config;
			CronogramaTarefaGridItem tarefa;

			if( autorEdicao != null )
			{
				List<Guid> oidTarefasAtualizarGrid = new List<Guid>();

				config = configUsuariosConectados.FirstOrDefault( o => o.Login == autorEdicao );
				tarefa = ConsultarTarefaPodOidCronogramaTarefaNoDataSource( Guid.Parse( oidCronogramaTarefa ) );
				oidTarefasAtualizarGrid.Add( Guid.Parse( oidCronogramaTarefa ) );
				if( config != null && tarefa != null )
				{
					tarefa.Cor = Convert.ToInt32( config.Cor );
					coresTarefas.Add( tarefa , Convert.ToInt32( config.Cor ) );
				}

				cronogramaView.NotificarInicioEdicaoTarefaExterna( coresTarefas );
				cronogramaView.AtualizarTarefaEmSelecao( oidTarefasAtualizarGrid );
			}
		}

		/// <summary>
		/// Método que efetua o retorna os valores antigos da tarefa editada
		/// </summary>
		/// <param name="oidCronogramaTarefa">oid da tarefa</param>
		public void RetornarValoresAnterioresTarefa( Guid oidCronogramaTarefa )
		{
			EsperarLeituraDataSource();
			CronogramaTarefaGridItem tarefa = ConsultarTarefaPodOidCronogramaTarefaNoDataSource( oidCronogramaTarefa );
			LiberarLeituraDataSource();

			if( tarefaEmEdicao == null || tarefa == null )
				return;

			tarefa = TarefaEditada.CopiarValores( tarefaEmEdicao.TarefaAntiga , tarefa );
			cronogramaView.AtualizarTarefaEmSelecao( tarefa.OidCronogramaTarefa );
		}

		/// <summary>
		/// Método para remover a tarefa de edição
		/// </summary>
		/// <param name="oidCronogramaTarefa"></param>
		public void RemoverTarefaDeEdicao( Guid oidTarefa )
		{
			if( IgualATarefaEmEdicao( oidTarefa ) )
				tarefaEmEdicao = null;
		}

		/// <summary>
		/// Método que remove a tarefa de edição caso não esteja esperando uma resposta do servidor
		/// </summary>
		public void RemoverTarefaDeEdicao()
		{
			if( tarefaEmEdicao != null && !tarefaEmEdicao.PermissaoPendente )
			{
				tarefaEmEdicao = null;
			}
		}

		/// <summary>
		/// Método responsável por verificar se houve alguma alteração na data de inicio ou término do cronograma
		/// </summary>
		/// <param name="dataInicio">Data de início do cronogama recentemente pesquisada</param>
		/// <param name="dataTermino">Data de término do cronogama recentemente pesquisada</param>
		/// <returns>Retorna se houve alteração ou não.</returns>
		private bool VerificarSeHouveAlteracaoDatasCronograma( DateTime dataInicio , DateTime dataTermino )
		{
			if( cronogramaView.DataInicio.Date != dataInicio.Date || cronogramaView.DataTermino.Date != dataTermino.Date )
				return true;

			return false;
		}

		/// <summary>
		/// Método responsável por verificar se existem usuários online, caso existam ele valida-os e solicita que a view os adicione na mesma.
		/// </summary>
		/// <param name="usuariosOnline"></param>
		private void InserirUsuariosOnline( ref List<string> usuariosOnline )
		{
			if( usuariosOnline != null && usuariosOnline.Count > 0 )
			{
				usuariosOnline = usuariosOnline.Distinct().ToList();
				List<CronogramaColaboradorConfigDto> colaboradorConfigs = ListarConfigColaboradores( usuariosOnline );

				if( colaboradorConfigs != null && colaboradorConfigs.Count > 0 )
				{
					foreach( CronogramaColaboradorConfigDto config in colaboradorConfigs )
					{
						configUsuariosConectados.Add( config );
					}
					cronogramaView.AdicionarNovosUsuariosConectados( colaboradorConfigs );
				}
			}
		}

		/// <summary>
		/// Método utilizado para notificar a edição do nome do cronograma
		/// </summary>
		/// <param name="autorEdicao">usuário que está editando o nome do cronograma</param>
		private void NotificarEdicaoNomeCronograma( string autorEdicao )
		{
			CronogramaColaboradorConfigDto config = configUsuariosConectados.FirstOrDefault( o => o.Login == autorEdicao );
			if( config != null )
			{
				cronogramaView.NotificarInicioEdicaoDadosCronograma( Color.FromArgb( Convert.ToInt32( config.Cor ) ) );
			}
		}
	}
}
