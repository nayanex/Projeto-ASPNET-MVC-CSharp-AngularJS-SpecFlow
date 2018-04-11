using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace WexProject.Library.Libs.ActiveDirectory
{
    public class ADUtil
    {
        /// <summary>
        /// Método responsável por buscar o usuário de logado no windows
        /// </summary>
        /// <returns>String contendo o login do usuário do Windows</returns>
        public static String GetUsuarioLogadoPeloWindows()
        {
            string login = WindowsIdentity.GetCurrent().Name;
            string[] arrayLogin = login.Split( '\\' );
            login = arrayLogin[1];

            return login;
        }
    }
}
