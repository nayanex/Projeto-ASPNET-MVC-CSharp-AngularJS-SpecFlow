using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using WexProject.BLL.Shared.DTOs.Geral;
using System.Collections.Generic;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.BOs.Planejamento;

namespace WexProject.BLL.DAOs.Geral
{
    /// <summary>
    /// Util para usuário
    /// </summary>
    public class UsuarioDAO
    {
        #region Propriedades

        /// <summary>
        /// Propriedade que guarda o Usuário corrente
        /// </summary>
        public static User CurrentUser { get; set; }

        #endregion

        #region Consultas

        /// <summary>
        /// Método responsável por buscar o usuário logado.
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Objeto Usuário</returns>
        public static User GetUsuarioLogado(Session session)
        {
            if (CurrentUser == null)
            {
                User user = session.GetObjectByKey<User>(session.GetKeyValue(SecuritySystem.CurrentUser));

                validarUsuario(user);

                return user;
            }
            else
                return CurrentUser;
        }

        /// <summary>
        /// Método responsável por buscar o usuário através od login
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <param name="login">Login do usuário (UserName)</param>
        /// <returns>Objeto Usuário</returns>
        public static User GetUsuarioPorLogin(Session session, string login)
        {
            User user = session.FindObject<User>(CriteriaOperator.Parse("UserName = ?", login));

            validarUsuario(user);

            return user;
        }

        #endregion

        #region Utilitários

        /// <summary>
        /// Método responsável por validar se o usuário foi encontrado.
        /// </summary>
        /// <param name="user"></param>
        public static void validarUsuario(User user)
        {
            if (user == null)
                throw new Exception("O objeto usuário não pode ser nulo.");
        }

        #endregion
    }
}
