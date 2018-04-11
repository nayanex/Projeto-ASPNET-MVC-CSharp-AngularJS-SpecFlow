using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL;
using WexProject.BLL.Contexto;

namespace WexProject.Test.Helpers.Mocks
{
    public class ContextFactoryMock : ContextFactory
    {

        /// <summary>
        /// Delegate de assinatura para métodos que retornem um contexto wex de conexão com o banco
        /// </summary>
        /// <returns></returns>
        public delegate WexDb AoAbrirConexao();

        /// <summary>
        /// Ponteiro que pode armazenar um método de criação de conexão
        /// </summary>
        public static AoAbrirConexao AbrirConexao { get; set; }

        /// <summary>
        /// Retorna um novo contexto de banco
        /// </summary>
        /// <returns></returns>
        public override WexDb CriarWexDb()
        {
            if(AbrirConexao != null)
                return AbrirConexao();
            return new WexDb();
        }
    }
}
