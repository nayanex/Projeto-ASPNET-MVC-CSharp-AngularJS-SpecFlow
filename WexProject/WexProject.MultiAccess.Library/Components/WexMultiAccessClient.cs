using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using WexProject.MultiAccess.Library.Dtos;
using Newtonsoft.Json;
using WexProject.MultiAccess.Library.Libs;
using System.Threading;
using WexProject.MultiAccess.Library.Domains;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using WexProject.MultiAccess.Library.Components;
using WexProject.MultiAccess.Library.Delegates;
using WexProject.MultiAccess.Library.Interfaces;
using WexProject.MultiAccess.Library.Exceptions;
using WexProject.Library.Libs.Extensions.Log;

namespace WexProject.MultiAccess.Library
{
    /// <summary>
    /// Componente de conexão de um cronograma com WexMultiAccessManager
    /// </summary>
    public partial class WexMultiAccessClient : Component, IWexMultiAccessClient
    {

        #region Atributos

        /// <summary>
        /// Resposável por gerenciar a instância Tcp de comunicação
        /// </summary>
        protected ITcpAdapter tcpAdapter;

        /// <summary>
        /// Responsável pelo tráfego de informações através de stream
        /// </summary>
        private NetworkStream stream;

        /// <summary>
        /// Responsável por escutar em tempo real a comunicação com o servidor;
        /// </summary>
        private Thread threadComunicaoServidor;

        /// <summary>
        /// Resposável por armazenar o status do servidor
        /// </summary>
        private bool conectado;

        /// <summary>
        /// Responsável por armazenar o estado da conexão
        /// </summary>
        private bool autenticado;
        #endregion

        #region Propriedades

        /// <summary>
        /// Responsável por armazenar se o cliente conseguiu se autenticar
        /// </summary>
        public bool Autenticado { get { return autenticado; } }

        /// <summary>
        /// Responsável por armazenar o nome do cronograma o qual o cliente está conectado
        /// </summary>
        public string OidCronograma { get; set; }

        /// <summary>
        /// Responsável por receber o valor literal do ip do servidor
        /// </summary>
        public string EnderecoIp { get; set; }

        /// <summary>
        /// Responsável por armazenar o login do colaborador
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Responsável por armazenar o número da porta de conexão com o servidor
        /// </summary>
        public int Porta { get; set; }

        /// <summary>
        /// Armazenar Mensagens Json Incompletas
        /// </summary>
        public string Buffer { get; set; }

        /// <summary>
        /// Responsável por armazenar o nome do servidor
        /// </summary>
        public string NomeServidor { get; set; }

