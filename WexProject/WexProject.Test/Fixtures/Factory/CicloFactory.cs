using System;
using DevExpress.Xpo;
using WexProject.BLL.Models.Execucao;
using WexProject.BLL.Models.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory ciclo em desenvolvimento
    /// </summary>
    public class CicloFactory : BaseFactory
    {
        /// <summary>
        /// método criar
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">Projeto</param>
        /// <param name="txMeta">String</param>
        /// <param name="save">bool</param>
        /// <returns>ciclo</returns>
        public static CicloDesenv Criar(Session session, Projeto projeto, string txMeta = "", bool save = false)
        {
            CicloDesenv ciclo = new CicloDesenv(session);

            if (String.IsNullOrEmpty(txMeta))
                ciclo.TxMeta = GetDescricao();

            ciclo.Projeto = projeto;

            if (save)
                ciclo.Save();

            return ciclo;
        }

    }
}
