using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Net.Sockets;
using WexProject.MultiAccess.Library.Dtos;
using WexProject.MultiAccess.Library.Domains;
using WexProject.MultiAccess.Library.ArgumentosEventos;
using System.Reflection;
using log4net;
using WexProject.Library.Libs.Delegates.Logger;
using WexProject.Library.Libs.Extensions.Log;

namespace WexProject.MultiAccess.Library.Libs
{
    /// <summary>
    /// Classe de que trata a conexão do cliente em threads próprias de comunicação
    /// </summary>
    public class ConexaoCliente : IDisposable
    {
        #region Constantes
        /// <summary>
        /// Constante de armazenamento de palavra-chave, índice de propriedade em uma MensagemDto
        /// </summary>
        protected const string OIDCRONOGRAMA = "oidCronograma";

        /// <summary>
        /// Constante de armazenamento de palavra-chave, índice de propriedade em uma MensagemDto
        /// </summary>
        protected const string USUARIOS = "usuarios";

        #endregion

        #region Atributos


        /// <summary>
        /// Thread Responsável por receber e enfileirar novas mensagens a serem processadas no Manager
        /// </summary>
        public Thread threadProcessarLeitura;

        /// <summary>
        /// Permissão de funcionamento da thread de leitura 
        /// </summary>
        protected bool statusLeitura;

        /// <summary>
        /// Permissão de funcionamento da thread de escrita 
        /// </summary>
        protected bool statusEscrita;

        /// <summary>
        /// Verificar se a conexão está efetuando o envio de mensagens ao respectivo cliente
        /// </summary>
        protected bool executandoEnvioMensagens;

        /// <summary>
        /// Efetua o processamento das mensagens assincronamente
        /// </summary>
        protected Action processarMensagensAsync;

        /// <summary>
        /// Atributo que instancia o log (log4net)
        /// </summary>
        private readonly ILog log = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// Controlar o acesso ao método de processamento de escrita
        /// </summary>
        private readonly object lockerProcessarEscrita = new object();

        #endregion

        #region Propriedades
        /// <summary>
        /// Instancia do Socket de conexão com o cliente
        /// </summary>
        public TcpClient TcpCliente { get; set; }

        /// <summary>
        /// login do colaborador proprietário da conexão
        /// </summary>
        public string LoginCliente { get; set; }
        /// <summary>
        /// Propriedade que armazena as mensagens que serão enfileiradas no WexMultiAccessManager
        /// </summary>
        public Queue<MensagemDto> FilaLeitura { get; set; }

        /// <summary>
        /// Propriedade que armazena as mensagens que serão enviadas ao WexMultiAccessClient
        /// </summary>
        public Queue<MensagemDto> FilaEscrita { get; private set; }

        /// <summary>
        /// Retornar estado de Permissão de funcionamento da threadProcessarEscrita
        /// </summary>
        public bool PermissaoDeEscrita { get { return statusEscrita; } }
        /// <summary>
        /// Retornar estado de Permissão de funcionamento da threadProcessarLeitura
        /// </summary>
        public bool PermissaoDeLeitura { get { return statusLeitura; } }
        /// <summary>
        /// Armazenar Mensagens Json Incompletas
        /// </summary>
        public string Buffer { get; set; }

        /// <summary>
        /// Responsável por armazenar o cronograma em que está conectado
        /// </summary>
        public string OidCronograma { get; set; }
        #endregion

        #region Construtores
        /// <summary>
        /// Responsável por iniciar a thread de escuta de um cliente especifico, e receber a fila de eventos a processar
        /// </summary>
        /// <param name="login">login do colaborador</param>
        /// <param name="tcp">Socket de conexão com o socket do colaborador</param>
        /// <param name="fila">referencia a fila para ordenação da execuçao do processamento de mensagens</param>
        public ConexaoCliente( string login, TcpClient tcp, Queue<MensagemDto> fila )
        {
            LoginCliente = login;
            TcpCliente = tcp;
            FilaLeitura = fila;
            FilaEscrita = new Queue<MensagemDto>();
            processarMensagensAsync = RnProcessarFilaEscrita;
            InicializarConexao();
        }

