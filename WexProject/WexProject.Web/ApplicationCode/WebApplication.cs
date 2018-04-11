using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp;
using System.Configuration;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using RestSharp;

namespace WexProject.Web
{
    /// <summary>
    /// Class de Web Application
    /// </summary>
    public partial class WexProjectAspNetApplication : WebApplication
    {
        /// <summary>
        /// module1
        /// </summary>
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        /// <summary>
        /// module2
        /// </summary>
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
        /// <summary>
        /// module3
        /// </summary>
        private WexProject.Module.WexProjectModule module3;
        /*/// <summary>
                                        /// module4
                                        /// </summary>
                                        private WexProject.Module.Web.WexProjectAspNetModule module4;*/
        /// <summary>
        /// securityModule1
        /// </summary>
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        /// <summary>
        /// module6
        /// </summary>
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule module6;
        /// <summary>
        /// sqlConnection1
        /// </summary>
        private System.Data.SqlClient.SqlConnection sqlConnection1;
        /// <summary>
        /// dados referentes oa banco de dados
        /// </summary>
        private DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule conditionalAppearanceModule1;

        /// <summary>
        /// dados referentes oa banco de dados
        /// </summary>
        private DevExpress.ExpressApp.Security.SecurityComplex securityComplex1;

        /// <summary>
        /// dados referentes oa banco de dados
        /// </summary>
        private DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule htmlPropertyEditorAspNetModule1;

        /// <summary>
        /// Dados referentes ao gráfico da versão web
        /// </summary>
        private DevExpress.ExpressApp.Chart.Web.ChartAspNetModule chartAspNetModule1;
        /// <summary>
        /// modulo1
        /// </summary>
        private DevExpress.ExpressApp.Chart.ChartModule chartModule1;
        /// <summary>
        /// Modulo asp new
        /// </summary>
        private DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule htmlPropertyEditorAspNetModule2;
        private DevExpress.ExpressApp.Scheduler.SchedulerModuleBase schedulerModuleBase1;
        private DevExpress.ExpressApp.Scheduler.Win.SchedulerWindowsFormsModule schedulerWindowsFormsModule1;
        /// <summary>
        /// module5
        /// </summary>
        private DevExpress.ExpressApp.Validation.ValidationModule module5;

        /// <summary>
        /// dados referentes oa banco de dados
        /// </summary>
        private WexProject.Web.Libs.ActiveDirectory.WexStandardAuthentication wexStandardAuthentication1;

        /// <summary>
        /// WexProjectAspNetApplication
        /// </summary>
        public WexProjectAspNetApplication()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Método responsável por Autenticar o usuário, aciona o serviço para criar o colaborador caso não exista.
        /// É acionado no momento em que o AD é executado.
        /// </summary>
        /// <param name="session">Sessão corrente</param>
        /// <param name="login">Login (UserName) do usuário logado</param>
        /// <param name="extensaoEmail">Extensão de Email da empresa</param>
        /// <returns>Objeto Usuário Logado</returns>
        public static Object Autenticar( Session session, string login, string extensaoEmail )
        {
            RestClient cliente = new RestClient( ConfigurationManager.AppSettings.Get( "RestWebServicePath" ) );
            RestRequest requisicao = new RestRequest( "Colaboradores/" );

            requisicao.AddParameter( "login", login );
            requisicao.AddParameter( "extensaoEmail", extensaoEmail );
            cliente.Put( requisicao );

            return session.FindObject<User>( CriteriaOperator.Parse( "UserName = ?", login ) );
        }


        /// <summary>
        /// WexProjectAspNetApplication_DatabaseVersionMismatch
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void WexProjectAspNetApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e)
        {
#if EASYTEST
			e.Updater.Update();
			e.Handled = true;
#else
            if (System.Diagnostics.Debugger.IsAttached)
            {
                e.Updater.Update();
                e.Handled = true;
            }
            else
            {
                throw new InvalidOperationException(
                "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
                "This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
                "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
                "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
                "or manually create a database using the 'DBUpdater' tool.\r\n" +
                "Anyway, refer to the 'Update Application and Database Versions' help topic at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm " +
                "for more detailed information. If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/");
            }
#endif
        }

        /// <summary>
        /// InitializeComponent
        /// </summary>
        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.module3 = new WexProject.Module.WexProjectModule();
            this.module5 = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.module6 = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
            this.conditionalAppearanceModule1 = new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule();
            this.securityComplex1 = new DevExpress.ExpressApp.Security.SecurityComplex();
            this.wexStandardAuthentication1 = new WexProject.Web.Libs.ActiveDirectory.WexStandardAuthentication();
            this.htmlPropertyEditorAspNetModule1 = new DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule();
            this.chartAspNetModule1 = new DevExpress.ExpressApp.Chart.Web.ChartAspNetModule();
            this.chartModule1 = new DevExpress.ExpressApp.Chart.ChartModule();
            this.htmlPropertyEditorAspNetModule2 = new DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule();
            this.schedulerModuleBase1 = new DevExpress.ExpressApp.Scheduler.SchedulerModuleBase();
            this.schedulerWindowsFormsModule1 = new DevExpress.ExpressApp.Scheduler.Win.SchedulerWindowsFormsModule();
            ( (System.ComponentModel.ISupportInitialize)( this ) ).BeginInit();

            wexStandardAuthentication1.AoAutenticarUsuarioComSessao += Autenticar;

            // 
            // module5
            // 
            this.module5.AllowValidationDetailsAccess = true;
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            // 
            // securityComplex1
            // 
            this.securityComplex1.Authentication = this.wexStandardAuthentication1;
            this.securityComplex1.RoleType = typeof( DevExpress.Persistent.BaseImpl.Role );
            this.securityComplex1.UserType = typeof( DevExpress.Persistent.BaseImpl.User );
            // 
            // wexStandardAuthentication1
            // 
            this.wexStandardAuthentication1.LogonParametersType = typeof( DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters );
            // 
            // WexProjectAspNetApplication
            // 
            this.ApplicationName = "WexProject";
            this.Connection = this.sqlConnection1;
            this.Modules.Add( this.module1 );
            this.Modules.Add( this.module2 );
            this.Modules.Add( this.module6 );
            this.Modules.Add( this.conditionalAppearanceModule1 );
            this.Modules.Add( this.htmlPropertyEditorAspNetModule1 );
            this.Modules.Add( this.schedulerModuleBase1 );
            this.Modules.Add( this.schedulerWindowsFormsModule1 );
            this.Modules.Add( this.module3 );
            this.Modules.Add( this.module5 );
            this.Modules.Add( this.securityModule1 );
            this.Modules.Add( this.chartModule1 );
            this.Modules.Add( this.chartAspNetModule1 );
            this.Modules.Add( this.htmlPropertyEditorAspNetModule2 );
            this.Security = this.securityComplex1;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>( this.WexProjectAspNetApplication_DatabaseVersionMismatch );
            ( (System.ComponentModel.ISupportInitialize)( this ) ).EndInit();

        }

        /// <summary>
        /// Método chamado quando o usuário estiver logado
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnLoggedOn(LogonEventArgs e)
        {
            base.OnLoggedOn(e);

            // Código para exibição das propriedades de coleção no detail
            ((ShowViewStrategy)base.ShowViewStrategy).CollectionsEditMode =
            DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
        }
    }
}
