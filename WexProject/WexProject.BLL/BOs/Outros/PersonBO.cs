using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.BOs.Outros
{
    public class PersonBO
    {
        /// <summary>
        /// Método para criar uma Person padrão
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="party">Objeto Party( config )</param>
        /// <param name="primeiroNome">Primerio Nome do colaborador/usuario</param>
        /// <param name="ultimoNome">Ultimo Nome do colaborador/usuario</param>
        /// <param name="login">login do usuario</param>
        /// <param name="extensaoEmail">extensao de email da empresa</param>
        /// <returns></returns>
        public static Person CriarPersonPadrao( WexDb contexto, Party party, string primeiroNome, string ultimoNome, string login, string extensaoEmail )
        {
            Person person = new Person()
            {
                Oid = party.Oid,
                FirstName = primeiroNome,
                LastName = ultimoNome,
                MiddleName = "",
                Birthday = null,
                Email = string.Format( "{0}{1}", login, extensaoEmail )
            };

            contexto.Person.Add( person );
            contexto.SaveChanges();

            return person;
        }
    }
}
