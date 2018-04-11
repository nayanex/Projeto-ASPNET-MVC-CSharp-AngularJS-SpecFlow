using System;
using System.Collections.Generic;
using System.Security.Principal;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
using WexProject.Library.Properties;

namespace WexProject.Library.Libs.ActiveDirectory
{
    /// <summary>
    /// classe de autenticação de diretorio
    /// </summary>
    public class WexAuthenticationActiveDirectory : AuthenticationActiveDirectory
    {
        #region Construtores

        /// <summary>
        /// Construtor
        /// </summary>
        public WexAuthenticationActiveDirectory()
            : base()
        {
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="userType">UserType</param>
        /// <param name="logonParametersType">LogonParametersType</param>
        public WexAuthenticationActiveDirectory(Type userType, Type logonParametersType)
            : base(userType, logonParametersType)
        {
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Parâmetros de Login
        /// </summary>
        private AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters();
        /// <summary>
        /// Retorna se os dados de login são informados
        /// pelo usuário
        /// </summary>
        public override bool AskLogonParametersViaUI
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Retorna o nome do usuário
        /// </summary>
        /// <returns>nome</returns>
        protected override string GetUserName()
        {
            return parameters.UserName;
        }

        /// <summary>
        /// Parâmetros de Login
        /// </summary>
        public override object LogonParameters
        {
            get
            {
                return parameters;
            }
        }

        #endregion

        #region Eventos

        public delegate Object AoAutenticarUsuarioEventHandler(string login, string extensaoEmail);

        public event AoAutenticarUsuarioEventHandler AoAutenticarUsuario;

        public delegate Object AoAutenticarUsuarioComSessaoEventHandler( Session session, string login, string extensaoEmail );

        public event AoAutenticarUsuarioComSessaoEventHandler AoAutenticarUsuarioComSessao;

        #endregion

        #region Regras de Negócio

        /// <summary>
        /// Método de autenticação
        /// </summary>
        /// <param name="objectSpace">ObjectSpace</param>
        /// <returns>usuário autenticado</returns>
        public override object Authenticate(IObjectSpace objectSpace)
        {
            ObjectSpace objectSpaceComSessao = (ObjectSpace)objectSpace;

            parameters.UserName = ADUtil.GetUsuarioLogadoPeloWindows();

            WexAuthenticateAD ad = new WexAuthenticateAD(ADResources.ADServer);
            bool sucesso = ad.IsAuthenticated( ADResources.ADDomain, parameters.UserName );

            Object usuario = null;

            if (sucesso)
            {
                if(objectSpaceComSessao != null)
                {
                    usuario = AoAutenticarUsuarioComSessao( objectSpaceComSessao.Session, parameters.UserName, ADResources.ExtEmail );
                }
                else
                {
                    usuario = AoAutenticarUsuario( parameters.UserName, ADResources.ExtEmail );
                }
            }
            else
            {
                throw new AuthenticationException(parameters.UserName, SecurityExceptionLocalizer.GetExceptionMessage(SecurityExceptionId.RetypeTheInformation));
            }

            return usuario;
        }

        /// <summary>
        /// Método de autenticação com senha.
        /// </summary>
        /// <param name="userName">login do usuário.</param>
        /// <param name="password"> senha do usuário. </param>
        /// <returns>usuário autenticado</returns>
        public bool AuthenticatedPassword(String userName, String password)
        {

            WexAuthenticateAD ad = new WexAuthenticateAD(ADResources.ADServer);
            bool sucesso = ad.IsAuthenticated(ADResources.ADDomain, userName, password);

            return sucesso;
        }
        #endregion
    }
}