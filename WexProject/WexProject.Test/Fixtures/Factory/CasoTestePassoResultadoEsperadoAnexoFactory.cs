using System;
using DevExpress.Xpo;
using WexProject.BLL.Models.Qualidade;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory de caso de teste resultado esperado anexo
    /// </summary>
    public class CasoTestePassoResultadoEsperadoAnexoFactory : BaseFactory
    {
        /// <summary>
        /// metodo criar que instancia um objeto da factore
        /// </summary>
        /// <param name="session">seção</param>
        /// <param name="casoTestePassoResultadoEsperado">objeto de casoTestePassoResultadoEsperado</param>
        /// <param name="txDescricao">descrição</param>
        /// <param name="save">save</param>
        /// <returns>retorna um anexo de resultados esperados</returns>
        public static CasoTestePassoResultadoEsperadoAnexo Criar(Session session, CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado, String txDescricao, bool save = false)
        {

            CasoTestePassoResultadoEsperadoAnexo casoTestePassoResultadoEsperadoAnexo = new CasoTestePassoResultadoEsperadoAnexo(session);

            if (String.IsNullOrEmpty(txDescricao))
                casoTestePassoResultadoEsperadoAnexo.TxDescricao = GetDescricao();

            casoTestePassoResultadoEsperadoAnexo.CasoTestePassoResultadoEsperado = casoTestePassoResultadoEsperado;


            if (save)
                casoTestePassoResultadoEsperadoAnexo.Save();

            return casoTestePassoResultadoEsperadoAnexo;
        }

    }
}
