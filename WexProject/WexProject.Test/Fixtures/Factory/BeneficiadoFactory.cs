using System;
using DevExpress.Xpo;

using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Classe de Beneficiado Factory
    /// </summary>
    public class BeneficiadoFactory : BaseFactory
    {
        /// <summary>
        /// metodo criar que instancia um objeto da factory
        /// </summary>
        /// <param name="session">seção</param>
        /// <param name="txDescricao">descrição</param>
        /// <param name="save">save</param>
        /// <returns>retorna um beneficiado</returns>
        public static Beneficiado Criar(Session session, String txDescricao, bool save = false)
        {
            Beneficiado beneficiado = new Beneficiado(session);

            beneficiado.TxDescricao = txDescricao;
            if (String.IsNullOrEmpty(beneficiado.TxDescricao))
                beneficiado.TxDescricao = GetDescricao();

            if (save)
                beneficiado.Save();

            return beneficiado;
        }

    }
}
