using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using WexProject.MultiAccess.Library.Dtos;
using System.Diagnostics;
using WexProject.MultiAccess.Library.Libs;
using Newtonsoft.Json;
using WexProject.MultiAccess.Library.Domains;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using WexProject.Library.Libs.Rede;
using WexProject.MultiAccess.Library.ArgumentosEventos;
using log4net;
using System.Reflection;
using WexProject.Library.Libs.Extensions.Log;
using WexProject.Library.Libs.Delegates.Logger;
using WexProject.MultiAccess.Library.Delegates;

namespace WexProject.MultiAccess.Library
{

    public class WexMultiAccessManager
    {

        #region Atributos

        /// <summary>
        /// Responsável por armazenar os usuários conectados na aplicação
        /// </summary>
        protected Dictionary<string, List<string>> usuariosConectados;

        /// <summary>
        /// Responsável por armazenar os cronogramas e os usuários conectados em tal cronograma
        /// Key - oidCronograma
        /// Value - Hash de Usuários conectados em tal oidCronograma
        /// </summary>
        protected Dictionary<string, Dictionary<string, ConexaoCliente>> cronogramasConectados;

        /// <summary>
        /// Responsável por armazenar o  endereço do servidor
        /// </summary>
        private IPAddress enderecoIp;

        /// <summary>
        /// Thread Responsável por aguardar as novas conexões de clientes
        /// </summary>
        protected Thread threadAceitarNovosClientes;

        /// <summary>
        /// Thread Responsável por gerenciar novos eventos e mandar para as threads de comunicação
        /// </summary>
        protected Thread threadProcessarEventos;

        /// <summary>
        /// Responsável por armazenar o número da porta para conexão de rede
        /// </summary>
        public int Porta { get; set; }

        /// <summary>
        /// Responsável por armazenar e enfileirar os Eventos a serem processados no manager
        /// </summary>
        protected Queue<MensagemDto> filaProcessamento;

        /// <summary>
        /// Responsável por medir tempo de processamento de funcionalidades quando necessário
        /// </summary>
        protected static Stopwatch temporizador;

        /// <summary>
        /// Variável temporária para armazenar o cliente aceito
        /// </summary>
        protected Hashtable usuario;

        /// <summary>
        /// Responsável por armazenar as tarefas categorizadas por cronograma
        /// Chave Dic1 - oidCronograma
        /// Valor Dic1: Dic2  
        ///    Chave Dic2 - oidTarefa
        ///    Valor Dic2 - login colaborador que esta editando a tarefa
        /// </summary>
        protected Dictionary<string, Dictionary<string, string>> tarefasEmEdicaoPorCronograma;

        /// <summary>
        /// Responsável por armazenar as tarefas categorizadas por cronograma
        /// Chave Dic1 - oidCronograma
        /// Valor Dic1: Dic2  
        ///    Chave Dic2 - oidTarefa
        ///    Valor Dic2 - login colaborador que esta excluindo a tarefa
        /// </summary>
        protected Dictionary<string, Dictionary<string, string>> tarefasEmExclusaoPorCronograma;

        /// <summary>
        /// Responsável por armazenar os cronogramas que estão com o nome em edição
        /// Key   - oidCronograma
        /// Value - login do colaborador que está editando
        /// </summary>
        protected Dictionary<string, string> cronogramasComDadosEmEdicao;

        /// <summary>
        /// responsável por armazenar se o manager ainda deve estar ativo
        /// </summary>
        bool deveSerDesativado;

        private static readonly object processarMensagemLocker = new object();
        #endregion

        #region Delagadores
        /// <summary>
        /// Delegate para o método de identificação com timeout
        /// </summary>
        /// <param name="tcp">Conexão Tcp a ser validada</param>
        /// <returns>
        /// True - Válido
        /// False - Inválido
        /// </returns>
        public delegate bool EsperaPorIdentificacaoHandler( TcpClient tcp );

        /// <summary>
        /// Delegate utilizado no método Log para aceitar trechos de códigos
        /// </summary>
        public delegate void LogDebugCode();

        /// <summary>
        /// Delegate utilizado para realizar chamadas assincronas sem paramentro
        /// </summary>
        public delegate void ChamadaAssincrona();
        /// <summary>
        /// Delegate utilizado para representar condições que devem ser aguardadas por um determinado tempo
        /// </summary>
        /// <returns>a condição que deve ocorrer</returns>
        public delegate bool OcorrerCondicao();
        #endregion

        #region Eventos

        /// <summary>
        /// Evento disparado quando ocorrer um log do tipo Error
        /// </summary>
        public static event NotificarLogEventHandler AoLogarErro;

        /// <summary>
        /// Evento disparado quando ocorrer um log do tipo Info
        /// </summary>
        public static event NotificarLogEventHandler AoLogarInformacao;

        /// <summary>
        /// Evento disparado quando ocorrer um log do tipo debug
        /// </summary>
        public static event NotificarLogEventHandler AoLogarDebug;

		/// <summary>
		/// Evento disparado quando houver alterações na lista de usuários conectados
		/// </summary>
		public event AtualizarUsuariosConectadosEventHandler AoConectarNovoUsuario;

		/// <summary>
		/// Evento disparado quando houver a desconexão de um usuário
		/// </summary>
		public event AtualizarUsuariosConectadosEventHandler AoDesconectarUsuario;


		/// <summary>
		/// Evento disparado quando uma tarefa entrar em edição
		/// </summary>
		public event AtualizarEdicoesTarefaEventHandler AoIniciarEdicaoTarefa;

		/// <summary>
		/// Evento disparado quando uma tarefa encerrar a edição
		/// </summary>
		public event AtualizarEdicoesTarefaEventHandler AoFinalizarEdicaoTarefa;

		public event AtualizarEdicoesTarefaEventHandler AoCriarNovaTarefa;

        #endregion

        #region Propriedades
        /// <summary>
        /// Propriedade Endereço Ip do Servidor
        /// </summary>
        public string EnderecoIp
        {
            get { return enderecoIp.ToString(); }
            set { enderecoIp = IPAddress.Parse( value ); }
        }

        /// <summary>
        /// Responsável pelo recebimento controle de conexões
        /// </summary>
        public TcpListener ConexaoAtiva { get; set; }

        /// <summary>
        /// Responsável por armazenamento temporário 
        /// das informações da conexão do ultimo cliente 
        /// se conectar.
        /// </summary>
        public TcpClient TcpUltimoClienteConectado { get; set; }

        /// <summary>
        /// Responsável pelo tempo máximo de espera por conexão
        /// </summary>
        public int TempoMaximoAguardarIdentificacao { get; set; }
        #endregion

        #region Construtores
        /// <summary>
        /// Resposável por criar a instância do  Servidor descobrir ip local e iniciar um listener para conexões cliente
        /// </summary>
        public WexMultiAccessManager()
        {
            cronogramasConectados = new Dictionary<string, Dictionary<string, ConexaoCliente>>();
            usuariosConectados = new Dictionary<string, List<string>>();
            enderecoIp = GetEnderecoIp();
            TempoMaximoAguardarIdentificacao = 10000;
            filaProcessamento = new Queue<MensagemDto>();
            tarefasEmEdicaoPorCronograma = new Dictionary<string, Dictionary<string, string>>();
            tarefasEmExclusaoPorCronograma = new Dictionary<string, Dictionary<string, string>>();
            cronogramasComDadosEmEdicao = new Dictionary<string, string>();
            ConexaoCliente.AoOcorrerDesconexaoInesperada += ConexaoCliente_AoOcorrerDesconexaoInesperada;
            ConexaoCliente.AoLogarDebug += ConexaoCliente_AoLogarDebug;
            ConexaoCliente.AoLogarErro += ConexaoCliente_AoLogarErro;
            ConexaoCliente.AoLogarInformacao += ConexaoCliente_AoLogarInformacao;
            ConexaoCliente.AoReceberMensagemErrada += ConexaoCliente_AoReceberMensagemErrada;
        }

