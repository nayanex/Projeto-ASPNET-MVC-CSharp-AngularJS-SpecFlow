using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexMultiAcessManagerProject.Libs
{
	/// <summary>
	/// Interface de representação da view de controle sobre o servidor de comunicação
	/// </summary>
	internal interface IServerView
	{
		string EnderecoIp { get; set; }
		int Porta { get; set; }
		void AlterarEstadoConexao( bool status );
		void AtualizarListaEdicoesTarefas();
		void AtualizarListaUsuariosConectados( );
		void AtualizarLog( string mensagem );
		void NotificarErro( string mensagem );
	}
}
