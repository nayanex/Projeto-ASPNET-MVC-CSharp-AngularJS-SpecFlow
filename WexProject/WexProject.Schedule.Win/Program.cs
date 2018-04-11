using System;
using System.Windows.Forms;
using DevExpress.ExpressApp.Security;
using System.Threading;
using WexProject.Schedule.Library.Views.Forms;
using WexProject.Library.Libs.ActiveDirectory;
using DevExpress.Skins;
using WexProject.Schedule.Library.Presenters;
using WexProject.Schedule.Library.ServiceUtils;
using System.IO;
using WexProject.Library.Libs.Logger;
using RestSharp;
using System.Configuration;

namespace WexProject.Schedule.Win
{
    /// <summary>
    /// classe program
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
#if EASYTEST
			DevExpress.ExpressApp.EasyTest.WinAdapter.RemotingRegistration.Register(4100);
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            SkinManager.EnableFormSkins();

            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "Office 2010 Blue";
#if EASYTEST
			if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
				winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
			}
#endif
            FileInfo fileInfo = new FileInfo( Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Logger.config" ) );
            WexLogger.CreateSingletonInstance( "ScheduleWin", fileInfo );
            try
            {
                using(WexAuthenticationActiveDirectory AutenticadorWex = new WexAuthenticationActiveDirectory())
                {
                    AutenticadorWex.AoAutenticarUsuario += AutenticadorWex_AoAutenticarUsuario;
                    AutenticadorWex.Authenticate( null );
                }

                CronogramaPresenter.ServicoPlanejamento = new PlanejamentoServiceUtil();
                CronogramaPresenter.ServicoGeral = new GeralServiceUtil();
                Application.Run( new CronogramaView() );
            }
            catch(ObjectDisposedException e) 
            {
                WexLogger.Error( "Excessão ocorrida Em Program", e );
                //Excessão levantada pela execução do fechamento da aplicação pelo presenter
            }
            catch(Exception e)
            {
                WexLogger.Error( "Excessão ocorrida Em Program", e );
                throw e;
                //winApplication.HandleException( e );
            }
        }

        /// <summary>
        /// Método utilizado para verificar a existencia do login no extensaoEmail, verificar se já existe o colaborador na base do wex caso não exista irá criar o novo colaborador na base de dados
        /// </summary>
        /// <param name="login">login do colaborador</param>
        /// <param name="extensaoEmail">extensaoEmail padrão</param>
        /// <returns></returns>
        public static Object AutenticadorWex_AoAutenticarUsuario(string login,string extensaoEmail) 
        {
            RestClient cliente = new RestClient( ConfigurationManager.AppSettings.Get( "RestWebServicePath" ) );
            RestRequest requisicao = new RestRequest( "Colaboradores/" );

            requisicao.AddParameter( "login", login );
            requisicao.AddParameter( "extensaoEmail", extensaoEmail );
            cliente.Put( requisicao );

            return new Object();
        }
    }
}
