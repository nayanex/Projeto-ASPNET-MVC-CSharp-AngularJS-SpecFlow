using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace WexProject.Web.Libs.ActiveDirectory
{
    /// <summary>
    /// classe de autenticação
    /// </summary>
    public class WexStandardAuthentication : AuthenticationStandard
    {
        /// <summary>
        /// Metodo de autenticação
        /// </summary>
        public WexStandardAuthentication()
            : base()
        {
        }

        /// <summary>
        /// metodo de autenticação de tipos
        /// </summary>
        /// <param name="userType">tipo de usuario</param>
        /// <param name="logonParametersType">parametro de tipo de login</param>
        public WexStandardAuthentication(Type userType, Type logonParametersType)
            : base(userType, logonParametersType)
        {
        }


        public delegate Object AoAutenticarUsuarioComSessaoEventHandler( Session session, string login, string extensaoEmail );

        public event AoAutenticarUsuarioComSessaoEventHandler AoAutenticarUsuarioComSessao;


        /// <summary>
        /// cria um objeto do tipo uthenticationStandardLogonParametersuthenticationStandardLogonParameters
        /// </summary>
        private AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters();

        /// <summary>
        /// metodo que sobrescreve o metodo AskLogonParametersViaUI
        /// </summary>
        public override bool AskLogonParametersViaUI
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// parametros de login
        /// </summary>
        public override object LogonParameters
        {
            get
            {
                //parameters.UserName = "Sam";
                //parameters.Password = "";
                return parameters;
            }
        }

        /// <summary>
        /// metodo de autenticação 
        /// </summary>
        /// <param name="objectSpace">objeto de IObjectSpace</param>
        /// <returns>returna o usuario</returns>
        public override object Authenticate(IObjectSpace objectSpace)
        {
            ObjectSpace objectSpaceComSessao = (ObjectSpace)objectSpace;

            object user = null;

            WexAuthenticateAD ad = new WexAuthenticateAD( WexProject.Web.Properties.ADResources.ADServer );
            bool sucesso = ad.IsAuthenticated( WexProject.Web.Properties.ADResources.ADDomain, parameters.UserName, parameters.Password );

            if (sucesso)
            {
                if (user == null)
                {
                    user = AoAutenticarUsuarioComSessao( objectSpaceComSessao.Session, parameters.UserName, WexProject.Web.Properties.ADResources.ExtEmail );
                }
            }
            else
            {
                throw new AuthenticationException(parameters.UserName, SecurityExceptionLocalizer.GetExceptionMessage(SecurityExceptionId.RetypeTheInformation));
            }

            /* O código a seguir foi implementado devido a urgência de uma solicitação, 
             * o motivo de não estar implementado dentro dos padrões de código limpo e utilizando a devida segurança é que no momento não há tempo hábil dentro do cronograma do ciclo atual.
             * Obs: Posteiormente deve ser reimplementado para que esteja de acordo com os padrões de código e utilizando as devidas classes de segurança.
             */
            List<string> usuariosPermitidos = new List<string>();
            usuariosPermitidos.Add("acruz");
            usuariosPermitidos.Add("lbraga");
            usuariosPermitidos.Add("avieira");
            usuariosPermitidos.Add("lgavinho");
            usuariosPermitidos.Add("rgarcia");
            usuariosPermitidos.Add("amendes");
            usuariosPermitidos.Add("rcaetano");
            usuariosPermitidos.Add("roberto.garcia");
            usuariosPermitidos.Add("ana.horta");

            User usuario = (User)user;

            if(!usuariosPermitidos.Contains( usuario.UserName ))
            {
                throw new AuthenticationException( parameters.UserName, "Usuário não autorizado." );
            }

            return user;
        }
    }
}
