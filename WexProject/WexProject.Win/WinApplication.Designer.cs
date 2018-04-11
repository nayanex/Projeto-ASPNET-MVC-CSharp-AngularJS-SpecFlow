using System.Configuration;
using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using WexProject.Module.Win.Controllers.Planejamento;
using RestSharp;

namespace WexProject.Win
{
    partial class WexProjectWindowsFormsApplication
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            CronogramaController.FecharFormAplicacaoCronograma();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
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
            RestClient cliente = new RestClient( ConfigurationManager.AppSettings.Get( "RestWebServicePath" )  );
            RestRequest requisicao = new RestRequest(  "Colaboradores/" );
            
            requisicao.AddParameter( "login", login );
            requisicao.AddParameter( "extensaoEmail", extensaoEmail );
            cliente.Put( requisicao );

            return session.FindObject<User>( CriteriaOperator.Parse( "UserName = ?", login ) );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule();
            this.module3 = new WexProject.Module.WexProjectModule();
            this.module4 = new WexProject.Module.Win.WexProjectWindowsFormsModule();
            this.module5 = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.module6 = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.module7 = new DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
            this.htmlPropertyEditorWindowsFormsModule1 = new DevExpress.ExpressApp.HtmlPropertyEditor.Win.HtmlPropertyEditorWindowsFormsModule();
            this.securityComplex1 = new DevExpress.ExpressApp.Security.SecurityComplex();
            this.wexAuthenticationActiveDirectory1 = new WexProject.Library.Libs.ActiveDirectory.WexAuthenticationActiveDirectory();
            this.chartWindowsFormsModule1 = new DevExpress.ExpressApp.Chart.Win.ChartWindowsFormsModule();
            this.chartModule1 = new DevExpress.ExpressApp.Chart.ChartModule();
            this.conditionalEditorStateWindowsFormsModule1 = new DevExpress.ExpressApp.ConditionalEditorState.Win.ConditionalEditorStateWindowsFormsModule();
            this.conditionalEditorStateModuleBase1 = new DevExpress.ExpressApp.ConditionalEditorState.ConditionalEditorStateModuleBase();
            this.conditionalFormattingModule1 = new DevExpress.ExpressApp.ConditionalFormatting.ConditionalFormattingModule();
            this.fileAttachmentsWindowsFormsModule1 = new DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule();
            this.conditionalAppearanceModule1 = new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule();
            this.htmlPropertyEditorAspNetModule1 = new DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule();
            this.schedulerWindowsFormsModule1 = new DevExpress.ExpressApp.Scheduler.Win.SchedulerWindowsFormsModule();
            this.schedulerModuleBase1 = new DevExpress.ExpressApp.Scheduler.SchedulerModuleBase();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();

            this.wexAuthenticationActiveDirectory1.AoAutenticarUsuarioComSessao += Autenticar;
            
            // 
            // module5
            // 
            this.module5.AllowValidationDetailsAccess = true;
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.ConnectionString = null;
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            // 
            // securityComplex1
            // 
            this.securityComplex1.Authentication = this.wexAuthenticationActiveDirectory1;
            this.securityComplex1.RoleType = typeof(DevExpress.Persistent.BaseImpl.Role);
            this.securityComplex1.UserType = typeof(DevExpress.Persistent.BaseImpl.User);
            // 
            // wexAuthenticationActiveDirectory1
            // 
            this.wexAuthenticationActiveDirectory1.CreateUserAutomatically = true;
            this.wexAuthenticationActiveDirectory1.LogonParametersType = null;
            // 
            // WexProjectWindowsFormsApplication
            // 
            this.ApplicationName = "WexProject";
            this.Connection = this.sqlConnection1;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.module6);
            this.Modules.Add(this.conditionalEditorStateModuleBase1);
            this.Modules.Add(this.conditionalEditorStateWindowsFormsModule1);
            this.Modules.Add(this.conditionalAppearanceModule1);
            this.Modules.Add(this.htmlPropertyEditorAspNetModule1);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.module5);
            this.Modules.Add(this.module7);
            this.Modules.Add(this.securityModule1);
            this.Modules.Add(this.htmlPropertyEditorWindowsFormsModule1);
            this.Modules.Add(this.chartModule1);
            this.Modules.Add(this.chartWindowsFormsModule1);
            this.Modules.Add(this.conditionalFormattingModule1);
            this.Modules.Add(this.fileAttachmentsWindowsFormsModule1);
            this.Modules.Add(this.schedulerModuleBase1);
            this.Modules.Add(this.schedulerWindowsFormsModule1);
            this.Security = this.securityComplex1;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.WexProjectWindowsFormsApplication_DatabaseVersionMismatch);

            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule module2;
        private WexProject.Module.WexProjectModule module3;
        private WexProject.Module.Win.WexProjectWindowsFormsModule module4;
        private DevExpress.ExpressApp.Validation.ValidationModule module5;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule module6;
        private DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule module7;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private System.Data.SqlClient.SqlConnection sqlConnection1;
        private DevExpress.ExpressApp.HtmlPropertyEditor.Win.HtmlPropertyEditorWindowsFormsModule htmlPropertyEditorWindowsFormsModule1;
        private DevExpress.ExpressApp.Security.SecurityComplex securityComplex1;
        private DevExpress.ExpressApp.Chart.Win.ChartWindowsFormsModule chartWindowsFormsModule1;
        private DevExpress.ExpressApp.Chart.ChartModule chartModule1;
        private DevExpress.ExpressApp.ConditionalEditorState.Win.ConditionalEditorStateWindowsFormsModule conditionalEditorStateWindowsFormsModule1;
        private DevExpress.ExpressApp.ConditionalEditorState.ConditionalEditorStateModuleBase conditionalEditorStateModuleBase1;
        private DevExpress.ExpressApp.ConditionalFormatting.ConditionalFormattingModule conditionalFormattingModule1;
        private DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule fileAttachmentsWindowsFormsModule1;
        private DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule conditionalAppearanceModule1;
        private DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule htmlPropertyEditorAspNetModule1;
        private WexProject.Library.Libs.ActiveDirectory.WexAuthenticationActiveDirectory wexAuthenticationActiveDirectory1;
        private DevExpress.ExpressApp.Scheduler.Win.SchedulerWindowsFormsModule schedulerWindowsFormsModule1;
        private DevExpress.ExpressApp.Scheduler.SchedulerModuleBase schedulerModuleBase1;
    }
}
