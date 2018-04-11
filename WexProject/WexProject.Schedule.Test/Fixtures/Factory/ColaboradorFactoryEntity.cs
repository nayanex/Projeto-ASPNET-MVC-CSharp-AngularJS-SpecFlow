using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.RH;

namespace WexProject.Schedule.Test.Fixtures.Factory
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
        public static Colaborador CriarColaborador( WexDb contexto, string matricula, DateTime admissao, string email,
            string fistName, string middleName, string lastName, string username, Cargo cargo = null, bool save = true )
        {
            Colaborador colaborador = new Colaborador();
            colaborador.Usuario = new User()
            {
                Person = new Person()
                {
                    Party = new Party()
                }
            };

            string[] nome = username.Split( '.' );
            colaborador.TxMatricula = matricula;
            colaborador.DtAdmissao = admissao;
            colaborador.Usuario.Person.Email = email;

            if(nome != null && nome.Length > 0)
            {
                if(nome.Length > 1)
                {
                    colaborador.Usuario.Person.FirstName = AumentarPrimeiraLetra( nome[0] );
                    colaborador.Usuario.Person.LastName = AumentarPrimeiraLetra( nome[ nome.Length - 1] );
                }
                else
                    colaborador.Usuario.Person.FirstName = AumentarPrimeiraLetra( nome[0] ); 
            }
            colaborador.Usuario.UserName = username;

            if(save)
            {
                contexto.Colaboradores.Add( colaborador );
                contexto.SaveChanges();
            }

            return colaborador;
        }

        /// <summary>
        /// Converter a primeira letra para Maiuscula
        /// </summary>
        /// <param name="palavra">palavra</param>
        /// <returns></returns>
        public static string AumentarPrimeiraLetra(string palavra)
        {
            if(string.IsNullOrWhiteSpace( palavra ))
                return string.Empty;
            char[] letras = palavra.ToCharArray();
            letras[0] = char.ToUpper( letras[0] );
            return new string( letras );
        }

        /// <summary>
        /// Cria um Colaborador a partir do UserName
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="username">UserName</param>
        /// <param name="save">Se é para salvar ou não</param>
        /// <returns>Colaborador criado</returns>
        public static Colaborador CriarColaborador( WexDb contexto, string username, bool save = true )
        {
            return CriarColaborador( contexto, null, DateTime.Now, null, username, string.Empty, string.Empty, username, null, save );
        }
    }
}