        /// <summary>
        /// Armazenar o estado de conexão com o servidor
        /// </summary>
        public bool Conectado
        {
            get { return conectado; }
            set
            {
                conectado = value;
                if(!conectado)
                    if(AoSerDesconectado != null)
                        AoSerDesconectado();
            }
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Responsável por armazenar os métodos a serem disparados no evento de falha de conexão
        /// </summary>
        public virtual event AoFalharConexaoNoServidorEventHandler AoFalharConexaoNoServidor;
        /// <summary>
        /// Responsável por armazenar os métodos a serem disparados no evento de Conexão de Novo Usuário
        /// </summary>
        public virtual event AoConectarNovoUsuarioEventHandler AoConectarNovoUsuario;
        /// <summary>
        /// Evento Disparado Ao receber a recusa de uma conexão com o servidor
        /// </summary>
        public virtual event MensagemDtoEventHandler AoReceberConexaoRecusada;
        /// <summary>
        /// Evento Disparado Quando um usuário é desconectado
        /// </summary>
        public virtual event MensagemDtoEventHandler AoUsuarioDesconectar;

        /// <summary>
        /// Evento Disparado Quando um usuário é autenticado
        /// </summary>
        public virtual event MensagemDtoEventHandler AoSerAutenticadoComSucesso;

        /// <summary>
        /// Evento Disparado Quando o receber mensagem de que o servidor está desligando
        /// </summary>
        public virtual event MensagemDtoEventHandler AoServidorDesconectar;

        /// <summary>
        /// Evento Disparado quando um usuário criar uma tarefa.
        /// </summary>
        public virtual event MensagemDtoEventHandler AoSerCriadaNovaTarefa;

        /// <summary>
        /// Evento Disparado quando uma ou mais tarefas estiverem sendo editadas
        /// </summary>
        public virtual event MensagemDtoEventHandler AoIniciarEdicaoTarefa;

        /// <summary>
        /// Evento Disparado quando uma tentativa de edicao de tarefa for recusada pelo manager
        /// </summary>
        public virtual event MensagemDtoEventHandler AoSerRecusadaEdicaoTarefa;

        /// <summary>
        /// Evento disparado quando uma tarefa que estava bloqueada para edição para outro usuário tiver edição concluída
        /// </summary>
        public virtual event MensagemDtoEventHandler AoSerFinalizadaEdicaoTarefaPorOutroUsuario;

        /// <summary>
        /// Evento disparado quando uma tarefa acabou de ser excluida por algum usuário
        /// </summary>
        public virtual event MensagemDtoEventHandler AoSerExcluidaTarefaPorOutroUsuario;

        /// <summary>
        /// Evento disparado quando recebida a mensagem de Permissão de Exclusão das tarefas solicitadas
        /// </summary>
        public virtual event MensagemDtoEventHandler ExecutarExclusaoTarefa;

        /// <summary>
        /// Evento disparo quando recebida uma mensagem concedendo a edição da tarefa
        /// </summary>
        public virtual event MensagemDtoEventHandler AoSerAutorizadaEdicaoTarefa;

        /// <summary>
        /// Evento disparado quando é recebido o broadcast de aviso de que houve uma movimentação de tarefas no cronograma
        /// </summary>
        public virtual event MensagemDtoEventHandler AoOcorrerMovimentacaoPosicaoTarefa;

        /// <summary>
        /// Evento disparado quando é recebido a notificação de que o nome do cronograma foi alterado
        /// </summary>
        public virtual event MensagemDtoEventHandler AoSerNotificadoAlteracaoDadosCronograma;

        /// <summary>
        /// Evento disparado quando é recebida a notificação de não foi permitda a edição do nome do cronograma
        /// </summary>
        public virtual event MensagemDtoEventHandler AoSerRecusadaEdicaoDadosCronograma;

        /// <summary>
        /// Evento disparado quando outro usuário iniciar a edição do nome do cronograma
        /// </summary>
        public virtual event MensagemDtoEventHandler AoIniciarEdicaoDadosCronograma;

        /// <summary>
        /// Evento disparado ao receber a notificação de permissão de atualização do nome do cronograma
        /// </summary>
        public virtual event MensagemDtoEventHandler AoSerPermitidaEdicaoDadosCronograma;

        /// <summary>
        /// Evento disparado quando o multiacesscliente é desconectado
        /// </summary>
        public virtual event Action AoSerDesconectado;

        /// <summary>
        /// Evento disparado quando o alguma exception for logada.
        /// </summary>
        public event EventHandler LogarAoOcorrerException;


        #endregion

        #region Construtores
        /// <summary>
        /// método de inicialização que possui as inicializações comuns entre os construtores
        /// </summary>
        private void InicializaClasse()
        {
            InitializeComponent();
            conectado = false;
        }

        /// <summary>
        /// Construtor para Inicialização do component
        /// </summary>
        public WexMultiAccessClient()
        {
            InicializaClasse();
        }

        /// <summary>
        /// Construtor para Inicialização do component
        /// </summary>
        /// <param name="container"></param>
        public WexMultiAccessClient( IContainer container )
        {
            container.Add( this );
            InicializaClasse();
        }
        #endregion

        #region Regras de Negócio
        /// <summary>
        /// Analisar constantemente se o WexMultiAccessClient ainda está conectado
        /// </summary>
        /// <returns>
        /// - Falso para não conectado
        /// - Verdadeiro para conectado
        /// </returns>
        public bool ValidarConexao()
        {
            Conectado = tcpAdapter != null && tcpAdapter.ValidarConexao();
            return conectado;
        }
        /// <summary>
        /// Responsável pela tentativa de Conexão do Componente MultiAccessClient 
        /// disparando evento AoFalharConexaoNoServidor em caso de falha de conexão
        /// </summary>
        public virtual void Conectar()
        {
            if(OidCronograma != null && Login != null)
            {
                try
                {
                    if(tcpAdapter == null || ( tcpAdapter != null && !tcpAdapter.Conectado ))
                    {
                        tcpAdapter = TcpAdapterFactory();
                        //tentativa de conexão no ip e porta predefinidos     
                        tcpAdapter.Conectar( EnderecoIp, Porta );
                        Conectado = true;
                        autenticado = true;
                        threadComunicaoServidor = new Thread( RnProcessarEventos )
                        {
                            IsBackground = true,
                            Priority = ThreadPriority.Normal,
                            Name = string.Format( "Client#{0}", Login )
                        };
                        threadComunicaoServidor.Start();
                    }
                    else
                        return;
                    RnEnviarMensagemIdentificao();
                }
                catch(Exception excessao)
                {
                    // verifica instância do tcpCliente tal como se está conectado
                    AcionarEventoAoFalharConexao();
                    Debug.WriteLine( string.Format( "AccessClient({2})Mensagem:{0}\nCallStack{1}", excessao.Message, excessao.StackTrace, Login ), "JsonConvert:" );
                    DispararExcessao( excessao );
                }
            }
            else
            {
                MessageBox.Show( "Deve se preencher as Propriedades CronogramaId e o Login do MultiAccessClient" );
            }
        }

        /// <summary>
        /// Retornar um instância de um TcpAdapter
        /// </summary>
        /// <returns></returns>
        protected virtual ITcpAdapter TcpAdapterFactory()
        {
            return new TcpAdapter();
        }

        private void DispararExcessao( Exception excessao )
        {
            if(LogarAoOcorrerException != null)
            {
                LogarAoOcorrerException( excessao, new EventArgs() );
            }
        }

        /// <summary>
        /// Método responsável por acionar evento de falha de conexão 
        /// validando se há um método instânciado no evento
        /// </summary>
        public virtual void AcionarEventoAoFalharConexao()
        {
            conectado = false;
            autenticado = false;
            if(AoFalharConexaoNoServidor == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoFalharConexaoNoServidor" );
        }

        /// <summary>
        /// Responsável por processar a mensagem de ConexaoEfetuadaComSucesso
        /// e acionamento do evento de ConexaoEfetuadaComSucesso
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo ConexaoEfetuadaComSucesso</param>
        private void ProcessarMensagemDeConexaoEfetuadaComSucesso( MensagemDto mensagem )
        {
            AcionarEventoDeConexaoEfetuadaComSucesso( mensagem );
            Conectado = true;
            autenticado = true;
        }

        /// <summary>
        /// Método responsável por acionar evento de UsuarioConectadoComSucesso e validar
        /// seu disparo
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo ConexaoEfetuadaComSucesso</param>
        public virtual void AcionarEventoDeConexaoEfetuadaComSucesso( MensagemDto mensagem )
        {
            if(AoSerAutenticadoComSucesso == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoSerAutenticadoComSucesso" );
            AoSerAutenticadoComSucesso( mensagem );
        }

        /// <summary>
        /// Método responsável por acionar evento de MovimentacaoPosicaoTarefa e validar
        /// seu disparo
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo MovimentacaoPosicaoTarefa</param>
        public virtual void AcionarEventoMovimentacaoPosicaoTarefa( MensagemDto mensagem )
        {
            if(AoOcorrerMovimentacaoPosicaoTarefa == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoOcorrerMovimentacaoPosicaoTarefa" );

            AoOcorrerMovimentacaoPosicaoTarefa( mensagem );
        }

        /// <summary>
        /// Método responsável por acionar evento de MovimentacaoPosicaoTarefa e validar
        /// seu disparo
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo MovimentacaoPosicaoTarefa</param>
        public virtual void AcionarEventoAoSerNotificadoModificacaoDadosCronograma( MensagemDto mensagem )
        {
            if(AoSerNotificadoAlteracaoDadosCronograma == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoSerNotificadoAlteracaoNomeCronograma" );

            AoSerNotificadoAlteracaoDadosCronograma( mensagem );
        }

        /// <summary>
        /// Método utilizado para executar o evento de edição de nome de cronograma recusada
        /// </summary>
        /// <param name="objetoMensagem"></param>
        public virtual void AcionarEventoAoSerRecusadaEdicaoNomeCronograma( MensagemDto mensagem )
        {
            if(AoSerRecusadaEdicaoDadosCronograma == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoSerRecusadaEdicaoNomeCronograma" );

            AoSerRecusadaEdicaoDadosCronograma( mensagem );
        }

        /// <summary>
        /// Método utilizado para executar o evento de notificação de edição do nome do cronograma
        /// </summary>
        /// <param name="objetoMensagem"></param>
        public virtual void AcionarEventoAoSerIniciadaEdicaoNomeCronograma( MensagemDto mensagem )
        {
            if(AoIniciarEdicaoDadosCronograma == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoIniciarEdicaoNomeCronograma" );

            AoIniciarEdicaoDadosCronograma( mensagem );
        }

        /// <summary>
        /// Método utilizado para sinalizar a permissão de edição do nome do cronograma
        /// </summary>
        /// <param name="objetoMensagem"></param>
        public virtual void AcionarEventoAoPermitirEdicaoNomeCronograma( MensagemDto objetoMensagem )
        {
            if(AoSerPermitidaEdicaoDadosCronograma == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoSerPermitidaEdicaoNomeCronograma" );
            AoSerPermitidaEdicaoDadosCronograma( objetoMensagem );
        }

        /// <summary>
        /// Método responsável por acionar evento de UsuarioConectadoComSucesso e validar
        /// seu disparo
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo ConexaoEfetuadaComSucesso</param>
        public virtual void AcionarEventoDeAutorizacaoEdicaoTarefa( MensagemDto mensagem )
        {
            if(AoSerAutorizadaEdicaoTarefa == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoSerAutorizadaEdicaoTarefa" );
            AoSerAutorizadaEdicaoTarefa( mensagem );
        }

        /// <summary>
        /// Responsável por processar a mensagem de FinalizarExclusaoTarefa
        /// e acionamento do evento de AoSerExcluidaTarefa
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo ConexaoEfetuadaComSucesso</param>
        private void ProcessarMensagemDeExclusaoTarefa( MensagemDto mensagem )
        {
            AcionarEventoAoTarefaSerExcluida( mensagem );
        }

        /// <summary>
        /// Método responsável por acionar evento de AoSerExcluidaTarefa e validar
        /// seu disparo.
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo ConexaoEfetuadaComSucesso</param>
        public virtual void AcionarEventoAoTarefaSerExcluida( MensagemDto mensagem )
        {
            if(AoSerExcluidaTarefaPorOutroUsuario == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoSerExcluidaTarefaPorOutroUsuario" );
            AoSerExcluidaTarefaPorOutroUsuario( mensagem );
        }

        /// <summary>
        /// Método responsável por acionar evento de AoSerFinalizadaEdicaoTarefaPorOutroUsuario e validar
        /// seu disparo
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo FinalizarEdicaoTarefa</param>
        public virtual void AcionarEventoAoSerFinalizadaEdicaoTarefaPorOutroUsuario( MensagemDto mensagem )
        {
            if(AoSerFinalizadaEdicaoTarefaPorOutroUsuario == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoSerFinalizadaEdicaoTarefaPorOutroUsuario" );
            AoSerFinalizadaEdicaoTarefaPorOutroUsuario( mensagem );
        }

        /// <summary>
        /// Método responsável por acionar evento de ExecutarExclusaoTarefa e validar
        /// seu disparo
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo PermitirExclusaoTarefa</param>
        public virtual void AcionarEventoExecutarExclusaoTarefa( MensagemDto mensagem )
        {
            if(ExecutarExclusaoTarefa == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento ExecutarExclusaoTarefa" );
            ExecutarExclusaoTarefa( mensagem );
        }


        /// <summary>
        /// Método responsável por acionar evento de AoIniciarEdicaoTarefa e validar
        /// seu disparo
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo ConexaoEfetuadaComSucesso</param>
        public virtual void AcionarEventoAoIniciarEdicaoTarefa( MensagemDto mensagem )
        {
            if(AoIniciarEdicaoTarefa == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoIniciarEdicaoTarefa" );
            AoIniciarEdicaoTarefa( mensagem );
        }

        /// <summary>
        /// Método responsável por acionar evento de AoIniciarEdicaoTarefa e validar
        /// seu disparo
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo AoEdicaoTarefaSerRecusada</param>
        public virtual void AcionarEventoAoEdicaoTarefaSerRecusada( MensagemDto mensagem )
        {
            if(AoSerRecusadaEdicaoTarefa == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoSerRecusadaEdicaoTarefa" );
            AoSerRecusadaEdicaoTarefa( mensagem );
        }

        /// <summary>
        /// Responsável por processar mensagem do tipo AoServidorRecusarConexao e disparar
        /// o metodo de acionarEventoAoServidorRecusarConexao
        /// </summary>
        /// <param name="mensagem">MensagemDto do tipo UsuarioDesconectado</param>
        private void ProcessarMensagemAoServidorRecusarConexao( MensagemDto mensagem )
        {
            RnDesconectar();
            AcionarEventoAoServidorRecusarConexao( mensagem );
        }

        /// <summary>
        /// Método responsável por disparar o evento de recusa de conexão
        /// </summary>
        /// <param name="mensagem">Motivo de recusa de conexão</param>
        protected virtual void AcionarEventoAoServidorRecusarConexao( MensagemDto mensagem )
        {
            if(AoReceberConexaoRecusada == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoReceberConexaoRecusada" );
            AoReceberConexaoRecusada( mensagem );
        }

        /// <summary>
        /// Responsável por processar mensagem do tipo UsuarioDesconectar e disparar
        /// o metodo de acionarEventoUsuarioDesconectar
        /// </summary>
        /// <param name="mensagem">MensagemDto do tipo UsuarioDesconectado</param>
        private void ProcessarMensagemAoUsuarioDesconectar( MensagemDto mensagem )
        {
            AcionarEventoAoUsuarioDesconectar( mensagem );
        }

        /// <summary>
        /// Método responsável por disparar o evento quando outro usuário se desconecta
        /// </summary>
        /// <param name="mensagem">MensagemDto do tipo UsuarioDesconectado</param>
        public virtual void AcionarEventoAoUsuarioDesconectar( MensagemDto mensagem )
        {
            if(AoUsuarioDesconectar == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoUsuarioDesconectar" );
            AoUsuarioDesconectar( mensagem );
        }

        /// <summary>
        /// Resposável por efetuar finalização do client e acionamento do evento disparado
        /// ao verificar a desconexão dos servidor
        /// </summary>
        /// <param name="mensagem">Mensagem de servidor desconectado</param>
        private void ProcessarMensagemServidorDesconectando( MensagemDto mensagem )
        {
            RnDesconectar();
            AcionarEventoAoServidorDesconectar( mensagem );
        }

        /// <summary>
        /// Método executado quando o servidor é desconectado
        /// </summary>
        /// <param name="mensagem">Mensagem de servidor desconectado</param>
        protected virtual void AcionarEventoAoServidorDesconectar( MensagemDto mensagem )
        {
            if(AoServidorDesconectar == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoServidorDesconectar" );
            AoServidorDesconectar( mensagem );
        }

        /// <summary>
        /// Responsável por processar a mensagem de NovoUsuarioConectado e
        /// acionar o método de disparo de evento
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo NovoUsuarioConectado</param>
        private void ProcessarMensagemDeNovoUsuarioConectado( MensagemDto mensagem )
        {
            AcionarEventoAoConectarNovoUsuario( mensagem, Login );
        }

        /// <summary>
        /// Método responsável por disparar o evento ao Conectar um Novo Usuário
        /// </summary>
        public virtual void AcionarEventoAoConectarNovoUsuario( MensagemDto mensagem, string login )
        {
            if(AoConectarNovoUsuario == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoConectarNovoUsuario" );
            AoConectarNovoUsuario( mensagem, login );
        }

        /// <summary>
        /// Método responsável por disparar o evento ao ser criada uma nova tarefa
        /// </summary>
        /// <param name="mensagem">mensagem de CriacaoNovaTarefa</param>
        public virtual void AcionarEventoAoNovaTarefaSerCriada( MensagemDto mensagem )
        {
            if(AoSerCriadaNovaTarefa == null)
                throw new EventoNuloException( "Deve ser associado um comportamento ao evento AoSerCriadaNovaTarefa" );
            AoSerCriadaNovaTarefa( mensagem );
        }

        /// <summary>
        /// Responsável por fazer a escuta em tempo real da threadComunicaoServidor
        /// </summary>
        protected virtual void RnProcessarEventos()
        {
            String mensagemJson = "";
            MensagemDto m;
            List<string> mensagens;
            string msgAtual = "";
            try
            {
                while(autenticado)
                {

                    if(tcpAdapter == null || !tcpAdapter.ValidarConexao())
                    {
                        ProcessarMensagemEvento( Mensagem.RnCriarMensagemServidorDesconectando( "Você acabou se ser desconectado, verifique sua rede ou contate o administrador" ) );
                        continue;
                    }
                    if(!autenticado)
                        continue;
                    if(!tcpAdapter.PossuiDadosParaLeitura())
                    {
                        Thread.Sleep( 15 );
                        continue;
                    }
                    mensagemJson = tcpAdapter.EfetuarLeitura();
                    if(mensagemJson.Trim() == "")
                        continue;

                    if(mensagemJson.Contains( "\n" ))
                    {
                        mensagens = mensagemJson.Split( '\n' ).ToList();
                        foreach(string msg in mensagens)
                        {
                            if(String.IsNullOrEmpty( msg ))
                                continue;

                            msgAtual = msg.Trim();
                            if(Buffer != "")
                            {
                                msgAtual = Buffer + msgAtual;
                                Buffer = "";
                            }
                            m = Mensagem.DeserializarMensagemDto( TcpUtil.RemoverStringProtecaoDeIntegridade( msgAtual ) );
                            ProcessarMensagemEvento( m );
                            msgAtual = "";
                        }
                    }
                    else
                        Buffer += mensagemJson;

                    Thread.Sleep( 10 );
                }
            }
            catch(JsonException excessao)
            {
                Debug.WriteLine( string.Format( "MultiAccessClient do {3} (Leitura) - Mensagem Json:{2}Excessão:{0}\nCallStack{1}", excessao.Message, excessao.StackTrace, msgAtual, Login ), "JsonConvert:" );
                //continue;
                throw excessao;
            }
            catch(InvalidOperationException e)
            {
                Debug.WriteLine( string.Format( "Ocorreu a excessão:{0}\nLocal:{1}", e.Message, e.StackTrace ), Thread.CurrentThread.Name );
                ProcessarMensagemEvento( Mensagem.RnCriarMensagemServidorDesconectando( String.Format( "{0}\n {1}", e.Message, e.StackTrace ) ) );
                RnDesconectar();
                throw e;
            }
            catch(Exception e)
            {
                DumperExtension.ShowStackTrace("Exception no client.");
                Debug.WriteLine( string.Format( "Ocorreu a excessão:{0}\nLocal:{1}\n Target Site:{2}", e.Message, e.StackTrace, e.TargetSite ), Thread.CurrentThread.Name );
                RnDesconectar();
                throw e;
            }
        }

        /// <summary>
        /// Método responsável por ao receber uma mensagem definir o evento disparado pela mensagem
        /// </summary>
        /// <param name="objeto">MensagemDto Recebida do Manager</param>
        protected virtual void ProcessarMensagemEvento( MensagemDto objeto )
        {
            if(objeto != null)
            {
                MensagemDto objetoMensagem = (MensagemDto)objeto;
                string autorAcao;
                string json = JsonConvert.SerializeObject( objetoMensagem );
                if(objetoMensagem.Propriedades.ContainsKey( Constantes.AUTOR_ACAO ))
                    autorAcao = objetoMensagem.Propriedades[Constantes.AUTOR_ACAO] as string;
                else
                    autorAcao = "-";
                Debug.WriteLine( string.Format( "\nRecebido {0} - Autor:{1}\nJson:{2}", objetoMensagem.Tipo, autorAcao, json ) );
                switch(objetoMensagem.Tipo)
                {
                    case CsTipoMensagem.NovosUsuariosConectados:
                        ProcessarMensagemDeNovoUsuarioConectado( objetoMensagem );
                        break;
                    case CsTipoMensagem.ConexaoRecusadaServidor:
                        ProcessarMensagemAoServidorRecusarConexao( objetoMensagem );
                        break;
                    case CsTipoMensagem.UsuarioDesconectado:
                        ProcessarMensagemAoUsuarioDesconectar( objetoMensagem );
                        break;
                    case CsTipoMensagem.ServidorDesconectando:
                        ProcessarMensagemServidorDesconectando( objetoMensagem );
                        break;
                    case CsTipoMensagem.NovaTarefaCriada:
                        AcionarEventoAoNovaTarefaSerCriada( objetoMensagem );
                        break;
                    case CsTipoMensagem.InicioEdicaoTarefa:
                        AcionarEventoAoIniciarEdicaoTarefa( objetoMensagem );
                        break;
                    case CsTipoMensagem.EdicaoTarefaRecusada:
                        AcionarEventoAoEdicaoTarefaSerRecusada( objetoMensagem );
                        break;
                    case CsTipoMensagem.ConexaoEfetuadaComSucesso:
                        ProcessarMensagemDeConexaoEfetuadaComSucesso( objetoMensagem );
                        break;
                    case CsTipoMensagem.EdicaoTarefaFinalizada:
                        AcionarEventoAoSerFinalizadaEdicaoTarefaPorOutroUsuario( objetoMensagem );
                        break;
                    case CsTipoMensagem.ExclusaoTarefaFinalizada:
                        ProcessarMensagemDeExclusaoTarefa( objetoMensagem );
                        break;
                    case CsTipoMensagem.EdicaoTarefaAutorizada:
                        AcionarEventoDeAutorizacaoEdicaoTarefa( objetoMensagem );
                        break;
                    case CsTipoMensagem.ExclusaoTarefaPermitida:
                        AcionarEventoExecutarExclusaoTarefa( objetoMensagem );
                        break;
                    case CsTipoMensagem.MovimentacaoPosicaoTarefa:
                        AcionarEventoMovimentacaoPosicaoTarefa( objetoMensagem );
                        break;
                    case CsTipoMensagem.DadosCronogramaAlterados:
                        AcionarEventoAoSerNotificadoModificacaoDadosCronograma( objetoMensagem );
                        break;
                    case CsTipoMensagem.EdicaoNomeCronogramaRecusada:
                        AcionarEventoAoSerRecusadaEdicaoNomeCronograma( objetoMensagem );
                        break;
                    case CsTipoMensagem.InicioEdicaoNomeCronograma:
                        AcionarEventoAoSerIniciadaEdicaoNomeCronograma( objetoMensagem );
                        break;
                    case CsTipoMensagem.EdicaoNomeCronogramaPermitida:
                        AcionarEventoAoPermitirEdicaoNomeCronograma( objetoMensagem );
                        break;
                }
            }
        }

        /// Resposável por efetuar qualquer procedimento de pré-configuração ou tratamento na mensagem de CriacaoNovaTarefa
        /// </summary>
        /// <param name="objetoMensagem">MensagemDto CriacaoNovaTarefa</param>
        protected virtual void ProcessarMensagemMensagemNovaTarefaCriada( MensagemDto objetoMensagem )
        {
            objetoMensagem.Propriedades.Add( Constantes.LOGIN_WEX_CLIENT, Login );
            AcionarEventoAoNovaTarefaSerCriada( objetoMensagem );
        }

        /// <summary>
        /// Método responsável por serializar e enviar A mensagem do colaborador
        /// </summary>
        public virtual void RnEnviarMensagemIdentificao()
        {
            try
            {
                MensagemDto objetoMensagem = Mensagem.RnCriarMensagemNovoUsuarioConectado( new string[] { Login }, OidCronograma );
                string mensagemJson = Mensagem.Serializar( objetoMensagem );
                tcpAdapter.EnviarMensagem( mensagemJson );
            }
            catch(Exception excessao)
            {
                throw excessao;
            }
        }

        /// <summary>
        /// Método responsável por comunicar ao MultiAccessManager (Servidor) de que uma nova tarefa foi criada.
        /// </summary>
        /// <param name="oidNovaTarefa">Oid da nova tarefa</param>
        /// <param name="oidCronograma">Oid do cronograma atual</param>
        public void RnComunicarNovaTarefaCriada( string oidNovaTarefa, string oidCronograma, Dictionary<string, Int16> tarefasImpactadas, DateTime dataHoraAcao )
        {
            MensagemDto mensagem = Mensagem.RnCriarMensagemNovaTarefaCriada( oidNovaTarefa, Login, oidCronograma, tarefasImpactadas, dataHoraAcao );
            RnEnviarMensagem( mensagem );
        }

        /// <summary>
        /// Método responsável por comunicar ao MultiAccessManager de que uma tarefa persistida está sendo editada
        /// </summary>
        /// <param name="oidTarefa">guid da tarefa editada</param>
        /// <param name="autoSalvarEdicao">boolean para informar se deve auto salvar a edição caso receba a resposta de autorização da edição da tarefa</param>
        public void RnComunicarInicioEdicaoTarefa( string oidTarefa, string idRequisicao = "" )
        {
            Debug.WriteLine( String.Format( "Enviando Requisicao: {0} - OidTarefa: {1} - Autor: {2}", idRequisicao, oidTarefa, Login ) );
            MensagemDto mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa, Login, OidCronograma, idRequisicao );
            RnEnviarMensagem( mensagem );
        }

        /// <summary>
        ///  Responsável por finalizar a conexão, avisando o servidor de que está se desconectando
        /// </summary>
        public void RnFinalizarConexao( bool forcarAtualizacao = false )
        {
            if(ValidarConexao())
            {
                ChamadaAssincrona metodo = () =>
                {
                    MensagemDto mensagemDesconectar = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { Login }, OidCronograma, forcarAtualizacao );
                    string mensagemJson = JsonConvert.SerializeObject( mensagemDesconectar );
                    mensagemJson = TcpUtil.AdicionarStringProtecaoDeIntegridade( mensagemJson );
                    tcpAdapter.EnviarMensagem( mensagemJson );
                };

                IAsyncResult result = metodo.BeginInvoke( null, null );
                result.AsyncWaitHandle.WaitOne();
            }

            RnDesconectar();
        }

        /// <summary>
        /// Responsável por desconectar o cliente  encerrando a conexão
        /// e finalizando a thread de comunicação
        /// </summary>
        public void RnDesconectar()
        {
            autenticado = false;
            Conectado = false;

            if(tcpAdapter != null && tcpAdapter.ValidarConexao())
                try
                {
                    lock(tcpAdapter)
                    {
                        tcpAdapter.Dispose();
                    }
                }
                catch(SocketException e)
                {
                    Debug.WriteLine( String.Format( "Message: {0}, Stacktrace:{1}", e.Message, e.StackTrace ), Thread.CurrentThread.Name );
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
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Responsável por sinalizar o manager de que foi encerrada a edição de uma tarefa
        /// </summary>
        /// <param name="oidTarefa">guid da tarefa que estava em edição</param>
        public virtual void RnComunicarFimEdicaoTarefa( string oidTarefa )
        {
            MensagemDto mensagem = Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( oidTarefa, Login, OidCronograma );
            RnEnviarMensagem( mensagem );
        }

        /// <summary>
        /// Responsável por comunicar o manager quais tarefas o colaborador deseja excluir
        /// </summary>
        /// <param name="tarefas"></param>
        public void RnComunicarInicioExclusaoTarefa( string[] tarefas )
        {
            MensagemDto mensagem = Mensagem.RnCriarMensagemInicioExclusaoTarefas( tarefas, Login, OidCronograma );
            RnEnviarMensagem( mensagem );
        }

        /// <summary>
        /// Responsável por comunicar o manager quais tarefas foram realmente excluídas
        /// </summary>
        /// <param name="tarefas">vetor de tarefas efetivamente excluidas</param>
        /// <param name="tarefasNaoExcluidas">vetor de tarefas que não puderam ser excluidas</param>
        public void RnComunicarFimExclusaoTarefaConcluida( string[] tarefas, Dictionary<string, Int16> tarefasImpactadas, string[] tarefasNaoExcluidas, DateTime dataHoraAcao )
        {
            MensagemDto mensagem = Mensagem.RnCriarMensagemComunicarExclusaoTarefaConcluida( tarefas, tarefasImpactadas, OidCronograma, Login, dataHoraAcao, tarefasNaoExcluidas );
            RnEnviarMensagem( mensagem );
        }

        /// <summary>
        /// Método responsável por comunicar uma movimentação de tarefa no grid
        /// </summary>
        /// <param name="posicaoAtual">NBID atual da tarefa</param>
        /// <param name="posicaoFinal">NBID da posicao final</param>
        /// <param name="oidTarefa">oid da tarefa atual</param>
        /// <param name="oidTarefasImpactadas">dicionario de tarefas impactadas reordenadas</param>
        public void RnComunicarMovimentacaoTarefa( short posicaoAtual, short posicaoFinal, string oidTarefa, Dictionary<string, Int16> oidTarefasImpactadas, DateTime dataHoraAcao )
        {
            MensagemDto mensagem = Mensagem.RnCriarMensagemMovimentacaoTarefa( posicaoAtual, posicaoFinal, oidTarefa, oidTarefasImpactadas, Login, OidCronograma, dataHoraAcao );
            RnEnviarMensagem( mensagem );
        }

        /// <summary>
        /// Método responsável por comunicar ao manager sobre a modificação do nome do cronograma
        /// </summary>
        /// <param name="nomeCronograma">novo nome do cronograma</param>
        public virtual void RnComunicarAlteracaoDadosCronograma()
        {
			MensagemDto mensagem = Mensagem.RnCriarMensagemFimEdicaoDadosCronograma( OidCronograma , Login );
            RnEnviarMensagem( mensagem );
        }

        /// <summary>
        /// Método responsável por comunicar o inicio da edição do nome do cronograma atual
        /// </summary>
        public virtual void RnComunicarInicioEdicaoDadosCronograma()
        {
            MensagemDto mensagem = Mensagem.RnCriarMensagemInicioEdicaoNomeCronograma( OidCronograma, Login );
            RnEnviarMensagem( mensagem );
        }

        /// <summary>
        /// Método responsável por enviar mensagem ao manager
        /// </summary>
        /// <param name="mensagem">mensagem a ser enviada</param>
        protected virtual void RnEnviarMensagem( MensagemDto mensagem )
        {
            string json = JsonConvert.SerializeObject( mensagem );
            if(tcpAdapter == null || !tcpAdapter.ValidarConexao())
            {
                Debug.WriteLine( string.Format( "\nFalha no envio da mensagem({0})- {2} - Cronograma:{3}\nObs: conexão inválida ou desconectada!\nJson:{1} ", mensagem.Tipo, json, Login, OidCronograma ) );
                return;
            }
            Debug.WriteLine( string.Format( "\nMensagem Enviada! ({0}) - {2} - Cronograma:{3}\nJson:{1}", mensagem.Tipo, json, Login, OidCronograma ) );
            json = TcpUtil.AdicionarStringProtecaoDeIntegridade( json );
            tcpAdapter.EnviarMensagem( json );
        }
        #endregion
    }
}
