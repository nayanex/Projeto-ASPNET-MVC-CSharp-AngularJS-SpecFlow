using System;

using DevExpress.Xpo;
using WexProject.BLL.Models.Qualidade;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// factory caso  teste pré-condição informação adicional
    /// </summary>
    public class CasoTestePreCondicaoInformacaoAdicionalFactory : BaseFactory
    {
        /// <summary>
        /// método Criar 
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="casoTestePreCondicao">CasoTestePreCondicao</param>
        /// <param name="save">bool</param>
        /// <returns>casoTestePreCondicaoInfoAdicional</returns>
        public static CasoTestePreCondicaoInformacaoAdicional Criar(Session session, CasoTestePreCondicao casoTestePreCondicao, bool save = false)
        {

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional = new CasoTestePreCondicaoInformacaoAdicional(session);

            casoTestePreCondicaoInfoAdicional.CasoTestePreCondicao = casoTestePreCondicao;


            if (save)
                casoTestePreCondicaoInfoAdicional.Save();

            return casoTestePreCondicaoInfoAdicional;
        }

    }
}
