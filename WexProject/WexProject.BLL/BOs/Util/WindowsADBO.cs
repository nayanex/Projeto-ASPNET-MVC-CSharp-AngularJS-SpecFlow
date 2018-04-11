using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.Library.Libs.ActiveDirectory;
using System.Security.Authentication;

namespace WexProject.BLL.BOs.Util
{
    public class WindowsADBO
    {
        public static bool AutenticarColaboradorAD(string login, string senha)
        {
            try
            {
                WexAuthenticationActiveDirectory wexAuthenticationActiveDirectory = new WexAuthenticationActiveDirectory();
                return wexAuthenticationActiveDirectory.AuthenticatedPassword(login, senha);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
