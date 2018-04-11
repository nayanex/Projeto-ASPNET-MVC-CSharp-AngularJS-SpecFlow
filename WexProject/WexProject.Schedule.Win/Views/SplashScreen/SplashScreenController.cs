using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Win;
using System.Windows.Forms;
using System.Configuration;

namespace WexProject.Schedule.Win.Views.SplashScreen
{
    /// <summary>
    /// Classe contrladora do splashscreen
    /// </summary>
    [DefaultClassOptions]
    public class SplashScreenController : ISplash
    {
        /// <summary>
        /// Splash da applicação
        /// </summary>
        private static Form splash;

        /// <summary>
        /// Controla a exibição do Splash
        /// </summary>
        private static bool isStarted = false;

        /// <summary>
        /// Controller da Interface de Splash
        /// Iniciar a Exibição do Splash
        /// </summary>
        public void Start()
        {
            isStarted = true;
           // splash = new CronogramaForm(createSession());
            splash.Show();
            System.Windows.Forms.Application.DoEvents();
        }

        /// <summary>
        /// Controller da Interface de Splash
        /// Parar a exibição do Splash
        /// </summary>
        public void Stop()
        {
            if (splash != null)
            {
                splash.Hide();
                splash.Close();
                splash = null;
            }
            isStarted = false;
        }

        /// <summary>
        /// Controller da Interface de Splash
        /// </summary>
        /// <param name="displayText">string</param>
        public void SetDisplayText(string displayText)
        {
        }

        /// <summary>
        /// retorna o status da exibição.
        /// </summary>
        public bool IsStarted
        {
            get
            {
                return isStarted;
            }
        }

        /// <summary>
        /// Cria uma sessão.
        /// </summary>
        public static Session createSession()
        {
            var session = new Session()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString
            };

            return session;
        }
    }
}
