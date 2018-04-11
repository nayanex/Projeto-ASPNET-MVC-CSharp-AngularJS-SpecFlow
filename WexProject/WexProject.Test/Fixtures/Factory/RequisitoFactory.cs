using System;

using DevExpress.Xpo;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// factory requisito
    /// </summary>
    public class RequisitoFactory : BaseFactory
    {
        /// <summary>
        /// método crair
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="modulo">modulo</param>
        /// <param name="txNome">TxNome</param>
        /// <param name="txDescricao">TxDescricao</param>
        /// <param name="txLinkPrototipo">TxLinkPrototipo</param>
        /// <param name="save">bool</param>
        /// <returns>requisito</returns>
        public static Requisito Criar(Session session, Modulo modulo, string txNome = "", String txDescricao = "", String txLinkPrototipo = "", bool save = false)
        {
            Requisito requisito = new Requisito(session);
            if (String.IsNullOrEmpty(txNome))
                requisito.TxNome = GetDescricao();

            if (String.IsNullOrEmpty(txDescricao))
                requisito.TxDescricao = GetDescricao();

            if (String.IsNullOrEmpty(txLinkPrototipo))
                requisito.TxLinkPrototipo = GetDescricao();

            requisito.Modulo = modulo;

            if (save)
                requisito.Save();

            return requisito;
        }
    }
}