        /// <summary>
        /// Comportamento quando ocorrer risco de uma notificação errada.
        /// </summary>
        /// <param name="mensagem">Mensagem que seria enviada erroneamente</param>
        void ConexaoCliente_AoReceberMensagemErrada( MensagemDto mensagem )
        {
            string oidCronograma = (string)mensagem.Propriedades[Constantes.OIDCRONOGRAMA];
            string login = (string)mensagem.Propriedades[Constantes.AUTOR_ACAO];
            ComunicarRespostaSolicitacaoAoUsuario( mensagem, login, oidCronograma );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mensagem"></param>
        /// <param name="excessao"></param>
        void ConexaoCliente_AoLogarInformacao( string mensagem, Exception excessao )
        {
            LogInfo( mensagem );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mensagem"></param>
        /// <param name="excessao"></param>
        void ConexaoCliente_AoLogarErro( string mensagem, Exception excessao )
        {
            if(string.IsNullOrEmpty( mensagem ))
                LogError( excessao );
            else
                LogError( mensagem );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mensagem"></param>
        /// <param name="excessao"></param>
        void ConexaoCliente_AoLogarDebug( string mensagem, Exception excessao )
        {
            LogDebug( mensagem );
        }

        /// <summary>
        /// Método que implementa o comportamento do evento que ocorre na conexaoCliente quando um colaborador for desconectado
        /// </summary>
        /// <param name="sender">conexaocliente que foi desconectada</param>
        /// <param name="e">paramentros da desconexao inesperada</param>
        void ConexaoCliente_AoOcorrerDesconexaoInesperada( object sender, DesconexaoInesperadaEventArgs e )
        {
            //caso tenha perdido a conexão com o client deverá enfileirar no manager uma mensagem de desconexão para avisar aos outros usuários
            filaProcessamento.Enqueue( Mensagem.RnCriarMensagemUsuarioDesconectado( e.LoginUsuario, e.OidCronograma, true ) );
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Método para efetuar o log do tipo Info
        /// </summary>
        /// <param name="mensagem">Mensagem de informação passada para o log</param>
        public static void LogInfo( string mensagem )
        {
            if(AoLogarInformacao != null)
                AoLogarInformacao( mensagem, null );
        }

        /// <summary>
        /// Método para efetuar o log do tipo Info
        /// </summary>
        /// <param name="mensagem">Mensagem de informação passada para o log</param>
        public static void LogDebug( string mensagem )
        {
            if(AoLogarDebug != null)
                AoLogarDebug( mensagem, null );
        }

        /// <summary>
        /// Método para efetuar o log do tipo Info
        /// </summary>
        /// <param name="mensagem">Mensagem de informação passada para o log</param>
        public static void LogError( string mensagem )
        {
            if(AoLogarDebug != null)
                AoLogarDebug( mensagem, null );
        }

        /// <summary>
        /// Método para efetuar o log do tipo Info
        /// </summary>
        /// <param name="mensagem">Mensagem de informação passada para o log</param>
        public static void LogError( Exception excessao )
        {
            if(AoLogarDebug != null)
                AoLogarDebug( null, excessao );
        }

        /// <summary>
        /// Responsável por Descobrir o Endereço do servidor local
        /// </summary>
        /// <param name="nomeServidor"></param>
        /// <returns>Retorna o Número Ip da Máquina Local</returns>
        private IPAddress GetEnderecoIp()
        {
            return RedeUtil.GetEnderecoIpComputadorAtual();
        }
        /// Resposável por:
        /// - instanciar o Listener para recebimento de informações no ip e porta predefinidos.
        /// - Iniciar a Thread de Processar Eventos
        /// </summary>
        public void Conectar()
        {
            try
            {
                ConexaoAtiva = new TcpListener( enderecoIp, Porta );
                ConexaoAtiva.Start();
                IniciaAtendimento();
                threadProcessarEventos = new Thread( RnProcessarEventos ) { Name = "Manager#ProcessarEventos", Priority = ThreadPriority.Normal };
                threadProcessarEventos.Start();
            }
            catch(Exception ex)
            {
                LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                throw ex;
            }
        }

        /// <summary>
        /// Responsável por iniciar o atendimento da conexaoAtiva instanciando o método em um thread
        /// </summary>
        public virtual void IniciaAtendimento()
        {
            //Inicia uma nova tread que hospeda o listener
            threadAceitarNovosClientes = new Thread( ManterAtendimento ) { Priority = ThreadPriority.Normal, Name = "Manager#AceitarNovosClientes" };
            threadAceitarNovosClientes.Start();
        }

        /// <summary>
        /// Responsável por ficar rodando em concorrência aceitando novas conexões
        /// </summary>
        public virtual void ManterAtendimento()
        {
            bool flag = true;
            // Enquanto o servidor estiver rodando
            while(flag)
            {
                // Devido ao recurso de threads deve-se certificar de que o Servidor ainda está conectado
                //Pois entre o while e o teste de Pending() poderá haver o processamento de outras threads
                try
                {
                    if(ServidorEstiverConectado() && ConexaoAtiva.Pending())
                        RnAguardarConexao();

                    if(deveSerDesativado)
                        flag = false;
                }
                catch(ThreadAbortException ex)
                {
                    LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                    if(AoLogarErro != null)
                        AoLogarErro( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ), ex );
                }
                catch(ObjectDisposedException ex)
                {
                    LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                    if(AoLogarErro != null)
                        AoLogarErro( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ), ex );
                    continue;
                    // Ocorrerá em casos em que o o manager se desconectou, e servirá para terminar a thread
                }
                catch(Exception ex)
                {
                    LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                    if(AoLogarErro != null)
                        AoLogarErro( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ), ex );
                    flag = false;
                    continue;
                }
                Thread.Sleep( 10 );
            }
        }

        /// <summary>
        /// Responsável por setar o atributo TcpUltimoCLienteConectdo com o socket do ultimo cliente aceito
        /// e mandar efetuar a o Recebimento da Conexão.
        /// </summary>
        public void RnAguardarConexao()
        {
            RnReceberConexao( ConexaoAtiva.AcceptTcpClient() );
        }

        /// <summary>
        /// Método que verifica se uma chave composta de edição já existe
        /// </summary>
        /// <param name="login"></param>
        /// <param name="oidCronograma"></param>
        /// <returns>true para existe chave composta e false caso contrário</returns>
        public bool VerificarExisteEdicaoPara( string login, string oidCronograma )
        {
            return tarefasEmEdicaoPorCronograma.ContainsKey( oidCronograma ) && tarefasEmEdicaoPorCronograma[oidCronograma].ContainsValue( login );
        }

        /// <summary>
        /// Responsável por receber uma conexão tcp 
        /// e aguardar a identificação em um tempo Máximo de Espera
        /// </summary>
        public void RnReceberConexao( TcpClient tcp )
        {
            if(TcpUtil.ConexaoTcpValida( tcp ))
            {
                Log( () =>
                {
                    try
                    {
						
                        RnAguardarIdentificacaoComTempoMaximoDeEspera( TempoMaximoAguardarIdentificacao, tcp );
                    }
                    catch(TimeoutException ex)
                    {
                        LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                        RnComunicarRecusaConexao( "Conexão recusada por tempo máximo de identificação excedido!.", tcp );
                        tcp.Close();
                    }
                    catch(Exception ex)
                    {
                        LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                        throw ex;
                    }
                }, "RnReceberConexao" );
            }
        }

        /// <summary>
        /// Responsável por efetuar uma chamada a um método assíncronamente dentro de tempo limite de espera
        /// </summary>
        /// <param name="Acao">Método invocado para execução</param>
        /// <param name="tempoEmMilisegundos">Tempo de máximo de espera pela execução</param>
        public void RnAguardarIdentificacaoComTempoMaximoDeEspera( int tempoEmMilisegundos, TcpClient tcp )
        {
            Log( () =>
            {
                EsperaPorIdentificacaoHandler funcaoEmEspera = RnAguardarIdentificacao;
                IAsyncResult resultado = funcaoEmEspera.BeginInvoke( tcp, null, null );
                int tempo = 0;
                while(tempo < tempoEmMilisegundos && !resultado.IsCompleted)
                {
                    tempo += 10;
                    Thread.Sleep( 10 );
                }


                LogInfo( string.Format( "{0} - tempo de execucao:{1} milisegundos", Thread.CurrentThread.Name, tempo ) );

                if(!resultado.IsCompleted)
                {
                    LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", "Tempo máximo de espera por identificação esgotado.", "RnAguardarIdentificacaoComTempoMaximoDeEspera" ) );
                    LogDebug( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", "Tempo máximo de espera por identificação esgotado.", "RnAguardarIdentificacaoComTempoMaximoDeEspera" ) );
                    throw new TimeoutException( "Tempo máximo de espera por identificação esgotado." );
                }
                else
                    if(!funcaoEmEspera.EndInvoke( resultado ))
                        if(TcpUtil.ConexaoTcpValida( tcp ))
                            RnComunicarRecusaConexao( "Conexão Recusada por identificação inválida.", tcp );


            }, "AguardarConexaoComTempoMaximoDeEspera" );
        }

        /// <summary>
        /// Responsável por Aguardar e Identificar o tipo de mensagem enviada , receber nova conexão ou recusar nova conexão
        /// - Novos Usuários Conectados
        /// </summary>
        /// <param name="tcp">Conexão tcp do Cliente</param>
        public bool RnAguardarIdentificacao( TcpClient tcp )
        {
            string mensagem = "";
            MensagemDto objetoMensagem;
            do
            {

                if(TcpUtil.ConexaoTcpValida( tcp ))
                {
                    if(tcp.Available > 0)
                        mensagem = TcpUtil.ReceberMensagemTcp( tcp );
                }
                else
                    mensagem = null;
            } while(String.IsNullOrEmpty( mensagem ));

            if(!String.IsNullOrEmpty( mensagem ) && TcpUtil.ConexaoTcpValida( tcp ))
            {
                mensagem.Trim();
                objetoMensagem = Mensagem.DeserializarMensagemDto( mensagem );
                if(objetoMensagem.Tipo == CsTipoMensagem.NovosUsuariosConectados)
                {
                    string oidCronograma = objetoMensagem.Propriedades[Constantes.OIDCRONOGRAMA].ToString();
                    string[] login = (string[])objetoMensagem.Propriedades[Constantes.USUARIOS];
                    RnAceitarConexao( oidCronograma, login[0], tcp );
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Responsável por Aceitar a conexão do colaborador ao cronograma e armazenar informações
        /// Colaborador: - Conectado e Qual Cronograma está Conectado
        /// Cronograma : - Quais Colaboradores estão conectados em um determinado cronograma
        /// </summary>
        /// <param name="oidCronograma">Nome do cronograma</param>
        /// <param name="login">Login do Colaborador</param>
        /// <param name="tcp">Conexão tcp do colaborador</param>
        public void RnAceitarConexao( string oidCronograma, string login, TcpClient tcp )
        {
            if(!ColaboradorConectado( login, oidCronograma ))
            {
                //Efetuar a conexao do cliente em uma thread especifica e receber as mensagens em uma fila
                ConexaoCliente conexaoCliente = ConexaoClienteFactory( login, tcp );
                conexaoCliente.OidCronograma = oidCronograma;
                Dictionary<string, ConexaoCliente> usuariosPorCronograma;
                cronogramasConectados.TryGetValue( oidCronograma, out usuariosPorCronograma );

                if(usuariosPorCronograma == null)
                {
                    usuariosPorCronograma = new Dictionary<string, ConexaoCliente>();
                    cronogramasConectados[oidCronograma] = usuariosPorCronograma;
                }

                usuariosPorCronograma[login] = conexaoCliente;

                List<string> cronogramasPorUsuario;
                usuariosConectados.TryGetValue( login, out cronogramasPorUsuario );

                if(cronogramasPorUsuario == null)
                {
                    cronogramasPorUsuario = new List<string>();
                    usuariosConectados[login] = cronogramasPorUsuario;
                }

                if(!cronogramasPorUsuario.Contains( oidCronograma ))
                    cronogramasPorUsuario.Add( oidCronograma );

                RnComunicarUsuarioNovoConectado( login, oidCronograma );
				ExecutarNotificacaoConexao( oidCronograma , login );
            }
            else
            {
                cronogramasConectados[oidCronograma][login].TcpCliente = tcp;
            }
            EnviarConfirmacaoConexaoAoCliente( tcp, oidCronograma, login );
        }

		/// <summary>
		/// Método executado quando um usuário se conectar
		/// </summary>
		/// <param name="oidCronograma">oid do cronograma</param>
		/// <param name="login">login do colaborador</param>
		private void ExecutarNotificacaoConexao( string oidCronograma , string login )
		{
			if( AoConectarNovoUsuario == null )
				return;
			AoConectarNovoUsuario( oidCronograma , login );
		}

		/// <summary>
		/// Método executado quando um usuário é desconectado
		/// </summary>
		/// <param name="oidCronograma">oid do cronograma</param>
		/// <param name="login">login do colaborador</param>
		private void ExecutarNotificacaoDesconexaoUsuario( string oidCronograma , string login )
		{
			if( AoDesconectarUsuario == null )
				return;
			AoDesconectarUsuario( oidCronograma , login );
		}

        /// <summary>
        /// Envia mensagem de confirmação de que o usuário foi conectado com sucesso,
        /// tal como a lista recente de usuarios online
        /// </summary>
        /// <param name="tcp">Conexão do destinatário</param>
        /// <param name="cronograma">Cronograma Atual</param>
        protected virtual void EnviarConfirmacaoConexaoAoCliente( TcpClient tcp, string cronograma, string login )
        {
            string[] usuarios = ListarUsuariosOnlineNoCronograma( cronograma, login );
            LogInfo( string.Format( "{0} logou no cronograma {1}", login, cronograma ) );
            Dictionary<string, string> edicoes = ListarEdicoesPorCronograma( cronograma );
            MensagemDto objetoMensagem = GetMensagemConexaoComSucesso( cronograma, usuarios, edicoes );
            string mensagemJson = JsonConvert.SerializeObject( objetoMensagem );
            mensagemJson = TcpUtil.AdicionarStringProtecaoDeIntegridade( mensagemJson );
            TcpUtil.EnviarMensagemTcp( mensagemJson, tcp );
        }

        /// <summary>
        /// Método utilizado para copiar a lista de edições de um determinado cronograma
        /// </summary>
        /// <param name="oidCronograma">identificador do cronograma</param>
        /// <returns>Listagem de edições de um cronograma</returns>
        private Dictionary<string, string> ListarEdicoesPorCronograma( string oidCronograma )
        {
            if(tarefasEmEdicaoPorCronograma != null && tarefasEmEdicaoPorCronograma.ContainsKey( oidCronograma ))
                return new Dictionary<string, string>( tarefasEmEdicaoPorCronograma[oidCronograma] );
            else
                return new Dictionary<string, string>();
        }

        /// <summary>
        /// Cria a mensagem de conexao de sucesso utilizada ao aceitar a conexao de um novo usuario.
        /// </summary>
        /// <param name="oidCronograma">identificador do cronograma atual</param>
        /// <param name="usuarios">lista de usuarios conectados atualmente</param>
        /// <returns>Mensagem do tipo conectado com sucesso preenchida com os usuários online atualmente</returns>
        protected virtual MensagemDto GetMensagemConexaoComSucesso( string oidCronograma, string[] usuarios, Dictionary<string, string> edicoes )
        {
            string autorEdicaoNomeCronograma;
            if(cronogramasComDadosEmEdicao.ContainsKey( oidCronograma ))
                autorEdicaoNomeCronograma = cronogramasComDadosEmEdicao[oidCronograma];
            else
                autorEdicaoNomeCronograma = null;
            return Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( usuarios, oidCronograma, edicoes, autorEdicaoNomeCronograma );
        }
        /// <summary>
        /// Responsável por fabricar objetos do tipo ConexaoCliente.
        /// Utilizado para facilitar os testes da classe
        /// </summary>
        /// <param name="login">Login Usuario</param>
        /// <param name="tcp">Tcp cliente atual</param>
        /// <returns>um Objeto ConexaoCliente</returns>
        protected virtual ConexaoCliente ConexaoClienteFactory( string login, TcpClient tcp )
        {
            return new ConexaoCliente( login, tcp, filaProcessamento );
        }

        /// <summary>
        /// Responsável por Identificar se um Colaborador está conectado em um cronograma especifico
        /// </summary>
        /// <param name="Login">Login do Colaborador</param>
        /// <param name="Cronograma">Nome do cronograma no qual deve estar Conectado</param>
        /// <returns>Verdadeiro Caso esteja Conectado e Falso Para Caso não esteja conectado em um determinado cronograma </returns>
        public bool ColaboradorConectado( string Login, string Cronograma )
        {
            Dictionary<string, ConexaoCliente> listaColaboradores = ColaboradoresEmCronograma( Cronograma );
            return listaColaboradores != null && listaColaboradores.ContainsKey( Login );
        }

        /// <summary>
        /// Responsável por verificar se o colaborador está conectado, independente de estar ou não
        /// conectado em um cronograma
        /// </summary>
        /// <param name="Login">Colaborador a ser encontrado</param>
        /// <returns>
        /// true - existe
        /// false - não existe</returns>
        public bool ColaboradorConectado( string Login )
        {
            return usuariosConectados.ContainsKey( Login );
        }

        /// <summary>
        /// Responsável por retornar os Colaboradores Conectados em um determinado Cronograma
        /// </summary>
        /// <param name="oidCronograma">Nome do Cronograma a ser localizado</param>
        /// <returns>Retorna um Dictionary contendo os Colaboradores Conectados em um determinado cronograma</returns>
        public virtual Dictionary<string, ConexaoCliente> ColaboradoresEmCronograma( string oidCronograma )
        {
            Dictionary<string, ConexaoCliente> listaColaboradores;

            if(cronogramasConectados.ContainsKey( oidCronograma ))
                listaColaboradores = cronogramasConectados[oidCronograma];
            else
                listaColaboradores = new Dictionary<string, ConexaoCliente>();
            return listaColaboradores;
        }

        /// <summary>
        /// Responsável por retornar os Colaboradores Conectados em um determinado Cronograma
        /// </summary>
        /// <param name="oidCronograma">Nome do Cronograma a ser localizado</param>
        /// <returns>Retorna um vetor de colaboradores conectados no cronograma</returns>
        public string[] ListarUsuariosOnlineNoCronograma( string oidCronograma, string login )
        {
            return cronogramasConectados[oidCronograma].Keys.Where( o => o != login ).ToArray();
        }

        /// <summary>
        /// Responsável por Comunicar Envio de Mensagem de Recusa de Conexão
        /// </summary>
        /// <param name="motivo">Motivo da recusa de conexao</param>
        /// <param name="tcp">TcpClient para o qual enviar a mensagem</param>
        protected virtual void RnComunicarRecusaConexao( string motivo, TcpClient tcp )
        {
            if(!TcpUtil.ConexaoTcpValida( tcp ))
                return;
            string json = Mensagem.Serializar( Mensagem.RnCriarMensagemConexaoRecusada( motivo ) );
            json = TcpUtil.AdicionarStringProtecaoDeIntegridade( json );
            TcpUtil.EnviarMensagemTcp( json, tcp );
        }

        /// <summary>
        /// Responsável por enviar a mensagem que informa que um novo usuário se conectou
        /// em um determinado cronograma
        /// </summary>
        /// <param name="login">Nome do usuário</param>
        /// <param name="oidCronograma">Nome do cronograma em que o usuário se conectou</param>
        public void RnComunicarUsuarioNovoConectado( string login, string oidCronograma )
        {
            MensagemDto objetoMensagem = Mensagem.RnCriarMensagemNovoUsuarioConectado( new string[] { login }, oidCronograma );
            filaProcessamento.Enqueue( objetoMensagem );
        }

        /// <summary>
        /// Responsável por sintetizar em uma unica mensagens várias mensagens de mesmo tipo
        /// e mesmo cronograma
        /// </summary>
        /// <param name="fila">Fila de mensagens</param>
        /// <returns>Uma MensagemDto resumo das mensagens semelhantes</returns>
        protected virtual List<MensagemDto> RnResumirMensagens( List<MensagemDto> fila )
        {
            string[] cronogramas = ( from c in fila
                                     select c.Propriedades[Constantes.OIDCRONOGRAMA].ToString() ).Distinct().ToArray();
            List<MensagemDto> listaResumida = new List<MensagemDto>();
            List<MensagemDto> listaAResumir = new List<MensagemDto>();

            foreach(string cronograma in cronogramas)
            {
                var mensagensPorCronograma = from selecao in fila
                                             where selecao.Propriedades.ContainsKey( Constantes.OIDCRONOGRAMA ) &&
                                             (string)selecao.Propriedades[Constantes.OIDCRONOGRAMA] == cronograma
                                             group selecao by selecao.Tipo into agrupamento
                                             select agrupamento;

                foreach(var item in mensagensPorCronograma)
                {
                    listaAResumir = new List<MensagemDto>( item.ToList() );
                    if(listaAResumir.Count > 0)
                        switch(item.Key)
                        {
                            case CsTipoMensagem.NovosUsuariosConectados:
                                listaResumida.Add( ResumirMensagemHashComUsuarios( listaAResumir, cronograma, item.Key ) );
                                break;
                            case CsTipoMensagem.ConexaoEfetuadaComSucesso:
                            case CsTipoMensagem.MovimentacaoPosicaoTarefa:
                            case CsTipoMensagem.NovaTarefaCriada:
                            case CsTipoMensagem.ExclusaoTarefaFinalizada:
                            case CsTipoMensagem.DadosCronogramaAlterados:
                            case CsTipoMensagem.UsuarioDesconectado:
                            case CsTipoMensagem.InicioEdicaoNomeCronograma:
                            case CsTipoMensagem.InicioEdicaoTarefa:
                            case CsTipoMensagem.EdicaoTarefaFinalizada:
                                foreach(var mensagem in listaAResumir)
                                {
                                    listaResumida.Add( mensagem );
                                }
                                break;
                        }
                }
            }
            return listaResumida;
        }

        /// <summary>
        /// Responsavel por sintetizar mensagens em que a hashtable possua um vetor de usuário com indice 'usuarios'
        /// </summary>
        /// <param name="lista">lista de mensagens contendo vetor de usuário</param>
        /// <param name="oidCronograma"> cronograma atual</param>
        /// <param name="tipo"> tipo da mensagem</param>
        /// <returns>uma única mensagem com usuários extraidos dos vetores de todas as mensagens unificando em uma unica mensagem
        /// que contem um vetor sintetizado com todos usuários
        /// </returns>
        public MensagemDto ResumirMensagemHashComUsuarios( List<MensagemDto> lista, string oidCronograma, CsTipoMensagem tipo )
        {
            List<string> listaUsuarios = new List<string>();
            string[] usuarios;
            foreach(MensagemDto item in lista)
            {
                usuarios = (string[])item.Propriedades[Constantes.USUARIOS];
                if(usuarios != null)
                    foreach(string usuario in usuarios)
                    {
                        listaUsuarios.Add( usuario );
                    }
            }
            Hashtable propriedades = new Hashtable();
            propriedades.Add( Constantes.USUARIOS, listaUsuarios.ToArray() );
            propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );

            return new MensagemDto() { Propriedades = propriedades, Tipo = tipo };
        }

        /// <summary>
        /// Resumir mensagens em que a hashtable possua o indice 'login' a ser resumido
        /// </summary>
        /// <param name="lista"> lista de mensagens que possuam tarefas</param>
        /// <param name="oidCronograma"> cronograma desta mensagem</param>
        /// <param name="tipo"> tipo de mensagem</param>
        /// <returns></returns>
        public MensagemDto ResumirMensagemDtoHashComLogin( List<MensagemDto> lista, string oidCronograma, CsTipoMensagem tipo )
        {
            List<string> usuarios = new List<string>();
            string login;
            foreach(MensagemDto item in lista)
            {
                login = (string)item.Propriedades[Constantes.AUTOR_ACAO];
                if(!string.IsNullOrEmpty( login ))
                    usuarios.Add( login );
            }
            Hashtable propriedades = new Hashtable();
            propriedades.Add( Constantes.USUARIOS, usuarios.ToArray() );
            propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );

            return new MensagemDto() { Propriedades = propriedades, Tipo = tipo };
        }

        /// <summary>
        /// Responsável por resumir mensagens de exclusão de tarefas
        /// </summary>
        /// <param name="lista">lista de mensagens de confirmação de exclusão tarefa</param>
        /// <param name="oidCronograma">guid cronograma atual da mensagem</param>
        /// <param name="tipo">Mensagem do tipo ComunicarExclusaoTarefa</param>
        /// <returns>várias mensagens sintetizada em apenas uma</returns>
        public MensagemDto ResumirMensagensExclusaoTarefa( List<MensagemDto> lista, string oidCronograma, CsTipoMensagem tipo )
        {
            Hashtable propriedades = new Hashtable();
            Dictionary<string, List<string>> tarefasTemp = new Dictionary<string, List<string>>();
            string login;
            string[] tarefas;
            foreach(MensagemDto mensagem in lista)
            {
                login = (string)mensagem.Propriedades[Constantes.AUTOR_ACAO];
                tarefas = (string[])mensagem.Propriedades[Constantes.TAREFAS];
                foreach(string tarefa in tarefas)
                {
                    if(!tarefasTemp.ContainsKey( login ))
                        tarefasTemp.Add( login, new List<string>() );

                    tarefasTemp[login].Add( tarefa );
                }
            }

            foreach(var item in tarefasTemp)
            {
                propriedades.Add( item.Key, item.Value.ToArray() );
            }
            propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            return new MensagemDto() { Tipo = tipo, Propriedades = propriedades };
        }

        /// <summary>
        /// Desconectar e encerrar todas as Conexões
        /// </summary>
        public void Desconectar()
        {
            deveSerDesativado = true;
            MensagemDto mensagemDesconexaoServidor = Mensagem.RnCriarMensagemServidorDesconectando( "O servidor está desconectando!" );
            string json = JsonConvert.SerializeObject( mensagemDesconexaoServidor );
            json = TcpUtil.AdicionarStringProtecaoDeIntegridade( json );
            foreach(var cronograma in cronogramasConectados)
            {
                foreach(var cliente in cronograma.Value)
                {
                    if(ColaboradorConectado( cliente.Key ) && cliente.Value != null)
                    {
                        if(TcpUtil.ConexaoTcpValida( cliente.Value.TcpCliente ))
                            TcpUtil.EnviarMensagemTcp( json, cliente.Value.TcpCliente );
                        cliente.Value.Dispose();
                    }
                }
            }
            if(threadAceitarNovosClientes != null)
                AguardarAte( () => { return !threadAceitarNovosClientes.IsAlive; } );

            if(threadProcessarEventos != null)
                AguardarAte( () => { return !threadProcessarEventos.IsAlive; } );

            if(ConexaoAtiva != null)
            {
                ConexaoAtiva.Stop();
                ConexaoAtiva = null;
            }
            cronogramasConectados.Clear();
            tarefasEmEdicaoPorCronograma.Clear();
            cronogramasComDadosEmEdicao.Clear();
            tarefasEmExclusaoPorCronograma.Clear();
            usuariosConectados.Clear();
        }

        /// <summary>
        /// Método responsável por aguardar por um tempo a ocorrencia de uma condição
        /// </summary>
        /// <param name="ocorrerACondicao">método retornar um booleano representando a condição que deve ser esperada</param>
        /// <param name="segundos">tempo em segundos que devem ser esperado</param>
        public static void AguardarAte( OcorrerCondicao ocorrerACondicao, double segundos = 3 )
        {
            double tempoEmMilisegundos = segundos * 1000;
            int tempoDecorrido = 0;
            while(!ocorrerACondicao() && tempoDecorrido < tempoEmMilisegundos)
            {
                Thread.Sleep( 100 );
                tempoDecorrido += 100;
            }
        }


        /// <summary>
        /// Executar ações no manager necessárias para a mensagem
        /// </summary>
        /// <param name="mensagemAtual"></param>
        /// <returns>MensagemDto Depois de Processada</returns>
        protected virtual MensagemDto RnProcessarMensagem( MensagemDto mensagemAtual )
        {
            lock(processarMensagemLocker)
            {
                switch(mensagemAtual.Tipo)
                {
                
                    case CsTipoMensagem.UsuarioDesconectado:
                        FinalizarConexaoUsuario( mensagemAtual );
                        break;
                    case CsTipoMensagem.InicioEdicaoTarefa:
                        return ProcessarMensagemInicioEdicaoTarefa( mensagemAtual );
                    case CsTipoMensagem.EdicaoTarefaFinalizada:
                        return ProcessarMensagemFinalizarEdicaoTarefa( mensagemAtual );
                    case CsTipoMensagem.ExclusaoTarefaIniciada:
                        ProcessarMensagemInicioExclusaoTarefa( mensagemAtual );
                        return null;
                    case CsTipoMensagem.ExclusaoTarefaFinalizada:
                        return ProcessarMensagemExclusaoTarefaFinalizada( mensagemAtual );
                    case CsTipoMensagem.InicioEdicaoNomeCronograma:
                        return ProcessarMensagemInicioEdicaoNomeCronograma( mensagemAtual );
                    case CsTipoMensagem.DadosCronogramaAlterados:
                        ProcessarMensagemNomeCronogramaAlterado( mensagemAtual );
                        break;
					case CsTipoMensagem.NovaTarefaCriada:
						LogarTarefaCriada( mensagemAtual );
						break;

                }
                return mensagemAtual;
            }
        }

		/// <summary>
		/// Responsável por criar o log de uma tarefa criada
		/// </summary>
		/// <param name="mensagemAtual"></param>
		private void LogarTarefaCriada( MensagemDto mensagemAtual )
		{
			if( AoCriarNovaTarefa != null )
				AoCriarNovaTarefa( mensagemAtual.Propriedades[Constantes.OIDCRONOGRAMA] as string 
					, mensagemAtual.Propriedades[Constantes.AUTOR_ACAO] as string 
					, mensagemAtual.Propriedades[Constantes.OIDTAREFA]  as string);
		}

        /// <summary>
        /// Método responsável por efetuar o processamento da mensagem de
        /// </summary>
        /// <param name="mensagemAtual"></param>
        protected MensagemDto ProcessarMensagemNomeCronogramaAlterado( MensagemDto mensagemAtual )
        {
            string oidCronograma = mensagemAtual.Propriedades[Constantes.OIDCRONOGRAMA] as string;
            string login = mensagemAtual.Propriedades[Constantes.AUTOR_ACAO] as string;
            RemoverEdicaoNomeCronograma( oidCronograma, login );
            return mensagemAtual;
        }

        /// <summary>
        /// Método utilizado para remover cronograma do modo de edição do nome
        /// </summary>
        /// <param name="oidCronograma">oid do cronograma em edição</param>
        /// <param name="login">login do colaborador editor</param>
        protected virtual void RemoverEdicaoNomeCronograma( string oidCronograma, string login )
        {
            if(cronogramasComDadosEmEdicao.ContainsKey( oidCronograma ) && cronogramasComDadosEmEdicao[oidCronograma] == login)
                cronogramasComDadosEmEdicao.Remove( oidCronograma );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mensagemAtual"></param>
        public virtual MensagemDto ProcessarMensagemInicioEdicaoNomeCronograma( MensagemDto mensagemAtual )
        {
            string oidCronograma = mensagemAtual.Propriedades[Constantes.OIDCRONOGRAMA] as string;
            string login = mensagemAtual.Propriedades[Constantes.AUTOR_ACAO] as string;
            if(string.IsNullOrEmpty( oidCronograma ) || string.IsNullOrEmpty( login ) ||
                ( !string.IsNullOrEmpty( oidCronograma ) && cronogramasComDadosEmEdicao.ContainsKey( oidCronograma ) ))
            {
                //recusar edição do nome do crograma
                ComunicarRespostaSolicitacaoAoUsuario( Mensagem.RnCriarMensagemRecusaEdicaoNomeCronograma( oidCronograma, login ), login, oidCronograma );
                return null;
            }
            else
            {
                cronogramasComDadosEmEdicao.Add( oidCronograma, login );
                //permitir a edição do nome do cronograma
                ComunicarRespostaSolicitacaoAoUsuario( Mensagem.RnCriarMensagemPermitirEdicaoNomeCronograma( oidCronograma, login ), login, oidCronograma );
                return mensagemAtual;
            }
        }

        /// <summary>
        /// Responsável por efetuar os procedimentos necessários ao Receber a confirmação
        /// de que tarefas foram excluidas
        /// </summary>
        /// <param name="mensagem">Mensagem do Tipo ComunicarExclusãoConcluida</param>
        /// <returns>ExclusaoTarefaFinalizada</returns>
        protected virtual MensagemDto ProcessarMensagemExclusaoTarefaFinalizada( MensagemDto mensagem )
        {
            string oidCronograma = (string)mensagem.Propriedades[Constantes.OIDCRONOGRAMA];
            string login = (string)mensagem.Propriedades[Constantes.AUTOR_ACAO];
            string[] oidTarefas = (string[])mensagem.Propriedades[Constantes.TAREFAS];
            Dictionary<string, Int16> tarefasImpactadas = (Dictionary<string, Int16>)mensagem.Propriedades[Constantes.TAREFAS_IMPACTADAS];
            DateTime dataHoraAcao = (DateTime)mensagem.Propriedades[Constantes.DATAHORA_ACAO];

            foreach(string oidTarefa in oidTarefas)
            {
                RemoverTarefaExclusao( oidCronograma, oidTarefa );
            }

            if(mensagem.Propriedades.ContainsKey( Constantes.TAREFAS_NAO_EXCLUIDAS ))
            {
                string[] tarefasNaoExcluidas = (string[])mensagem.Propriedades[Constantes.TAREFAS_NAO_EXCLUIDAS];
                foreach(string oidTarefa in tarefasNaoExcluidas)
                {
                    RemoverTarefaExclusao( oidCronograma, oidTarefa );
                }
            }
            return Mensagem.RnCriarMensagemComunicarExclusaoTarefaConcluida( oidTarefas, tarefasImpactadas, oidCronograma, login, dataHoraAcao );
        }

        /// <summary>
        /// Responsável por efetuar os procedimentos necessários para finalizar a edição de uma tarefa
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo FinalizarEdicaoTarefa</param>
        /// <returns></returns>
        private MensagemDto ProcessarMensagemFinalizarEdicaoTarefa( MensagemDto mensagem )
        {
            string oidCronograma = (string)mensagem.Propriedades[Constantes.OIDCRONOGRAMA];
            string login = (string)mensagem.Propriedades[Constantes.AUTOR_ACAO];
            string oidTarefa = (string)mensagem.Propriedades[Constantes.OIDTAREFA];

            if(tarefasEmEdicaoPorCronograma.ContainsKey( oidCronograma ) && tarefasEmEdicaoPorCronograma[oidCronograma].ContainsKey( oidTarefa ))
            {
                if(tarefasEmEdicaoPorCronograma[oidCronograma][oidTarefa] == login)
                {
                    RemoverTarefaDeEdicao( oidCronograma, oidTarefa );
					if( AoFinalizarEdicaoTarefa != null )
						AoFinalizarEdicaoTarefa( oidCronograma , login , oidTarefa );
                    LogDebug( string.Format( "Edição da tarefa {0} finalizada pelo colaborador {1} no cronograma {2}", oidTarefa, login, oidCronograma ) );
                    return mensagem;
                }
            }
            return null;
        }

        /// <summary>
        /// Método responsável por gerar a resposta ao MultiAcessClient para excluir tarefas
        /// </summary>
        /// <param name="mensagem">MensagemDto do tipo InicioExclusaoTarefa</param>
        protected virtual void ProcessarMensagemInicioExclusaoTarefa( MensagemDto mensagem )
        {
            string oidCronograma = (string)mensagem.Propriedades[Constantes.OIDCRONOGRAMA];
            string login = (string)mensagem.Propriedades[Constantes.AUTOR_ACAO];
            string[] tarefas = (string[])mensagem.Propriedades[Constantes.TAREFAS];
            List<string> tarefasExclusaoNaoPermitida = new List<string>();
            List<string> Permitidas = new List<string>();
            foreach(string oidTarefa in tarefas)
            {
                if(TarefaEstaLivre( oidCronograma, login, oidTarefa ))
                {
                    Permitidas.Add( oidTarefa );
                    AdicionarTarefaATarefasEmExclusao( oidCronograma, login, oidTarefa );
                }
                else
                {
                    tarefasExclusaoNaoPermitida.Add( oidTarefa );
                }
            }
            RnResponderSolicitacaoExclusaoTarefa( Mensagem.RnCriarMensagemEfetuarExclusaoTarefas( Permitidas.ToArray(), tarefasExclusaoNaoPermitida.ToArray(), oidCronograma, login ) );
        }

        /// <summary>
        /// Método para responder a solicitação de exclusão de tarefas, retornando
        /// Uma mensagemDto de permissão com as mensagens que podem ser excluídas
        /// </summary>
        /// <param name="mensagem">MensagemDto do tipo PermitirExclusaoTarefa</param>
        public virtual void RnResponderSolicitacaoExclusaoTarefa( MensagemDto mensagem )
        {
            string oidCronograma = (string)mensagem.Propriedades[Constantes.OIDCRONOGRAMA];
            string login = (string)mensagem.Propriedades[Constantes.AUTOR_ACAO];
            ComunicarRespostaSolicitacaoAoUsuario( mensagem, login, oidCronograma );
        }

        /// <summary>
        /// Método responsável por comunicar ao MultiAccessManager de que uma tarefa persistida não pode ser editada
        /// </summary>
        /// <param name="oidCronograma">guid cronograma</param>
        /// <param name="login">login de quem quer editar a tarefa</param>
        /// <param name="oidTarefa">guid da tarefa editada</param>
        /// <param name="editorTarefa">login de quem esta editando a tarefa</param>
        public virtual void RnRecusarEdicaoTarefa( string idRequisicao, string oidCronograma, string login, string oidTarefa, string editorTarefa )
        {
            Debug.WriteLine( String.Format( "Requisicao: {0} recusada - OidTarefa: {1} - Autor: {2} Método: RnRecusarEdicaoTarefa", idRequisicao, oidTarefa, editorTarefa ) );
            LogInfo( string.Format( "{0} teve a edição da tarefa de oid {1} recusada  no cronograma {2}", login, oidTarefa, oidCronograma ) );
            MensagemDto mensagemDeRecusa = Mensagem.RnCriarMensagemRecusarEdicaoTarefa(  oidTarefa, editorTarefa, oidCronograma , idRequisicao);
            ComunicarRespostaSolicitacaoAoUsuario( mensagemDeRecusa, login, oidCronograma );
        }

        /// <summary>
        /// Método utilizado para responder solicitações do usuário
        /// </summary>
        /// <param name="mensagem">mensagem de resposta</param>
        /// <param name="login">login do colaborador</param>
        /// <param name="oidCronograma">oid do cronograma</param>
        protected virtual void ComunicarRespostaSolicitacaoAoUsuario( MensagemDto mensagem, string login, string oidCronograma )
        {
            if(!string.IsNullOrEmpty( login ) && !string.IsNullOrEmpty( oidCronograma ))
            {
                if(ColaboradorConectado( login, oidCronograma ) && cronogramasConectados[oidCronograma][login] != null)
                {
                    string idRequisicao = string.Empty;
                    string msg = string.Empty;
                    if(mensagem.Propriedades.ContainsKey( Constantes.ID_REQUISICAO ))
                    {
                        idRequisicao = mensagem.Propriedades[Constantes.ID_REQUISICAO] as string;
                        msg = string.Format( "{0} foi enfileirada para {1} no cronograma {2}", idRequisicao, login, oidCronograma );
                        LogInfo( msg );
                    }
                    mensagem.Dump( string.Format( "{1}  Enviando para {0}: Requisição:{2}", login, mensagem.Tipo.ToString(),idRequisicao ) );
                    cronogramasConectados[oidCronograma][login].EnfileirarMensagem(mensagem);
                }
            }
        }

        /// <summary>
        /// Responsável por Conceder ou não a edição de uma tarefa
        /// </summary>
        /// <param name="mensagem">mensagem dto contendo informações da tentativa de edição de uma tarefa</param>
        /// <returns>
        /// MensagemDto de InicioEdicaoTarefa caso a tarefa não esteja em edição
        /// MensagemDto de RecusarEdicaoTarefa casa o tarefa já esteja em edição
        /// </returns>
        protected MensagemDto ProcessarMensagemInicioEdicaoTarefa( MensagemDto mensagem )
        {
            string oidCronograma = (string)mensagem.Propriedades[Constantes.OIDCRONOGRAMA];
            string login = (string)mensagem.Propriedades[Constantes.AUTOR_ACAO];
            string oidTarefa = (string)mensagem.Propriedades[Constantes.OIDTAREFA];
            string idRequisicao = (string)mensagem.Propriedades[Constantes.ID_REQUISICAO];

            if(VerificarExisteEdicaoPara( login, oidCronograma ))
            {
                RnRecusarEdicaoTarefa( idRequisicao, oidCronograma, login, oidTarefa, login );
                return null;
            }

            if(!TarefaEstaLivre( oidCronograma, login, oidTarefa ))
            {
                string autorEdicao;
                if(tarefasEmEdicaoPorCronograma.ContainsKey( oidCronograma ) && tarefasEmEdicaoPorCronograma[oidCronograma].ContainsKey( oidTarefa ))
                    autorEdicao = tarefasEmEdicaoPorCronograma[oidCronograma][oidTarefa];
                else
                    autorEdicao = string.Empty;
                RnRecusarEdicaoTarefa( idRequisicao, oidCronograma, login, oidTarefa, autorEdicao );
                return null;
            }
            else
            {
                AdicionarTarefaATarefasEmEdicao( oidCronograma, login, oidTarefa );
                RnAutorizarEdicaoTarefa( oidCronograma, login, oidTarefa, idRequisicao );
                return mensagem;
            }
        }

        /// <summary>
        /// Método responsável por conceder a edição de uma tarefa a um usuário
        /// </summary>
        /// <param name="oidCronograma"> oid cronograma atual</param>
        /// <param name="login">login do colaborador</param>
        /// <param name="oidTarefa">oid da tarefa em edição</param>
        public virtual void RnAutorizarEdicaoTarefa( string oidCronograma, string login, string oidTarefa, string idRequisicao = "" )
        {
            Debug.WriteLine( String.Format( "Requisicao: {0} autorizada - OidTarefa: {1} - Autor: {2} Método: RnAutorizarEdicaoTarefa", idRequisicao, oidTarefa, login ) );
            LogInfo( string.Format( "{0} teve a edição da tarefa de oid {1} permitida  no cronograma {2} ", login, oidTarefa, oidCronograma ) );
            MensagemDto mensagem = Mensagem.RnCriarMensagemEdicaoTarefaAutorizada(  login, oidCronograma, oidTarefa, idRequisicao );
            if(ColaboradorConectado( login, oidCronograma ))
            {
                Debug.WriteLine( String.Format( "Requisicao: {0} autorizada e ENVIANDO para Usuario- OidTarefa: {1} - Autor: {2} Método: RnAutorizarEdicaoTarefa", idRequisicao, oidTarefa, login ) );
                ComunicarRespostaSolicitacaoAoUsuario( mensagem, login, oidCronograma );
            }
            else
            {
                Debug.WriteLine( String.Format( "Requisicao: {0} autorizada e NAO ENVIA para Usuario- OidTarefa: {1} - Autor: {2} Método: RnAutorizarEdicaoTarefa", idRequisicao, oidTarefa, login ) );
                RemoverTarefaDeEdicao( oidCronograma, oidTarefa );
            }
        }

        /// <summary>
        /// Responsável por armazenar tarefas em edição no dicionário
        /// </summary>
        /// <param name="oidCronograma">guid cronograma</param>
        /// <param name="login">login usuário que está editando a tarefa</param>
        /// <param name="oidTarefa"> guid da tarefa em edição</param>
        protected void AdicionarTarefaATarefasEmEdicao( string oidCronograma, string login, string oidTarefa )
        {
            if(!tarefasEmEdicaoPorCronograma.ContainsKey( oidCronograma ))
                tarefasEmEdicaoPorCronograma.Add( oidCronograma, new Dictionary<string, string>() );
            tarefasEmEdicaoPorCronograma[oidCronograma].Add( oidTarefa, login );

			if( AoIniciarEdicaoTarefa != null )
				AoIniciarEdicaoTarefa( oidCronograma , login , oidTarefa );
            LogDebug( string.Format( "tarefa {0} em edição para {1} no cronograma {2}", oidTarefa, login, oidCronograma ) );

        }

        /// <summary>
        /// Responsável por armazenar tarefas em exclusão no dicionário
        /// </summary>
        /// <param name="oidCronograma">guid cronograma</param>
        /// <param name="login">login usuário que está excluindo a tarefa</param>
        /// <param name="oidTarefa"> guid da tarefa em exclusão</param>
        protected void AdicionarTarefaATarefasEmExclusao( string oidCronograma, string login, string oidTarefa )
        {
            if(!tarefasEmExclusaoPorCronograma.ContainsKey( oidCronograma ))
                tarefasEmExclusaoPorCronograma.Add( oidCronograma, new Dictionary<string, string>() );

            tarefasEmExclusaoPorCronograma[oidCronograma].Add( oidTarefa, login );
        }

        /// <summary>
        /// Verificar a Possibilidade de Iniciar Edição da Tarefa
        /// </summary>
        /// <param name="oidCronograma">guid cronograma</param>
        /// <param name="login">login usuário que está editando a tarefa</param>
        /// <param name="oidTarefa"> guid da tarefa em edição</param>
        /// <returns>
        /// True - Pode Entrar em Edição
        /// False - Já está em Edição ou em Exclusão e Deverá ser recusada
        /// </returns>
        public bool TarefaEstaLivre( string oidCronograma, string login, string oidTarefa )
        {
            //Caso possua algum dos campos em branco deverá ser recusada
            if(string.IsNullOrEmpty( oidTarefa ) || string.IsNullOrEmpty( login ) || string.IsNullOrEmpty( oidCronograma ))
                return false;

            //Caso o cronograma não exista na hash de edição ou de exclusão
            if(!tarefasEmEdicaoPorCronograma.ContainsKey( oidCronograma ) && !tarefasEmExclusaoPorCronograma.ContainsKey( oidCronograma ))
                return true;

            //Caso o cronograma exista em alguma das hashs (Edição ou Exclusão) e contenha a tarefa
            if(( tarefasEmEdicaoPorCronograma.ContainsKey( oidCronograma ) && tarefasEmEdicaoPorCronograma[oidCronograma].ContainsKey( oidTarefa ) ) ||
                tarefasEmExclusaoPorCronograma.ContainsKey( oidCronograma ) && tarefasEmExclusaoPorCronograma[oidCronograma].ContainsKey( oidTarefa ))
                return false;

            //Caso não tenha correspondido a nenhuma das condições anteriores deverá permitir a edição da tarefa
            return true;
        }

        /// <summary>
        /// Responsável por remover de edição uma determinda tarefa
        /// </summary>
        /// <param name="mensagem"></param>
        public void RemoverTarefaDeEdicao( string oidCronograma, string oidTarefa )
        {
            //caso exista o cronograma na dicionario
            if(tarefasEmEdicaoPorCronograma.ContainsKey( oidCronograma ))
                if(tarefasEmEdicaoPorCronograma[oidCronograma].ContainsKey( oidTarefa ))
                {
                    tarefasEmEdicaoPorCronograma[oidCronograma].Remove( oidTarefa );
                    //caso não haja mais nenhuma tarefa em edição no determinado cronograma
                    //remove também o cronograma
                    if(tarefasEmEdicaoPorCronograma[oidCronograma].Count < 1)
                    {
                        tarefasEmEdicaoPorCronograma.Remove( oidCronograma );
                    }
                }
        }

        /// <summary>
        /// Método para remover tarefas da lista de tarefas em exclusão
        /// </summary>
        /// <param name="oidCronograma">oidCronograma do cronogramaAtual</param>
        /// <param name="login">login colaborador</param>
        /// <param name="tarefa">oidTarefa a ser removida da lista de exclusão</param>
        public void RemoverTarefaExclusao( string oidCronograma, string tarefa )
        {
            if(tarefasEmExclusaoPorCronograma.ContainsKey( oidCronograma ))
            {
                if(tarefasEmExclusaoPorCronograma[oidCronograma].ContainsKey( tarefa ))
                {
                    tarefasEmExclusaoPorCronograma[oidCronograma].Remove( tarefa );
                }

                if(tarefasEmExclusaoPorCronograma[oidCronograma].Count < 1)
                    tarefasEmExclusaoPorCronograma.Remove( oidCronograma );
            }
        }


        /// <summary>
        /// Realiza o processo de finalizar conexão de um WexMultiAccessClient
        /// Enviando uma mensagem de sinalização de fim de conexão, antes da remoção das informações
        /// do usuário da hash de usuários conectados em um cronograma
        /// </summary>
        /// <param name="mensagemAtual">MensagemDto do Tipo UsuarioDesconectado</param>
        protected virtual void FinalizarConexaoUsuario( MensagemDto mensagemAtual )
        {
            string[] logins = (string[])mensagemAtual.Propriedades[Constantes.USUARIOS];
            string oidCronograma = (string)mensagemAtual.Propriedades[Constantes.OIDCRONOGRAMA];
            foreach(string login in logins)
            {
                RemoverEdicoesUsuarioDesconectado( oidCronograma, login );
                RnRemoverUsuarioQueSeDesconectou( login, oidCronograma );
            }
        }


        /// <summary>
        /// Método utilizado para remover as edições do usuário que acabou de se desconectar
        /// </summary>
        /// <param name="oidCronograma"></param>
        /// <param name="login"></param>
        public void RemoverEdicoesUsuarioDesconectado( string oidCronograma, string login )
        {
            if(tarefasEmEdicaoPorCronograma.ContainsKey( oidCronograma ))
                if(tarefasEmEdicaoPorCronograma[oidCronograma].ContainsValue( login ))
                {
                    string[] oidTarefas = tarefasEmEdicaoPorCronograma[oidCronograma].Where( o => o.Value == login ).Select( o => o.Key ).ToArray();
                    foreach(string oidTarefa in oidTarefas)
                    {
                        RemoverTarefaDeEdicao( oidCronograma, oidTarefa );
                    }
                }

            if(tarefasEmExclusaoPorCronograma.ContainsKey( oidCronograma ))
                if(tarefasEmExclusaoPorCronograma[oidCronograma].ContainsValue( login ))
                {
                    string[] oidTarefas = tarefasEmExclusaoPorCronograma[oidCronograma].Where( o => o.Value == login ).Select( o => o.Key ).ToArray();
                    foreach(string oidTarefa in oidTarefas)
                    {
                        RemoverTarefaExclusao( oidCronograma, oidTarefa );
                    }
                }

            if(cronogramasComDadosEmEdicao != null && cronogramasComDadosEmEdicao.ContainsValue( login ))
            {
                if(cronogramasComDadosEmEdicao.ContainsKey( oidCronograma ) && cronogramasComDadosEmEdicao[oidCronograma] == login)
                {
                    cronogramasComDadosEmEdicao.Remove( oidCronograma );
					MensagemDto mensagem = Mensagem.RnCriarMensagemFimEdicaoDadosCronograma( oidCronograma , login );
                    filaProcessamento.Enqueue( mensagem );
                }
            }
        }

        /// <summary>
        /// Responsável por:
        /// - Receber Novas Mensagens das conexões clientes 
        /// - Mandar sintetizar as mensagensDto
        /// - Enfileirar as Mensagens sintetizadas nas conexões cliente agora para envio
        /// </summary>
        public virtual void RnProcessarEventos()
        {
            try
            {
                MensagemDto mensagemAtual;
            //Enquanto o Servidor estiver conectado
            while(ServidorEstiverConectado() || filaProcessamento.Count > 0)
            {
                //Se houver eventos enfileirados
                if(filaProcessamento.Count > 0)
                {
                    List<MensagemDto> listaMensagens = new List<MensagemDto>();
                    string oidCronograma;
                    while(filaProcessamento.Count > 0)
                    {
                        mensagemAtual = filaProcessamento.Dequeue();
                        mensagemAtual = RnProcessarMensagem( mensagemAtual );
                        if(mensagemAtual != null)
                            listaMensagens.Add( mensagemAtual );
                    }
                    //Resume A Lista de Eventos
                    listaMensagens = RnResumirMensagens( listaMensagens );
                    if(cronogramasConectados.Count == 0)
                        continue;
                    foreach(MensagemDto mensagem in listaMensagens)
                    {
                        if(mensagem.Tipo != CsTipoMensagem.ConexaoEfetuadaComSucesso)
                        {
                            if(mensagem.Propriedades.ContainsKey( Constantes.OIDCRONOGRAMA ))
                            {
                                oidCronograma = mensagem.Propriedades[Constantes.OIDCRONOGRAMA].ToString();
                                if(cronogramasConectados.ContainsKey( oidCronograma ))
                                    foreach(var colaborador in cronogramasConectados[oidCronograma])
                                    {
                                        colaborador.Value.EnfileirarMensagem( mensagem );
                                    }
                            }
                            else
                            {
                                foreach(var cronograma in cronogramasConectados)
                                {
                                    foreach(var conexao in cronograma.Value)
                                    {
                                        conexao.Value.EnfileirarMensagem(mensagem);
                                    }
                                }
                            }
                        }
                    }
                }
                Thread.Sleep( 1 );
            }
            }
            catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// Responsável por buscar os cronogramasOid em que o colaborador
        /// estiver conectado
        /// </summary>
        /// <param name="login">Login do Usuario</param>
        /// <returns>Retornar uma lista de cronogramasOid em que o colaborador estiver conectado</returns>
        private List<string> CronogramasEmQueOColaboradorEstaConectado( string login )
        {
            if(ColaboradorConectado( login ))
            {
                return usuariosConectados[login];
            }
            return null;
        }

        /// <summary>
        /// Responsável por remover um colaborador de  um determinado cronograma
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="oidCronograma">Cronograma</param>
        protected virtual void RnRemoverUsuarioQueSeDesconectou( string login, string oidCronograma )
        {
            if(ColaboradorConectado( login, oidCronograma ))
            {
                if(usuariosConectados[login].Count < 2)
                    usuariosConectados.Remove( login );
                else
                    usuariosConectados[login].Remove( oidCronograma );

                if(cronogramasConectados[oidCronograma].Count < 2 && cronogramasConectados[oidCronograma].ContainsKey( login ))
                {
                    cronogramasConectados[oidCronograma][login].Dispose();
                    cronogramasConectados.Remove( oidCronograma );
                }
                else
                {
                    cronogramasConectados[oidCronograma][login].Dispose();
                    cronogramasConectados[oidCronograma].Remove( login );
                }
				ExecutarNotificacaoDesconexaoUsuario( oidCronograma , login );
            }
        }

        /// <summary>
        /// Responsável por remover um colaborador de todos os cronogramas
        /// </summary>
        /// <param name="login">Login a ser removido</param>
        protected virtual void RnRemoverUsuarioQueSeDesconectou( string login )
        {
            if(ColaboradorConectado( login ))
            {
                string[] cronogramas = usuariosConectados[login].ToArray();
                foreach(string cronograma in cronogramas)
                {
                    RnRemoverUsuarioQueSeDesconectou( login, cronograma );
                }
            }
        }

        /// <summary>
        /// Método de validação de uma string Json
        /// </summary>
        /// <param name="json">MensagemDto em Json</param>
        /// <returns>
        /// True - Json Válido
        /// False - Json Inválido
        /// </returns>
        public static bool JsonEhValidar( string json )
        {
            JsonSchema schema = new JsonSchema()
            {
                Type = JsonSchemaType.Object,
                Properties =
                    new Dictionary<string, JsonSchema>
                    {
                        {Constantes.OIDCRONOGRAMA,new JsonSchema{Type = JsonSchemaType.String}},
                        {
                            Constantes.USUARIOS, new JsonSchema
                            { 
                                Type = JsonSchemaType.Array,
                                Items = new List<JsonSchema>{ new JsonSchema{Type = JsonSchemaType.String} }
                            }
                        }
                    }
            };
            try
            {
                JObject mensagemDto = JObject.Parse( @json );
                return mensagemDto.IsValid( schema );
            }
            catch(Exception ex)
            {
                LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                return false;
            }
        }

        /// <summary>
        /// Responsável por Efetuar um log de tempo de execução de um trecho de código
        /// </summary>
        /// <typeparam name="T">Tipo de Retorno Código</typeparam>
        /// <param name="codigo">Código em Delegate Anônimo</param>
        /// <param name="descricao">Nome do trecho de código</param>
        public static void Log( LogDebugCode codigo, string descricao )
        {
            LogDebug( String.Format( "******** [{0}] Entrou em processamento: {1} ********", Thread.CurrentThread.Name, descricao ) );
            if(temporizador == null)
            {
                temporizador = new Stopwatch();
                temporizador.Start();
            }
            lock(temporizador)
            {
                temporizador.Restart();
                try
                {
                    codigo();
                }
                catch(Exception ex)
                {
                    LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                    LogDebug( string.Format( "***** Excessão em {0} : {1} *******", descricao, ex.Message ) );

                    throw ex;
                }
                temporizador.Stop();
            }
            LogDebug( string.Format( "******** Saiu do processamento :{0}, tempo de execução de código: {1} milisegundos **********", descricao, temporizador.ElapsedMilliseconds ) );
        }

        /// <summary>
        /// Responsável por identificar se o servidor ainda está conectado
        /// </summary>
        /// <returns>
        /// True - Conectado
        /// False - Desconectado
        /// </returns>
        protected bool ServidorEstiverConectado()
        {
            return ConexaoAtiva != null && !deveSerDesativado;
        }
        #endregion
    }
}
