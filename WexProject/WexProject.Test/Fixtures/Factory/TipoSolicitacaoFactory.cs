using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Models.NovosNegocios;
using DevExpress.Xpo;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory de TipoSolicitacao
    /// </summary>
    class TipoSolicitacaoFactory : BaseFactory
    {
        /// <summary>
        /// Cria um objeto de TipoSolicitacao
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <param name="descricao">Descrição do Tipo de Solicitação</param>
        /// <param name="save">É para salvar?</param>
        /// <returns>Objeto de TipoSolicitacao</returns>
        public static TipoSolicitacao CriarTipoSolicitacao(Session session, string descricao, bool save = false)
        {
            TipoSolicitacao tipo = new TipoSolicitacao(session);

            tipo.TxDescricao = descricao;

            if (save)
                tipo.Save();

            return tipo;
        }
    }
}
