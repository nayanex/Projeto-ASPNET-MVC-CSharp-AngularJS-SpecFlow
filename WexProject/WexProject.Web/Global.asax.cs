using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;

namespace WexProject.Web
{
    /// <summary>
    /// classe global que inicializa os componentes
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Construtor da classe global
        /// </summary>
        public Global()
        {
            InitializeComponent();
        }
        /// <summary>
        /// iniciando a aplicação
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">EventArgs</param>
        protected void Application_Start(Object sender, EventArgs e)
        {
            WebApplication.OldStyleLayout = false;

#if EASYTEST
			DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
			ConfirmationsHelper.IsConfirmationsEnabled = false;
#endif
        }
        /// <summary>
        /// iniciando a seção
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Session_Start(Object sender, EventArgs e)
        {
            WebApplication.SetInstance(Session, new WexProjectAspNetApplication());
#if EASYTEST
			if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
				WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
			}
#endif
            //testanda a conecção
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();

            WebApplication.DefaultPage = "DefaultVertical.aspx";
        }
        /// <summary>
        /// iniciando a resposta
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">argumento de evento</param>
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            string filePath = HttpContext.Current.Request.PhysicalPath;
            //testendo se é nulo arquivo do path
            if (!string.IsNullOrEmpty(filePath)
            && (filePath.IndexOf("Images") >= 0) && !System.IO.File.Exists(filePath))
            {
                //resposta
                HttpContext.Current.Response.End();
            }
        }
        /// <summary>
        /// metodo fim da resposta
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">argumento de evento</param>
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Aplicação de resposta
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">Argumento de evento</param>
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Aplicação de erro
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">Argumento de evento</param>
        protected void Application_Error(Object sender, EventArgs e)
        {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        /// <summary>
        /// fim da seção
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">Argumento de evento</param>
        protected void Session_End(Object sender, EventArgs e)
        {
            WebApplication.DisposeInstance(Session);
        }
        /// <summary>
        /// fim da aplicação
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">Argumento de evento</param>
        protected void Application_End(Object sender, EventArgs e)
        {
        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }
}
