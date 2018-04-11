using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Contexto
{
    /// <summary>
    /// Classe abstrata para a criação de contextos para o banco
    /// </summary>
    public abstract class ContextFactory
    {
        /// <summary>
        /// Método de criação de um contexto de banco WebDb
        /// </summary>
        /// <returns></returns>
        public abstract WexDb CriarWexDb();
    }
}
