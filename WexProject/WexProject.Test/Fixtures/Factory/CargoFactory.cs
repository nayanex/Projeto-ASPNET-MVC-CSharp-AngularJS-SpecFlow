using System;
using DevExpress.Xpo;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Models.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Classe da Factory de Cargo
    /// </summary>
    public class CargoFactory : BaseFactory
    {
        /// <summary>
        /// metodo criar que instancia um objeto da factory
        /// </summary>
        /// <param name="session">seção</param>
        /// <param name="txDescricao">descrição</param>
        /// <param name="save">save</param>
        /// <returns>retorna um cargo</returns>
        public static Cargo Criar(Session session, string txDescricao = "",  bool save = false)
        {
            Cargo cargo = new Cargo(session);

            if (String.IsNullOrEmpty(txDescricao))
            {
                cargo.TxDescricao = GetDescricao();
            }
            else
            {
                cargo.TxDescricao = txDescricao;
            }

            if (save)
                cargo.Save();

            return cargo;
        }

    }
}
