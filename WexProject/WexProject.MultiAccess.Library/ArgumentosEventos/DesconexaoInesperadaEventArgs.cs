using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.MultiAccess.Library.ArgumentosEventos
{
    /// <summary>
    /// classe de argumento para quando houver uma desconexao inesperada na classe ConexaoCliente
    /// </summary>
    public class DesconexaoInesperadaEventArgs : EventArgs
    {
        public string LoginUsuario { get; private set; }
        public string OidCronograma { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login">login do usuário conectado</param>
        /// <param name="oidCronograma">oid do cronograma atual</param>
        public DesconexaoInesperadaEventArgs(string login,string oidCronograma)
        {
            LoginUsuario = login;
            OidCronograma = oidCronograma;
        }
       
    }
}
