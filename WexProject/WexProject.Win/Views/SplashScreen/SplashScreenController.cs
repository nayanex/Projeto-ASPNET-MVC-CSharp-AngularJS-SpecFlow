using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Win;

namespace WexProject.Win.SplashScreen
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
        private static SplashScreen splash;

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
            splash = new SplashScreen();
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
    }
}