        /// <summary>
        /// Inicialização de Threads e variaveis Pós-Construção do Objeto
        /// </summary>
        private void InicializarConexao()
        {
            //Inicializar Buffer de mensagens
            Buffer = "";
            //Iniciar Threads da ConexaoCliente
            try
            {
                statusLeitura = true;
                statusEscrita = true;
                threadProcessarLeitura = new Thread( RnProcessarLeitura )
                {
                    Name = String.Format( "Manager#{0} (Recebeu)", LoginCliente ),
                    Priority = ThreadPriority.Normal
                };
                threadProcessarLeitura.Start();
            }
            catch(Exception ex)
            {
                log.Info( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Delegate que representa o evento de quando ocorrer uma desconexão inesperada
        /// </summary>
        /// <param name="sender">Conexão cliente atual</param>
        /// <param name="e"></param>
        public delegate void DesconexaoInesperadaEventHandler( object sender, DesconexaoInesperadaEventArgs e );

        public delegate void AoReceberMensagemErradaEventHandler( MensagemDto mensagem );

        /// <summary>
        /// Evento disparado quando houver uma desconexão inesperada a conexão do cliente
        /// </summary>
        public static event DesconexaoInesperadaEventHandler AoOcorrerDesconexaoInesperada;

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
        /// Evento disparado quando uma mensagem estiver prestes a ser enviada para o usuário errado
        /// obs: deveria ser enviada para o usuário A, mas está sendo enviada para o usuário B.
        /// </summary>
        public static event AoReceberMensagemErradaEventHandler AoReceberMensagemErrada;

        #region Regras de Negócio

        /// <summary>
        /// Responsável por processar a chegada e envio de mensagens 
        /// (enfileirar novas mensagens e processar mensagens de escrita).
        /// </summary>
        protected virtual void RnProcessarLeitura()
        {
            LogDebug( string.Format( "\nManager iniciou comunicação com {0} - Cronograma:{1} | {2} iniciou... ", LoginCliente, OidCronograma, Thread.CurrentThread.Name ) );
            String mensagemJson = "";
            MensagemDto m;
            List<string> mensagens;
            string msgAtual = "";

            while(statusLeitura)
            {
                try
                {
                    //validar caso de perda de conexão com o WexMultiAccessClient
                    if(!TcpUtil.ConexaoTcpValida( TcpCliente ))
                    {
                        if(AoOcorrerDesconexaoInesperada != null)
                            AoOcorrerDesconexaoInesperada( this, new DesconexaoInesperadaEventArgs( LoginCliente, OidCronograma ) );
                        statusLeitura = false;
                        continue;
                    }
                    if(!statusLeitura)
                        continue;
                    if(!TcpCliente.GetStream().DataAvailable)
                    {
                        Thread.Sleep( 15 );
                        continue;
                    }
                    mensagemJson = TcpUtil.ReceberMensagemTcp( TcpCliente );
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
                            LogDebug( String.Format( "\nManager recebeu a mensagem {0} | Autor: {1} | Cronograma: {2}\nMensagem Json:{3}", m.Tipo, LoginCliente, OidCronograma, msgAtual ) );
                            FilaLeitura.Enqueue( m );
                            msgAtual = "";
                        }
                    }
                    else
                        Buffer += mensagemJson;
                }
                catch(JsonException ex)
                {
                    LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                    LogDebug( string.Format( "\nConexaoCliente do {3} (Leitura) - Mensagem Json:{2}Excessão:{0}\nCallStack{1} Categoria: JsonConvert", ex.Message, ex.StackTrace, msgAtual, LoginCliente ) );
                    continue;
                }
                catch(InvalidOperationException ex)
                {
                    LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                    LogDebug( string.Format( "\nOcorreu a excessão:{0}\nLocal:{1} Thread: {2}", ex.Message, ex.StackTrace, Thread.CurrentThread.Name ), ex );
                    Desconectar();
                }
                catch(Exception ex)
                {
                    LogInfo( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
                    LogDebug( string.Format( "\nOcorreu a excessão:{0}\nLocal:{1} Thread: {2}", ex.Message, ex.StackTrace, Thread.CurrentThread.Name ) );
                    Desconectar();
                }

                Thread.Sleep( 10 );
            }
            LogDebug( string.Format( "\nManager finalizou comunicação com {0} - Cronograma:{1} | {2} finalizou... ", LoginCliente, OidCronograma, Thread.CurrentThread.Name ) );
        }

        /// <summary>
        /// Método Responsável por remover o usuário que receberá a mensagem do vetor de usuários
        /// </summary>
        /// <param name="mensagem">mensagem que possui um vetor de usuários</param>
        /// <returns>mensagem dto tratada removendo a o colaborador da conexão cliente atual do vetor de usuário</returns>
        public MensagemDto RemoverProprioUsuarioDoVetorUsuariosDaMensagemDto( MensagemDto mensagem )
        {
            if(mensagem == null)
                return null;

            if(!mensagem.Propriedades.ContainsKey( Constantes.USUARIOS ))
                return mensagem;

            string[] usuarios = (string[])mensagem.Propriedades[Constantes.USUARIOS];
            string[] usuariosRetornados = usuarios.Where( o => o != LoginCliente ).ToArray();
            mensagem.Propriedades[Constantes.USUARIOS] = usuariosRetornados;
            return mensagem;
        }

        /// <summary>
        /// Metodo responsável por remover o proprio usuário do dicionário resumido de autores e ação sobre uma determinada tarefa
        /// </summary>
        /// <param name="mensagem">mensagemDto resumida com um dicionário autoresAcao</param>
        /// <returns></returns>
        public MensagemDto RemocaoProprioUsuarioDoDicionarioAutoresAcao( MensagemDto mensagem )
        {
            if(mensagem == null)
                return null;

            if(mensagem.Propriedades.ContainsKey( Constantes.AUTORES_ACAO ) && mensagem.Propriedades[Constantes.AUTORES_ACAO] != null)
            {
                Dictionary<string, string> autoresAcao = new Dictionary<string, string>( (Dictionary<string, string>)mensagem.Propriedades[Constantes.AUTORES_ACAO] );
                if(autoresAcao.ContainsValue( LoginCliente ))
                {
                    var tarefas = autoresAcao.Where( o => o.Value.Equals( LoginCliente ) ).Select( o => o.Key ).ToList();
                    foreach(var item in tarefas)
                    {
                        autoresAcao.Remove( item );
                    }
                }
                mensagem.Propriedades[Constantes.AUTORES_ACAO] = autoresAcao;
            }
            return mensagem;
        }

        /// <summary>
        /// Método responsável por verificar se a mensagem corresponde a uma ação ocasionada pelo próprio colaborador
        /// </summary>
        /// <returns>
        /// True - Caso seja uma auto notificação
        /// False - Caso não seja uma auto notificação
        /// </returns>
        private bool SeMensagemForUmaAutoNotificacao( MensagemDto mensagem )
        {
            return (string)mensagem.Propriedades[Constantes.AUTOR_ACAO] == LoginCliente;
        }

        /// <summary>
        /// Método responsável por validar se ainda há autoresAcao nas mensagens de ações resumidas
        /// </summary>
        /// <param name="mensagem"></param>
        /// <returns></returns>
        public static bool AindaContemAutoresAcao( MensagemDto mensagem )
        {
            if(mensagem.Propriedades.ContainsKey( Constantes.AUTORES_ACAO ) && mensagem.Propriedades[Constantes.AUTORES_ACAO] != null)
            {
                Dictionary<string, string> autoresAcao = (Dictionary<string, string>)mensagem.Propriedades[Constantes.AUTORES_ACAO];
                return autoresAcao.Count > 0;
            }
            return false;
        }

        /// <summary>
        /// Método responsável por tratar MensagensDto que possuem um vetor de usuários
        /// (Mensagem Sintetizada)
        /// </summary>
        /// <param name="mensagemTemporaria">MensagemDto</param>
        private void RnEnviarMensagensQuePossuemUsuarios( MensagemDto mensagemTemporaria )
        {
            //Caso seja nula não fazer nada
            if(mensagemTemporaria == null)
                return;

            mensagemTemporaria = RemoverProprioUsuarioDoVetorUsuariosDaMensagemDto( mensagemTemporaria );

            //Caso não contenha o indice usuários não fazer nada
            if(!mensagemTemporaria.Propriedades.ContainsKey( Constantes.USUARIOS ))
                return;

            string[] usuarios = (string[])mensagemTemporaria.Propriedades[Constantes.USUARIOS];
            if(usuarios.Length > 0)
                RnEnviarMensagemAoAccessClient( mensagemTemporaria );
            else
                LogDebug( string.Format( "\nManager não enviou {2} - Client#{0} - Cronograma:{1}\nObs:não foi enviada por não conter usuários!", LoginCliente, OidCronograma, mensagemTemporaria.Tipo ) );
        }

        /// <summary>
        /// Método responsável pelo envio da mensagem ao WexMultiAccessClient
        /// </summary>
        /// <param name="mensagemTemporaria">Mensagem a ser comunicada ao AccessClient</param>
        private void RnEnviarMensagemAoAccessClient( MensagemDto mensagemTemporaria )
        {
            string autorAcao;
            if(mensagemTemporaria.Propriedades.ContainsKey( Constantes.AUTOR_ACAO ))
            {
                autorAcao = mensagemTemporaria.Propriedades[Constantes.AUTOR_ACAO] as string;
            }
            else
                autorAcao = "-";

            if(mensagemTemporaria == null || !TcpUtil.ConexaoTcpValida( TcpCliente ))
            {
                if(mensagemTemporaria != null)
                    LogDebug( string.Format( "\n{4} não enviou {0} -  Client#{1} - Cronograma {2} - Autor Mensagem:{3} \nObs.: conexão tcp inválida ou encerrada!", mensagemTemporaria.Tipo, LoginCliente, OidCronograma, autorAcao, Thread.CurrentThread.Name ) );
                else
                    LogDebug( string.Format( "\n{4} não comunicou mensagem - Client#{0} Cronograma:{1} - Autor:{2},\nObs.: mensagem nula!", LoginCliente, OidCronograma, autorAcao, Thread.CurrentThread.Name ) );

                return;
            }

            string mensagemJson = JsonConvert.SerializeObject( mensagemTemporaria );

            LogDebug( string.Format( "\n{4} enviou {0}(Enum - {1}) - Client#{2} - Autor - {3}\nMensagem Json:{5}", mensagemTemporaria.Tipo, (int)mensagemTemporaria.Tipo, LoginCliente, autorAcao, Thread.CurrentThread.Name, mensagemJson ) );
            mensagemJson = TcpUtil.AdicionarStringProtecaoDeIntegridade( mensagemJson );
            TcpUtil.EnviarMensagemTcp( mensagemJson, TcpCliente );
            LogarEnvio( mensagemTemporaria );
        }

        /// <summary>
        /// Encerra a execucao dos processos de leitura e escrita da Conexao do cliente.
        /// </summary>
        public void Desconectar()
        {
            statusEscrita = false;
            statusLeitura = false;

            if(TcpUtil.ConexaoTcpValida( TcpCliente ))
            {
                lock(TcpCliente)
                {
                    TcpCliente.Client.Shutdown( SocketShutdown.Both );
                    TcpCliente.Client.Dispose();
                    TcpCliente.Close();
                }
            }
        }

        /// <summary>
        /// Método para destruir o objeto atual
        /// </summary>
        public void Dispose()
        {
            statusEscrita = false;
            statusLeitura = false;
            if(TcpCliente != null)
                Desconectar();
            GC.SuppressFinalize( this );
        }
        #endregion

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
        /// <param name="excessao">Excessão ocorrida</param>
        public static void LogDebug( string mensagem, Exception excessao )
        {
            if(AoLogarDebug != null)
                AoLogarDebug( mensagem, excessao );
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
        /// Efetuar o envio das mensagens para o respectivo cliente
        /// </summary>
        public void RnProcessarFilaEscrita()
        {
            lock(lockerProcessarEscrita)
            {
                if(executandoEnvioMensagens)
                    return;
                executandoEnvioMensagens = true;
                MensagemDto mensagemTemporaria;
                while(TcpUtil.ConexaoTcpValida( TcpCliente ) && FilaEscrita.Count > 0)
                {
                    //Desenfileirar uma mensagemDto efetuando um clone da mensagem
                    mensagemTemporaria = FilaEscrita.Dequeue();
                    if(mensagemTemporaria == null)
                        break;

                    mensagemTemporaria = mensagemTemporaria.Clone();
                    mensagemTemporaria.Dump( string.Format( "Clone para processamento(ConexaoCliente-{0}|Mensagem-{1}):", LoginCliente, ConverterTipoMensagemParaString( mensagemTemporaria ) ) );
                    switch(mensagemTemporaria.Tipo)
                    {
                        case CsTipoMensagem.NovosUsuariosConectados:
                        case CsTipoMensagem.UsuarioDesconectado:
                            RnEnviarMensagensQuePossuemUsuarios( mensagemTemporaria );
                            break;
                        case CsTipoMensagem.EdicaoTarefaAutorizada:
                            if(SeMensagemForUmaAutoNotificacao( mensagemTemporaria ))
                                RnEnviarMensagemAoAccessClient( mensagemTemporaria );
                            else
                                if(AoReceberMensagemErrada != null)
                                    AoReceberMensagemErrada( mensagemTemporaria );
                            break;
                        case CsTipoMensagem.EdicaoTarefaRecusada:
                        case CsTipoMensagem.ConexaoRecusadaServidor:
                        case CsTipoMensagem.ServidorDesconectando:
                        case CsTipoMensagem.ExclusaoTarefaPermitida:
                        case CsTipoMensagem.EdicaoNomeCronogramaPermitida:
                        case CsTipoMensagem.EdicaoNomeCronogramaRecusada:
                            RnEnviarMensagemAoAccessClient( mensagemTemporaria );
                            break;
                        case CsTipoMensagem.NovaTarefaCriada:
                        case CsTipoMensagem.ExclusaoTarefaFinalizada:
                        case CsTipoMensagem.MovimentacaoPosicaoTarefa:
                        case CsTipoMensagem.DadosCronogramaAlterados:
                        case CsTipoMensagem.InicioEdicaoNomeCronograma:
                        case CsTipoMensagem.InicioEdicaoTarefa:
                        case CsTipoMensagem.EdicaoTarefaFinalizada:
                            if(!SeMensagemForUmaAutoNotificacao( mensagemTemporaria ))
                                RnEnviarMensagemAoAccessClient( mensagemTemporaria );
                            break;
                    }
                }
                executandoEnvioMensagens = false; 
            }
        }


        /// <summary>
        /// Método para adicionar uma nova mensagem na lista de envio para o cliente
        /// </summary>
        /// <param name="mensagem">Mensagem que será enviada para o Cliente</param>
        public void EnfileirarMensagem( MensagemDto mensagem )
        {
            if(FilaEscrita != null)
                FilaEscrita.Enqueue( mensagem );
            if(processarMensagensAsync != null)
                processarMensagensAsync.BeginInvoke( null, null );
        }

        /// <summary>
        /// Efetuar o Log da mensagem antes do envio
        /// </summary>
        /// <param name="mensagem">Mensagem que será logada antes do envio</param>
        private void LogarEnvio( MensagemDto mensagem )
        {
            if(mensagem.Propriedades.ContainsKey( Constantes.ID_REQUISICAO ))
            {
                string idRequisicao = mensagem.Propriedades[Constantes.ID_REQUISICAO] as string;
                LogInfo( string.Format( "Mensagem:{1} - Requisição:{2})  WexClient:{0}", LoginCliente, ConverterTipoMensagemParaString( mensagem ), idRequisicao ) );
            }
            else
            {
                LogInfo( string.Format( "Mensagem:{1} WexClient:{0}", LoginCliente, ConverterTipoMensagemParaString( mensagem ) ) );
            }

        }

        /// <summary>
        /// Converte o tipo da mensagem para String no momento do Log
        /// </summary>
        /// <param name="mensagem">Mensagem a ser convertida</param>
        /// <returns>String do tipo da mensagem</returns>
        private string ConverterTipoMensagemParaString( MensagemDto mensagem )
        {
            return mensagem.Tipo.ToString();
        }
    }
}
