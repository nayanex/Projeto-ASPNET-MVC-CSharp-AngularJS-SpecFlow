using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using log4net.Config;
using System.Configuration;

namespace WexProject.Library.Libs.Logger
{
    public class WexLogger
    {
        /// <summary>
        /// Instancia singleton WexLogger
        /// </summary>
        private static WexLogger instanciaSingleton;

        private const string EXTENSAO = ".config";

        /// <summary>
        /// Objeto para efetuar o tratamento da concorrencia para seleção da instancia do objeto
        /// </summary>
        private readonly static object objectLocker = new object();

        /// <summary>
        /// Objeto que armanezar o logger configurador
        /// </summary>
        private  ILog log;

        /// <summary>
        /// Informa se o singleton foi instanciado
        /// </summary>
		public static bool Instanciado
		{
			get
			{
				return instanciaSingleton != null;
			}
		}

		/// <summary>
		/// Retorna uma instância ou lança uma exceção
		/// </summary>
		public static WexLogger Instancia
		{
			get
			{
				if (!Instanciado)
				{
					// Tenta criar instância padrão.
					var loggerName = ConfigurationManager.AppSettings["NomeLogger"];
					var loggerFile = ConfigurationManager.AppSettings["ConfigLogger"];

					if (loggerName == null || loggerFile == null)
					{
						throw new Exception("Não foi possível pegar instância");
					}

					CreateSingletonInstance(loggerName, loggerFile);
				}

				return instanciaSingleton;
			}
		}

        /// <summary>
        /// Logger Log4Net
        /// </summary>
        public  ILog Log
        {
            get { return log; }
        }

        /// <summary>
        /// Construtor privado para implementação do singleton
        /// </summary>
        /// <param name="logger">logger Log4net</param>
        /// <param name="fileConfigInfo"></param>
        private WexLogger( ILog logger, FileInfo fileConfigInfo )
        {
            log = logger;
            Configurar( fileConfigInfo );
        }

        /// <summary>
        /// Método para configurar o xml para escrever o log
        /// </summary>
        /// <param name="fileConfigInfo"></param>
        private static void Configurar( FileInfo fileConfigInfo )
        {
            if(fileConfigInfo != null)
                XmlConfigurator.ConfigureAndWatch( fileConfigInfo );
            else
                XmlConfigurator.Configure();
        }

		/// <summary>
		/// Método inicializador para criar uma instancia singleton
		/// </summary>
		/// <param name="loggerName">Nome do logger.</param>
		/// <param name="loggerFile">Nome do arquivo de configuração.</param>
		public static void CreateSingletonInstance(string loggerName, string loggerFile)
		{
			FileInfo fileConfigInfo;

			if (!loggerFile.EndsWith(EXTENSAO))
			{
				loggerFile += EXTENSAO;
			}

			fileConfigInfo = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, loggerFile));

			CreateSingletonInstance(loggerName, fileConfigInfo);
		}

        /// <summary>
		/// Método inicializador para criar uma instancia singleton
        /// </summary>
        /// <param name="loggerName">Nome do logger.</param>
        /// <param name="fileConfigInfo">Arquivo de configuração.</param>
		public static void CreateSingletonInstance(string loggerName, FileInfo fileConfigInfo)
        {
			if(Instanciado)
                return;

            if(string.IsNullOrEmpty(loggerName) || fileConfigInfo == null )
                throw new ArgumentNullException( "Os parametros para criar a instância singleton não devem ser nulos" );

            if(!fileConfigInfo.Exists)
                throw new FileNotFoundException( string.Format( "O arquivo de configuração {0} não existe",fileConfigInfo.FullName ));

            if(!fileConfigInfo.Extension.ToLower().Equals( EXTENSAO ))
                throw new FileLoadException( string.Format( "A extensão do arquivo '{0}' não corresponde a extensão esperada '{1}'", fileConfigInfo.Extension, EXTENSAO ) );

            lock(objectLocker)
            {
                if(instanciaSingleton == null)
                {
                    instanciaSingleton = new WexLogger( LogManager.GetLogger( loggerName ), fileConfigInfo );
                }
            }
        }

        /// <summary>
        /// Método inicializador para criar uma instancia singleton
        /// </summary>
        /// <param name="inicializador">inicializador do logger</param>
        public static void CreateSingletonInstance( string loggerName )
        {
            if(Instanciado)
                return;

            if(string.IsNullOrEmpty( loggerName ))
                throw new ArgumentNullException( "Os parametros para criar a instância singleton não devem ser nulos" );

            lock(objectLocker)
            {
                if(instanciaSingleton == null)
                    instanciaSingleton = new WexLogger( LogManager.GetLogger( loggerName ), null );
            }
        }

        /// <summary>
        /// Método para criar mensagens do level debug
        /// </summary>
        /// <param name="mensagem">mensagem de debug</param>
        public static void Debug( object mensagem )
        {
			try
			{
				Instancia.Log.Debug(mensagem);
			}
			catch
			{
				// Não foi possível pegar instância.
			}
        }

        /// <summary>
        /// Método para criar mensagens do level debug
        /// </summary>
        /// <param name="mensagem">mensagem de debug</param>
        /// <param name="exception">excessão gerada</param>
        public static void Debug( object mensagem, Exception exception )
        {
			try
			{
				Instancia.Log.Debug(mensagem, exception);
			}
			catch (Exception)
			{
				// Não foi possível pegar instância.
			}
        }

        /// <summary>
        /// Método para criar mensagens do level info
        /// </summary>
        /// <param name="mensagem">mensagem de info</param>
        public static void Info( object mensagem )
        {
			try
			{
				Instancia.Log.Info(mensagem);
			}
			catch (Exception)
			{
				// Não foi possível pegar instância.
			}
        }

        /// <summary>
        /// Método para criar mensagens do level info
        /// </summary>
        /// <param name="mensagem">mensagem de info</param>
        /// <param name="exception">excessão gerada</param>
        public static void Info( object mensagem, Exception exception )
        {
			try
			{
				Instancia.Log.Info(mensagem, exception);
			}
			catch (Exception)
			{
				// Não foi possível pegar instância.
			}
        }

        /// <summary>
        /// Método para criar mensagens do level error
        /// </summary>
        /// <param name="mensagem">mensagem de error</param>
        public static void Error( object mensagem )
        {
			try
			{
				Instancia.Log.Error(mensagem);
			}
			catch (Exception)
			{
				// Não foi possível pegar instância.
			}
        }

        /// <summary>
        /// Método para criar mensagens do level error
        /// </summary>
        /// <param name="mensagem">mensagem de error</param>
        /// <param name="exception">excessão gerada</param>
        public static void Error( object mensagem, Exception exception )
        {
			try
			{
				Instancia.Log.Error(mensagem, exception);
			}
			catch (Exception)
			{
				// Não foi possível pegar instância.
			}
		}
    }
}
