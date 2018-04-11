using System;

using DevExpress.Xpo;
using WexProject.BLL.Models.Qualidade;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory de caso de teste resultado esperado
    /// </summary>
    public class CasoTestePassoResultadoEsperadoFactory : BaseFactory
    {

        /// <summary>
        /// casoTestePassoResultadoEsperado
        /// </summary>
        /// <param name="session">seção</param>
        /// <param name="passo">passo</param>
        /// <param name="txResultadoEsperado">resultados esperados</param>
        /// <param name="save">save</param>
        /// <returns>retorna um passo de resultado esperado</returns>
        public static CasoTestePassoResultadoEsperado Criar(Session session, CasoTestePasso passo, String txResultadoEsperado = "", bool save = false)
        {

            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado = new CasoTestePassoResultadoEsperado(session);

            if (String.IsNullOrEmpty(txResultadoEsperado))
                casoTestePassoResultadoEsperado.TxResultadoEsperado = GetDescricao();


            casoTestePassoResultadoEsperado.Passo = passo;

            if (save)
                casoTestePassoResultadoEsperado.Save();

            return casoTestePassoResultadoEsperado;
        }

    }
}
