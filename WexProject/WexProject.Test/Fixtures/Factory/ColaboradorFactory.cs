using System;
using DevExpress.Xpo;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Models.Geral;
using DevExpress.Persistent.BaseImpl;
using WexProject.Library.Libs.DataHora;
using WexProject.BLL.DAOs.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe Colaborador
    /// </summary>
    public class ColaboradorFactory : BaseFactory
    {
        /// <summary>
        /// CriarColaborador
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="matricula">matricula</param>
        /// <param name="admissao">admissao</param>
        /// <param name="email">email</param>
        /// <param name="fistName">fistName</param>
        /// <param name="middleName">middleName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="username">username</param>
        /// <param name="cargo">Cargo do Colaborador</param>
        /// <param name="save">save</param>
        /// <returns>colaborador</returns>
        public static Colaborador CriarColaborador(Session session, string matricula, DateTime admissao, string email,
            string fistName, string middleName, string lastName, string username, Cargo cargo = null, bool save = true)
        {
            if (cargo == null)
            {
                cargo = CargoFactory.Criar(session, GetDescricao(), true);
            }

            Colaborador colaborador = new Colaborador(session)
            {
                TxMatricula = matricula,
                DtAdmissao = admissao,
                Cargo = cargo
            };

            colaborador.Usuario.Email = email;
            colaborador.Usuario.FirstName = fistName;
            colaborador.Usuario.MiddleName = middleName;
            colaborador.Usuario.LastName = lastName;
            colaborador.Usuario.UserName = username;

            if (save)
            {
                colaborador.Usuario.Save();
                colaborador.Save();
            }

            return colaborador;
        }

        /// <summary>
        /// Cria um Colaborador a partir do UserName
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <param name="username">UserName</param>
        /// <param name="save">Se é para salvar ou não</param>
        /// <returns>Colaborador criado</returns>
        public static Colaborador CriarColaborador(Session session, string username, bool save = true)
        {
            return CriarColaborador(session, GetDescricao(), DateTime.Now, GetDescricao(),
                username, string.Empty, string.Empty, username, null, save);
    }

        public static User CriarUsuario( Session session, String txUserName, String txFirstName, String txLastName,
    String txEmail, bool save = false )
        {
            User usuario = new User( session )
            {
                UserName = txUserName,
                FirstName = txFirstName,
                LastName = txLastName,
                Email = txEmail
            };
            DateUtil.CurrentDateTime = DateTime.MinValue;
            UsuarioDAO.CurrentUser = usuario;

            if(save)
                usuario.Save();

            return usuario;
        }
    }
}