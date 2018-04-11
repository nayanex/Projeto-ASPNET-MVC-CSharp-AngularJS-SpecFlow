using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL;
using WexProject.BLL.Models;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Contexto;

namespace WexProject.Test.Fixtures.Factory
{
    public class ColaboradorFactoryEntity
    {
        /// <summary>
        /// CriarColaborador
        /// </summary>
        /// <param name="contexto">contexto do banco</param>
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
        public static Colaborador CriarColaborador( WexDb contexto, string username, Guid? OidColaboradorUltimoFiltro = null, string matricula = "", DateTime admissao = new DateTime(), string email = "",
            string fistName = "", string middleName = "", string lastName = "", Cargo cargo = null )
        {
            Colaborador colaborador = new Colaborador();
            colaborador.Usuario = new User()
            {
                Person = new Person()
                {
                    Party = new Party()
                }
            };
            colaborador.Oid = Guid.NewGuid();
            colaborador.TxMatricula = matricula;
            colaborador.DtAdmissao = admissao;
            colaborador.OidColaboradorUltimoFiltro = OidColaboradorUltimoFiltro;
            colaborador.Usuario.Person.Email = email;
            colaborador.Usuario.Person.FirstName = fistName;
            colaborador.Usuario.Person.MiddleName = middleName;
            colaborador.Usuario.Person.LastName = lastName;
            colaborador.Usuario.UserName = username;

            contexto.Colaboradores.Add( colaborador );
            contexto.SaveChanges();

            return colaborador;
        }
    }
}
