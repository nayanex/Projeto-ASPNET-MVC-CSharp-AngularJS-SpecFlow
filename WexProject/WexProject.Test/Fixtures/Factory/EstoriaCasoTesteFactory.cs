using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Models.Qualidade;
using WexProject.BLL.Models.Escopo;
using DevExpress.Xpo;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// factory estaria caso teste
    /// </summary>
    public class EstoriaCasoTesteFactory : BaseFactory
    {
        /// <summary>
        /// méodo Criar
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="casoTeste">CasoTeste</param>
        /// <returns>estoriaCasoteste</returns>
        public static EstoriaCasoTeste Criar(Session session, CasoTeste casoTeste )
        {

            EstoriaCasoTeste estoriaCasoteste = new EstoriaCasoTeste(session);
            estoriaCasoteste.CasoTeste = casoTeste;

            return estoriaCasoteste;
        }
    }
}
