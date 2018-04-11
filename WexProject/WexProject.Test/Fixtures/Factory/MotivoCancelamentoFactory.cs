using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using WexProject.BLL.Models.Execucao;

using WexProject.BLL.Shared.Domains.Execucao;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe MotivoCancelamento
    /// </summary>
    public class MotivoCancelamentoFactory : BaseFactory
    {
        /// <summary>
        /// Factory para criação de motivos de cancelamento
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="txDescricao">Descrição do real motivo de cancelamento do ciclo.</param>
        /// <param name="csSituacao">Mostra se o motivo está ativo ou inativo para utilização</param>
        /// <param name="save">Se deverá ser salvo ou não</param>
        /// <returns>Objeto com dados</returns>
        public static MotivoCancelamento CriarMotivoCancelamento(Session session, String txDescricao, 
            CsStatusMotivoCancelamento csSituacao = CsStatusMotivoCancelamento.Ativo, bool save = true)
        {
            MotivoCancelamento motivoCancelamento = new MotivoCancelamento(session)
            {
                TxDescricao = txDescricao,
                CsSituacao = csSituacao
            };

            if (save)
                motivoCancelamento.Save();

            return motivoCancelamento;
        }
    }
}