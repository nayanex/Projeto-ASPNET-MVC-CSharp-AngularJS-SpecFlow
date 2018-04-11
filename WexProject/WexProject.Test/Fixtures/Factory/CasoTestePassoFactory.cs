using System;

using DevExpress.Xpo;
using WexProject.BLL.Models.Qualidade;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory de caso de teste passo
    /// </summary>
    public class CasoTestePassoFactory : BaseFactory
    {
        /// <summary>
        /// metodo criar que instancia um objeto da factore
        /// </summary>
        /// <param name="session">seção</param>
        /// <param name="casoteste">objeto de caso de teste</param>
        /// <param name="txPasso">passo</param>
        /// <param name="save">save</param>
        /// <returns>caso de teste passo</returns>
        public static CasoTestePasso Criar(Session session, CasoTeste casoteste, String txPasso = "", bool save = false)
        {

            CasoTestePasso casoTestePasso = new CasoTestePasso(session);

            if (String.IsNullOrEmpty(txPasso))
                casoTestePasso.TxPasso = GetDescricao();


            casoTestePasso.CasoTeste = casoteste;
            //casoTestePasso.ResultadosEsperados = resultados;


            if (save)
                casoTestePasso.Save();

            return casoTestePasso;
        }

    }
}
