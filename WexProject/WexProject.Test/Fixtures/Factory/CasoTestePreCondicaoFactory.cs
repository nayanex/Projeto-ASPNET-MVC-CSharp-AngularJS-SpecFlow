using System;

using DevExpress.Xpo;
using WexProject.BLL.Models.Qualidade;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory, caso de teste da pré condição
    /// </summary>
    public class CasoTestePreCondicaoFactory : BaseFactory
    {
        /// <summary>
        /// método  Criar
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="casoteste">Casoteste</param>
        /// <param name="txDescricao">String</param>
        /// <param name="save">bool</param>
        /// <returns>casoTestePreCondicao</returns>
        public static CasoTestePreCondicao Criar(Session session, CasoTeste casoteste, String txDescricao = "", bool save = false)
        {
            CasoTestePreCondicao casoTestePreCondicao = new CasoTestePreCondicao(session);

            if (String.IsNullOrEmpty(txDescricao))
                casoTestePreCondicao.TxDescricao = GetDescricao();


            casoTestePreCondicao.CasoTeste = casoteste;


            if (save)
                casoTestePreCondicao.Save();

            return casoTestePreCondicao;
        }

    }
}
