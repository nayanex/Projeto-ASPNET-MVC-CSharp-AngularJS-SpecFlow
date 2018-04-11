using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.BOs.Outros
{
    public class UserBO
    {
        /// <summary>
        /// Método responsável por criar um User padrão
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="person">Objeto Person</param>
        /// <param name="login">Login do usuário</param>
        /// <returns>Objeto User padrão (ENTITY)</returns>
        public static User CriarUserPadrao( WexDb contexto, Person person, string login )
        {
            User usuario = new User()
            {
                Oid = person.Oid,
                StoredPassword = null,
                UserName = login,
                ChangePasswordOnFirstLogon = false,
                IsActive = true
            };

            contexto.Usuario.Add( usuario );
            contexto.SaveChanges();

            return usuario;
        }

        /// <summary>
        /// Método responsável por instaciar e salvar no banco um colaboradorUltimoFiltro padrão.
        /// </summary>
        /// <returns>Objeto criado e salvo no banco</returns>
        public static UserUsers_RoleRoles CriarPermissaoParaUsuario( WexDb contexto, User usuario, Role role )
        {
            UserUsers_RoleRoles userRoles = new UserUsers_RoleRoles()
            {
                OidUser = usuario.Oid,
                OidRole = role.Oid,
                OptimisticLockField = 0
            };

            contexto.UserUsers_RoleRoles.Add( userRoles );
            contexto.SaveChanges();

            return userRoles;
        }
    }
}
