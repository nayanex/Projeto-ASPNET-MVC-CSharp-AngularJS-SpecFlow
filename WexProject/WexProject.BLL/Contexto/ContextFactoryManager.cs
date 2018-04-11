using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Contexto
{
    /// <summary>
    /// Classe para gerenciamento do
    /// </summary>
    public class ContextFactoryManager
    {
        /// <summary>
        /// Factory criador do contexto de banco
        /// </summary>
        private static ContextFactory factory;

        /// <summary>
        /// Setar um Factory específico
        /// </summary>
        /// <param name="contextFactoryDb">Criador de Db context Especifico</param>
        public static void SetContextFactory( ContextFactory contextFactoryDb ) 
        {
            if(contextFactoryDb != null)
                factory = contextFactoryDb;
        }

        /// <summary>
        /// Método para criar um context factory padrão
        /// </summary>
        private static void CriarSingletonFactoryPadrao()
        {
            if(factory == null)
            {
                factory = new ContextFactoryImpl();
            }
        }

        /// <summary>
        /// Método responsável por efetuar a criação de um contexto do WexDb
        /// </summary>
        /// <returns></returns>
        public static WexDb CriarWexDb() 
        {
            if(factory == null)
            {
                CriarSingletonFactoryPadrao();
            }
            return factory.CriarWexDb();
        }
    }
}
