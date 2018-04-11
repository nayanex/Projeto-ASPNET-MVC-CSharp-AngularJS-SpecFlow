using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.Schedule.Library.Domains;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Library.Views.Interfaces
{
	public interface ICronogramaView
	{
		#region Propriedades

		/// <summary>
		/// Atributo responsável por identificar se deve ou não haver a confirmação para fechar o cronograma
		/// -Obs.: Não deverá solicitar confirmação quando o cronograma for fechado de forma automática Ex.: tentar conectar com o servidor desligado
		/// </summary>
		bool ExibeConfirmacaoAoFechar { get; set; }

		/// <summary>
		/// Armazenar localmente os filtros de situação planejamento
		/// </summary>
		Dictionary<string , string> FiltrosSituacao { get; set; }

		/// <summary>
		/// Responsável por armazenar as linhas sinalizadas com "X",selecionadas para exclusão
		/// </summary>
		List<Guid> LinhasParaExcluir { get; set; }

		/// <summary>
		/// Responsável por armazenar o nome do cronograma
		/// </summary>
		string NomeCronograma { get; set; }

		/// <summary>
		/// Responsável por armazenar a data de inicio do cronograma
		/// </summary>
		DateTime DataInicio { get; set; }

		/// <summary>
		/// Responsável por armazenar a data de encerramento do cronograma
		/// </summary>
		DateTime DataTermino { get; set; }

		/// <summary>
		/// Responsável por armazenar a situação atual do cronograma
		/// </summary>
		string SituacaoCronograma { get; set; }

		/// <summary>
		/// Responsável por armazenar todas tarefas do cronograma atual
		/// </summary>
		BindingList<CronogramaTarefaGridItem> TarefasCronograma { get; set; }

		#endregion Propriedades

		#region Métodos

		/// <summary>
		/// Método responsável por adicionar um novo usuários conectado na barra de usuários conectados
		/// </summary>
		/// <param name="configs">configurações do colaborador (login/nome/cor/email/foto)</param>
		void AdicionarNovosUsuariosConectados( List<CronogramaColaboradorConfigDto> configs );

		/// <summary>
		/// Aplicar um filtro de seleção para as situações planejamento das tarefas
		/// </summary>
		/// <param name="filtro">filtro aplicado</param>
		/// <param name="filtroCustom">filtro costumizado</param>
		void AplicarFiltroSituacao( CsFiltroSituacaoPlanejamento filtro , string filtroCustom );

		/// <summary>
		/// Atualiza os dados do cronograma selecionado
		/// </summary>
		/// <param name="dto">Dto do cronograma selecionado</param>
		void AtualizarCronogramaSelecionado( CronogramaDto dto );

		/// <summary>
		/// Atualizar o grafico de burndown
		/// </summary>
		/// <param name="graficoDto"></param>
		void AtualizarGraficoBurndown( BurndownGraficoDto graficoDto );

		/// <summary>
		/// Método responsável por atualizar o hint da identificação dos usuários conectados
		/// </summary>
		/// <param name="nomeColaborador">nome do colaborador atual</param>
		/// <param name="acao">ação do colaborador (Modificação efetuada no cronograma ou tarefa)</param>
		/// <param name="quando"></param>
		void AtualizarHintColaborador( string nomeColaborador , string acao , DateTime quando );

		/// <summary>
		/// Método responsável por atualizar o nome do cronograma
		/// </summary>
		/// <param name="NovoNome">novo nome do cronograma</param>
		void AtualizarNomeCronograma( string NovoNome );

		/// <summary>
		/// Método responsável por atualizar na view a tarefa do oid selecionado
		/// </summary>
		/// <param name="oidTarefaSelecionada">identificador da tarefa atual</param>
		void AtualizarTarefaEmSelecao( Guid oidTarefaSelecionada );

		/// <summary>
		/// Método responsável por atualizar na view a seleção de tarefas atuais
		/// </summary>
		/// <param name="oidTarefasSelecionadas">lista com a identificação das tarefas selecionadas</param>
		void AtualizarTarefaEmSelecao( List<Guid> oidTarefasSelecionadas );

		/// <summary>
		/// método responsável por atualizar na view a última ação promovida pelo proprio colaborador
		/// </summary>
		/// <param name="atualizacao"></param>
		void AtualizarUltimaAcao( string atualizacao );

		/// <summary>
		/// Método responsável por atualizar a mensagem de ultima alteração causada por outro usuário
		/// </summary>
		/// <param name="notificacao">mensagem de atualização</param>
		void AtualizarUltimaAlteracaoCronograma( string notificacao );

		/// <summary>
		/// Método para efetuar a atualização da view de tarefas
		/// </summary>
		void AtualizarView();

		/// <summary>
		/// Método utilizado para definir a visibilidade dos botões da barra
		/// </summary>
		/// <param name="visibilidade"></param>
		void AtualizarVisibilidadeBotoesBarra( bool visibilidade );

		/// <summary>
		/// Método responsável por efetuar a solicitação de confirmação de excluão ao usuário do cronograma
		/// </summary>
		/// <returns></returns>
		bool ConfirmarExclusaoCronograma();

		/// <summary>
		/// Método responsável por efetuar a solicitação de confirmação de excluão ao usuário
		/// </summary>
		/// <returns></returns>
		bool ConfirmarExclusaoTarefas();

		/// <summary>
		/// Método responsável por desabilitar a view de edição de tarefas quando desconectado do servidor
		/// </summary>
		void DesabilitarViewTarefas( bool parcial = false );

		/// <summary>
		/// Método responsável pelo comportamento de execução quando o cronograma se conectador com o servidor
		/// </summary>
		void ExecutarAoConectar();

		/// <summary>
		/// Método responsável pelo comportamento de execuçãi quando o cronograma perder a conexão com o servidor
		/// </summary>
		void ExecutarAoDesconectar();

		/// <summary>
		/// Método responsável por preencher o componente com a descrição dos colaboradores (responsáveis)
		/// </summary>
		/// <param name="colaboradores"></param>
		void ExibirColaboradoresResponsaveis( List<ColaboradorDto> colaboradores );

		/// <summary>
		/// Método responsável por exibir as tarefas na view
		/// </summary>
		void ExibirTarefas();

		/// <summary>
		/// método responsável por exibir na view as tarefas do cronograma atual
		/// </summary>
		/// <param name="tarefas"></param>
		void ExibirTarefas( BindingList<CronogramaTarefaGridItem> tarefas );

		/// <summary>
		/// Método responsável pela tentativa de fechar a view (ação pode ser cancelada)
		/// </summary>
		void Fechar();

		/// <summary>
		/// Força a edição sobre uma situação de planejamento
		/// </summary>
		void ForcarEdicaoSituacaoPlanejamentoTarefa();

		void ForcarFimEdicao();

		void ForcarFimEdicaoDadosCronograma();

		/// <summary>
		/// método responsável por retornar a tarefa da posição selecionada
		/// </summary>
		/// <param name="posicao">posição da tarefa</param>
		/// <returns>tarefa selecionada</returns>
		CronogramaTarefaGridItem ConsultarTarefaPorPosicaoSelecionada( int posicao );

		/// <summary>
		/// método responsável por retornar a tarefa selecionada na view
		/// </summary>
		/// <returns></returns>
		CronogramaTarefaGridItem ConsultarTarefaSelecionada();

		void HabilitarAoConectar();

		/// <summary>
		/// Método responsável por habilitar e desabilitar os botões do cronograma(ao conectar || ao desconectar )
		/// </summary>
		/// <param name="estado">estado habilitado ou desabilitado</param>
		void HabilitarBotoes( bool estado );

		/// <summary>
		/// Método responsável por habilitar a view para edição quando conectado
		/// </summary>
		void HabilitarViewTarefas();

		void InicializarFormularioTarefaHistoricoView( Guid oidSituacaoPlanejamento );

		/// <summary>
		/// método responsável por inserir uma nova tarefa na view
		/// </summary>
		/// <param name="tarefa">nova tarefa</param>
		/// <param name="posicao">posição escolhida</param>
		void InserirTarefaPadrao( CronogramaTarefaGridItem tarefa , int posicao );

		/// <summary>
		/// método responsável por exibir a listagem dos nomes dos cronogramas cadastrados
		/// </summary>
		/// <param name="cronogramas">lista de cronogramas</param>
		/// <param name="DescricaoCronogramaSelecionado">nome do cronograna selecionado</param>
		void ListarCronogramas( List<CronogramaDto> cronogramas , string DescricaoCronogramaSelecionado );

		/// <summary>
		/// método responsável por listar e preencher o componente que armazenará as situações planejamento
		/// </summary>
		/// <param name="situacoes">lista de situações planejamentos ativas</param>
		/// <param name="situacaoPadrao">situação de planejamento padrão</param>
		void ListarSituacoesPlanejamento( List<SituacaoPlanejamentoDTO> situacoes , SituacaoPlanejamentoDTO situacaoPadrao );

		/// <summary>
		/// Método utilizado para executar um alerta
		/// </summary>
		/// <param name="caption">titulo do aviso</param>
		/// <param name="mensagem">mensagem de aviso</param>
		void NotificarAlerta( string caption , string mensagem );

		/// <summary>
		/// Método utilizado para executar um alerta de erro
		/// </summary>
		/// <param name="caption">titulo do erro</param>
		/// <param name="mensagem">mensagem de erro</param>
		void NotificarErro( string caption , string mensagem );

		/// <summary>
		/// método utilizado para sinalizar o fim de edição do nome do cronograma removendo a cor do campo
		/// </summary>
		void NotificarFimEdicaoDadosCronograma();

		/// <summary>
		/// Método que remove a notificação de que uma tarefa está sendo editada (remover a cor do colaborador que está editando)
		/// </summary>
		/// <param name="tarefasEditadas"></param>
		void NotificarFimEdicaoTarefaExterna( Dictionary<CronogramaTarefaGridItem , string> tarefasEditadas );

		/// <summary>
		/// método utilizado para sinalizar que o nome do cronograma se encontra em edição com a cor do colaborador que está editando
		/// </summary>
		/// <param name="cor"></param>
		void NotificarInicioEdicaoDadosCronograma( Color cor );

		/// <summary>
		/// Método responsável pelo comportamento da tela quando houver a comunicação de uma edição (Externa)
		/// </summary>
		/// <param name="tarefasCores">dicionario de tarefas e as respectivas cores do usuários que às estão editando</param>
		void NotificarInicioEdicaoTarefaExterna( Dictionary<CronogramaTarefaGridItem , int> tarefasCores );

		/// <summary>
		/// Método utilizado para notificar uma mensagem na lateral inferior da tela
		/// </summary>
		/// <param name="titulo">titulo da mensagem</param>
		/// <param name="mensagem">mensagem</param>
		void NotificarMensagem( string titulo , string mensagem );

		/// <summary>
		/// Método utilizado para comunicar uma mensagem na lateral inferior com a utilização de foto do colaborador que efetuou a ação
		/// </summary>
		/// <param name="titulo">titulo da mensagem</param>
		/// <param name="mensagem">mensagem de ação do colaborador</param>
		/// <param name="foto"></param>
		void NotificarMensagemComFoto( string titulo , string mensagem , byte[] foto );

		/// <summary>
		/// Método responsável por aplicar a ordenação as tarefas (Ordenação por ID)
		/// </summary>
		void Ordenar();

		/// <summary>
		/// método responsável por remover a cor de uma tarefa de edição através da cor
		/// </summary>
		/// <param name="cor"></param>
		void RemoverCorEdicao( int? cor );

		/// <summary>
		/// métoro responsável por remover a cor de uma tarefa de edição através de oid da tarefa
		/// </summary>
		/// <param name="oid"></param>
		void RemoverCorEdicao( Guid oid );

		/// <summary>
		/// método responsável por remover o foco de todas tarefas
		/// </summary>
		void RemoverFocoTarefas();

		/// <summary>
		/// método responsável por remover de edição uma tarefa que possui a edição recusada (já se encontra em edição por outro colaborador)
		/// </summary>
		/// <param name="oid">oid da tarefa recusada</param>
		void RemoverTarefaRecusadaDeEdicao( Guid oid );

		/// <summary>
		///
		/// </summary>
		/// <param name="oidTarefasExcluidas"></param>
		void RemoverTarefas( List<Guid> oidTarefasExcluidas );

		/// <summary>
		/// Método responsável por remover da lista de usuários conectados  todos colaboradores que saíram do cronograma
		/// </summary>
		/// <param name="usuariosDesconectados"></param>
		void RemoverUsuariosDesconectados( List<string> usuariosDesconectados );

		/// <summary>
		/// Método responsável por remover a ordenação das tarefas
		/// </summary>
		void RetirarOrdenacao();

		/// <summary>
		/// Seta o foco para uma tarefa
		/// </summary>
		/// <param name="posicao">posicao do foco</param>
		void AtribuirFocoTarefa( CronogramaTarefaGridItem cronogramaTarefaGridItem );

        /// <summary>
        /// Armazena a visibilidade do gráfico de burndown
        /// </summary>
        bool BurndownVisivel { get; set; }

		#endregion
	}
}
