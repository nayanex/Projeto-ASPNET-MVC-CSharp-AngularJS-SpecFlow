using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using WexProject.MultiAccess.Library;
using WexProject.Library.Libs.Rede;
using System.Configuration;
using log4net;
using System.Reflection;
using log4net.Config;

namespace WexProject.MultiAccess.Service
{
    public partial class MultiAccessService : ServiceBase
    {
        #region Atributos


        /// <summary>
        /// Atributo que receberá a instância do server.
        /// </summary>
        WexMultiAccessManager wexServer;

        /// <summary>
        /// Atributo que instancia o log (log4net)
        /// </summary>
        private readonly ILog log = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );


        #endregion

        #region Construtor


        /// <summary>
        /// Método que inicializa a instancia do serviço e inicia configurações do servidor.
        /// </summary>
        public MultiAccessService()
        {
            InitializeComponent();
            IniciarServidor();
        }


        #endregion

        #region Métodos

        /// <summary>
        /// Ocorre ao iniciar o serviço
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart( string[] args )
        {
            log.Info( String.Format("Mensagem: {0}", "Serviço iniciando...") );
            wexServer.Conectar();
            log.Info( String.Format( "Mensagem: {0}", "Serviço iniciado, servidor ligado." ) );
        }

        /// <summary>
        /// Ocorre ao parar o serviço.
        /// </summary>
        protected override void OnStop()
        {
            log.Info( String.Format( "Mensagem: {0}", "Serviço parado, servidor desligado." ) );
            wexServer.Desconectar();
        }

        /// <summary>
        /// Ocorre ao pausar o serviço.
        /// </summary>
        protected override void OnPause()
        {
            log.Info( String.Format( "Mensagem: {0}", "Serviço pausado, servidor desligado." ) );
            wexServer.Desconectar();
        }

        /// <summary>
        /// Ocorre enquanto serviço está em execução
        /// </summary>
        protected override void OnContinue()
        {
            log.Info( String.Format( "Mensagem: {0}", "Serviço reestabelecido, servidor iniciando..." ) );
            wexServer.Conectar();
            log.Info( String.Format( "Mensagem: {0}", "Serviço reestabelecido, servidor ligado." ) );
        }

        /// <summary>
        /// Ocorre quando serviço for desligado
        /// </summary>
        protected override void OnShutdown()
        {
            log.Info( String.Format( "Mensagem: {0}", "Serviço desligado." ) );
            wexServer.Desconectar();
        }

        /// <summary>
        /// Método responsável por iniciar o servidor.
        /// </summary>
        protected void IniciarServidor()
        {
            wexServer = new WexMultiAccessManager();
            try
            {
                wexServer.EnderecoIp = RedeUtil.GetEnderecoIpComputadorAtual().ToString();
                wexServer.Porta = Convert.ToInt16( ConfigurationManager.AppSettings.Get( "PortaManager" ) );
            }
            catch(Exception ex)
            {
                log.Info( String.Format( "Exception: {0}" + Environment.NewLine + " StackTrace: {1}", ex.Message, ex.StackTrace ) );
            }
        }


        #endregion
    }
}
