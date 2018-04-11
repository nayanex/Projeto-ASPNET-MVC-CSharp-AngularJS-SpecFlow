using System;
using DevExpress.Xpo;
using WexProject.BLL.Models.Qualidade;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory caso de teste resultado esperado
    /// </summary>
    public class CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory : BaseFactory
    {
        /// <summary>
        /// método Criar
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="casoTestePassoResultadoEsperado">CasoTestePassoResultadoEsperado</param>
        /// <param name="save">bool</param>
        /// <returns>casoTestePassoResultadoEsperadoInfoAdicional</returns>
        public static CasoTestePassoResultadoEsperadoInformacaoAdicional Criar(Session session, CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado, bool save = false)
        {

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional = new CasoTestePassoResultadoEsperadoInformacaoAdicional(session);

            casoTestePassoResultadoEsperadoInfoAdicional.CasoTestePassoResultadoEsperado = casoTestePassoResultadoEsperado;


            if (save)
                casoTestePassoResultadoEsperadoInfoAdicional.Save();

            return casoTestePassoResultadoEsperadoInfoAdicional;
        }

    }
}
