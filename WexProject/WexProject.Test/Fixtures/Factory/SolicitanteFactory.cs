using System;

using DevExpress.Xpo;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// factory solicitante
    /// </summary>
    public class SolicitanteFactory : BaseFactory
    {
        /// <summary>
        /// método criar
        /// </summary>
        /// <param name="session">Session</param>
        /// <param name="txNome">String</param>
        /// <param name="save">bool</param>
        /// <returns>solicitante</returns>
        public static Solicitante Criar(Session session, String txNome = "", bool save = false)
        {
            Solicitante solicitante = new Solicitante(session);
            if (String.IsNullOrEmpty(txNome))
                solicitante.TxNome = GetDescricao();

            solicitante.Cargo = CargoFactory.Criar(session);
            solicitante.EmpresaInstituicao = EmpresaInstituicaoFactory.Criar(session);

            if (save)
                solicitante.Save();

            return solicitante;
        }

    }
}
