using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Contexto
{
    /// <summary>
    /// Implementação concreta do ContextFactory para a criação de um contexto de banco
    /// </summary>
    public class ContextFactoryImpl : ContextFactory
    {
        /// <summary>
        /// Retorna um contexto WexDb utilizando a implementação real do banco
        /// </summary>
        /// <returns>contexto do banco</returns>
        public override WexDb CriarWexDb()
        {
            return new WexDb();
        }
    }
}
