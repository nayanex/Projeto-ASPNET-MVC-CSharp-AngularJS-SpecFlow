using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace WexProject.Web.Libs.ActiveDirectory
{
    /// <summary>
    /// classe de altenticação
    /// </summary>
    public class WexAuthenticateAD
    {
        #region Atributos
        /// <summary>
        /// atributo path
        /// </summary>
        private String _path;
        /// <summary>
        /// filtro de atributo
        /// </summary>
        private String _filterAttribute;
        #endregion

        #region Construtores
        /// <summary>
        /// Metodo de autenticação do path
        /// </summary>
        /// <param name="path">paramentro peth</param>
        public WexAuthenticateAD(String path)
        {
            _path = path;
        }
        #endregion

        #region Regras de Negócio


        /// <summary>
        /// verifica se esta autenticado
        /// </summary>
        /// <param name="dominio">dominio</param>
        /// <param name="loginDominio">nome de login</param>
        /// <param name="senha">senha</param>
        /// <returns>retorna verdadeiro ou falso</returns>
        public bool IsAuthenticated(string dominio, string loginDominio, string senha)
        {
			try
			{
				using( var contextoAutenticacao = new PrincipalContext( ContextType.Domain , dominio ) )
				{
					if( string.IsNullOrEmpty( senha ) )
						senha = "default";
					return contextoAutenticacao.ValidateCredentials( loginDominio , senha );
				}
			}
			catch( Exception )
			{
				throw new Exception( "Falha na autenticação no domínio" );
			}
        }

        /// <summary>
        /// Metodo de autenticação de dominio e nome
        /// </summary>
        /// <param name="domain">dominio</param>
        /// <param name="username">nome de usuario</param>
        /// <returns>retorna falso se for nulo ou verdadeiro se não for</returns>
        public bool IsAuthenticated(String domain, String username)
        {
            String domainAndUsername = String.Format(@"{0}\{1}", domain, username);

            DirectoryEntry entry = new DirectoryEntry(_path);

            try
            {
                //Bind to the native AdsObject to force authentication.			
                Object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry)
                {
                    Filter = String.Format("(SAMAccountName={0})", username)
                };

                search.PropertiesToLoad.Add("cn");

                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (String)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }
            return true;
        }

        /// <summary>
        /// metodo de grupos
        /// </summary>
        /// <returns>retorna nulo ou o nome do grupo</returns>
        public String GetGroups()
        {
            DirectorySearcher search = new DirectorySearcher(_path)
            {
                Filter = String.Format("(cn={0})", _filterAttribute)
            };

            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            try
            {
                SearchResult result = search.FindOne();

                int propertyCount = result.Properties["memberOf"].Count;

                String dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (String)result.Properties["memberOf"][propertyCounter];

                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }

                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            return groupNames.ToString();
        }
        #endregion
    }
}
