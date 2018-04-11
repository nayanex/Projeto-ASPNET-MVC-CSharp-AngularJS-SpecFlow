using System;
using DevExpress.Xpo;
using WexProject.BLL.Models.Qualidade;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory de caso de teste
    /// </summary>
    public class CasoTesteFactory : BaseFactory
    {

        /// <summary>
        /// metodo criar que instancia um objeto da factore
        /// </summary>
        /// <param name="session">seção</param>
        /// <param name="requisitos">objeto de requisito</param>
        /// <param name="txPassos">passos</param>
        /// <param name="txSumario">sumario</param>
        /// <param name="txPrecondicoes">precondição</param>
        /// <param name="save">save</param>
        /// <returns>caso de teste</returns>
        public static CasoTeste Criar(Session session, Requisito requisitos, String txPassos = "", String txSumario = "",
        String txPrecondicoes = "", bool save = false)
        {
            //String TxResultadoEsperado = "",
            CasoTeste casoteste = new CasoTeste(session);

            //if (String.IsNullOrEmpty(TxPassos))
            //    casoteste.TxPassos = GetDescricao();

            if (String.IsNullOrEmpty(txSumario))
                casoteste.TxSumario = GetDescricao();

            //if (String.IsNullOrEmpty(TxResultadoEsperado))
            //    casoteste.TxResultadoEsperado = GetDescricao();

            //if (String.IsNullOrEmpty(TxPrecondicoes))
            //    casoteste.TxPrecondicoes = GetDescricao();

            casoteste.Requisito = requisitos;

            if (save)
                casoteste.Save();

            return casoteste;
        }
    }
}
