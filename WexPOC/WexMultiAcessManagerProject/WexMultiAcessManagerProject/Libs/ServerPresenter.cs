using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.MultiAccess.Library;

namespace WexMultiAcessManagerProject.Libs
{
	/// <summary>
	/// Classe responsável pelo controle das operações sobre o servidor
	/// </summary>
	internal class ServerPresenter
	{
		/// <summary>
		/// Interface de controle das operações sobre o servidor
		/// </summary>
		private IServerView _view;

		/// <summary>
		/// Servidor de comunicação
		/// </summary>
		private WexMultiAccessManager wexServer;

		/// <summary>
		/// Representa o estado de conexão do servidor para controle da view
		/// </summary>
		private bool _conectado;

		/// <summary>
		/// Lista de usuários conectados até o momento
		/// </summary>
		public List<UsuarioConectado> UsuariosConectados { get; private set; }

		/// <summary>
		/// Lista de tarefas em edição
		/// </summary>
		public List<EdicaoTarefa> TarefasEmEdicao { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view">View que representa a tela de controle sobre o servidor de comunicação</param>
		public ServerPresenter( IServerView view )
		{
			_view = view;
			wexServer = new WexMultiAccessManager();
			UsuariosConectados = new List<UsuarioConectado>();
			TarefasEmEdicao = new List<EdicaoTarefa>();
			ConfigurarEventosManager();
		}

		/// <summary>
		/// Método responsável por configurar eventos do manager
		/// </summary>
		private void ConfigurarEventosManager()
		{
			// Atualização das listas
			wexServer.AoConectarNovoUsuario += wexServer_AoConectarNovoUsuario;
			wexServer.AoDesconectarUsuario += wexServer_AoDesconectarUsuario;
			wexServer.AoIniciarEdicaoTarefa += wexServer_AoIniciarEdicaoTarefa;
			wexServer.AoFinalizarEdicaoTarefa += wexServer_AoFinalizarEdicaoTarefa;
			wexServer.AoCriarNovaTarefa += wexServer_AoCriarNovaTarefa;
		}

		/// <summary>
		/// Comportamento ao criar uma nova tarefa
		/// </summary>
		/// <param name="oidCronograma"></param>
		/// <param name="login"></param>
		/// <param name="oidTarefa"></param>
		void wexServer_AoCriarNovaTarefa( string oidCronograma , string login , string oidTarefa )
		{
			_view.AtualizarLog( string.Format( "{0} criou uma nova tarefa ({1}) - {2: MM/dd/yy H:mm:ss}" , login , oidTarefa , DateTime.Now ) );
		}

		/// <summary>
		/// Comportamento de notificação quando for encerrada a edição de uma tarefa
		/// </summary>
		/// <param name="oidCronograma"></param>
		/// <param name="login"></param>
		/// <param name="oidTarefa"></param>
		void wexServer_AoFinalizarEdicaoTarefa( string oidCronograma , string login , string oidTarefa )
		{
			TarefasEmEdicao.RemoveAll( edicao => edicao.OidTarefa.ToLower() == oidTarefa.ToLower() );
			_view.AtualizarListaEdicoesTarefas();
			_view.AtualizarLog( string.Format( "{0} encerrou a edição da tarefa {1} - {2: MM/dd/yy H:mm:ss}" , login , oidTarefa , DateTime.Now ) );
		}

		/// <summary>
		/// Comportamento de notificação quando iniciar a edição de uma tarefa
		/// </summary>
		/// <param name="oidCronograma"></param>
		/// <param name="login"></param>
		/// <param name="oidTarefa"></param>
		void wexServer_AoIniciarEdicaoTarefa( string oidCronograma , string login , string oidTarefa )
		{
			TarefasEmEdicao.Add( new EdicaoTarefa() { OidCronograma = oidCronograma, Login = login , OidTarefa = oidTarefa } );
			_view.AtualizarListaEdicoesTarefas();
			_view.AtualizarLog( string.Format( "{0} iniciou a edição da tarefa {1} - {2: MM/dd/yy H:mm:ss}" , login , oidTarefa , DateTime.Now ) );
		}

		/// <summary>
		/// Comportamento de notificação de desconexão de um usuário
		/// </summary>
		/// <param name="oidCronograma"></param>
		/// <param name="login"></param>
		void wexServer_AoDesconectarUsuario( string oidCronograma , string login )
		{
			Func<string,string,bool> CompararString = ( s1 , s2 ) => s1.ToLower() == s2.ToLower();
			UsuariosConectados.RemoveAll( u => CompararString( u.OidCronograma , oidCronograma ) && CompararString( u.Login , login ) );
			_view.AtualizarListaUsuariosConectados();
			_view.AtualizarLog( string.Format( "{0} desconectou-se do cronograma {1} - {2: MM/dd/yy H:mm:ss}" , login , oidCronograma , DateTime.Now ) );

		}

		/// <summary>
		/// Comportamento do evento de notificação de conexão de um novo usuário
		/// </summary>
		/// <param name="oidCronograma"></param>
		/// <param name="login"></param>
		void wexServer_AoConectarNovoUsuario( string oidCronograma , string login )
		{
			Func<string,string,bool> CompararString = ( s1 , s2 ) => s1.ToLower() == s2.ToLower();
			if( !UsuariosConectados.Any( u => CompararString( u.OidCronograma , oidCronograma ) && CompararString( u.Login , login ) ) )
				UsuariosConectados.Add( new UsuarioConectado() { OidCronograma = oidCronograma , Login = login } );
			_view.AtualizarListaUsuariosConectados();
			_view.AtualizarLog( string.Format( "{0} conectou-se no cronograma {1} - {2: MM/dd/yy H:mm:ss}" , login , oidCronograma , DateTime.Now ) );
		}

		/// <summary>
		/// Método responsável por controlar a conexão do servidor de comunicação
		/// </summary>
		public void Conectar()
		{
			try
			{
				if( _conectado )
					return;

				wexServer.EnderecoIp = _view.EnderecoIp;
				wexServer.Porta = _view.Porta;
				wexServer.Conectar();
				_conectado = true;
				_view.AlterarEstadoConexao( _conectado );
				_view.AtualizarLog( string.Format( "Servidor Conectado {0: MM/dd/yy H:mm:ss }" , DateTime.Now ) );
			}
			catch( Exception ex )
			{
				_conectado = false;
				_view.NotificarErro( ex.Message );
				_view.AlterarEstadoConexao( _conectado );
				_view.AtualizarLog( string.Format( "Falha na inicialização do servidor {0: MM/dd/yy H:mm:ss }" , DateTime.Now ) );
			}
		}

		/// <summary>
		/// Método responsável por controlar a desconexão do servidor de comunicação
		/// </summary>
		public void Desconectar()
		{
			wexServer.Desconectar();
			_conectado = false;
			_view.AlterarEstadoConexao( _conectado );
			UsuariosConectados.Clear();
			_view.AtualizarListaUsuariosConectados();
			_view.AtualizarLog( string.Format( "Servidor Desconectado {0: MM/dd/yy H:mm:ss }" , DateTime.Now ) );
		}
	}
}
