using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using WexMultiAcessManagerProject.Libs;

namespace WexMultiAcessManagerProject
{
	public partial class Form1 :Form , IServerView
	{

		private object _usuarioUpdaterLocker;
		private object _edicoesUpdaterLocker;
		/// <summary>
		/// Responsável pelo controle sobre o servidor
		/// </summary>
		ServerPresenter presenter;
		public Form1()
		{
			InitializeComponent();
			txEnderecoIp.Enabled = false;
			txEnderecoIp.Text = GetEnderecoIp().ToString();
			presenter = new ServerPresenter( this );
			usuarioConectadoBindingSource.DataSource = presenter.UsuariosConectados;
			edicaoTarefaBindingSource.DataSource = presenter.TarefasEmEdicao;
			dataGridView1.DataSource = usuarioConectadoBindingSource;
			edicaoTarefaDataGridView.DataSource = edicaoTarefaBindingSource;
			_usuarioUpdaterLocker = new object();
			_edicoesUpdaterLocker = new object();
		}

		private void Form1_Load( object sender , EventArgs e )
		{
		}

		/// <summary>
		/// Método para procurar o endereço ip da máquina local
		/// </summary>
		/// <returns></returns>
		private IPAddress GetEnderecoIp()
		{
			string nomeServidor;
			IPHostEntry listaIps;
			nomeServidor = Dns.GetHostName();
			listaIps = Dns.GetHostEntry( nomeServidor );
			return listaIps.AddressList[1];
		}

		private void btConectar_Click( object sender , EventArgs e )
		{
			presenter.Conectar();
		}


		/// <summary>
		/// Comportamento executado ao fechar o formulário
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosing( object sender , FormClosingEventArgs e )
		{
			presenter.Desconectar();
		}

		private void btDesconectar_Click( object sender , EventArgs e )
		{
			presenter.Desconectar();
		}

		public string EnderecoIp
		{
			get
			{
				return txEnderecoIp.Text;
			}
			set
			{
				txEnderecoIp.Text = value;
			}
		}

		public int Porta
		{
			get
			{
				return Convert.ToInt16( txPort.Text );
			}
			set
			{
				txPort.Text = value.ToString();
			}
		}

		/// <summary>
		/// Método de notificação quando ocorrer um erro na view
		/// </summary>
		/// <param name="mensagem"></param>
		public void NotificarErro( string mensagem )
		{
			BeginInvoke( new Action( () =>
			{
				MessageBox.Show( mensagem );
			} ) );
		}


		/// <summary>
		/// Método responsável por alterar a view de acordo com o estado de conexão do servidor
		/// </summary>
		/// <param name="status">estado de conexão do servidor</param>
		public void AlterarEstadoConexao( bool status )
		{
			btConectar.Enabled = !status;
			txPort.Enabled = !status;
			btDesconectar.Enabled = status;
		}

		/// <summary>
		/// Responsável por atualizar a lista de usuários conectados
		/// </summary>
		/// <param name="usuarios"></param>
		public void AtualizarListaUsuariosConectados()
		{
			Action executarAcao = ExecutarAtualizacaoUsuariosConectados;
			BeginInvoke( executarAcao );
		}

		/// <summary>
		/// Comportamento de excução de atualização do grid
		/// </summary>
		private void ExecutarAtualizacaoUsuariosConectados()
		{
			lock( _usuarioUpdaterLocker )
			{
				usuarioConectadoBindingSource.ResetBindings( false ); 
			}
		}

		/// <summary>
		/// Comportamento de excução de atualização do grid
		/// </summary>
		private void ExecutarAtualizacaoListaEdicoesTarefas()
		{
			lock( _edicoesUpdaterLocker )
			{
				edicaoTarefaBindingSource.ResetBindings( false ); 
			}
		}

		/// <summary>
		/// Responsável por atualizar a lista de tarefas em edição
		/// </summary>
		public void AtualizarListaEdicoesTarefas()
		{
			Action executarAcao = ExecutarAtualizacaoListaEdicoesTarefas;
			BeginInvoke( executarAcao );
		}

		/// <summary>
		/// Responsável por fazer a atualização do log
		/// </summary>
		/// <param name="mensagemLog">mensagem de log a ser efetuada</param>
		public void AtualizarLog( string mensagemLog )
		{
			if( string.IsNullOrWhiteSpace( mensagemLog ) )
				return;

			BeginInvoke( new Action( () => 
			{
				txLog.Text += System.Environment.NewLine + mensagemLog;
			} ) );
		}
	}
}
