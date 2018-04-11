using System;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.Extensions.Clone;

namespace WexProject.Schedule.Library.Libs.ControleEdicao
{
	public class GerenciadorEdicaoCronograma
	{
		#region Atributos

		/// <summary>
		/// Armazena o valor do objeto que está sendo editado
		/// </summary>
		private CronogramaDto _editado;

		/// <summary>
		/// Armazena o valor do objeto com os valores inalterados do início da edição
		/// </summary>
		private CronogramaDto _original;

		/// <summary>
		/// Armazena a classe que trabalha a edição sobre o cronograma
		/// </summary>
		private IEditorDadosCronograma _editor;

		/// <summary>
		/// Armazena o comportamento de salvar edicao do editor
		/// </summary>
		private Action<CronogramaDto , CronogramaDto> salvarEdicao;

		/// <summary>
		/// Armazena o comportamento de recusa de edição do editor
		/// </summary>
		private Action<CronogramaDto , CronogramaDto> recusarEdicao;
		#endregion Atributos

		#region Propriedades

		/// <summary>
		/// Armazena se a edição ainda está aguardando retorno
		/// </summary>
		public bool AguardandoRetorno { get; private set; }

		/// <summary>
		/// Armazena se a edição está autorizada
		/// </summary>
		public bool Autorizado { get; private set; }

		/// <summary>
		/// Armazena uma cópia da versão original do objeto no início da edição
		/// </summary>
		public CronogramaDto Original { get { return _original; } }

		/// <summary>
		/// Armazena a referencia do objeto que está sendo editado
		/// </summary>
		public CronogramaDto ObjetoEmEdicao { get { return _editado; } }

		/// <summary>
		/// Armazena se a view do editor está em edição
		/// </summary>
		public bool EmEdicaoNaView { get; private set; }

		#endregion Propriedades

		#region Construtores

		/// <summary>
		/// Construtor que recebe um editor de cronograma
		/// </summary>
		/// <param name="editor">instancia de um editor de alterações em dados do cronograma</param>
		public GerenciadorEdicaoCronograma( IEditorDadosCronograma editor )
		{
			_editor = editor;
			salvarEdicao = _editor.SalvarEdicaoDadosCronograma;
			recusarEdicao = _editor.DesfazerEdicaoDadosCronograma;
		}

		#endregion Construtores

		#region Métodos

		/// <summary>
		/// Método responsável por sinalizar o fim da edição dos dados na view
		/// </summary>
		/// <param name="cronogramaAlterado">cronograma com os dados alterados</param>
		public void FimEdicaoDadosCronograma()
		{
			EmEdicaoNaView = false;
			ProcessarEstadoEdicao();
		}

		/// <summary>
		/// Método responsável por sinalizar o inicio da edição dos dados do cronograma
		/// </summary>
		public void InicioEdicaoDadosCronograma( CronogramaDto cronograma )
		{
			EmEdicaoNaView = true;
			if( AguardandoRetorno )
				return;

			AguardandoRetorno = true;
			_editado = cronograma;
			_original = cronograma.Clonar();
			_editor.ComunicarInicioEdicaoDadosCronograma();
		}

		/// <summary>
		/// Método responsável por sinalilzar se que edição foi permitida
		/// </summary>
		public void PermitirSalvarEdicao()
		{
			if( !AguardandoRetorno )
				return;

			AguardandoRetorno = false;
			Autorizado = true;
			ProcessarEstadoEdicao();
		}

		/// <summary>
		/// Método responsável por sinalizar que a edição foi recusada
		/// </summary>
		public void RecusarSalvarEdicao()
		{
			if( !AguardandoRetorno )
				return;

			AguardandoRetorno = false;
			Autorizado = false;
			ExecutarAcao( recusarEdicao );
		}

		/// <summary>
		/// Método responsável por processar o estado da edição
		/// </summary>
		public void ProcessarEstadoEdicao()
		{
			if( !EmEdicaoNaView && !AguardandoRetorno && Autorizado )
			{
				ExecutarAcao( salvarEdicao );
			}
		}

		/// <summary>
		/// Método responsável por processar a ação sobre o cronograma
		/// </summary>
		/// <param name="acao"> ação que será aplicada no cronograma</param>
		private void ExecutarAcao( Action<CronogramaDto , CronogramaDto> acao )
		{
			if( acao != null )
				acao( _editado , _original );
			RestaurarValoresPadroes();
		}

		/// <summary>
		/// Restaura os valores padrão
		/// </summary>
		private void RestaurarValoresPadroes()
		{
			_original = null;
			_editado = null;
			Autorizado = false;
		}
		#endregion Métodos
	}
}