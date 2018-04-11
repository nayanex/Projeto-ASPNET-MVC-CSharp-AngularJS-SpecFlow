using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using WexProject.Library.Libs.Delegates;
using WexProject.MultiAccess.Library;

namespace WexMultiAcessManagerProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            WexMultiAccessManager.AoLogarInformacao += WexMultiAccessManager_AoLogarInformacao;
            Application.Run( new Form1() );
        }

        static void WexMultiAccessManager_AoLogarInformacao( string mensagem, Exception excessao )
        {
            Debug.WriteLine(mensagem);
        }
    }
}
