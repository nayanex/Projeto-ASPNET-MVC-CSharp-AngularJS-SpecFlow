using System;
using DevExpress.Xpo;
using WexProject.BLL.Models.Qualidade;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory caso teste pré condição
    /// </summary>
    public class CasoTestePreCondicaoAnexoFactory : BaseFactory
    {
        /// <summary>
        /// método Criar
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="casoTestePreCondicao">CasoTestePreCondicao</param>
        /// <param name="txDescricao">String</param>
        /// <param name="save">bool</param>
        /// <returns>casoTestePreCondicaoAnexo</returns>
        public static CasoTestePreCondicaoAnexo Criar(Session session, CasoTestePreCondicao casoTestePreCondicao, String txDescricao, bool save = false)
        {

            CasoTestePreCondicaoAnexo casoTestePreCondicaoAnexo = new CasoTestePreCondicaoAnexo(session);

            if (String.IsNullOrEmpty(txDescricao))
                casoTestePreCondicaoAnexo.TxDescricao = GetDescricao();

            casoTestePreCondicaoAnexo.CasoTestePreCondicao = casoTestePreCondicao;


            if (save)
                casoTestePreCondicaoAnexo.Save();

            return casoTestePreCondicaoAnexo;
        }

    }
}
