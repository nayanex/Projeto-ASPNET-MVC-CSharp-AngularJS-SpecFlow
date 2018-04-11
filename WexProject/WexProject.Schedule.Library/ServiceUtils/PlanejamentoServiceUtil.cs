using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Collections;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Library.ServiceUtils.Interfaces;
using WexProject.Library.Libs.Delegates;
using WexProject.Schedule.Library.Helpers;
using WexProject.Library.Libs.Web.Http;
using System.Configuration;
using RestSharp;
using System.Diagnostics;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.Schedule.Library.ServiceUtils
{
	public class PlanejamentoServiceUtil : IPlanejamentoServiceUtil
	{
		public const string REST_SERVICE_PATH = "RestWebServicePath";
		public const string CRONOGRAMA_ACTION = "Cronogramas";
		public string BASE_URL { get; private set; }

		private IRestClient restClient;

		public PlanejamentoServiceUtil()
		{
			BASE_URL = ConfigurationManager.AppSettings.Get( REST_SERVICE_PATH );
			restClient = new RestClient( BASE_URL );
		}


		/// <summary>
		/// Evento disparado quando o servico concluir a solicitação de criação de uma tarefa
		/// </summary>
		public event RespostaAsyncServiceHandler AoCompletarSolicitacaoCriarNovaTarefa;

		/// <summary>
		/// Evento disparado cquando o serviço concluir a solicitação de exclusão das tarefas
		/// </summary>
		public event RespostaAsyncServiceHandler AoCompletarSolicitacaoExclusaoTarefas;

		/// <summary>
		/// Evento disparado quando o serviço concluir a reordenação das tarefas movimentadas;
		/// </summary>
		public event RespostaAsyncServiceHandler AoCompletarMovimentacaoTarefa;

		#region Constantes

		const string TAREFAS_EXCLUIDAS = "TarefasExcluidas";
		const string TAREFAS_REORDENADAS = "TarefasReordenadas";

		#endregion

		#region Consultas

        /// <summary>
        /// Método responsável por buscar uma lista de configurações de colaboradores em um determinado cronograma
        /// </summary>
        /// <param name="logins">login do colaborador atual</param>
        /// <param name="oidCronograma">oid de identificação do cronograma atual</param>
        /// <returns>lista de configurações dos colaboradores</returns>
        public List<CronogramaColaboradorConfigDto> ConsultarConfigUsuariosConectados( string[] logins, string oidCronograma )
        {
            if(( logins == null || logins.Length <= 0 ) || string.IsNullOrEmpty( oidCronograma ))
                return null;

            List<CronogramaColaboradorConfigDto> configs = new List<CronogramaColaboradorConfigDto>();

            for(int i = 0; i < logins.Count(); i++)
            {
                RestRequest requisicao = new RestRequest( "Cronogramas/ColaboradorConfig" );
                requisicao.AddParameter( "login", logins[i] );
                requisicao.AddParameter( "oidCronograma", oidCronograma );
                string retorno = restClient.Get( requisicao ).Content;

                configs.Add( JsonConvert.DeserializeObject<CronogramaColaboradorConfigDto>( retorno ) );
            }

            return configs;
        }

		/// <summary>
		/// Método responsável por acessar o serviço e buscar o valor da última hora trabalhada por um determinado colaborador.
		/// </summary>
		/// <param name="login">Login do colaborador</param>
		/// <param name="data">Data que será pesquisada a última hora de trabalho do colaborador</param>
		/// <returns>A última hora de trabalho de um colaborador</returns>
		public InicializadorEstimativaDto ConsultarHorarioUltimaTarefaDiaColaborador( string login, DateTime data )
		{
			//GET: {base_url}/TarefasHistoricosTrabalho/UltimoHistorico
			RestRequest requisicao = new RestRequest( "TarefasHistoricosTrabalho/UltimoHistorico" );
			requisicao.AddParameter( "login", login );
			requisicao.AddParameter( "data", data );
			return DeserializarResposta( restClient.Get<InicializadorEstimativaDto>( requisicao ) );
		}

		/// <summary>
		/// Método para buscar um inicializar para estimativa inicial da tarefa para o colaborador atual
		/// </summary>
		/// <param name="login">login do colaborador atual</param>
		/// <returns></returns>
		public InicializadorEstimativaDto ConsultarInicializadorEstimativaInicialColaborador( string login )
		{
			//GET: {base_url}/TarefasHistoricosTrabalho/UltimoHistorico
			RestRequest requisicao = new RestRequest( "TarefasHistoricosTrabalho/UltimoHistorico" );
			requisicao.AddParameter( "login", login );
			return DeserializarResposta( restClient.Get<InicializadorEstimativaDto>( requisicao ) );
			
		}

		/// <summary>
		/// Método responsável por acessar o serviço e buscar o histórico mais atual de uma tarefa.
		/// </summary>
		/// <returns>Objeto TarefaHistoricoTrabalhoDto</returns>
		public TarefaHistoricoTrabalhoDto ConsultarTarefaHistoricoTrabalhoAtual( Guid oidTarefa )
		{
			//GET: {base_url}/TarefasHistoricosTrabalho/UltimoHistorico
			RestRequest requisicao = new RestRequest( "TarefasHistoricosTrabalho/UltimoHistorico" );
			requisicao.AddParameter( "oidTarefa", oidTarefa );
			return DeserializarResposta( restClient.Get<TarefaHistoricoTrabalhoDto>( requisicao ) );
		}

		/// <summary>
		/// Método responsável por acessar o serviço e buscar uma situação de planejamento padrão.
		/// </summary>
		/// <returns>Objeto SituaçãoplanejamentopadrãoDTO</returns>
		public SituacaoPlanejamentoDTO ConsultarSituacaoPlanejamentoPadrao()
		{
			//GET: {base_url}/SituacoesPlanejamento?padrao=true
			RestRequest requisicao = new RestRequest( "SituacoesPlanejamento" );
			requisicao.AddParameter( "padrao", true );
			return DeserializarResposta( restClient.Get<SituacaoPlanejamentoDTO>( requisicao ) );
		}

		/// <summary>
		/// Método responsável por acessar o serviço e buscar todos os cronogramas.
		/// </summary>
		/// <returns>Lista de cronogramas</returns>
		public List<CronogramaDto> ListarCronogramas()
		{
			//GET: {base_url}/Cronogramas
			RestRequest requisicao = new RestRequest( CRONOGRAMA_ACTION );
			return DeserializarResposta( restClient.Get<List<CronogramaDto>>( requisicao ) );
		}

		/// <summary>
		/// Método responsável por acessar o serviço e buscar todas as situações de planejamento
		/// </summary>
		/// <returns>Lista de Situações de Planejamento</returns>
		public List<SituacaoPlanejamentoDTO> ConsultartSituacoesPlanejamento()
		{
			//GET: {base_url}/SituacoesPlanejamento
			RestRequest requisicao = new RestRequest( "SituacoesPlanejamento" );
			return DeserializarResposta( restClient.Get<List<SituacaoPlanejamentoDTO>>( requisicao ) );
		}

		/// <summary>
		/// Método responsável por acessar o serviço e buscar todas as situações de planejamento inativas
		/// </summary>
		/// <returns>Lista de Situações de Planejamento inativas</returns>
		public List<SituacaoPlanejamentoDTO> ConsultarSituacoesInativas()
		{
			//GET: {base_url}/SituacoesPlanejamento?inativas=true
			RestRequest requisicao = new RestRequest( "SituacoesPlanejamento" );
			requisicao.AddParameter( "inativas", true );
			return DeserializarResposta( restClient.Get<List<SituacaoPlanejamentoDTO>>( requisicao ) );
		}

		/// <summary>
		/// Método responsável por acessar o serviço e buscar todas as situações de planejamento tipadas.
		/// </summary>
		/// <returns>Lista de Situações de Planejamento inativas</returns>
		public List<SituacaoPlanejamentoDTO> ConsultarSituacoesPlanejamentoTipadas()
		{
			//GET: {base_url}/SituacoesPlanejamento
			RestRequest requisicao = new RestRequest( "SituacoesPlanejamento" );
			return DeserializarResposta( restClient.Get<List<SituacaoPlanejamentoDTO>>( requisicao ) );
		}

		/// <summary>
		/// Método responsável por acessar o serviço e buscar todos os colaboradores.
		/// </summary>
		/// <returns>Retorna uma lista de colaboradores dto</returns>
		public List<ColaboradorDto> ListarColaboradores()
		{
			//GET: {base_url}/Colaboradores
			RestRequest requisicao = new RestRequest( "Colaboradores" );
			return DeserializarResposta( restClient.Get<List<ColaboradorDto>>( requisicao ) );
		}

		/// <summary>
		/// Metódo responsável por buscar o último cronograma de um usuário através do login do usuário
		/// </summary>
		/// <param name="login">Login do usuário</param>
		/// <returns>Objeto Dto de Cronograma</returns>
		public CronogramaDto ConsultarUltimoCronogramaSelecionado( string login )
		{
			//GET: {base_url}/Cronogramas/UltimoSelecionadoPor/?login={login}
			RestRequest requisicao = new RestRequest( "Cronogramas/UltimoSelecionadoPor" );
			requisicao.AddParameter( "login", login );
			return DeserializarResposta( restClient.Get<CronogramaDto>( requisicao ) );
		}

		/// <summary>
		/// Método responsável por efetuar a busca de um cronograma pelo nome
		/// </summary>
		/// <param name="nomeCronograma">nome do cronograma atual</param>
		/// <returns>Cronograma do nome selecionado</returns>
		public CronogramaDto ConsultarCronogramaPorNome( string nomeCronograma )
		{
			//GET: {base_url}/Cronogramas/?nomeCronograma={nomeCronograma}
			RestRequest requisicao = new RestRequest( "Cronogramas" );
			requisicao.AddParameter( "nomeCronograma", nomeCronograma );
			return DeserializarResposta( restClient.Get<CronogramaDto>( requisicao ) );
		}

		/// <summary>
		/// Responsável por buscar uma lista de alterações efetuadas em uma determinada tarefa por oidTarefa
		/// </summary>
		/// <param name="oidTarefa">oid da tarefa</param>
		/// <returns> lista de alterações da tarefa</returns>
		public List<TarefaLogAlteracaoDto> ConsultarTarefaLogAlteracaoPorOid( string oidTarefa )
		{
			RestRequest requisicao = new RestRequest( "Cronogramas/LogTarefas" );
			requisicao.AddParameter( "oidTarefa", oidTarefa );
			return DeserializarResposta( restClient.Get<List<TarefaLogAlteracaoDto>>( requisicao ) );
		}

		/// <summary>
		/// método  responsável por buscar tarefas pesquisando por uma lista de oidTarefas
		/// </summary>
		/// <param name="oidCronogramaTarefas">lista de oid das tarefas pesquisadas</param>
		/// <returns></returns>
		public List<CronogramaTarefaGridItem> ConsultarCronogramaTarefasPorOid( List<string> oidCronogramaTarefas )
		{
			//PUT: {base_url}/Cronogramas/ConsultarTarefas
			RestRequest requisicao = new RestRequest( "Cronogramas/ConsultarTarefas" );
			requisicao.RequestFormat = DataFormat.Json;
			requisicao.AddBody( oidCronogramaTarefas );
			return DeserializarResposta( restClient.Put<List<CronogramaTarefaGridItem>>( requisicao ) );
		}

		/// <summary>
		/// Método responsável por buscar um cronogramaTarefa pelo Oid
		/// </summary>
		/// <param name="oidCronogramaTarefa">Oid CronogramaTarefa</param>
		/// <returns>Objeto CronogamaTarefa em formato JSON</returns>
		public CronogramaTarefaGridItem ConsultarCronogramaTarefaPorOid( Guid oidCronogramaTarefa )
		{
			//GET: {base_url}/Cronogramas/Tarefas/?oidCronogramaTarefa={oidCronogramaTarefa}
			RestRequest requisicao = new RestRequest( "Cronogramas/Tarefas" );
			requisicao.AddParameter( "oidCronogramaTarefa", oidCronogramaTarefa );
			return DeserializarResposta( restClient.Get<CronogramaTarefaGridItem>( requisicao ) );
		}

		/// <summary>
		/// Método responsável por buscar todas as taredas pelo Oid do Cronograma
		/// </summary>
		/// <param name="oidCronograma">Oid Cronograma</param>
		/// <returns>Lista contendo todas as tarefas contendo de um respectivo cronograma</returns>
		public List<CronogramaTarefaGridItem> ConsultarCronogramaTarefasPorOidCronograma( Guid oidCronograma )
		{
			//GET: {base_url}/Cronogramas/Tarefas/?oidCronograma={oidCronograma}
			RestRequest requisicao = new RestRequest( "Cronogramas/Tarefas" );
			requisicao.AddParameter( "id", oidCronograma );
			return DeserializarResposta<List<CronogramaTarefaGridItem>>( restClient.Get( requisicao ) );
		}

        /// <summary>
        /// Método responsável por consumir do serviço os dados do gráfico de Burndown
        /// </summary>
        /// <param name="oidCronograma">oid de identificação do cronograma </param>
        /// <returns></returns>
        public BurndownGraficoDto ConsultarDadosGraficoBurndown( Guid oidCronograma )
        {
            //GET: {base_url}/Cronogramas/Burndown/?oidCronograma={oidCronograma}
            RestRequest requisicao = new RestRequest( "Cronogramas/Burndown" );
            requisicao.AddParameter( "oidCronograma", oidCronograma );

            return DeserializarResposta<BurndownGraficoDto>( restClient.Get( requisicao ) );
        }

		#endregion

		#region Regras de Negócio

        /// <summary>
        /// Método responsável por efetuar a seleção de uma cor para um colaborador no cronograma atual
        /// </summary>
        /// <param name="login">login do colaborador atual</param>
        /// <param name="oidCronograma">oid do cronograma atual</param>
        /// <returns></returns>
        public void EscolherCorColaborador( string login, string oidCronograma )
        {
            if(!string.IsNullOrEmpty( login ) && !string.IsNullOrEmpty( oidCronograma ))
            {
                CronogramaColaboradorConfigDto dto = new CronogramaColaboradorConfigDto() { Login = login, OidCronograma = Guid.Parse( oidCronograma ) };
				RestRequest requisicao = new RestRequest( "Cronogramas/ColaboradorConfig" );
				requisicao.RequestFormat = DataFormat.Json;
                requisicao.AddBody( dto );
                restClient.Put( requisicao );
            }
        }

		/// <summary>
		/// Método responsável 
		/// </summary>
		/// <param name="oidTarefas"></param>
		/// <returns></returns>
		public void ExcluirTarefas( List<Guid> oidTarefas, Guid oidCronograma )
		{
			//PUT: {base_url}/Cronogramas/ExcluirTarefa
			RestRequest requisicao = new RestRequest( "Cronogramas/ExcluirTarefa" );
			requisicao.RequestFormat = DataFormat.Json;
			requisicao.AddBody( new TarefaExclusaoDto() { OidCronograma = oidCronograma, OidsCronogramaTarefas = oidTarefas } );
			restClient.PutAsync<TarefasExcluidasDto>( requisicao, ExcluirTarefasCallBack );
		}

		/// <summary>
		/// Método callback executado ao fim do processamento de excluir tarefas no serviço
		/// </summary>
		/// <param name="resultado"></param>
		private void ExcluirTarefasCallBack( IRestResponse<TarefasExcluidasDto> resultado, RestRequestAsyncHandle handler )
		{
			if(resultado.ResponseStatus == ResponseStatus.Completed)
			{
				string json = resultado.Content;
				TarefasExcluidasDto tarefas = DeserializarResposta( resultado );

				if(tarefas != null)
					if(AoCompletarSolicitacaoExclusaoTarefas != null)
						AoCompletarSolicitacaoExclusaoTarefas( tarefas );

			}
		}

		/// <summary>
		/// Método responsável por acessar o serviço e requerer a movimentação de tarefas.
		/// </summary>
		/// <param name="oidCronogramaTarefaSelecionada">Oid da tarefa selecionada</param>
		/// <param name="oidCronogramaTarefaDestino">Oid da tarefa destino</param>
		/// <returns>Dicionário contendo as tarefas impactadas</returns>
		public void MoverTarefa( Guid oidCronogramaTarefaSelecionada, short nbIDDestino )
		{
			RestRequest requisicao = new RestRequest( "Cronogramas/MoverTarefa" );
			requisicao.AddParameter( "oidTarefaSelecionada", oidCronogramaTarefaSelecionada );
			requisicao.AddParameter( "nbIdDestino", nbIDDestino );
			restClient.PutAsync<TarefasMovidasDto>( requisicao, MoverTarefaCallBack );
		}

		/// <summary>
		/// Método callback executado ao fim do processamento de mover tarefa no serviço
		/// </summary>
		/// <param name="resultado">resposta da consulta http assincrona</param>
		private void MoverTarefaCallBack( IRestResponse<TarefasMovidasDto> resultado, RestRequestAsyncHandle handler )
		{
			if(resultado.ResponseStatus == ResponseStatus.Completed)
			{
				TarefasMovidasDto tarefasMovidasDto = DeserializarResposta( resultado );
				if(tarefasMovidasDto != null && tarefasMovidasDto.TarefasImpactadas != null)
				{
					if(AoCompletarMovimentacaoTarefa != null)
						AoCompletarMovimentacaoTarefa( tarefasMovidasDto );
				}
			}
		}

		/// <summary>
		/// Método responsável por acessar o serviço e solicitar a criação de um histórico para uma determinada tarefa.
		/// </summary>
		/// <param name="oidTarefa">Oid da Tarefa</param>
		/// <param name="login">Login do usuário</param>
		/// <param name="nbHoraRealizado">Horas realizadas na atividade.</param>
		/// <param name="dtRealizado">Data de realização da atividade.</param>
		/// <param name="nbHoraInicial">Hora Inicial da atividade</param>
		/// <param name="nbHoraFinal">Hora Final da atividade</param>
		/// <param name="txComentario">Comentário da atividade</param>
		/// <param name="nbHoraRestante">Horas restantes da atividade</param>
		/// <param name="oidSituacaoPlanejamento">Oid da Situação Planejamento da atividade</param>
		/// <param name="txJustificativaReducao">Justificativa de redução da atividade</param>
		public void CriarHistoricoTarefa( Guid oidTarefa, string login, TimeSpan nbHoraRealizado, DateTime dtRealizado, TimeSpan nbHoraInicial,
												  TimeSpan nbHoraFinal, string txComentario, TimeSpan nbHoraRestante, Guid oidSituacaoPlanejamento, string txJustificativaReducao )
		{

			CriarHistoricoTarefaDto dto = new CriarHistoricoTarefaDto();
			dto.Autor = login;
			dto.OidTarefa = oidTarefa;
			dto.NbHoraRealizado = nbHoraRealizado.Ticks;
			dto.NbHoraFinal = nbHoraFinal.Ticks;
			dto.DtRealizado = dtRealizado;
			dto.NbHoraInicial = nbHoraInicial.Ticks;
			dto.Comentario = txComentario;
			dto.JustificativaReducao = txJustificativaReducao;
			dto.NbHoraRestante = nbHoraRestante.Ticks;
			dto.OidSituacaoPlanejamento = oidSituacaoPlanejamento;

			//POST: {base_url}/TarefasHistoricosTrabalho/
			RestRequest requisicao = new RestRequest( "TarefasHistoricosTrabalho" );
			requisicao.RequestFormat = DataFormat.Json;
			requisicao.AddBody( dto );
			restClient.Post( requisicao );
		}

		/// <summary>
		/// Método responsável por criar uma instância de cronograma e retornar para a tela
		/// </summary>
		/// <returns>Objeto Dto de Cronograma</returns>
		public CronogramaDto CriarCronogramaPadrao()
		{
			//POST: {base_url}/Cronogramas/
			RestRequest requisicao = new RestRequest( "Cronogramas" );
			return DeserializarResposta( restClient.Post<CronogramaDto>( requisicao ) );
		}

		/// <summary>
		/// Método responsável por criar uma nova tarefa e retornar dados dessa tarefa e as tarefas impactadas por esta ação.
		/// </summary>
		/// <param name="oidCronograma">Oid do Cronograma</param>
		/// <param name="txDescricao">Descrição da Tarefa</param>
		/// <param name="txObservacao">Observação da Tarefa</param>
		/// <param name="oidSituacao">Oid Situação Planejamento</param>
		/// <param name="responsaveis">Responsáveis pela Tarefa</param>
		/// <param name="nbEstimativaInicial">Estimativa Inicial da Tarefa</param>
		/// <param name="dtInicio">Data Início da Tarefa</param>
		/// <param name="oidTarefaSelecionada">Oid da Tarefa Selecionada naquele momento em que a tarefa estava sendo criada</param>
		/// <returns>Objeto Dto contendo dados da tarefa e a lista de tarefa impactadas</returns>
		public void CriarNovaTarefa( Guid oidCronograma, string txDescricao = "", string oidSituacao = "", DateTime dtInicio = new DateTime(), string responsaveis = "", string login = "",
													  string txObservacao = "", Int16 nbEstimativaInicial = 0, short nbIDReferencia = 0 )
		{
			//POST: {base_url}/Cronogramas/Tarefas/
			TarefaCriacaoDto tarefaCriada = new TarefaCriacaoDto();
			tarefaCriada.OidCronograma = oidCronograma;
			tarefaCriada.OidSituacaoPlanejamentoTarefa = oidSituacao;
			tarefaCriada.NbIdReferencia = nbIDReferencia;
			tarefaCriada.NbEstimativaInicial = nbEstimativaInicial;
			tarefaCriada.TxDescricaoTarefa = txDescricao;
			tarefaCriada.DtInicio = dtInicio;
			tarefaCriada.TxResponsaveis = responsaveis;
			tarefaCriada.AutorCriacao = login;
			tarefaCriada.TxObservacaoTarefa = txObservacao;

			RestRequest requisicao = new RestRequest( "Cronogramas/Tarefas" );
			requisicao.RequestFormat = DataFormat.Json;
			requisicao.AddBody( tarefaCriada );

			restClient.PostAsync<TarefaCriadaDto>( requisicao, CriarNovaTarefaCallBack );
		}

		/// <summary>
		/// Método responsável pelo comportamento ao receber o retorno assincrono da criação da nova tarefa
		/// </summary>
		/// <param name="response"></param>
		/// <param name="handle"></param>
		private void CriarNovaTarefaCallBack( IRestResponse<TarefaCriadaDto> response, RestRequestAsyncHandle handle )
		{
			if(response.ResponseStatus == ResponseStatus.Completed)
			{
				TarefaCriadaDto novaTarefa = DeserializarResposta( response );
				if(novaTarefa != null)
					if(AoCompletarSolicitacaoCriarNovaTarefa != null)
						AoCompletarSolicitacaoCriarNovaTarefa( novaTarefa );
			}
		}

		/// <summary>
		/// Método responsável por solicitar ao serviço a edição de uma tarefa.
		/// </summary>
		/// <param name="oidTarefa">Oid (Guid) da tarefa a ser editada</param>
		/// <param name="txDescricao">Descrição da tarefa alterada</param>
		/// <param name="oidSituacaoPlanejamento">Oid (Guid) da tarefa editada</param>
		/// <param name="dataInicio">Data de Inicio da tarefa editada</param>
		/// <param name="login">Login do usuário que editou a tarefa</param>
		/// <param name="txObservacao">Observação da tarefa editada</param>
		/// <param name="responsaveis">responsáveis pela tarefa</param>
		/// <param name="nbEstimativaInicial">Estimativa inicial da tarefa</param>
		/// <param name="nbEstimativaRestante">Estimativa restante da tarefa</param>
		/// <param name="nbRealizado">Horas realizadas da tarefa</param>
		/// <param name="csLinhaBaseSalva">Boolean afirmando se a tarefa foi salva a linda de base</param>
		/// <returns>Retorna dados da edicao como atualizado em, atualizado por e confirmação da edicao</returns>
		public Hashtable EditarTarefa( CronogramaTarefaDto tarefa )
		{
			//PUT: {base_url}/Cronogramas/Tarefas/
			RestRequest requisicao = new RestRequest( "Cronogramas/Tarefas" );
			requisicao.RequestFormat = DataFormat.Json;
			requisicao.AddBody( tarefa );
			return DeserializarResposta( restClient.Put<Hashtable>( requisicao ) );
		}

		/// <summary>
		/// Método responsável por acessar o serviço e salvar o último cronograma selecionado para aquele usuário.
		/// </summary>
		/// <param name="login">Login do usuário</param>
		/// <param name="oidCronograma">Oid do cronograma selecionado</param>
		public void SalvarUltimoCronogramaSelecionado( string login, string oidCronograma )
		{
			//PUT: {base_url}/Cronogramas/UltimoSelecionadoPor?login={login}&oidCronograma=?{oidCronograma}
			RestRequest requisicao = new RestRequest( "Cronogramas/UltimoSelecionadoPor" );
			requisicao.RequestFormat = DataFormat.Json;
			requisicao.AddBody( new { login = login, oidCronograma = oidCronograma } );
			restClient.Put( requisicao );
		}


		/// <summary>
		/// Método responsável por acionar o serviço e solicitar a exclusão do cronograma.
		/// </summary>
		/// <param name="oidCronograma">Oid (Guid) do cronograma</param>
		/// <returns>Boolean confirmando se o cronograma foi excluido ou não.</returns>
		public bool ExcluirCronograma( string oidCronograma )
		{
			//DELETE: {base_url}/Cronogramas/?id={oidCronograma}
			RestRequest requisicao = new RestRequest( CRONOGRAMA_ACTION );
			requisicao.AddParameter( "id", oidCronograma );
			return DeserializarResposta( restClient.Delete<bool>( requisicao ) );
		}


		/// <summary>
		/// Método responsável por efetuar a alteração no nome do cronograma
		/// </summary>
		/// <param name="oidCronograma">oid do cronograma com nome a ser alterado</param>
		/// <param name="novaDescricao">novo nome do cronograma</param>
        public bool EditarCronograma( CronogramaDto cronograma )
		{
			//PUT: {base_url}/Cronogramas/
			RestRequest requisicao = new RestRequest( "Cronogramas" );
			requisicao.RequestFormat = DataFormat.Json;
            requisicao.AddBody( cronograma );
			return DeserializarResposta( restClient.Put<bool>( requisicao ) );
		}
		#endregion

		/// <summary>
		/// Método utilizado para efetuar a deserializacao do resultado
		/// </summary>
		/// <typeparam name="TResult">Tipo do objeto a ser deserializado</typeparam>
		/// <param name="response">resposta da requisição Http</param>
		/// <returns>Retorna o objeto deserializado</returns>
		private static TResult DeserializarResposta<TResult>( IRestResponse<TResult> response )
		{
			return JsonConvert.DeserializeObject<TResult>( response.Content );
		}

		/// <summary>
		/// Método utilizado para efetuar a deserializacao do resultado
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="response"></param>
		/// <returns></returns>
		private TResult DeserializarResposta<TResult>( IRestResponse response )
		{
			return JsonConvert.DeserializeObject<TResult>( response.Content );
		}
	}
}
